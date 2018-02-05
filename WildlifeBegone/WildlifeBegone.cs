using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace WildlifeBegone {
	internal class WildlifeBegone {

		private const string configFileName = "WildlifeBegoneConfig.json";

		private static WildlifeBegoneConfig config = null;
		internal static WildlifeBegoneConfig Config {
			get {
				return config;
			}
		}

		public static void OnLoad() {
			string modsDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string configPath = Path.Combine(modsDir, configFileName);
			if (!File.Exists(configPath))
				CopyDefaultConfigFile(configPath);

			string configJson = File.ReadAllText(configPath, Encoding.UTF8);
			config = WildlifeBegoneConfig.Parse(configJson);
		}

		private static void CopyDefaultConfigFile(string configPath) {
			Debug.LogError("[WildlifeBegone] Could not find config file - copying default file.");

			byte[] configBytes = Properties.Resources.WildlifeBegoneConfig;
			File.WriteAllBytes(configPath, configBytes);
		}
	}
}
