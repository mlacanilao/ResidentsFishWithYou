using System.Collections.Generic;

namespace ResidentsFishWithYou.Patches;

public static class AI_FishProgressFishPatch
{
    public static void OnStartPrefix(AI_Fish.ProgressFish __instance)
    {
        if (__instance == null ||
            EClass.core?.IsGameStarted == false ||
            AI_Fish.shouldCancel == true)
        {
            return;
        }

        Zone zone = EClass._zone;
        if (__instance.owner?.IsPC != true ||
            zone?.branch is null)
        {
            return;
        }

        LogDebug($"PC fishing start at water {__instance.posWater}; checking {zone.branch.members.Count} branch members.");

        foreach (Chara chara in zone.branch.members)
        {
            if (chara == null)
            {
                LogDebug("Skipping branch member: resident is null.");
                continue;
            }

            Hobby? fishingWork = null;
            Hobby? fishingHobby = null;

            foreach (Hobby w in chara.ListWorks())
            {
                if (w.source.alias == "Fish" == false)
                {
                    continue;
                }

                fishingWork = w;
                break;
            }

            foreach (Hobby h in chara.ListHobbies())
            {
                if (h.source.alias == "Fish" == false)
                {
                    continue;
                }

                fishingHobby = h;
                break;
            }

            if (fishingWork == null &&
                fishingHobby == null)
            {
                continue;
            }

            string? blockReason = GetInviteBlockReason(chara: chara);
            if (blockReason != null)
            {
                LogDebug($"Skipping {GetLogName(chara: chara)}: {blockReason}.");
                continue;
            }

            LogDebug($"Found fishing resident {GetLogName(chara: chara)}; work={fishingWork != null}, hobby={fishingHobby != null}.");

            if (fishingWork != null &&
                TryInviteResidentToFish(chara: chara, fishing: fishingWork, isHobby: false, posWater: __instance.posWater) == true)
            {
                continue;
            }

            if (fishingHobby != null)
            {
                TryInviteResidentToFish(chara: chara, fishing: fishingHobby, isHobby: true, posWater: __instance.posWater);
            }
        }
    }

    private static bool CanInviteResident(Chara chara)
    {
        return GetInviteBlockReason(chara: chara) == null;
    }

    private static string? GetInviteBlockReason(Chara chara)
    {
        if (chara == null)
        {
            return "resident is null";
        }

        if (chara.IsPC == true)
        {
            return "resident is the player";
        }

        if (chara.IsAliveInCurrentZone == false)
        {
            return "resident is not alive in the current zone";
        }

        if (chara.memberType != FactionMemberType.Default)
        {
            return $"resident member type is {chara.memberType}";
        }

        if (chara.IsDeadOrSleeping == true)
        {
            return "resident is dead, sleeping, or suspended";
        }

        if (chara.IsDisabled == true)
        {
            return "resident is disabled";
        }

        if (chara.IsInCombat == true)
        {
            return "resident is in combat";
        }

        if (chara.HasHost == true)
        {
            return "resident is hosted by another object";
        }

        if (chara.noMove == true)
        {
            return "resident cannot move";
        }

        if (chara.ai is AIWork_Fish)
        {
            return "resident is already fishing";
        }

        if (chara.ai is AI_Eat)
        {
            return "resident is eating";
        }

        return null;
    }

    private static bool TryInviteResidentToFish(Chara chara, Hobby fishing, bool isHobby, Point posWater)
    {
        if (CanInviteResident(chara: chara) == false)
        {
            return false;
        }

        string goalType = isHobby ? "hobby" : "work";
        if (FishingBait.HasPlayerBaitIfRequired() == false)
        {
            LogDebug($"Cannot invite {GetLogName(chara: chara)} for fishing {goalType}: player has no usable bait.");
            return false;
        }

        Goal goal = isHobby ? chara.GetGoalHobby() : chara.GetGoalWork();
        if (goal is not GoalWork goalWork)
        {
            LogDebug($"Cannot invite {GetLogName(chara: chara)} for fishing {goalType}: goal was {goal.GetType().Name}, not GoalWork.");
            return false;
        }

        chara.TryMoveTowards(p: posWater);
        LogDebug($"Trying fishing {goalType} for {GetLogName(chara: chara)} near water {posWater}.");

        if (TryFindWork(goalWork: goalWork, chara: chara, fishing: fishing, setAI: true) == false)
        {
            LogDebug($"No valid fishing destination found for {GetLogName(chara: chara)} using {goalType}.");
            return false;
        }

        LogDebug($"Invited {GetLogName(chara: chara)} to fish using {goalType}; bait will be consumed if the resident catch is delivered.");
        return true;
    }

    private static bool TryFindWork(GoalWork goalWork, Chara chara, Hobby fishing, bool setAI)
    {
        goalWork.owner = chara;
        WorkSummary workSummary = chara.GetWorkSummary();
        WorkSession originalWork = workSummary.work;
        List<WorkSession> originalHobbies = new List<WorkSession>(collection: workSummary.hobbies);
        LogDebug($"Saved work summary for {GetLogName(chara: chara)} before fishing {GetGoalType(goalWork: goalWork)} attempt; work={originalWork != null}, hobbies={originalHobbies.Count}.");

        if (goalWork.IsHobby == true)
        {
            workSummary.hobbies.Clear();
        }
        else
        {
            workSummary.work = null;
        }

        bool foundDestination = goalWork.TryWork(destArea: null, h: fishing, setAI: setAI);
        if (foundDestination == true)
        {
            LogDebug($"Fishing destination accepted for {GetLogName(chara: chara)} from the fishing sign search.");
            return true;
        }

        workSummary.work = originalWork;
        workSummary.hobbies.Clear();
        workSummary.hobbies.AddRange(collection: originalHobbies);
        LogDebug($"Restored work summary for {GetLogName(chara: chara)} after fishing {GetGoalType(goalWork: goalWork)} found no valid destination.");

        return false;
    }

    private static string GetGoalType(GoalWork goalWork)
    {
        return goalWork.IsHobby ? "hobby" : "work";
    }

    private static void LogDebug(string message)
    {
        ResidentsFishWithYou.LogDebug(message: message, caller: nameof(AI_FishProgressFishPatch));
    }

    private static string GetLogName(Chara chara)
    {
        return $"{chara.NameSimple} ({chara.id})";
    }
}
