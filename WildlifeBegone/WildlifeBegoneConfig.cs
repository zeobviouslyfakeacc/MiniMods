using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace WildlifeBegone {
	internal class WildlifeBegoneConfig {

		internal static WildlifeBegoneConfig Parse(string json) {
			JsonSerializerSettings settings = new JsonSerializerSettings();
			settings.Error += (_, args) => {
				string errorMessagge = args.ErrorContext.Error.Message;
				Debug.LogError("[WildlifeBegone] Couldn't parse the configuration JSON string. Error:\n" + errorMessagge);
			};

			ConfigProxy proxy = JsonConvert.DeserializeObject<ConfigProxy>(json, settings);
			return new WildlifeBegoneConfig(proxy);
		}

		internal readonly bool logging;
		internal readonly RSOSettings rsoSettings;
		internal readonly Dictionary<int, SpawnRateSetting> spawnRates;

		private WildlifeBegoneConfig(ConfigProxy proxy) {
			logging = proxy.Logging;

			rsoSettings = proxy.SpawnerGroups;
			if (rsoSettings == null) {
				Debug.LogError("[WildlifeBegone] Couldn't load the \"SpawnerGroups\" configuration entry, not modifying group spawns.");
				rsoSettings = new RSOSettings();
			}

			if (proxy.SpawnRates == null) {
				Debug.LogError("[WildlifeBegone] Couldn't load the \"SpawnRates\" configuration entry, not modifying spawn settings.");
				proxy.SpawnRates = new Dictionary<string, SpawnRateSetting>();
			}

			Array values = Enum.GetValues(typeof(AiSubType));
			string[] names = Enum.GetNames(typeof(AiSubType));

			spawnRates = new Dictionary<int, SpawnRateSetting>(values.Length);
			for (int i = 0; i < values.Length; ++i) {
				int value = (int) values.GetValue(i);
				string name = names[i];

				if (!proxy.SpawnRates.TryGetValue(name, out SpawnRateSetting setting)) {
					Debug.LogError("[WildlifeBegone] Couldn't find a spawn rate setting for animal type \"" + name + "\". Not modifying spawn rates.");
					setting = new SpawnRateSetting();
				}

				spawnRates[value] = setting;
			}
		}

		private class ConfigProxy {
			public bool Logging = false;
			public RSOSettings SpawnerGroups = null;
			public Dictionary<string, SpawnRateSetting> SpawnRates = null;
		}
	}

	internal class RSOSettings {
		public float RerollActiveSpawnersTimeMultiplier = 1.0f;
		public float ActiveSpawnerCountMultiplier = 1.0f;
	}

	internal class SpawnRateSetting {
		public float SpawnRegionActiveTimeMultiplier = 1.0f;
		public float MaximumRespawnsPerDayMultiplier = 1.0f;
		public float MaximumSpawnedAnimalsMultiplier = 1.0f;
	}
}
