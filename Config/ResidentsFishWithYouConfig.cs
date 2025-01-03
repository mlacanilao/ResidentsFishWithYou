using BepInEx.Configuration;
using UnityEngine;

namespace ResidentsFishWithYou
{
    internal static class ResidentsFishWithYouConfig
    {
        internal static ConfigEntry<bool> EnableAutoPlaceFishingItems;
        internal static ConfigEntry<bool> EnableRequireBait;

        internal static void LoadConfig(ConfigFile config)
        {
            EnableAutoPlaceFishingItems = config.Bind(
                section: ModInfo.Name,
                key: "Enable Auto Place Fishing Items",
                defaultValue: false,
                description: "Enable or disable automatically placing fishing items into shared containers.\n" +
                             "Set to 'true' to enable automatic placement, or 'false' to disable it.\n" +
                             "釣りアイテムを共有コンテナに自動的に配置する機能を有効または無効にします。\n" +
                             "'true' に設定すると自動配置が有効になり、'false' に設定すると無効になります。\n" +
                             "启用或禁用自动将钓鱼物品放入共享容器。\n" +
                             "设置为 'true' 以启用自动放置，或设置为 'false' 禁用此功能。"
            );
            
            EnableRequireBait = config.Bind(
                section: ModInfo.Name,
                key: "Enable Resident Require Bait",
                defaultValue: false,
                description: "Enable or disable requiring residents to use the player's bait to start fishing.\n" +
                             "Set to 'true' to require bait, or 'false' to allow fishing without bait.\n" +
                             "住人が釣りを開始する際にプレイヤーの餌を使用するかどうかを設定します。\n" +
                             "'true' に設定すると餌が必要になり、'false' に設定すると餌がなくても釣りが可能です。\n" +
                             "启用或禁用要求居民使用玩家的鱼饵才能开始钓鱼。\n" +
                             "设置为 'true' 以需要鱼饵，或设置为 'false' 允许无鱼饵钓鱼。"
            );
        }
    }
}