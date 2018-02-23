using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.CrashReportHandler;
using UnityEngine.Networking.PlayerConnection;

public static class DisableTracking {

	public static void OnLoad() {
		GA_Queue.EndSubmit();
		FieldInfo baseURLField = typeof(GA_Submit).GetField("_baseURL", BindingFlags.NonPublic | BindingFlags.Static);
		baseURLField.SetValue(null, "://localhost");

		Analytics.enabled = false;
		Analytics.deviceStatsEnabled = false;
		Analytics.limitUserTracking = true;
		CrashReportHandler.enableCaptureExceptions = false;

		Assembly unityConnectModule = typeof(RemoteSettings).Assembly;
		SetInternalProperty(unityConnectModule, "UnityEngine.Connect.UnityConnectSettings", "enabled", false);
		SetInternalProperty(unityConnectModule, "UnityEngine.Advertisements.UnityAdsSettings", "enabled", false);

		PlayerConnection.instance.DisconnectAll();
		Debug.Log("[DisableTracking] Disabled GameAnalytics and limited UnityEngine tracking");
	}

	private static void SetInternalProperty(Assembly assembly, string className, string propertyName, object value) {
		try {
			Type type = assembly.GetType(className, true);
			PropertyInfo prop = type.GetProperty(propertyName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			prop.SetValue(null, value, null);
		} catch (Exception ex) {
			Debug.LogError("[DisableTracking] Could not set property '" + propertyName + "' of class '" + className + "'");
			Debug.LogException(ex);
		}
	}
}
