using Medieval.ObjectBuilders.Definitions.Stats;
using Sandbox.Definitions.Components.Entity.Stats.Effects;
using System;
using VRage.Game;
using VRage.Game.Definitions;
using System.Collections.Generic;
using VRage.Audio;
using VRage.ObjectBuilders;

namespace RomScripts76561197972467544.RomDeathEffect
{
    [MyDefinitionType(typeof(MyObjectBuilder_RomDeathEffectDefinition))]
    public class MyRomDeathEffectDefinition : MyEntityStatEffectDefinition
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

        private System.Collections.Generic.List<MyRomDeathEffectDefinition.Trigger> m_triggers = new System.Collections.Generic.List<MyRomDeathEffectDefinition.Trigger>();

        public IReadOnlyList<MyRomDeathEffectDefinition.Trigger> Triggers
        {
            get
            {
                return this.m_triggers;
            }
        }

        protected override void Init(MyObjectBuilder_DefinitionBase builder)
        {
            base.Init(builder);
            MyObjectBuilder_RomDeathEffectDefinition myObjectBuilder_RomDeathEffectDefinition = builder as MyObjectBuilder_RomDeathEffectDefinition;
            this.m_triggers.Clear();
            
            if (myObjectBuilder_RomDeathEffectDefinition.Triggers != null)
            {
                MyObjectBuilder_RomDeathEffectDefinition.Trigger[] triggers = myObjectBuilder_RomDeathEffectDefinition.Triggers;
                for (int i = 0; i < triggers.Length; i++)
                {
                    MyObjectBuilder_RomDeathEffectDefinition.Trigger ob_trigger = triggers[i];
                    MyRomDeathEffectDefinition.Trigger new_trigger = new MyRomDeathEffectDefinition.Trigger();
                    new_trigger.Threshold = ob_trigger.Threshold;
                    //new_trigger.CueId = new MyCueId(trigger_ob.CueId);
                    SerializableDefinitionId? effect = ob_trigger.Effect;
                    new_trigger.Effect = (effect.HasValue ? new MyDefinitionId?(effect.GetValueOrDefault()) : null);
                    this.m_triggers.Add(new_trigger);
                }
            }
            this.m_triggers.Sort((MyRomDeathEffectDefinition.Trigger x, MyRomDeathEffectDefinition.Trigger y) => x.Threshold.CompareTo(y.Threshold));
        }

    }
    
}
