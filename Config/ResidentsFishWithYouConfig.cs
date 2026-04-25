using BepInEx.Configuration;
using UnityEngine;

namespace ResidentsFishWithYou;

internal static class ResidentsFishWithYouConfig
{
    internal static ConfigEntry<bool> EnableAutoPlaceFishingItems = null!;
    internal static ConfigEntry<bool> EnableRequireBait = null!;
    internal static string TranslationXlsxPath { get; private set; } = string.Empty;

    internal static void LoadConfig(ConfigFile config)
    {
        EnableAutoPlaceFishingItems = config.Bind(
            section: ModInfo.Name,
            key: "Enable Auto Place Fishing Items",
            defaultValue: false,
            description: "Enable or disable automatically placing exact player catches and while-you-are-fishing resident/follower catches into containers whose permission is set to shared after fishing completes.\n" +
                         "Set to 'true' to send tracked caught fishing items to containers whose permission is set to shared, including catches vanilla would otherwise pick up into the player's inventory. Set to 'false' to leave vanilla catch placement alone.\n" +
                         "釣り完了後、プレイヤーの釣果と、あなたが釣りをしている間の住人や仲間の実際の釣果を、権限が「共有中」に設定されているコンテナへ自動配置するかどうかを設定します。\n" +
                         "'true' に設定すると、通常はプレイヤーのインベントリに入る釣果も含めて、追跡された釣果を権限が「共有中」に設定されているコンテナへ送ります。'false' に設定すると通常の釣果配置を維持します。\n" +
                         "启用或禁用在钓鱼完成后自动将玩家的实际收获，以及玩家正在钓鱼时居民或同伴的实际收获放入共享容器。\n" +
                         "设置为 'true' 会将已追踪的钓鱼收获送入共享容器，包括原版通常会放入玩家背包的收获；设置为 'false' 则保留原版放置行为。"
        );

        EnableRequireBait = config.Bind(
            section: ModInfo.Name,
            key: "Enable Resident Require Bait",
            defaultValue: false,
            description: "Enable or disable consuming one player bait when a resident/follower catch is delivered to the player's position.\n" +
                         "Set to 'true' to require bait for resident/follower invites and spend one bait when the resident or follower catch is delivered. Set to 'false' to invite residents/followers and deliver catches without spending bait.\n" +
                         "住人や仲間の釣果がプレイヤーの位置に届けられるときに、プレイヤーの餌を1つ消費するかどうかを設定します。\n" +
                         "'true' に設定すると住人や仲間の招待に餌が必要になり、住人や仲間の釣果が届けられるときに餌を1つ消費します。'false' に設定すると餌を消費せずに住人や仲間を招待し、釣果を届けます。\n" +
                         "启用或禁用在居民或同伴的钓鱼收获送到玩家位置时消耗一个玩家鱼饵。\n" +
                         "设置为 'true' 会要求邀请居民或同伴时有鱼饵，并在居民或同伴收获送达时消耗一个鱼饵；设置为 'false' 则不消耗鱼饵。"
        );
    }

    internal static void InitializeTranslationXlsxPath(string xlsxPath)
    {
        TranslationXlsxPath = xlsxPath;
    }
}
