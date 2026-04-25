namespace ResidentsFishWithYou.Patches;

public static class ZonePatch
{
    public static void AddCardPrefix(Zone __instance, Card t, ref Point point)
    {
        if (t is not Thing thing ||
            AI_FishPatch.ContainsPendingResidentCatch(thing: thing) == false)
        {
            return;
        }

        if (EClass.core?.IsGameStarted != true ||
            __instance == null ||
            __instance.IsPCFaction == false ||
            EClass.pc?.ai is AI_Fish == false)
        {
            return;
        }

        if (EClass.pc?.pos is not Point pcPosition)
        {
            return;
        }

        if (AI_FishPatch.TryPrepareResidentCatchPlacement(thing: thing) == false)
        {
            return;
        }

        point = pcPosition;
        LogDebug($"Moving resident catch {thing.NameSimple} ({thing.id}) to PC position {pcPosition}.");
    }

    public static bool PetFollowPrefix(Zone __instance, ref bool __result)
    {
        if (EClass.core?.IsGameStarted != true ||
            __instance == null ||
            __instance.IsPCFaction == false ||
            EClass.pc?.ai is AI_Fish == false)
        {
            return true;
        }

        __result = false;
        return false;
    }

    private static void LogDebug(string message)
    {
        ResidentsFishWithYou.LogDebug(message: message, caller: nameof(ZonePatch));
    }
}
