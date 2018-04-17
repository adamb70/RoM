using Medieval.ObjectBuilders.Definitions.Stats;
using Sandbox.Definitions.Components.Entity.Stats.Effects;
using System;
using VRage.Game;
using VRage.Game.Definitions;

namespace RomScripts.RomHungerEffect
{
    [MyDefinitionType(typeof(MyObjectBuilder_RomHungerEffectDefinition))]
    public class MyRomHungerEffectDefinition : MyEntityStatEffectDefinition
    {
        /// <summary>
        /// Effect that is activated when hunger level gets over 30.
        /// Not applied if left blank.
        /// </summary>
        public MyDefinitionId? WellFedEffect
        {
            get;
            private set;
        }

        /// <summary>
        /// Effect that is activated when hunger level gets at or under 30.
        /// If blank, will try to maintain WellFedEffect instead.
        /// </summary>
        public MyDefinitionId? HungryEffect
        {
            get;
            private set;
        }

        /// <summary>
        /// Effect that is activated when hunger level to 0.
        /// If blank, will first try to maintain VeryHungryEffect, if not possible, try to maintain HungryEffect.
        /// </summary>
        public MyDefinitionId? StarvationEffect
        {
            get;
            private set;
        }

        public MyDefinitionId? OverstuffedEffect
        {
            get;
            private set;
        }

        protected override void Init(MyObjectBuilder_DefinitionBase builder)
        {
            base.Init(builder);
            MyObjectBuilder_RomHungerEffectDefinition myObjectBuilder_RomHungerEffectDefinition = builder as MyObjectBuilder_RomHungerEffectDefinition;
            this.WellFedEffect = null;
            if (myObjectBuilder_RomHungerEffectDefinition.WellFedEffect.HasValue)
            {
                this.WellFedEffect = new MyDefinitionId?(myObjectBuilder_RomHungerEffectDefinition.WellFedEffect.Value);
            }
            this.HungryEffect = null;
            if (myObjectBuilder_RomHungerEffectDefinition.HungryEffect.HasValue)
            {
                this.HungryEffect = new MyDefinitionId?(myObjectBuilder_RomHungerEffectDefinition.HungryEffect.Value);
            }
            this.StarvationEffect = null;
            if (myObjectBuilder_RomHungerEffectDefinition.StarvationEffect.HasValue)
            {
                this.StarvationEffect = new MyDefinitionId?(myObjectBuilder_RomHungerEffectDefinition.StarvationEffect.Value);
            }
            this.OverstuffedEffect = null;
            if (myObjectBuilder_RomHungerEffectDefinition.OverstuffedEffect.HasValue)
            {
                this.OverstuffedEffect = new MyDefinitionId?(myObjectBuilder_RomHungerEffectDefinition.OverstuffedEffect.Value);
            }
        }
    }
}
