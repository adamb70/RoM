using Medieval.Definitions.Tools;
using Medieval.GameSystems.Tools;
using ObjectBuilders.Definitions.Tools;
using Sandbox.Definitions.Equipment;
using Sandbox.Game.EntityComponents.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.Inventory;
using VRage.Game.Entity;
using VRage.Game.Input;
using VRage.Input.Input;
using VRage.Network;
using VRage.Utils;
using VRage.Session;
using VRage.Game.ModAPI;
using VRageMath;
using Sandbox.Game.Entities;
using VRage.Components;
using VRage.Systems;
using VRage.Game;
using Sandbox.Game.EntityComponents;
using Sandbox.Entities.Components;
using System.Collections;
using VRage.Game.Components;
using System.Collections.Generic;
using VRage.Game.ModAPI;
using Sandbox.ModAPI;
using System;
using Sandbox.Game.Components;



namespace RomScripts.ReadableItem
{
    [MyHandItemBehavior(typeof(MyObjectBuilder_ReadableItemBehaviorDefinition))]
    // The StaticEventOwner attribute is for multiplayer purposes.
    [StaticEventOwner]
    public class MyReadableItemBehavior : MyHandItemBehaviorBase
    {
        private MyReadableItemBehaviorDefinition m_definition = null;
        private int current_page = 0;
        private int total_pages = 0;

        public override float TargetingDistance
        {
            get
            {
                return -1f;
            }
        }
        public override bool SetSecondary(MyHandItem secondaryItem, MyHandItemBehaviorDefinition secondaryDefinition)
        {
            return false;
        }
        public override bool SetTarget()
        {
            return true;
        }
        public override void EndAction(MyHandItemActionEnum action)
        {
        }

        public override void Init(MyEntity holder, MyHandItem item, MyHandItemBehaviorDefinition definition)
        {
            base.Init(holder, item, definition);
            m_definition = (MyReadableItemBehaviorDefinition)definition;

            if (m_definition.Pages != null)
            {
                total_pages = m_definition.Pages.Count;
            }
        }


        public override MyHandItemBehaviorBase.StartActionResponse StartAction(MyHandItemActionEnum action)
        {
            if (MySession.Static.IsDedicated)
            {
                return MyHandItemBehaviorBase.StartActionResponse.Unhandled;
            }
            switch (action)
            {
                case MyHandItemActionEnum.Primary:
                    this.ShowMessage();
                    EndAction(action);
                    return MyHandItemBehaviorBase.StartActionResponse.Handled;
                case MyHandItemActionEnum.Secondary:
                    if (current_page + 1 >= total_pages)
                    {
                        current_page = 0;
                        ((IMyUtilities)MyAPIUtilities.Static).ShowNotification("You turn back to page 1/" + total_pages.ToString(), 1000, null, Color.White);
                    }
                    else
                    {
                        current_page += 1;
                        ((IMyUtilities)MyAPIUtilities.Static).ShowNotification("You turn to page " + (current_page + 1).ToString() + "/" + total_pages.ToString(), 1500, null, Color.White);
                    }
                    return MyHandItemBehaviorBase.StartActionResponse.Handled;
                default:
                    return MyHandItemBehaviorBase.StartActionResponse.Unhandled;
            }
        }

        // Called when the tool is equipped.
        public override void Activate()
        {
            base.Activate();
        }

        // Called when the tool is unequipped
        public override void Deactivate()
        {
            base.Deactivate();
        }

        protected void ShowMessage()
        {
            if (!MySession.Static.IsServer)
            {
                return;
            }
            MyAPIGateway.Utilities.ShowMissionScreen(m_definition.Title, "Page " + (current_page + 1).ToString() + "/" + total_pages.ToString(), "", m_definition.Pages[current_page].ToString(), CloseBook, "Close");
            return;
        }

        private void CloseBook(ResultEnum result)
        {
            EndAction(MyHandItemActionEnum.Primary); // Doesn't actually seem to work...
            return;
        }

    }




}

