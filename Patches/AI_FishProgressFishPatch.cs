namespace ResidentsFishWithYou.Patches
{
    public static class AI_FishProgressFishPatch
    {
        public static void OnStartPrefix(AI_Fish.ProgressFish __instance)
        {
            if (__instance.owner?.IsPC == false)
            {
                return;
            }

            if (EClass._zone is null)
            {
                return;
            }

            if (EClass._zone?.branch is null)
            {
                return;
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
                    chara.ai is AIWork_Fish == false)
                {
                    chara.TryMoveTowards(__instance.posWater);
                    
                    Goal goal = chara.GetGoalWork();
                    if (goal is GoalWork goalWork)
                    {
                        goalWork.FindWork(chara);
                    }
                }

                if (hasFishingHobby == true &&
                    chara.ai is AIWork_Fish == false)
                {
                    chara.TryMoveTowards(__instance.posWater);

                    Goal goal = chara.GetGoalHobby();
                    if (goal is GoalHobby goalHobby)
                    {
                        goalHobby.FindWork(chara);
                    }
                }
            }
        }
    }
}