using System;
using Sandbox.Definitions.Inventory;
using Sandbox.Game.Inventory;
using Sandbox.Graphics;
using Sandbox.Gui.Controls;
using Sandbox.Gui.Styles;
using Sandbox.Gui.Utility;
using VRage.Utils;
using VRageMath;
using Sandbox.Game.GUI.ItemRenderers;

namespace RomScripts.RomGUI
{
    [MyItemRendererDescriptor("DecayingItem", false)]
    class MyDecayingItemRenderer : MyDurableItemRenderer
    {

        public override void Draw(MyGrid.Item item, MyGrid.GridItemState state, RectangleF itemRect, Color colormask, MyStateBase style, float transitionAlpha)
        {
            MyDurableItem myDurableItem = item.UserData as MyDurableItem;
            if (myDurableItem == null)
            {
                // If not a durable item, pass to base and allow to treat as inventory item, then return without running my code
                base.Draw(item, state, itemRect, colormask, style, transitionAlpha);
                return;
            }
            // If durable, run base durable then run my code after
            base.Draw(item, state, itemRect, colormask, style, transitionAlpha);


            if (myDurableItem.Amount > 1)
            {
                Vector2 vector = itemRect.Position;
                Vector2 size = itemRect.Size;
                bool enabled = item.Enabled && state != MyGrid.GridItemState.Disabled;

                string text = string.Format("{0}x", myDurableItem.Amount);
                MyFontStyle font = style.Font;
                vector += new Vector2(0f, size.Y * 0.86f);
                Color color = base.ApplyColorMaskModifiers(font.Color, enabled, transitionAlpha);
                MyFontHelper.DrawString(font.Font, text, vector, font.Size, new Color?(color), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM, false, size.X, null);
            }
        }

    }
}
