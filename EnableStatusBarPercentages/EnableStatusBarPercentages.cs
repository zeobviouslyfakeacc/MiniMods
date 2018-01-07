using Harmony;
using UnityEngine;

[HarmonyPatch(typeof(Panel_FirstAid), "Start")]
public class EnableStatusBarPercentages {
	private static void Postfix(Panel_FirstAid __instance) {
		ActivateAndMove(__instance.m_ColdPercentLabel);
		ActivateAndMove(__instance.m_FatiguePercentLabel);
		ActivateAndMove(__instance.m_ThirstPercentLabel);
		ActivateAndMove(__instance.m_HungerPercentLabel);
	}

	private static void ActivateAndMove(UILabel label) {
		label.pivot = UIWidget.Pivot.Center;
		label.gameObject.transform.localPosition = new Vector3(-79, -16, 0);
		label.fontSize = 14;

		label.enabled = true;
		NGUITools.SetActive(label.gameObject, true);
	}
}
