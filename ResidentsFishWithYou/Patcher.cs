using ResidentsFishWithYou.Patches;
using HarmonyLib;
using UnityEngine;

namespace ResidentsFishWithYou
{
    public class Patcher
    {
        [HarmonyPrefix]
        [HarmonyPatch(declaringType: typeof(AI_Fish.ProgressFish), methodName: nameof(AI_Fish.ProgressFish.OnStart))]
        public static bool AI_FishProgressFishOnStart(AI_Fish.ProgressFish __instance)
        {
            return AI_FishProgressFishPatch.OnStartPrefix(__instance: __instance);
        }
        
        [HarmonyPrefix]
        [HarmonyPatch(declaringType: typeof(Zone), methodName: nameof(Zone.AddCard), argumentTypes: new[] { typeof(Card), typeof(Point) })]
        public static void ZoneAddCard(Zone __instance, Card t, ref Point point)
        {
            ZonePatch.AddCardPrefix(__instance: __instance, t:t, point: ref point);
        }
        
        [HarmonyPrefix]
        [HarmonyPatch(declaringType: typeof(CardRenderer), methodName: nameof(CardRenderer.PlayAnime), argumentTypes: new[] { typeof(AnimeID), typeof(Vector3), typeof(bool) })]
        public static bool CardRendererPlayAnime(CardRenderer __instance, AnimeID id)
        {
            return CardRendererPatch.PlayAnimePrefix(__instance: __instance, id: id);
        }
        
        [HarmonyPrefix]
        [HarmonyPatch(declaringType: typeof(Chara), methodName: nameof(Chara.Pick))]
        public static bool CharaPick()
        {
            return CharaPatch.PickPrefix();
        }
    }
}