using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.EntityComponents.Character;
using System;
using System.Collections.Generic;
using VRage.Components.Interfaces;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Utils;
using VRage.Library.Logging;

using VRage.Game.ModAPI;
using Sandbox.ModAPI;
using VRageMath;

namespace RomScripts
{
    [MyStatLogicDescriptor("RomStatEffect")]
    public class MyRomStatEffect : RomStatLogic
    {
        public static MyStringHash HealthId = MyStringHash.GetOrCompute("Health");

        public static MyStringHash StaminaId = MyStringHash.GetOrCompute("Stamina");

        public static MyStringHash FoodId = MyStringHash.GetOrCompute("Food");

        private int m_healthEffectId;

        private int m_staminaMovementEffectId;

        private int m_staminaExternalEffectId;

        private int m_foodEffectId;

        private MyEntityStat Health
        {
            get
            {
                MyEntityStat result;
                if (this.m_stats.TryGetValue(MyRomStatEffect.HealthId, out result))
                {
                    return result;
                }
                return null;
                
            }
        }

        private MyEntityStat Stamina
        {
            get
            {
                MyEntityStat result;
                if (this.m_stats.TryGetValue(MyRomStatEffect.StaminaId, out result))
                {
                    return result;
                }
                return null;
            }
        }

        private MyEntityStat Food
        {
            get
            {
                MyEntityStat result;
                if (this.m_stats.TryGetValue(MyRomStatEffect.FoodId, out result))
                {
                    return result;
                }
                return null;
            }
        }

        public override void Init(MyEntity character, System.Collections.Generic.Dictionary<MyStringHash, MyEntityStat> stats, string scriptName)
        {
            base.Init(character, stats, scriptName);
            this.InitPermanentEffects();
            this.InitActions();
            this.InitModifiers();
            this.InitEfficiency();
            MyEntityStat health = this.Health;
            if (health != null)
            {
                health.OnStatChanged += new MyEntityStat.StatChangedDelegate(this.OnHealthChanged);
            }
        }

        public override void Close()
        {
            MyEntityStat health = this.Health;
            if (health != null)
            {
                health.OnStatChanged -= new MyEntityStat.StatChangedDelegate(this.OnHealthChanged);
            }
            MyCharacterMovementComponent myCharacterMovementComponent = (base.Entity != null) ? base.Entity.Components.Get<MyCharacterMovementComponent>() : null;
            if (myCharacterMovementComponent != null)
            {
                myCharacterMovementComponent.OnMovementStateChanged -= new MyCharacterMovementComponent.CharacterMovementStateDelegate(this.OnMovementChanged);
            }
            this.ClearPermanentEffects();
            base.Close();
        }

        protected override void OnCharacterChanged(MyEntity oldCharacter)
        {
            base.OnCharacterChanged(oldCharacter);
            MyCharacterMovementComponent myCharacterMovementComponent = (oldCharacter != null) ? oldCharacter.Components.Get<MyCharacterMovementComponent>() : null;
            if (myCharacterMovementComponent != null)
            {
                myCharacterMovementComponent.OnMovementStateChanged -= new MyCharacterMovementComponent.CharacterMovementStateDelegate(this.OnMovementChanged);
            }
            MyCharacterMovementComponent myCharacterMovementComponent2 = (base.Entity != null) ? base.Entity.Components.Get<MyCharacterMovementComponent>() : null;
            if (myCharacterMovementComponent2 != null)
            {
                myCharacterMovementComponent2.OnMovementStateChanged += new MyCharacterMovementComponent.CharacterMovementStateDelegate(this.OnMovementChanged);
            }
        }

        private void OnHealthChanged(float newValue, float oldValue, object statChangeData)
        {
            ((IMyUtilities)MyAPIUtilities.Static).ShowNotification("health changed", 900, null, Color.Aqua);
            MyEntityStat health = this.Health;
            if (health != null && health.Value - health.MinValue < 0.001f && base.Entity != null)
            {
                IMyDamageReceiver myDamageReceiver = base.Entity.Components.Get<IMyDamageReceiver>();
                if (myDamageReceiver != null && statChangeData != null)
                {
                    myDamageReceiver.Kill(((RomDamageInformation)statChangeData).Type, true, base.Entity.EntityId);
                }
            }
        }

        public override void Update10()
        {
            ((IMyUtilities)MyAPIUtilities.Static).ShowNotification("update", 3000, null, Color.Red);
            MyEntityStat health = this.Health;
            MyEntityStat food = this.Food;
            MyEntityStat stamina = this.Stamina;
            if (food != null)
            {
                MyEntityStatRegenEffect myEntityStatRegenEffect = null;
                MyEntityStatRegenEffect myEntityStatRegenEffect2 = null;
                MyEntityStatRegenEffect myEntityStatRegenEffect3 = null;
                MyEntityStatRegenEffect myEntityStatRegenEffect4 = null;
                if (stamina != null)
                {
                    stamina.TryGetEffect(this.m_staminaMovementEffectId, out myEntityStatRegenEffect);
                }
                if (stamina != null)
                {
                    stamina.TryGetEffect(this.m_staminaExternalEffectId, out myEntityStatRegenEffect2);
                }
                if (food != null)
                {
                    food.TryGetEffect(this.m_foodEffectId, out myEntityStatRegenEffect3);
                }
                if (health != null)
                {
                    health.TryGetEffect(this.m_healthEffectId, out myEntityStatRegenEffect4);
                }
                bool flag = myEntityStatRegenEffect != null && myEntityStatRegenEffect.Amount > 0f;
                bool flag2 = food != null && food.CurrentRatio < 0.5f;
                bool flag3 = food != null && food.Value <= food.MinValue + 0.0001f;
                bool flag4 = stamina != null && food != null && stamina.CurrentRatio < 0.15f && food.CurrentRatio > 0.7f;
                myEntityStatRegenEffect2.Enabled = false;
                myEntityStatRegenEffect.Enabled = true;
                if (flag && (flag2 || flag4))
                {
                    myEntityStatRegenEffect2.Enabled = true;
                    myEntityStatRegenEffect.Enabled = false;
                    float interval = flag2 ? RomEffectConstants.StaminaIntervalLowFood : RomEffectConstants.StaminaIntervalDuringSecondBreath;
                    float amount = myEntityStatRegenEffect.Amount;
                    myEntityStatRegenEffect2.SetAmountAndInterval(amount, interval, true);
                }
                else
                {
                    myEntityStatRegenEffect2.ResetRegenTime();
                }
                if (myEntityStatRegenEffect3 != null)
                {
                    if (flag && flag4)
                    {
                        myEntityStatRegenEffect3.Interval = RomEffectConstants.FoodIntervalSecondBreath;
                    }
                    else
                    {
                        myEntityStatRegenEffect3.Interval = RomEffectConstants.FoodInterval;
                    }
                }
                if (myEntityStatRegenEffect4 != null)
                {
                    if (flag3)
                    {
                        myEntityStatRegenEffect4.Amount = RomEffectConstants.HealthTickOutOfFood;
                        myEntityStatRegenEffect4.Interval = RomEffectConstants.HealthIntervalOutOfFood;
                        return;
                    }
                    myEntityStatRegenEffect4.Amount = RomEffectConstants.HealthTick;
                    myEntityStatRegenEffect4.Interval = RomEffectConstants.HealthInterval;
                }
            }
        }

        private void OnMovementChanged(MyCharacterMovement oldState, MyCharacterMovement newState)
        {
            if (oldState == newState)
            {
                return;
            }
            MyEntityStat stamina = this.Stamina;
            MyEntityStatRegenEffect myEntityStatRegenEffect;
            if (stamina != null && stamina.TryGetEffect(this.m_staminaMovementEffectId, out myEntityStatRegenEffect))
            {
                float amount = RomEffectConstants.StaminaTick;
                float interval = RomEffectConstants.StaminaInterval;
                if (newState.Modifier.IsMoving())
                {
                    if ((byte)(newState.Modifier & MyCharacterMovementModifier.Sprint) != 0)
                    {
                        amount = RomEffectConstants.StaminaTickSprint;
                        interval = RomEffectConstants.StaminaIntervalRun;
                    }
                    else if ((byte)(newState.Modifier & MyCharacterMovementModifier.Run) != 0)
                    {
                        amount = RomEffectConstants.StaminaTickRun;
                        interval = RomEffectConstants.StaminaIntervalRun;
                    }
                    else if ((byte)(newState.Modifier & MyCharacterMovementModifier.Crouch) != 0)
                    {
                        amount = RomEffectConstants.StaminaTickCrouchWalk;
                        interval = RomEffectConstants.StaminaIntervalCrouchWalk;
                    }
                    if (newState.State == MyCharacterMovementState.Falling || newState.State == MyCharacterMovementState.Jump)
                    {
                        amount = 0f;
                        interval = RomEffectConstants.StaminaIntervalCrouchIdle;
                    }
                }
                else if ((byte)(newState.Modifier & MyCharacterMovementModifier.Crouch) != 0)
                {
                    amount = RomEffectConstants.StaminaTickCrouchIdle;
                    interval = RomEffectConstants.StaminaIntervalCrouchIdle;
                }
                else if (newState.State == MyCharacterMovementState.Falling || newState.State == MyCharacterMovementState.Jump)
                {
                    amount = 0f;
                    interval = RomEffectConstants.StaminaIntervalCrouchIdle;
                }
                myEntityStatRegenEffect.SetAmountAndInterval(amount, interval, true);
            }
        }

        private void InitPermanentEffects()
        {
            MyEntityStat health = this.Health;
            if (health != null)
            {
                this.m_healthEffectId = health.AddEffect(RomEffectConstants.HealthTick, RomEffectConstants.HealthInterval, -1f, 0f, 1f);
            }
            MyEntityStat stamina = this.Stamina;
            if (stamina != null)
            {
                this.m_staminaMovementEffectId = stamina.AddEffect(RomEffectConstants.StaminaTick, RomEffectConstants.StaminaInterval, -1f, 0f, 1f);
                this.m_staminaExternalEffectId = stamina.AddEffect(RomEffectConstants.StaminaTick, RomEffectConstants.StaminaInterval, -1f, 0f, 1f);
            }
            MyEntityStat food = this.Food;
            if (food != null)
            {
                this.m_foodEffectId = food.AddEffect(RomEffectConstants.FoodTick, RomEffectConstants.FoodInterval, -1f, 0f, 1f);
            }
        }

        private void ClearPermanentEffects()
        {
            MyEntityStat health = this.Health;
            if (health != null)
            {
                health.RemoveEffect(this.m_healthEffectId);
            }
            MyEntityStat stamina = this.Stamina;
            if (stamina != null)
            {
                stamina.RemoveEffect(this.m_staminaMovementEffectId);
                stamina.RemoveEffect(this.m_staminaExternalEffectId);
            }
            MyEntityStat food = this.Food;
            if (food != null)
            {
                food.RemoveEffect(this.m_foodEffectId);
            }
        }

        private void InitActions()
        {
            RomStatLogic.MyStatAction action = default(RomStatLogic.MyStatAction);
            string actionId = "Jump";
            action.StatId = MyRomStatEffect.StaminaId;
            action.Cost = RomEffectConstants.StaminaCostJump;
            action.AmountToActivate = 0f;
            action.CanPerformWithout = false;
            base.AddAction(actionId, action);
            actionId = "SwingTool";
            action.StatId = MyRomStatEffect.StaminaId;
            action.Cost = RomEffectConstants.StaminaCostSwingTool;
            action.AmountToActivate = 0f;
            action.CanPerformWithout = true;
            base.AddAction(actionId, action);
            actionId = "GeneralWork";
            action.StatId = MyRomStatEffect.StaminaId;
            action.Cost = RomEffectConstants.StaminaCostWork;
            action.AmountToActivate = 0f;
            action.CanPerformWithout = true;
            base.AddAction(actionId, action);
            actionId = "Block";
            action.StatId = MyRomStatEffect.StaminaId;
            action.Cost = RomEffectConstants.StaminaCostBlock;
            action.AmountToActivate = 0f;
            action.CanPerformWithout = true;
            base.AddAction(actionId, action);
            actionId = "Sprint";
            action.StatId = MyRomStatEffect.StaminaId;
            action.Cost = System.Math.Abs(RomEffectConstants.StaminaTickSprint);
            action.AmountToActivate = 25f;
            action.CanPerformWithout = false;
            base.AddAction(actionId, action);
            actionId = "Run";
            action.StatId = MyRomStatEffect.StaminaId;
            action.Cost = System.Math.Abs(RomEffectConstants.StaminaTickRun);
            action.AmountToActivate = 0f;
            action.CanPerformWithout = false;
            base.AddAction(actionId, action);
        }

        private void InitModifiers()
        {
            RomStatLogic.MyStatRegenModifier modifier = default(RomStatLogic.MyStatRegenModifier);
            string modifierId = "Attack";
            modifier.StatId = MyRomStatEffect.StaminaId;
            modifier.AmountMultiplier = 0.05f;
            modifier.Duration = 5f;
            base.AddModifier(modifierId, modifier);
            modifierId = "Block";
            modifier.StatId = MyRomStatEffect.StaminaId;
            modifier.AmountMultiplier = 0.05f;
            modifier.Duration = 5f;
            base.AddModifier(modifierId, modifier);
            modifierId = "Jump";
            modifier.StatId = MyRomStatEffect.StaminaId;
            modifier.AmountMultiplier = 0.05f;
            modifier.Duration = 5f;
            base.AddModifier(modifierId, modifier);
            modifierId = "Sprint";
            modifier.StatId = MyRomStatEffect.StaminaId;
            modifier.AmountMultiplier = 0.05f;
            modifier.Duration = 5f;
            base.AddModifier(modifierId, modifier);
        }

        private void InitEfficiency()
        {
            RomStatLogic.MyStatEfficiencyModifier modifier = default(RomStatLogic.MyStatEfficiencyModifier);
            string modifierId = "WeakAttack";
            modifier.StatId = MyRomStatEffect.StaminaId;
            modifier.EfficiencyMultiplier = 0.55f;
            modifier.Threshold = 0.25f;
            base.AddEfficiency(modifierId, modifier);
            modifierId = "WeakBlock";
            modifier.StatId = MyRomStatEffect.StaminaId;
            modifier.EfficiencyMultiplier = 0.7f;
            modifier.Threshold = 0.25f;
            base.AddEfficiency(modifierId, modifier);
        }
    }
}
