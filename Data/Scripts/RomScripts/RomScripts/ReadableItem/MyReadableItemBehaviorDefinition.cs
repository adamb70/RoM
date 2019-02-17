using System;
using System.Collections.Generic;
using ObjectBuilders.Definitions.Tools;
using Sandbox.Definitions.Equipment;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Utils;
using Medieval.Definitions.Tools;
using Medieval.Definitions.Combat;
using Sandbox.Game.EntityComponents;


namespace RomScripts76561197972467544.ReadableItem

{
    [MyDefinitionType(typeof(MyObjectBuilder_ReadableItemBehaviorDefinition))]

    public class MyReadableItemBehaviorDefinition : MyToolBehaviorDefinition
    {

        public List<string> Pages = new List<string>();
        public string Title = "Book";

        protected override void Init(MyObjectBuilder_DefinitionBase builder)
        {
            base.Init(builder);
            var ob = (MyObjectBuilder_ReadableItemBehaviorDefinition)builder;
            Pages = ob.Pages;
            Title = ob.Title;
        }

    }

}

