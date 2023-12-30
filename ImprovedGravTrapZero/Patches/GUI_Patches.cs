using HarmonyLib;
using EnhancedGravTrapZero.Monobehaviours;

namespace EnhancedGravTrapZero.Patches
{
    internal static class GUI_Patches
    {
        public static bool IsGravTrap(this TechType techType) => techType == EnhancedTrap_Craftable.techType;

        [HarmonyPatch(typeof(GUIHand), nameof(GUIHand.OnUpdate)), HarmonyPostfix]
        static void GUIHand_OnUpdate(GUIHand __instance)
        {
            if (!__instance.player.IsFreeToInteract() || !AvatarInputHandler.main.IsEnabled())
                return;

            if (__instance.GetTool() is PlayerTool tool && tool.pickupable?.GetTechType().IsGravTrap() == true)
            {
                string text = tool.GetCustomUseText();
                text += $"\n{tool.gameObject.GetComponent<AllowedObjectManager>().GetCurrentListName()}";
                HandReticle.main.SetText(HandReticle.TextType.Use, text, true);
            }
        }

        [HarmonyPatch(typeof(Pickupable), nameof(Pickupable.OnHandHover)), HarmonyPostfix]
        static void GUIHand_OnHover(Pickupable __instance)
        {
            if (__instance.GetTechType().IsGravTrap())
                HandReticle.main.SetText(HandReticle.TextType.Use, __instance.gameObject.GetComponent<AllowedObjectManager>().GetCurrentListName(), true);
        }
    }
}
