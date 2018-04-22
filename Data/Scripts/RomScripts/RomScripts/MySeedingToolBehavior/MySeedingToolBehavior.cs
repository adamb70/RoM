using Medieval.Inventory;
using ObjectBuilders.Definitions.Tools;
using Sandbox.Definitions.Equipment;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.EntityComponents.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.Inventory;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Components;
using VRage.Game;
using VRage.Game.Entity;
using VRage.GUI.Crosshair;
using VRage.Library.Logging;
using VRage.Systems;
using VRage.Utils;
using VRageMath;
using Medieval.GameSystems.Tools;
using Medieval.GameSystems;

using Sandbox.Definitions.Inventory;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI;
using VRage.Game.ModAPI;
using VRage.Library.Collections;
using VRage.ObjectBuilders.Inventory;

using Medieval.Definitions.GameSystems;
using Medieval.Definitions.Tools;
using Medieval.WorldEnvironment.Modules;
using ObjectBuilders.GameSystems;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.WorldEnvironment;
using VRage.Definitions;
using VRage.Game.Components;
using VRage.Game.Components.Session;
using VRage.Game.Voxels;
using VRage.Network;
using VRage.Voxels;

using Sandbox.Game.Entities;

namespace Romscripts.SeedingToolBehavior
{
    /// <summary>
    /// Seeding behavior for farming.
    /// </summary>
    [MyHandItemBehavior(typeof(MyObjectBuilder_RomSeedingToolBehaviorDefinition), true)]
    public class MyRomSeedingToolBehavior : MySeedingToolBehavior
    {
        public float? MinAltitudePercentage;
        public float? MaxAltitudePercentage;
        public float? MaxNorthPercentage;
        public float? MinNorthPercentage;


        public override void Init(MyEntity holder, MyHandItem item, MyHandItemBehaviorDefinition definition)
        {
            base.Init(holder, item, definition);

            var ob = (MyRomSeedingToolBehaviorDefinition)definition;
            this.MinAltitudePercentage = ob.MinAltitudePercentage;
            this.MaxAltitudePercentage = ob.MaxAltitudePercentage;
            this.MaxNorthPercentage = ob.MaxNorthPercentage;
            this.MinNorthPercentage = ob.MinNorthPercentage;
        }


        protected override bool Start(MyHandItemActionEnum action)
        {
            if (action != MyHandItemActionEnum.Primary)
            {
                return false;
            }
            
            if (base.Start(action) == true)
            {
                return this.IsPlantable(this.Target.Position, this.Target.Normal, null);
            }
            return false;
        }


        public bool IsPlantable(Vector3D position, Vector3 normal, MyPlanet planet = null)
        {
            if (planet == null)
            {
                planet = MyGamePruningStructure.GetClosestPlanet(position);
                if (planet == null)
                {
                    return false;
                }
            }

            // latitude limit
            if (this.MaxNorthPercentage != null || this.MinNorthPercentage != null)
            {
                MyPlanetAreasComponent areas_component = planet.Components.Get<MyPlanetAreasComponent>();

                if (areas_component != null)
                {
                    long areaID = areas_component.GetArea(position);

                    //string Kingdom; string Region; string Area;
                    //MyPlanetAreasComponent.UnpackAreaId(areaID, out Kingdom, out Region, out Area);
                    //((IMyUtilities)MyAPIUtilities.Static).ShowNotification(Kingdom, 3000, null, Color.Gold);
                    //((IMyUtilities)MyAPIUtilities.Static).ShowNotification(Region, 3000, null, Color.Teal);
                    //((IMyUtilities)MyAPIUtilities.Static).ShowNotification(Area, 3000, null, Color.Orange);


                    //kingdom (face) ids: fareon=0, bar hadur=1, levos=2, rintel=3 umbril=4, darios=5
                    //                    front     back         left     right    up        down

                    int face;
                    int x;
                    int y;
                    MyPlanetAreasComponent.UnpackAreaId(areaID, out face, out x, out y);
                    //((IMyUtilities)MyAPIUtilities.Static).ShowNotification(x.ToString() + " across", 3000, null, Color.Teal);
                    //(IMyUtilities)MyAPIUtilities.Static).ShowNotification(y.ToString() + " down", 3000, null, Color.Orange);
                    //((IMyUtilities)MyAPIUtilities.Static).ShowNotification("Face: " + face.ToString(), 3000, null, Color.Gold);

                    float percent_north = (areas_component.AreaCount - (float)y) / areas_component.AreaCount * 100f;
                    //((IMyUtilities)MyAPIUtilities.Static).ShowNotification("Percent North: " + percent_north.ToString(), 3000, null, Color.HotPink);
                    //((IMyUtilities)MyAPIUtilities.Static).ShowNotification("max North: " + this.MaxNorthPercentage.ToString(), 3000, null, Color.HotPink);
                    //((IMyUtilities)MyAPIUtilities.Static).ShowNotification("min North: " + this.MinNorthPercentage.ToString(), 3000, null, Color.HotPink);

                    if (face < 4)  // If face is Up or Down, ignore for now. Will implement when needed. (north and south are abnormal on those faces)
                    {
                        if (this.MaxNorthPercentage != null && percent_north > this.MaxNorthPercentage) // too far north
                        {
                            ((IMyUtilities)MyAPIUtilities.Static).ShowNotification("Cannot be planted this far north!", 2000, null, Color.Red);
                            return false;
                        }
                        if (this.MinNorthPercentage != null && percent_north < this.MinNorthPercentage) // too far south
                        {
                            ((IMyUtilities)MyAPIUtilities.Static).ShowNotification("Cannot be planted this far south!", 2000, null, Color.Red);
                            return false;
                        }
                    }
                }
            }


            // altitude limit
            if (this.MaxAltitudePercentage != null || this.MinAltitudePercentage != null)
            {
                double distance_from_center = (position - planet.GetPosition()).Length();
                double altitude = distance_from_center - planet.MinimumRadius;
                double altitude_percentage = altitude / (planet.MaximumRadius - planet.MinimumRadius) * 100;

                //((IMyUtilities)MyAPIUtilities.Static).ShowNotification("Altitude: " + altitude.ToString(), 3000, null, Color.HotPink);
                //((IMyUtilities)MyAPIUtilities.Static).ShowNotification("Altitude %: " + altitude_percentage.ToString(), 3000, null, Color.HotPink);
                
                if (this.MinAltitudePercentage != null && altitude_percentage < this.MinAltitudePercentage) // too low down
                {
                    ((IMyUtilities)MyAPIUtilities.Static).ShowNotification("Cannot be planted this low down!", 2000, null, Color.Red);
                    return false;
                }
                if (this.MaxAltitudePercentage != null && altitude_percentage > this.MaxAltitudePercentage) // too high up
                {
                    ((IMyUtilities)MyAPIUtilities.Static).ShowNotification("Cannot be planted this high up!", 2000, null, Color.Red);
                    return false;
                }
            }

            return true;
        }

    }
}
