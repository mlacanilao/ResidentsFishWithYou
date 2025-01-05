namespace ResidentsFishWithYou.Patches
{
    public static class CharaPatch
    {
        public static bool PickPrefix()
        {
            if (ResidentsFishWithYouConfig.EnableAutoPlaceFishingItems?.Value == false ||
                EClass.core?.IsGameStarted == false ||
                EClass._zone?.IsPCFaction == false ||
                EClass.pc?.ai is AI_Fish == false)
            {
                return true;
            }

            return false;
        }
    }
}