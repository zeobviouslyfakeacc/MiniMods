using System.Reflection;
using UnityEngine;
using UnityEngine.Analytics;

public class DisableTracking
{
	public static void OnLoad()
	{
		GA_Queue.EndSubmit();
		FieldInfo baseURLField = typeof(GA_Submit).GetField("_baseURL", BindingFlags.NonPublic | BindingFlags.Static);
		baseURLField.SetValue(null, "://localhost");
		Debug.Log("[DisableTracking] Redirecting all tracking events to localhost");

		Analytics.enabled = false;
		Analytics.deviceStatsEnabled = false;
		Analytics.limitUserTracking = true;
		Debug.Log("[DisableTracking] Attempted to limit Unity tracking");
	}
}
