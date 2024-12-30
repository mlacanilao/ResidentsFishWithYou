using ResidentsFishWithYou.Patches;
using HarmonyLib;

namespace ResidentsFishWithYou
{
    public class Patcher
    {
        [HarmonyPrefix]
        [HarmonyPatch(declaringType: typeof(AI_Fish.ProgressFish), methodName: nameof(AI_Fish.ProgressFish.OnStart))]
        public static void AI_FishProgressFishOnStart(AI_Fish.ProgressFish __instance)
        {
            AI_FishProgressFishPatch.OnStartPrefix(__instance: __instance);
        }
        
        [HarmonyPrefix]
        [HarmonyPatch(declaringType: typeof(Zone), methodName: nameof(Zone.AddCard), argumentTypes: new[] { typeof(Card), typeof(Point) })]
        public static void ZoneAddCard(Zone __instance, Card t, ref Point point)
        {
            ZonePatch.AddCardPrefix(__instance: __instance, t:t, point: ref point);
        }
    }
}