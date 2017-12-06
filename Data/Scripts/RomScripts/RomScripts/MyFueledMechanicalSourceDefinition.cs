using System;
using ObjectBuilders.Definitions.Tools;
using Sandbox.Definitions.Equipment;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Utils;

using Medieval.Definitions.Tools;
using Medieval.Definitions.MechanicalPower;

namespace RomScripts
{
    [MyDefinitionType(typeof(MyObjectBuilder_FueledMechanicalSourceComponentDefinition))]
    public class MyFueledMechanicalSourceComponentDefinition : MyMechanicalSourceComponentDefinition
    {
        protected override void Init(MyObjectBuilder_DefinitionBase builder)
        {
            base.Init(builder);
            MyObjectBuilder_FueledMechanicalSourceComponentDefinition componentDefinition = (MyObjectBuilder_FueledMechanicalSourceComponentDefinition)builder;
        
            //float? maxAltitudeDelta = myObjectBuilder_BlockWindmillComponentDefinition.MaxAltitudeDelta;
            //this.MaxAltitudeDelta = (maxAltitudeDelta.HasValue ? maxAltitudeDelta.GetValueOrDefault() : 20f);
            //this.PowerInterpolation = (myObjectBuilder_BlockWindmillComponentDefinition.PowerInterpolation ?? InterpolationMethod.Linear);
        }
    }
}
