using System;
using Harmony;
using UnityEngine;

[HarmonyPatch(typeof(SpawnRegion), "Start", new Type[0])]
class SpawnRegionPatch
{
	static void Prefix(SpawnRegion __instance)
	{
		bool m_StartHasBeenCalled = (bool) AccessTools.Field(typeof(SpawnRegion), "m_StartHasBeenCalled").GetValue(__instance);
		if (m_StartHasBeenCalled) return;

		if (__instance.m_SpawnablePrefab.name.ToLowerInvariant().Contains("wolf"))
		{
			AdjustRegion(__instance, 0.1f, 0.1f, 0.7f);
		}
		else
		{
			AdjustRegion(__instance, 0.1f, 0.2f, 0.9f);
		}
	}

	private static void AdjustRegion(SpawnRegion region, float activeModifier, float respawnModifier, float maximumCountModifier)
	{
		float oldChanceActive = region.m_ChanceActive;
		region.m_ChanceActive *= 0.1f;

		float oldRespawnTime = region.m_MaxRespawnsPerDayStalker;
		region.m_MaxRespawnsPerDayPilgrim *= 0.1f;
		region.m_MaxRespawnsPerDayVoyageur *= 0.1f;
		region.m_MaxRespawnsPerDayStalker *= 0.1f;
		region.m_MaxRespawnsPerDayInterloper *= 0.1f;

		int oldMaximumCountDay = region.m_MaxSimultaneousSpawnsDayStalker;
		int oldMaximumCountNight = region.m_MaxSimultaneousSpawnsNightStalker;
		region.m_MaxSimultaneousSpawnsDayPilgrim = Math.Max(1, (int)(region.m_MaxSimultaneousSpawnsDayPilgrim * maximumCountModifier));
		region.m_MaxSimultaneousSpawnsDayVoyageur = Math.Max(1, (int)(region.m_MaxSimultaneousSpawnsDayVoyageur * maximumCountModifier));
		region.m_MaxSimultaneousSpawnsDayStalker = Math.Max(1, (int)(region.m_MaxSimultaneousSpawnsDayStalker * maximumCountModifier));
		region.m_MaxSimultaneousSpawnsDayInterloper = Math.Max(1, (int)(region.m_MaxSimultaneousSpawnsDayInterloper * maximumCountModifier));
		region.m_MaxSimultaneousSpawnsNightPilgrim = Math.Max(1, (int)(region.m_MaxSimultaneousSpawnsNightPilgrim * maximumCountModifier));
		region.m_MaxSimultaneousSpawnsNightVoyageur = Math.Max(1, (int)(region.m_MaxSimultaneousSpawnsNightVoyageur * maximumCountModifier));
		region.m_MaxSimultaneousSpawnsNightStalker = Math.Max(1, (int)(region.m_MaxSimultaneousSpawnsNightStalker * maximumCountModifier));
		region.m_MaxSimultaneousSpawnsNightInterloper = Math.Max(1, (int)(region.m_MaxSimultaneousSpawnsNightInterloper * maximumCountModifier));

		Debug.LogFormat("Adjusted spawner {0}: Active chance {1:F1} -> {2:F1}, respawns / day {3:F2} -> {4:F2}, maximum spawns ({5:D}, {6:D}) -> ({7:D}, {8:D})",
			region.name,
			oldChanceActive, region.m_ChanceActive,
			oldRespawnTime, region.m_MaxRespawnsPerDayStalker,
			oldMaximumCountDay, oldMaximumCountNight, region.m_MaxSimultaneousSpawnsDayStalker, region.m_MaxSimultaneousSpawnsNightStalker
		);
	}
}

[HarmonyPatch(typeof(RandomSpawnObject), "Start", new Type[0])]
class RandomSpawnObjectPatch
{
	static void Prefix(RandomSpawnObject __instance)
	{
		if (__instance.m_RerollAfterGameHours > 0.0)
		{
			float oldRerollTime = __instance.m_RerollAfterGameHours;
			int oldMaxObjects = __instance.m_NumObjectsToEnableStalker;

			__instance.m_RerollAfterGameHours *= 3;
			__instance.m_NumObjectsToEnablePilgrim = 1;
			__instance.m_NumObjectsToEnableVoyageur = 1;
			__instance.m_NumObjectsToEnableStalker = 1;
			__instance.m_NumObjectsToEnableInterloper = 1;

			Debug.LogFormat("Adjusted RSO {0}: Reroll time {1:F1} -> {2:F1}, maximum active {3:D} -> {4:D}",
				__instance.name, oldRerollTime, __instance.m_RerollAfterGameHours, oldMaxObjects, __instance.m_NumObjectsToEnableStalker);
		}
	}
}

[HarmonyPatch(typeof(ConsoleManager), "RegisterCommands", new Type[0])]
class AddConsoleCommands
{
	static void Postfix()
	{
		uConsole.RegisterCommand("animals_count", new uConsole.DebugCommand(CountAnimals));
		uConsole.RegisterCommand("animals_kill_all", new uConsole.DebugCommand(KillAllAnimals));
	}

	private static void CountAnimals()
	{
		int bears = 0;
		int rabbits = 0;
		int deer = 0;
		int wolves = 0;
		int none = 0;

		BaseAi[] animals = UnityEngine.Object.FindObjectsOfType<BaseAi>() as BaseAi[];
		foreach (BaseAi animal in animals)
		{
			if (animal.GetAiMode() == AiMode.Dead) continue;

			switch (animal.m_AiSubType)
			{
				case AiSubType.Bear:
					++bears;
					break;
				case AiSubType.Rabbit:
					++rabbits;
					break;
				case AiSubType.Stag:
					++deer;
					break;
				case AiSubType.Wolf:
					++wolves;
					break;
				case AiSubType.None:
				default:
					++none;
					break;
			}
		}

		Debug.LogFormat("{0} bears, {1} rabbits, {2} deer, {3} wolves, {4} unknown", bears, rabbits, deer, wolves, none);
	}

	private static void KillAllAnimals()
	{
		BaseAi[] animals = UnityEngine.Object.FindObjectsOfType<BaseAi>() as BaseAi[];
		foreach (BaseAi animal in animals)
		{
			if (animal.GetAiMode() != AiMode.Dead)
			{
				animal.SetAiMode(AiMode.Dead);
				animal.Despawn();
			}
		}
	}
}
