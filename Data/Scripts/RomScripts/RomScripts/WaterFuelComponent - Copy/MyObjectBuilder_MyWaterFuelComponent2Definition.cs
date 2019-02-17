//using System;
//using System.Xml.Serialization;
//using VRage.Game;
//using VRage.ObjectBuilders;
//using VRage.ObjectBuilders.Definitions.Inventory;

//namespace RomScripts76561197972467544
//{
//    [MyObjectBuilderDefinition]
//    [XmlSerializerAssembly("MedievalEngineers.ObjectBuilders.XmlSerializers")]
//    public class MyObjectBuilder_WaterFuelComponent2Definition : MyObjectBuilder_EntityComponentDefinition
//    {
//        public class FuelTimeDef
//        {
//            [XmlIgnore]
//            public DefinitionTagId Id;

//            public TimeDefinition? Time;

//            [XmlAttribute]
//            public string Type
//            {
//                get
//                {
//                    return this.Id.Type;
//                }
//                set
//                {
//                    this.Id.Type = value;
//                }
//            }

//            [XmlAttribute]
//            public string Subtype
//            {
//                get
//                {
//                    return this.Id.Subtype;
//                }
//                set
//                {
//                    this.Id.Subtype = value;
//                }
//            }

//            [XmlAttribute]
//            public string Tag
//            {
//                get
//                {
//                    return this.Id.Tag;
//                }
//                set
//                {
//                    this.Id.Tag = value;
//                }
//            }

//            protected bool Equals(FuelTimeDef other)
//            {
//                return this.Id.Equals(other.Id);
//            }

//            public override bool Equals(object obj)
//            {
//                return !object.ReferenceEquals(null, obj) && (object.ReferenceEquals(this, obj) || (!(obj.GetType() != base.GetType()) && this.Equals((FuelTimeDef)obj)));
//            }

//            public override int GetHashCode()
//            {
//                return this.Id.GetHashCode();
//            }
//        }

//        public string FuelInventory = string.Empty;

//        [XmlArrayItem("Fuel")]
//        public System.Collections.Generic.List<FuelTimeDef> FuelTimes = new System.Collections.Generic.List<FuelTimeDef>();
//    }
//}
