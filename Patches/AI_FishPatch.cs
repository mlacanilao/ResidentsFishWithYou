using System.Collections.Generic;

namespace ResidentsFishWithYou.Patches;

public static class AI_FishPatch
{
    private static readonly List<CatchContext> PendingCatches = new List<CatchContext>();

    public static void MakefishPostfix(Chara c, Thing __result)
    {
        if (ShouldTrackCatch(chara: c, thing: __result) == false)
        {
            return;
        }

        bool isResidentCatch = IsResidentCatch(chara: c);
        PendingCatches.Add(item: new CatchContext(chara: c, thing: __result, isResidentCatch: isResidentCatch));

        string ownerType = isResidentCatch ? "resident" : "PC";
        LogDebug($"Tracking {ownerType} catch {GetLogName(chara: c)} -> {__result.NameSimple} ({__result.id}).");
    }

    public static void OnProgressCompletePostfix(AI_Fish.ProgressFish __instance)
    {
        if (__instance == null)
        {
            return;
        }

        Chara owner = __instance.owner;
        if (owner == null)
        {
            return;
        }

        CatchContext? catchContext = ConsumeCatchForOwner(chara: owner);
        if (catchContext?.Thing == null ||
            catchContext.Thing.isDestroyed == true)
        {
            return;
        }

        bool enableAutoPlaceFishingItems = ResidentsFishWithYouConfig.EnableAutoPlaceFishingItems?.Value ?? false;
        Zone zone = EClass._zone;
        if (enableAutoPlaceFishingItems == false ||
            EClass.core?.IsGameStarted == false ||
            zone == null ||
            zone.IsPCFaction == false)
        {
            return;
        }

        Thing caughtThing = catchContext.Thing;
        string caughtThingLogName = GetThingLogName(thing: caughtThing);
        if (catchContext.IsResidentCatch == true &&
            catchContext.IsResidentCatchBaitSatisfied == false)
        {
            LogDebug($"Skipping shared-container auto-place for resident catch without bait payment: {caughtThingLogName}.");
            return;
        }

        if (zone.TryAddThingInSharedContainer(t: caughtThing, containers: null, add: true, msg: true, chara: owner, sharedOnly: true) == true)
        {
            LogDebug($"Moved catch to shared container: {caughtThingLogName}.");
            return;
        }

        LogDebug($"No shared container accepted catch; leaving it in vanilla location: {caughtThingLogName}.");
    }

    public static bool ContainsPendingResidentCatch(Thing thing)
    {
        CleanupPendingCatches();

        foreach (CatchContext catchContext in PendingCatches)
        {
            if (catchContext.IsResidentCatch == true &&
                ReferenceEquals(objA: catchContext.Thing, objB: thing) == true)
            {
                return true;
            }
        }

        return false;
    }

    public static bool TryPrepareResidentCatchPlacement(Thing thing)
    {
        CatchContext? catchContext = GetResidentCatch(thing: thing);
        if (catchContext == null)
        {
            return false;
        }

        if (catchContext.IsResidentCatchBaitSatisfied == true)
        {
            return true;
        }

        if (FishingBait.TryConsumePlayerBaitIfRequired(remainingBait: out int remainingBait) == false)
        {
            LogDebug($"Cannot deliver resident catch to PC position because player has no usable bait: {thing.NameSimple} ({thing.id}).");
            return false;
        }

        catchContext.IsResidentCatchBaitSatisfied = true;

        bool enableRequireBait = ResidentsFishWithYouConfig.EnableRequireBait?.Value ?? false;
        if (enableRequireBait == true)
        {
            LogDebug($"Consumed 1 player bait for resident catch delivery; remaining equipped bait count is {remainingBait}.");
        }

        return true;
    }

    private static bool ShouldTrackCatch(Chara chara, Thing thing)
    {
        if (EClass.core?.IsGameStarted == false ||
            EClass._zone?.IsPCFaction != true ||
            EClass.pc?.ai is not AI_Fish ||
            chara == null ||
            thing == null)
        {
            return false;
        }

        if (IsResidentCatch(chara: chara) == true)
        {
            return true;
        }

        bool enableAutoPlaceFishingItems = ResidentsFishWithYouConfig.EnableAutoPlaceFishingItems?.Value ?? false;
        return chara.IsPC == true &&
               enableAutoPlaceFishingItems == true;
    }

    private static bool IsResidentCatch(Chara chara)
    {
        return chara.IsPC == false &&
               chara.IsPCFaction == true &&
               chara.IsAliveInCurrentZone == true &&
               chara.memberType == FactionMemberType.Default;
    }

    private static CatchContext? ConsumeCatchForOwner(Chara chara)
    {
        CleanupPendingCatches();

        for (int i = PendingCatches.Count - 1; i >= 0; i--)
        {
            CatchContext catchContext = PendingCatches[index: i];
            if (ReferenceEquals(objA: catchContext.Chara, objB: chara) == false)
            {
                continue;
            }

            PendingCatches.RemoveAt(index: i);
            return catchContext;
        }

        return null;
    }

    private static CatchContext? GetResidentCatch(Thing thing)
    {
        CleanupPendingCatches();

        foreach (CatchContext catchContext in PendingCatches)
        {
            if (catchContext.IsResidentCatch == true &&
                ReferenceEquals(objA: catchContext.Thing, objB: thing) == true)
            {
                return catchContext;
            }
        }

        return null;
    }

    private static void CleanupPendingCatches()
    {
        for (int i = PendingCatches.Count - 1; i >= 0; i--)
        {
            CatchContext catchContext = PendingCatches[index: i];
            if (catchContext.Thing == null ||
                catchContext.Thing.isDestroyed == true)
            {
                PendingCatches.RemoveAt(index: i);
            }
        }
    }

    private static void LogDebug(string message)
    {
        ResidentsFishWithYou.LogDebug(message: message, caller: nameof(AI_FishPatch));
    }

    private static string GetLogName(Chara chara)
    {
        return $"{chara.NameSimple} ({chara.id})";
    }

    private static string GetThingLogName(Thing thing)
    {
        return $"{thing.NameSimple} ({thing.id})";
    }

    private sealed class CatchContext
    {
        public CatchContext(Chara chara, Thing thing, bool isResidentCatch)
        {
            Chara = chara;
            Thing = thing;
            IsResidentCatch = isResidentCatch;
        }

        public Chara Chara { get; }

        public Thing Thing { get; }

        public bool IsResidentCatch { get; }

        public bool IsResidentCatchBaitSatisfied { get; set; }
    }
}
