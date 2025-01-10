using System.Collections.Generic;

namespace ResidentsFishWithYou.Patches
{
    public static class CardRendererPatch
    {
        public static bool PlayAnimePrefix(CardRenderer __instance, AnimeID id)
        {
            bool enableAutoPlaceFishingItems = ResidentsFishWithYouConfig.EnableAutoPlaceFishingItems?.Value ?? false;
            
            if (enableAutoPlaceFishingItems == false ||
                EClass.core?.IsGameStarted == false ||
                EClass._zone?.IsPCFaction == false ||
                id != AnimeID.Jump ||
                EClass.pc?.ai is AI_Fish == false)
            {
                return true;
            }
            
            if (__instance.owner?.sourceCard?._origin != "fish" &&
                __instance.owner?.sourceCard?._origin != "statue_god" &&
                __instance.owner?.sourceCard?._origin != "junkFlat" &&
                __instance.owner?.sourceCard?.id != "book_ancient" &&
                __instance.owner?.sourceCard?.category != "currency")
            {
                return true;
            }
            
            List<Thing> thingsToMove = new List<Thing>(EClass.pc?.pos?.Things);
            
            foreach (Thing thing in thingsToMove)
            {
                if (EClass._zone?.TryAddThingInSharedContainer(t: thing, containers: null, add: true, msg: false,
                        chara: null, sharedOnly: true) == false)
                {
                    EClass._map?.TrySmoothPick(cell: EClass.pc?.pos?.cell, t: thing, c: EClass.pc);
                }
            }
            
            return false;
        }
    }
}