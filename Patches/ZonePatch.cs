namespace ResidentsFishWithYou.Patches
{
    public static class ZonePatch
    {
        public static void AddCardPrefix(Zone __instance, Card t, ref Point point)
        {
            if (__instance?.IsPCFaction == false)
            {
                return;
            }
        
            if (t?.sourceCard?._origin != "fish" &&
                t?.sourceCard?._origin != "statue_god" &&
                t?.sourceCard?._origin != "junkFlat" &&
                t?.sourceCard?.id != "book_ancient" &&
                t?.sourceCard?.category != "currency")
            {
                return;
            }
        
            point = EClass.pc?.pos;
        }
    }
}