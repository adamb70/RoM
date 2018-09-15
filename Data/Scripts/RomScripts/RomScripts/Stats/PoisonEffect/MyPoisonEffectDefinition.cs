using Medieval.ObjectBuilders.Definitions.Stats;
using Sandbox.Definitions.Components.Entity.Stats.Effects;
using System;
using VRage.Game;
using VRage.Game.Definitions;
using System.Collections.Generic;
using VRage.Audio;
using VRage.ObjectBuilders;

namespace RomScripts.PoisonEffect
{
    [MyDefinitionType(typeof(MyObjectBuilder_PoisonEffectDefinition))]
    public class MyPoisonEffectDefinition : MyEntityStatEffectDefinition
    {

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

        private System.Collections.Generic.List<MyPoisonEffectDefinition.Trigger> m_triggers = new System.Collections.Generic.List<MyPoisonEffectDefinition.Trigger>();

        public IReadOnlyList<MyPoisonEffectDefinition.Trigger> Triggers
        {
            get
            {
                return this.m_triggers;
            }
        }

        protected override void Init(MyObjectBuilder_DefinitionBase builder)
        {
            base.Init(builder);
            MyObjectBuilder_PoisonEffectDefinition myObjectBuilder_PoisonEffectDefinition = builder as MyObjectBuilder_PoisonEffectDefinition;
            this.m_triggers.Clear();

            if (myObjectBuilder_PoisonEffectDefinition.Triggers != null)
            {
                MyObjectBuilder_PoisonEffectDefinition.Trigger[] triggers = myObjectBuilder_PoisonEffectDefinition.Triggers;
                for (int i = 0; i < triggers.Length; i++)
                {
                    MyObjectBuilder_PoisonEffectDefinition.Trigger ob_trigger = triggers[i];
                    MyPoisonEffectDefinition.Trigger new_trigger = new MyPoisonEffectDefinition.Trigger();
                    new_trigger.Threshold = ob_trigger.Threshold;
                    //new_trigger.CueId = new MyCueId(trigger_ob.CueId);
                    SerializableDefinitionId? effect = ob_trigger.Effect;
                    new_trigger.Effect = (effect.HasValue ? new MyDefinitionId?(effect.GetValueOrDefault()) : null);
                    this.m_triggers.Add(new_trigger);
                }
            }
            this.m_triggers.Sort((MyPoisonEffectDefinition.Trigger x, MyPoisonEffectDefinition.Trigger y) => x.Threshold.CompareTo(y.Threshold));
        }
    }
}
