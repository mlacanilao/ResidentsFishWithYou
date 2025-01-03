namespace ResidentsFishWithYou.Patches
{
    public static class CardRendererPatch
    {
        public static bool PlayAnimePrefix(CardRenderer __instance, AnimeID id)
        {
            if (ResidentsFishWithYouConfig.EnableAutoPlaceFishingItems?.Value == false ||
                EClass.core?.IsGameStarted == false ||
                EClass._zone?.IsPCFaction == false ||
                id != AnimeID.Jump)
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
            
            if (EClass._zone?.TryAddThingInSharedContainer(t: __instance.owner?.Thing, containers: null, add: true, msg: false,
                    chara: null, sharedOnly: true) == false)
            {
                EClass._map?.TrySmoothPick(cell: __instance.owner?.pos.cell, t: __instance.owner?.Thing, c: EClass.pc);
            }
            
            return false;
        }
    }
}