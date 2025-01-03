namespace ResidentsFishWithYou.Patches
{
    public static class CharaPatch
    {
        public static bool PickPrefix(Chara __instance, Thing t)
        {
            if (EClass.core?.IsGameStarted == false ||
                __instance.IsPC == false)
            {
                return true;
            }
            
            if (t?.sourceCard?._origin != "fish" &&
                t?.sourceCard?._origin != "statue_god" &&
                t?.sourceCard?._origin != "junkFlat" &&
                t?.sourceCard?.id != "book_ancient" &&
                t?.sourceCard?.category != "currency")
            {
                return true;
            }

            return false;
        }
    }
}