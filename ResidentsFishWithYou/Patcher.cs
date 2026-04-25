using HarmonyLib;
using ResidentsFishWithYou.Patches;

namespace ResidentsFishWithYou;

internal static class Patcher
{
    [HarmonyPrefix]
    [HarmonyPatch(declaringType: typeof(AI_Fish.ProgressFish), methodName: nameof(AI_Fish.ProgressFish.OnStart))]
    internal static void AI_FishProgressFishOnStart(AI_Fish.ProgressFish __instance)
    {
        AI_FishProgressFishPatch.OnStartPrefix(__instance: __instance);
    }

    [HarmonyPostfix]
    [HarmonyPatch(declaringType: typeof(AI_Fish.ProgressFish), methodName: nameof(AI_Fish.ProgressFish.OnProgressComplete))]
    internal static void AI_FishProgressFishOnProgressComplete(AI_Fish.ProgressFish __instance)
    {
        AI_FishPatch.OnProgressCompletePostfix(__instance: __instance);
    }

    [HarmonyPostfix]
    [HarmonyPatch(declaringType: typeof(AI_Fish), methodName: nameof(AI_Fish.Makefish))]
    internal static void AI_FishMakefish(Chara c, Thing __result)
    {
        AI_FishPatch.MakefishPostfix(c: c, __result: __result);
    }

    [HarmonyPrefix]
    [HarmonyPatch(declaringType: typeof(Zone), methodName: nameof(Zone.AddCard), argumentTypes: new[] { typeof(Card), typeof(Point) })]
    internal static void ZoneAddCard(Zone __instance, Card t, ref Point point)
    {
        ZonePatch.AddCardPrefix(__instance: __instance, t: t, point: ref point);
    }

    [HarmonyPrefix]
    [HarmonyPatch(declaringType: typeof(Zone), methodName: nameof(Zone.PetFollow), methodType: MethodType.Getter)]
    internal static bool ZonePetFollow(Zone __instance, ref bool __result)
    {
        return ZonePatch.PetFollowPrefix(__instance: __instance, __result: ref __result);
    }
}
