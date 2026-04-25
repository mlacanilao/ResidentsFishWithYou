namespace ResidentsFishWithYou.Patches;

public static class FishingBait
{
    public static bool HasPlayerBaitIfRequired()
    {
        bool enableRequireBait = ResidentsFishWithYouConfig.EnableRequireBait?.Value ?? false;
        if (enableRequireBait == false)
        {
            return true;
        }

        return TryGetPlayerBait(bait: out _);
    }

    public static bool TryConsumePlayerBaitIfRequired(out int remainingBait)
    {
        remainingBait = 0;

        bool enableRequireBait = ResidentsFishWithYouConfig.EnableRequireBait?.Value ?? false;
        if (enableRequireBait == false)
        {
            return true;
        }

        if (TryGetPlayerBait(bait: out Thing? bait) == false ||
            bait == null)
        {
            return false;
        }

        bait.ModNum(a: -1, notify: true);
        remainingBait = bait.Num;
        return true;
    }

    private static bool TryGetPlayerBait(out Thing? bait)
    {
        bait = null;

        if (EClass.player == null ||
            EClass.pc == null)
        {
            return false;
        }

        EClass.player.TryEquipBait();

        bait = EClass.player.eqBait;
        return bait != null &&
               bait.isDestroyed == false &&
               bait.GetRootCard() == EClass.pc &&
               bait.Num > 0;
    }
}
