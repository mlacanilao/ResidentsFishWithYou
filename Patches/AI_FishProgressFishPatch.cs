namespace ResidentsFishWithYou.Patches
{
    public static class AI_FishProgressFishPatch
    {
        public static bool OnStartPrefix(AI_Fish.ProgressFish __instance)
        {
            if (EClass.core?.IsGameStarted == false)
            {
                return true;
            }

            bool enableRequireBait = ResidentsFishWithYouConfig.EnableRequireBait?.Value ?? false;
            
            if (enableRequireBait == true && 
                __instance.owner?.IsPC == false)
            {
                var inventory = EClass.pc?.things;

                if (inventory != null && inventory.Count > 0)
                {
                    Thing bait = inventory.Find<TraitBait>();

                    if (bait != null && bait.Num > 0)
                    {
                        bait.ModNum(a:-1, notify: true);
                    }
                    else
                    {
                        __instance.Cancel();
                        return false;
                    }
                }
            }
            
            if (__instance.owner?.IsPC == false ||
                EClass._zone?.branch is null)
            {
                return true;
            }

            foreach (Chara chara in EClass._zone?.branch?.members)
            {
                if (chara.IsPC == true)
                {
                    continue;
                }
                
                bool hasFishingWork = false;
                bool hasFishingHobby = false;
                
                foreach (Hobby w in chara.ListWorks())
                {
                    if (w.Name.Contains("Fishing") == false)
                    {
                        continue;
                    }
                    hasFishingWork = true;
                }
                
                foreach (Hobby h in chara.ListHobbies())
                {
                    if (h.Name.Contains("Fishing") == false)
                    {
                        continue;
                    }
                    hasFishingHobby = true;
                }

                if (hasFishingWork == true &&
                    chara.ai is AIWork_Fish == false &&
                    chara.ai is AI_Eat == false)
                {
                    chara.TryMoveTowards(__instance.posWater);
                    
                    Goal goal = chara.GetGoalWork();
                    if (goal is GoalWork goalWork)
                    {
                        goalWork.FindWork(chara);
                    }
                }

                if (hasFishingHobby == true &&
                    chara.ai is AIWork_Fish == false &&
                    chara.ai is AI_Eat == false)
                {
                    chara.TryMoveTowards(__instance.posWater);

                    Goal goal = chara.GetGoalHobby();
                    if (goal is GoalHobby goalHobby)
                    {
                        goalHobby.FindWork(chara);
                    }
                }
            }

            return true;
        }
    }
}