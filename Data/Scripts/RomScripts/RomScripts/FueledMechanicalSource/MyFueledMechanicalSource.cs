using Medieval.Entities.UseObject;
using Medieval.GameSystems;
using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Replication;
using Sandbox.ModAPI;
using System.Collections.Generic;
using VRage;
using VRage.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Network;
using VRage.Systems;
using VRageMath;
using System;
using System.Linq;
using VRage.Game.Entity.UseObject;
using VRage.ModAPI;
using Sandbox.Game.Components;
using VRage.Utils;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.EntityComponents;
using Medieval.Entities.Components;
using Medieval.Entities.Components.Crafting;
using Medieval.MechanicalPower;

using VRage.Game.Entity;


namespace RomScripts.FueledMechanicalSource
{
    [ReplicatedComponent]
    [MyComponent(typeof(MyObjectBuilder_FueledMechanicalSourceComponent))]
    public class MyFueledMechanicalSourceComponent : MyMechanicalSourceComponent
    {
        private MyFueledMechanicalSourceComponentDefinition m_definition;
        protected List<IMyPowerProvider> m_powerProviders;
        protected bool m_isPowered;

        public override void Init(MyEntityComponentDefinition definition)
        {
            base.Init(definition);

            this.m_definition = definition as MyFueledMechanicalSourceComponentDefinition;
            base.Power = 0;
        }

        public override void OnAddedToScene()
        {
            base.OnAddedToScene();

            // Look for power providers
            IEnumerable<IMyPowerProvider> fuel_components = this.Entity.Components.GetComponents<IMyPowerProvider>();

            int test2 = 0;
            foreach (IMyPowerProvider component in fuel_components)
            {
                test2 += 1;
            }


            ((IMyUtilities)MyAPIUtilities.Static).ShowNotification(test2.ToString(), 3000, null, Color.Red);


            if (fuel_components != null)
            {
                this.m_powerProviders = fuel_components.ToList<IMyPowerProvider>(); // ignore this IDE error
                foreach (IMyPowerProvider component in fuel_components)
                {
                    component.PowerStateChanged += new Action<IMyPowerProvider, bool>(this.Power_OnPowerStateChanged);
                }
            }
            
            //((IMyUtilities)MyAPIUtilities.Static).ShowNotification("Power is set!", 1000, null, Color.Red);
        }


        private void Power_OnPowerStateChanged(IMyPowerProvider provider, bool state)
        {
            ((IMyUtilities)MyAPIUtilities.Static).ShowNotification("Power state changed!", 3000, null, Color.Red);

            TryTurnOnPower();
            //base.Power = provider.IsPowered ? this.m_definition.MaxPowerOutput : 0;
            this.Group.Recalculate();
        }

        private void TryTurnOnPower()
        {

            this.m_powerProviders.All((IMyPowerProvider e) => e.IsPowered);

            foreach (IMyPowerProvider provider in m_powerProviders)
            {
                ((IMyUtilities)MyAPIUtilities.Static).ShowNotification("new", 3000, null, Color.Red);
                ((IMyUtilities)MyAPIUtilities.Static).ShowNotification(provider.IsPowered.ToString(), 3000, null, Color.Red);
            }

        }

        // Vanilla altitude calculation
        //private void CalculatePowerOutput()
        //{
        //    int power = base.Power;
        //    base.Power = 0;
        //    if (this.m_definition == null)
        //    {
        //        return;
        //    }
        //    if (this.m_definition.MaxAltitudeDelta <= 0f)
        //    {
        //        base.Power = this.m_definition.MaxPowerOutput;
        //        if (power != base.Power && this.Group != null)
        //        {
        //            this.Group.Recalculate();
        //        }
        //        return;
        //    }
        //    Vector3D translation = this.Block.WorldMatrix.Translation;
        //    MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(translation);
        //    if (closestPlanet == null)
        //    {
        //        if (power != base.Power && this.Group != null)
        //        {
        //            this.Group.Recalculate();
        //        }
        //        return;
        //    }
        //    double num = (closestPlanet.GetClosestSurfacePointGlobal(ref translation) - closestPlanet.WorldMatrix.Translation).LengthSquared();
        //    double num2 = (translation - closestPlanet.WorldMatrix.Translation).LengthSquared();
        //    if (num >= num2)
        //    {
        //        if (power != base.Power && this.Group != null)
        //        {
        //            this.Group.Recalculate();
        //        }
        //        return;
        //    }
        //    float time = (float)(translation - closestPlanet.GetClosestSurfacePointGlobal(ref translation) - closestPlanet.WorldMatrix.Translation).Length();
        //    float num3 = global::System.Math.Min(1f, InterpolationEquationsF.Interpolate(this.m_definition.PowerInterpolation, time, 0f, 1f, this.m_definition.MaxAltitudeDelta, null));
        //    base.Power = (int)global::System.Math.Floor((double)((float)this.m_definition.MaxPowerOutput * num3));
        //    if (power != base.Power && this.Group != null)
        //    {
        //        this.Group.Recalculate();
        //    }
        //}
    }
}
