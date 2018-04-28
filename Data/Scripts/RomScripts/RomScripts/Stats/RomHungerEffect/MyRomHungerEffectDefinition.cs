using Medieval.ObjectBuilders.Definitions.Stats;
using Sandbox.Definitions.Components.Entity.Stats.Effects;
using System;
using VRage.Game;
using VRage.Game.Definitions;
using System.Collections.Generic;
using VRage.Audio;
using VRage.ObjectBuilders;

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

        public class Trigger
        {
            /// <summary>
            /// At which value does this trigger fire?
            /// </summary>
            public int Threshold;

            /// <summary>
            /// Is there a matching sound effect?
            /// </summary>
            //public MyCueId CueId;

            /// <summary>
            /// Is there a matching effect?
            /// </summary>
            public MyDefinitionId? Effect;
        }

        private System.Collections.Generic.List<MyRomHungerEffectDefinition.Trigger> m_triggers = new System.Collections.Generic.List<MyRomHungerEffectDefinition.Trigger>();

        public IReadOnlyList<MyRomHungerEffectDefinition.Trigger> Triggers
        {
            get
            {
                return this.m_triggers;
            }
        }

        protected override void Init(MyObjectBuilder_DefinitionBase builder)
        {
            base.Init(builder);
            MyObjectBuilder_RomHungerEffectDefinition myObjectBuilder_RomHungerEffectDefinition = builder as MyObjectBuilder_RomHungerEffectDefinition;
            this.m_triggers.Clear();
            
            if (myObjectBuilder_RomHungerEffectDefinition.Triggers != null)
            {
                MyObjectBuilder_RomHungerEffectDefinition.Trigger[] triggers = myObjectBuilder_RomHungerEffectDefinition.Triggers;
                for (int i = 0; i < triggers.Length; i++)
                {
                    MyObjectBuilder_RomHungerEffectDefinition.Trigger ob_trigger = triggers[i];
                    MyRomHungerEffectDefinition.Trigger new_trigger = new MyRomHungerEffectDefinition.Trigger();
                    new_trigger.Threshold = ob_trigger.Threshold;
                    //new_trigger.CueId = new MyCueId(trigger_ob.CueId);
                    SerializableDefinitionId? effect = ob_trigger.Effect;
                    new_trigger.Effect = (effect.HasValue ? new MyDefinitionId?(effect.GetValueOrDefault()) : null);
                    this.m_triggers.Add(new_trigger);
                }
            }
            this.m_triggers.Sort((MyRomHungerEffectDefinition.Trigger x, MyRomHungerEffectDefinition.Trigger y) => x.Threshold.CompareTo(y.Threshold));
        }

    }
    
}
