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
using Sandbox.Game.EntityComponents;
using Medieval.Entities.Components;
using Medieval.Entities.Components.Crafting;
using Medieval.MechanicalPower;

using VRage.Game.Entity;


namespace RomScripts76561197972467544.FueledMechanicalSource
{
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
            
            if (fuel_components != null)
            {
                this.m_powerProviders = fuel_components.ToList<IMyPowerProvider>(); // ignore this IDE error
                foreach (IMyPowerProvider component in fuel_components)
                {
                    component.PowerStateChanged += new Action<IMyPowerProvider, bool>(this.Power_OnPowerStateChanged);
                }
            }

            if (this.Group != null)
            {
                this.Group.Recalculate();
            }
            
            //((IMyUtilities)MyAPIUtilities.Static).ShowNotification("Power is set!", 1000, null, Color.Red);
        }


        private void Power_OnPowerStateChanged(IMyPowerProvider provider, bool state)
        {
            if (!MyAPIGateway.Multiplayer?.IsServer ?? false)
            {
                return;
            }

            //((IMyUtilities)MyAPIUtilities.Static).ShowNotification("Power state changed!", 3000, null, Color.Red);
            TryTurnOnPower();
            //base.Power = provider.IsPowered ? this.m_definition.MaxPowerOutput : 0;
            if (this.Group != null)
            {
                this.Group.Recalculate();
            }
        }

        private void TryTurnOnPower()
        {
            if (this.m_powerProviders.All((IMyPowerProvider e) => e.IsPowered))
            {
                // If all providers have power, turn on.
                base.Power = this.m_definition.MaxPowerOutput;
            } else
            {
                base.Power = 0;
            }
        }

    }
}
