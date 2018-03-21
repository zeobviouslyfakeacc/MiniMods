using System;
using Harmony;

public static class DisableTracking {

	[HarmonyPatch(typeof(Utils), "SendAnalyticsEvent", new Type[] { typeof(string), typeof(int) })]
	private static class DisableAnalytics1 {
		private static bool Prefix() {
			return false;
		}
	}

	[HarmonyPatch(typeof(Utils), "SendAnalyticsEvent", new Type[] { typeof(string), typeof(string) })]
	private static class DisableAnalytics2 {
		private static bool Prefix() {
			return false;
		}
	}

	[HarmonyPatch(typeof(Utils), "SendAnalyticsEvent", new Type[] { typeof(string), typeof(string), typeof(string) })]
	private static class DisableAnalytics3 {
		private static bool Prefix() {
			return false;
		}
	}

	[HarmonyPatch(typeof(Utils), "SendAnalyticsBucketEvent", new Type[] { typeof(string), typeof(float), typeof(int[]) })]
	private static class DisableAnalytics4 {
		private static bool Prefix() {
			return false;
		}
	}
}
