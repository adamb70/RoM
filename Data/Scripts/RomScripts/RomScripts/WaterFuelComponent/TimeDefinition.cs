using System;
using System.Xml.Serialization;

namespace RomScripts.WaterFuelComponent
{
    public struct TimeDefinition
    {
        [XmlAttribute]
        public float Days;

        [XmlAttribute]
        public float Hours;

        [XmlAttribute]
        public float Minutes;

        [XmlAttribute]
        public float Seconds;

        [XmlAttribute]
        public float Milliseconds;

        [XmlAttribute]
        public string Value
        {
            get
            {
                return this.ToString();
            }
            set
            {
                System.TimeSpan timeSpan;
                if (!System.TimeSpan.TryParse(value, out timeSpan))
                {
                    return;
                }
                this.Days = (float)timeSpan.Days;
                this.Hours = (float)timeSpan.Hours;
                this.Minutes = (float)timeSpan.Minutes;
                this.Seconds = (float)timeSpan.Seconds;
                this.Milliseconds = (float)timeSpan.Milliseconds;
            }
        }

        public static implicit operator System.TimeSpan(TimeDefinition time)
        {
            long num = (long)(time.Days * 8.64E+11f);
            long num2 = (long)(time.Hours * 3.6E+10f);
            long num3 = (long)(time.Minutes * 6E+08f);
            long num4 = (long)(time.Seconds * 1E+07f);
            long num5 = (long)(time.Milliseconds * 10000f);
            return new System.TimeSpan(num + num2 + num3 + num4 + num5);
        }

        public static implicit operator TimeDefinition(System.TimeSpan time)
        {
            return new TimeDefinition
            {
                Days = (float)time.Days,
                Hours = (float)time.Hours,
                Minutes = (float)time.Minutes,
                Seconds = (float)time.Seconds,
                Milliseconds = (float)time.Milliseconds
            };
        }
    }
}
