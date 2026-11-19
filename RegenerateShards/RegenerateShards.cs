using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using GlobalSettings;
using HarmonyLib;
using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using UnityEngine;

[BepInPlugin("com.slimybonesjones.regenerateshards", "Regenerate Shards", "1.0.0")]
public class RegenerateShards : BaseUnityPlugin
{

    private ConfigEntry<int> configShardIncrement;
    private ConfigEntry<int> configTimePeriod;
    private ConfigEntry<bool> configPendantRequired;
    
    public static float timer = 0f;
    public static bool flag = false;
    private void Awake()
    {
        configShardIncrement = Config.Bind("General", "ShardIncrement", 5, 
            "The number of shards gained per increment");
        configTimePeriod = Config.Bind("General", "TimePeriod", 30,
            "The amount of time passed in seconds before shards are incremented. Takes Integer Values.");
        configPendantRequired = Config.Bind("General", "PendantRequired", false, 
            "Decides if Shard Pendant is required for the effect to work.");

        Logger.LogInfo("Plugin loaded and initialised");

        Harmony.CreateAndPatchAll(typeof(RegenerateShards), null);
    }

    private void Update()
    {
        if (!(configPendantRequired.Value == true && ToolItemManager.IsToolEquipped("Bone Necklace") == false))
        {
            if (CurrencyManager.GetCurrencyAmount(CurrencyType.Shard) < Gameplay.GetCurrencyCap(CurrencyType.Shard))
            {
                timer += Time.deltaTime;

                if (timer >= configTimePeriod.Value)
                {
                    CurrencyManager.AddShards(configShardIncrement.Value);
                    timer = 0;
                }
            }
        }
    }
}