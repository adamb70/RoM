using Medieval.Inventory;
using ObjectBuilders.Definitions.Tools;
using Sandbox.Definitions.Equipment;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.EntityComponents.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.Inventory;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Components;
using VRage.Game;
using VRage.Game.Entity;
using VRage.GUI.Crosshair;
using VRage.Library.Logging;
using VRage.Systems;
using VRage.Utils;
using VRageMath;
using Medieval.GameSystems.Tools;
using Medieval.GameSystems;

using Sandbox.Definitions.Inventory;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI;
using VRage.Game.ModAPI;
using VRage.Library.Collections;
using VRage.ObjectBuilders.Inventory;

using Medieval.Definitions.GameSystems;
using Medieval.Definitions.Tools;
using Medieval.WorldEnvironment.Modules;
using ObjectBuilders.GameSystems;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.WorldEnvironment;
using VRage.Definitions;
using VRage.Game.Components;
using VRage.Game.Components.Session;
using VRage.Game.Voxels;
using VRage.Network;
using VRage.Voxels;
using Sandbox.Game.SessionComponents;

using Sandbox.Game.Entities;

namespace Romscripts.SeedingToolBehavior
{
    /// <summary>
    /// Seeding behavior for farming.
    /// </summary>
    [MyHandItemBehavior(typeof(MyObjectBuilder_RomSeedingToolBehaviorDefinition), true)]
    public class MyRomSeedingToolBehavior : MySeedingToolBehavior
    {
        public List<MyRomSeedingToolBehaviorDefinition.Limit> AltitudeLimits;
        public List<MyRomSeedingToolBehaviorDefinition.Limit> LatitudeLimits;
        public List<MyRomSeedingToolBehaviorDefinition.Limit> LongitudeLimits;

        private MySeedBagHandItem m_seedBag;
        private MyFarmingSystem m_farmingSystem;

        public bool Debug;

        public override void Init(MyEntity holder, MyHandItem item, MyHandItemBehaviorDefinition definition)
        {
            this.m_seedBag = (item as MySeedBagHandItem);
            if (this.m_seedBag == null)
            {
                MyLog.Default.Error("Hand item have to be of type 'MySeedBagHandItem' in order to work with seeding tool behavior");
            }
            base.Init(holder, item, definition);


            var ob = (MyRomSeedingToolBehaviorDefinition)definition;
            this.AltitudeLimits = ob.AltitudeLimits;
            this.LatitudeLimits = ob.LatitudeLimits;
            this.LongitudeLimits = ob.LongitudeLimits;

            this.m_farmingSystem = VRage.Session.MySession.Static.Components.Get<MyFarmingSystem>();
            this.Debug = ob.Debug;
        }


        protected override bool Start(MyHandItemActionEnum action)
        {
            if (action != MyHandItemActionEnum.Primary)
            {
                return false;
            }
            
            if (base.Start(action) == true)
            {
                return this.IsPlantable(this.Target.Position, this.Target.Normal, null);
            }
            return false;
        }


        public bool IsPlantable(Vector3D position, Vector3 normal, MyPlanet planet = null)
        {
            if (planet == null)
            {
                planet = MyGamePruningStructure.GetClosestPlanet(position);
                if (planet == null)
                {
                    return false;
                }
            }

            if (this.AltitudeLimits.Count > 0 || this.LatitudeLimits.Count > 0 || this.LongitudeLimits.Count > 0)
            {
                MySectorWeatherComponent weather = VRage.Session.MySession.Static.Components.Get<MySectorWeatherComponent>();
                SolarObservation obs = weather.CreateSolarObservation(VRage.Session.MySession.Static.ElapsedGameTime, position);

                if (Debug)
                {
                    ((IMyUtilities)MyAPIUtilities.Static).ShowNotification(obs.ToString(), 3000, null, Color.Azure);
                    ((IMyUtilities)MyAPIUtilities.Static).ShowNotification("Biome: " + obs.Biome.ToString(), 3000, null, Color.Gold);
                    ((IMyUtilities)MyAPIUtilities.Static).ShowNotification("Altitude: " + obs.Altitude.ToString(), 3000, null, Color.Gold);
                    ((IMyUtilities)MyAPIUtilities.Static).ShowNotification("Lat: " + obs.Latitude.ToString(), 3000, null, Color.Gold);
                    ((IMyUtilities)MyAPIUtilities.Static).ShowNotification("Long: " + obs.Longitude.ToString(), 3000, null, Color.Gold);
                    //((IMyUtilities)MyAPIUtilities.Static).ShowNotification("Dist to Surface: " + obs.DistanceToSurface.ToString(), 3000, null, Color.Gold);
                    //((IMyUtilities)MyAPIUtilities.Static).ShowNotification("Solar Elev: " + obs.SolarElevation.ToString(), 3000, null, Color.Gold);
                    //((IMyUtilities)MyAPIUtilities.Static).ShowNotification("Solar Alt: " + obs.SolarAltitude.ToString(), 3000, null, Color.Gold);
                }

                if (this.AltitudeLimits.Count != 0)
                {
                    int flag = 0;
                    foreach (MyRomSeedingToolBehaviorDefinition.Limit lim in this.AltitudeLimits)
                    {
                        if (obs.Altitude >= lim.Lower && obs.Altitude <= lim.Upper)
                        {
                            flag += 1;
                            break;
                        }
                    }
                    if (flag == 0)
                    {
                        ((IMyUtilities)MyAPIUtilities.Static).ShowNotification("Cannot be planted at this altitude!", 2000, null, Color.Red);
                        return false;
                    }
                }
                
                if (this.LatitudeLimits.Count != 0)
                {
                    int flag = 0;
                    foreach (MyRomSeedingToolBehaviorDefinition.Limit lim in this.LatitudeLimits)
                    {
                        if (obs.Latitude >= lim.Lower && obs.Latitude <= lim.Upper)
                        {
                            flag += 1;
                            break;
                        }
                    }
                    if (flag == 0)
                    {
                        ((IMyUtilities)MyAPIUtilities.Static).ShowNotification("Cannot be planted at this latitude!", 2000, null, Color.Red);
                        return false;
                    }
                }

                if (this.LongitudeLimits.Count != 0)
                {
                    int flag = 0;
                    foreach (MyRomSeedingToolBehaviorDefinition.Limit lim in this.LongitudeLimits)
                    {
                        if (obs.Longitude >= lim.Lower && obs.Longitude <= lim.Upper)
                        {
                            flag += 1;
                            break;
                        }
                    }
                    if (flag == 0)
                    {
                        ((IMyUtilities)MyAPIUtilities.Static).ShowNotification("Cannot be planted at this longitude!", 2000, null, Color.Red);
                        return false;
                    }
                }
                
            }
            
            return true;
        }


        protected override void Hit()
        {
            if (this.m_farmingSystem.Plant(this.Holder, this.Target.Position, this.Target.Normal, this.m_seedBag.GetDefinition().GrowableDefinitionId, this.m_seedBag.GetDefinition().Id) == MyFarmingSystem.PlantingCheckResult.PlantingOk)
            {
                this.UpdateDurability(-1);
            }
        }

    }
}
