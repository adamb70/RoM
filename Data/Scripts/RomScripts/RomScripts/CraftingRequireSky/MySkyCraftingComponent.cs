using Medieval.GameSystems;
using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Replication;
using Sandbox.ModAPI;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.Network;
using VRage.Systems;
using VRageMath;

namespace RomScripts.SkyCraftingComponent
{
    [MyComponent(typeof(MyObjectBuilder_SkyCraftingComponent))]
    [ReplicatedComponent]
    public class MySkyCraftingComponent : MyEntityComponent, IMyEventOwner, IMyEventProxy
    {
        private enum WarningMessage
        {
            IsBlocked = 0,
            IsFreed,
            IsBlockedFinalWarning,
            IsDestroyed
        }

        private MySkyCraftingComponentDefinition m_definition = null;
        private bool m_wasWarningSent = false;

        public override void Init(MyEntityComponentDefinition definition)
        {
            m_definition = definition as MySkyCraftingComponentDefinition;
        }

        public override void OnAddedToScene()
        {
            base.OnAddedToScene();

            MyUpdateComponent.Static.Schedule(PerformInitialPlacementTest);

            // Register for update
            MyUpdateComponent.Static.AddForUpdate(Update, m_definition.CheckIntervalMS);
        }

        public override void OnRemovedFromScene()
        {
            base.OnRemovedFromScene();

            MyUpdateComponent.Static.RemoveFromUpdate(Update);
        }

        private void PerformInitialPlacementTest(long interval)
        {
            // If Utilities is not yet available, reschedule and try again next frame.
            if (MyAPIGateway.Utilities == null)
            {
                MyUpdateComponent.Static.Schedule(PerformInitialPlacementTest);
                return;
            }

            // Don't run initial placement test on DS
            if (MyAPIGateway.Utilities.IsDedicated)
                return;

            // Get the entity position and owner ID
            var position = Entity.PositionComp.GetPosition();
            long blockOwnerId = (Entity as MyCubeBlock).OwnerId;

            // Calculate up direction by inverting the gravity at this position, if it is zero, use a default direction
            Vector3 up = MyGravityProviderSystem.CalculateNaturalGravityInPoint(position);
            if (Vector3.IsZero(up))
            {
                up = Vector3.Up;
            }
            else
            {
                up = -up;
                up.Normalize();
            }

            // Check if we can see the sky
            bool canSeeSky = CanSeeSky(position, up);
            if (!canSeeSky)
            {
                NotifyOwner(position, blockOwnerId, WarningMessage.IsBlocked);
            }
        }

        private void Update(long interval)
        {
            // Only need to run on the server
            if (!MyAPIGateway.Multiplayer.IsServer)
            {
                MyUpdateComponent.Static.RemoveFromUpdate(Update);
                return;
            }

            // Get the entity position and owner ID
            var position = Entity.PositionComp.GetPosition();
            long blockOwnerId = (Entity as MyCubeBlock).OwnerId;

            // Calculate up direction by inverting the gravity at this position, if it is zero, use a default direction
            Vector3 up = MyGravityProviderSystem.CalculateNaturalGravityInPoint(position);
            if (Vector3.IsZero(up))
            {
                up = Vector3.Up;
            }
            else
            {
                up = -up;
                up.Normalize();
            }

            // Check if we can see the sky
            bool canSeeSky = CanSeeSky(position, up);
            if (!canSeeSky)
            {
                if (!m_wasWarningSent)
                {
                    // Notify owner that the entity is no longer able to find the sky.
                    MyMultiplayerModApi.Static.RaiseEvent(this, x => x.NotifyOwner, position, blockOwnerId, WarningMessage.IsBlockedFinalWarning);

                    m_wasWarningSent = true;
                }
                else
                {
                    // We hit something, block destruction time!
                    bool isDestroyed = TryDestroyBlock(position, up);

                    // Play particle effect if the block was destroyed.
                    if (isDestroyed)
                    {
                        // Notify owner that the entity is no longer able to find the sky.
                        MyMultiplayerModApi.Static.RaiseEvent(this, x => x.NotifyOwner, position, blockOwnerId, WarningMessage.IsDestroyed);

                        PlayParticleEffect(position, up);
                    }
                }
            }
            else
            {
                if (m_wasWarningSent)
                {
                    // Notify owner that the entity can see the sky again
                    MyMultiplayerModApi.Static.RaiseEvent(this, x => x.NotifyOwner, position, blockOwnerId, WarningMessage.IsFreed);
                }

                m_wasWarningSent = false;
            }
        }

        /// <summary>
        /// Performs a check to see the sky
        /// </summary>
        private bool CanSeeSky(Vector3D position, Vector3 up)
        {
            // Perform a short-range physics raycast, this allows players to build nice shrines with open roofs up to the preset meters tall (50 meters = 25 large blocks, quite tall in ME)
            List<IHitInfo> toList = new List<IHitInfo>();
            MyAPIGateway.Physics.CastRay(position, position + (m_definition.PhysicsCheckDistance * up), toList);

            // Iterate through all collisions
            foreach (var hitInfo in toList)
            {
                // Ignore voxels if requested
                bool isVoxel = (hitInfo.HitEntity is MyVoxelPhysics);
                if (m_definition.IgnoreVoxels && isVoxel)
                    continue;

                // Ignore blocks/grids if desired
                bool isBlock = (hitInfo.HitEntity is MyCubeGrid || hitInfo.HitEntity is MyCubeBlock);
                if (m_definition.IgnoreBlocks && isBlock)
                    continue;

                // Ignore everything else if desired
                if (m_definition.IgnoreOther && !isVoxel && !isBlock)
                    continue;

                // We hit something not a player, cannot see sky!
                return false;
            }

            // Nothing was hit, sky can be seen
            return true;
        }

        /// <summary>
        /// Tries to destroy the block, returns true when destroyed, false otherwise
        /// </summary>
        private bool TryDestroyBlock(Vector3D position, Vector3 up)
        {
            // Cast Entity to block
            MyCubeBlock block = Entity as MyCubeBlock;
            if (block == null)
                return false;

            // If the block is a compound block, destroy the block within the compound parent
            // Otherwise, just straight-up delete the block
            if (block.Parent is MyCompoundCubeBlock)
            {
                var compound = block.Parent as MyCompoundCubeBlock;
                var blockId = compound.GetBlockId(block.SlimBlock);

                if (blockId.HasValue)
                {
                    List<MyTuple<Vector3I, ushort>> toRaze = new List<MyTuple<Vector3I, ushort>>();
                    toRaze.Add(new MyTuple<Vector3I, ushort>(block.Position, blockId.Value));
                    block.CubeGrid.RazeBlockInCompoundBlock(toRaze);
                    return true;
                }
            }
            else
            {
                block.CubeGrid.RazeBlock(block.Position);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Plays a nice particle effect simulating the destruction of the block
        /// </summary>
        private void PlayParticleEffect(Vector3D position, Vector3 up)
        {
            MyParticleEffect effect;
            if (MyParticlesManager.TryCreateParticleEffect(m_definition.DestructionEffect.String, out effect))
            {
                effect.WorldMatrix = MatrixD.CreateWorld(position, up, Vector3.CalculatePerpendicularVector(-up));
            }
        }

        [Event, Reliable, Server, Broadcast]
        private void NotifyOwner(Vector3D position, long ownerId, WarningMessage message)
        {
            // No need to display notifications on a DS
            if (MyAPIGateway.Utilities.IsDedicated)
                return;

            // Only display the message for the owner, not everyone
            if (MyAPIGateway.Session.LocalHumanPlayer.IdentityId != ownerId)
                return;

            // Get nearest planet and PlanetAreasComponent
            MyPlanet planet = MyGamePruningStructure.GetClosestPlanet(position);
            MyPlanetAreasComponent areasComp = planet.Components.Get<MyPlanetAreasComponent>();

            // Compute area id
            Vector3D planetPosition = planet.PositionComp.GetPosition();
            long areaId = areasComp.GetArea(position - planetPosition);

            // Compute area text
            string kingdomName, regionName, areaName;
            MyPlanetAreasComponent.UnpackAreaId(areaId, out kingdomName, out regionName, out areaName);
            string locationString = kingdomName + ", " + regionName + ", " + areaName;

            string messageText = string.Empty;
            int messageDuration = m_definition.NotifactionDurationMS;
            Color messageColor = Color.Yellow;

            switch (message)
            {
                case WarningMessage.IsBlocked:
                    messageText = MyTexts.GetString(m_definition.Notification_SkyBlocked);
                    break;
                case WarningMessage.IsFreed:
                    messageText = MyTexts.GetString(m_definition.Notification_SkyUnblocked);
                    break;
                case WarningMessage.IsBlockedFinalWarning:
                    messageText = MyTexts.GetString(m_definition.Notification_SkyBlockedFinalWarning);
                    break;
                case WarningMessage.IsDestroyed:
                    messageText = MyTexts.GetString(m_definition.Notification_Destroyed);
                    messageColor = Color.Red;
                    messageDuration = m_definition.NotifactionDurationLongMS;
                    break;
            }

            messageText = string.Format(messageText, locationString);
            ((IMyUtilities)MyAPIUtilities.Static).ShowNotification(messageText, messageDuration, null, messageColor);
        }
    }
}
