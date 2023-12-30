using HarmonyLib;
using EnhancedGravTrapZero.Monobehaviours;
using UnityEngine;

namespace EnhancedGravTrapZero.Patches
{
    [HarmonyPatch(typeof(Gravsphere))]
    internal static class GravSpherePatches
    {
        [HarmonyPatch(nameof(Gravsphere.Start)), HarmonyPostfix]
        private static void Start_Postfix(Gravsphere __instance)
        {
            __instance.gameObject.EnsureComponent<AllowedObjectManager>();
        }

        [HarmonyPatch(nameof(Gravsphere.AddAttractable)), HarmonyPostfix]
        static void Gravsphere_AddAttractable_Postfix(Gravsphere __instance, Rigidbody r)
        {
            __instance.GetComponent<AllowedObjectManager>().HandleAttracted(r.gameObject, true);
        }

        [HarmonyPatch(nameof(Gravsphere.DestroyEffect)), HarmonyPostfix]
        static void Gravsphere_DestroyEffect_Postfix(Gravsphere __instance, int index)
        {
            var rigidBody = __instance.attractableList[index];
            if (rigidBody)
                __instance.GetComponent<AllowedObjectManager>().HandleAttracted(rigidBody.gameObject, false);
        }

        [HarmonyPatch(nameof(Gravsphere.IsValidTarget)), HarmonyPostfix]
        static void Gravsphere_IsValidTarget_Prefix(Gravsphere __instance, GameObject obj, ref bool __result)
        {
            if(__instance.GetComponent<EnhancedGravSphere>() == null)
            {
                return;
            }

            __result = __instance.GetComponent<AllowedObjectManager>().IsValidTarget(obj);
        }
    }
}
