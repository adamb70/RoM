using System;
using VRage.Utils;

namespace RomScripts
{
    /// <summary>
    /// This structure contains all information about damage being done.
    /// </summary>
    internal struct RomDamageInformation
    {
        public bool IsDeformation;

        public float Amount;

        public MyStringHash Type;

        public long AttackerId;

        public RomDamageInformation(bool isDeformation, float amount, MyStringHash type, long attackerId)
        {
            this.IsDeformation = isDeformation;
            this.Amount = amount;
            this.Type = type;
            this.AttackerId = attackerId;
        }
    }
}
