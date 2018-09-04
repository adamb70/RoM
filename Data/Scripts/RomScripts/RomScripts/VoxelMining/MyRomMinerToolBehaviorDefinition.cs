using ObjectBuilders.Definitions.Tools;
using Sandbox.Definitions.Equipment;
using System;
using VRage.Game;
using VRage.Game.Definitions;
using Medieval.Definitions.Tools;

namespace RomScripts.VoxelMining
{
    [MyDefinitionType(typeof(MyObjectBuilder_RomMinerToolBehaviorDefinition))]
    public class MyRomMinerToolBehaviorDefinition : MyMinerToolBehaviorDefinition
    {
        public MyDefinitionId MiningLootTable;

        protected override void Init(MyObjectBuilder_DefinitionBase builder)
        {
            base.Init(builder);

            MyObjectBuilder_RomMinerToolBehaviorDefinition ob = (MyObjectBuilder_RomMinerToolBehaviorDefinition)builder;
            this.MiningLootTable = ob.MiningLootTable;
        }
    }
}
