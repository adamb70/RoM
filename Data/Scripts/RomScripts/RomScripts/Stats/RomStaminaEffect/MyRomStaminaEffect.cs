using Medieval.Definitions.Stats;
using Medieval.ObjectBuilders.Components.Stats;
using Sandbox.Definitions.Components.Entity.Stats.Effects;
using Sandbox.Game.Entities.Entity.Stats;
using Sandbox.Game.Entities.Entity.Stats.Effects;
using Sandbox.Game.Entities.Entity.Stats.Extensions;
using Sandbox.Game.GUI;
using Sandbox.Game.Multiplayer;
using System;
using VRage.Audio;
using VRage.Game;
using VRage.Library.Logging;
using VRage.Utils;
using Sandbox.ModAPI;
using VRage.Game.ModAPI;
using VRageMath;
using VRage.Game.Components;
using VRage.Session;
using RomScripts.StatExtensions;


namespace RomScripts.RomStaminaEffect
{
    [MyEntityEffect(typeof(MyObjectBuilder_RomStaminaEffect))]
    public class MyRomStaminaEffect : MyEntityStatEffect
    {
        private MyEntityStat m_staminaStat;

        private MyPositionComponentBase m_positionComp;

        private Vector3D m_lastMovePosition = Vector3D.Zero;

        private System.TimeSpan m_lastMovedTime = default(System.TimeSpan);

        public override void Activate(MyEntityStatComponent owner)
        {
            base.Activate(owner);
            if (!MyAPIGateway.Multiplayer?.IsServer ?? false)
            {
                return;
            }
            MyRomStaminaEffectDefinition myStaminaEffectDefinition = base.Definition as MyRomStaminaEffectDefinition;
            if (!owner.TryGetStat(myStaminaEffectDefinition.Stat, out this.m_staminaStat))
            {
                this.m_staminaStat = owner.GetRomStamina();
            }
            if (this.m_staminaStat == null)
            {
                MyLog.Default.Error("Stamina effect '{0}' applied to an entity '{1}' without source stat!", new object[]
                {
                    base.Definition.Id,
                    base.Owner.Entity.Definition.Id
                });
                return;
            }
            this.m_staminaStat.OnValueChanged += new MyEntityStat.ValueChangedDelegate(this.OnStaminaChanged);
            this.m_positionComp = base.Owner.Entity.PositionComp;
            this.m_positionComp.OnPositionChanged += new System.Action<MyPositionComponentBase>(this.OnPositionChanged);
        }

        public override void Deactivate()
        {
            if (this.m_staminaStat != null)
            {
                this.m_staminaStat.OnValueChanged -= new MyEntityStat.ValueChangedDelegate(this.OnStaminaChanged);
                this.m_staminaStat = null;
            }
            if (this.m_positionComp != null)
            {
                this.m_positionComp.OnPositionChanged -= new System.Action<MyPositionComponentBase>(this.OnPositionChanged);
                this.m_positionComp = null;
            }
            base.Deactivate();
        }

        public override void Tick(float timeSinceUpdate)
        {
            MyRomStaminaEffectDefinition myRomStaminaEffectDefinition = base.Definition as MyRomStaminaEffectDefinition;
            System.TimeSpan t = MySession.Static.ElapsedGameTime - this.m_lastMovedTime;
            if (t > myRomStaminaEffectDefinition.RestingTime)
            {
                if (!base.Owner.HasEffect(myRomStaminaEffectDefinition.RestingEffect))
                {
                    base.Owner.AddEffect(myRomStaminaEffectDefinition.RestingEffect, 0L);
                }
            }
            else if (base.Owner.HasEffect(myRomStaminaEffectDefinition.RestingEffect))
            {
                base.Owner.RemoveEffect(myRomStaminaEffectDefinition.RestingEffect);
            }
            float c = this.m_staminaStat;
            base.Tick(timeSinceUpdate);
            float num = this.m_staminaStat - c;
            if (num > 0f)
            {
                MyEntityStat food = base.Owner.GetFood();
                if (food != null)
                {
                    food.Current -= num * myRomStaminaEffectDefinition.StaminaToFoodRatio;
                }

                MyEntityStat thirst = base.Owner.GetThirst();
                if (thirst != null)
                {
                    thirst.Current -= num * myRomStaminaEffectDefinition.StaminaToThirstRatio;
                }
            }
        }

        private void OnPositionChanged(MyPositionComponentBase obj)
        {
            if (MySession.Static != null)
            {
                Vector3D position = obj.GetPosition();
                if ((position - this.m_lastMovePosition).LengthSquared() > 0.25)
                {
                    this.m_lastMovedTime = MySession.Static.ElapsedGameTime;
                    this.m_lastMovePosition = position;
                }
            }
        }

        private void OnStaminaChanged(MyEntityStat stat, float oldValue, float newValue)
        {
            if (newValue < oldValue && MySession.Static != null)
            {
                this.m_lastMovedTime = MySession.Static.ElapsedGameTime;
            }
            if (newValue <= 0f && oldValue > 0f)
            {
                MyRomStaminaEffectDefinition myStaminaEffectDefinition = base.Definition as MyRomStaminaEffectDefinition;
                base.Owner.AddEffect(myStaminaEffectDefinition.ExhaustionEffect, 0L);
            }
        }
    }
}
