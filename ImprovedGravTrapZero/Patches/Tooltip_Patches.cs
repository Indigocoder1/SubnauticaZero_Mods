using HarmonyLib;
using EnhancedGravTrapZero.Monobehaviours;
using System.Text;
using UnityEngine;

namespace EnhancedGravTrapZero.Patches
{
    [HarmonyPatch(typeof(TooltipFactory))]
    internal class Tooltip_Patches
    {
        private static string GetActionString()
        {
            return Main_Plugin.AdvanceKey.Value.ToString();
        }

        private static int GetChangeListDir()
        {
            if (Main_Plugin.UseScrollWheel.Value)
            {
                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    return 1;
                }
                else if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return Input.GetKeyDown(Main_Plugin.AdvanceKey.Value) ? 1 : 0;
            }
        }

        static readonly string button = GetActionString();

        [HarmonyPatch(nameof(TooltipFactory.ItemCommons)), HarmonyPostfix]
        static void TooltipSubtitles(StringBuilder sb, TechType techType, GameObject obj)
        {
            if (!techType.IsGravTrap())
                return;

            var objectsType = obj.EnsureComponent<AllowedObjectManager>();
            objectsType.techTypeListIndex += GetChangeListDir();
            TooltipFactory.WriteDescription(sb, $"Allowed type = {objectsType.GetCurrentListName()}");
        }

        [HarmonyPatch(nameof(TooltipFactory.ItemActions)), HarmonyPostfix]
        static void Postfix(StringBuilder sb, InventoryItem item)
        {
            if (item.techType.IsGravTrap())
            {
                TooltipFactory.WriteAction(sb, button, "Switch object's type");
            }
        }
    }
}
