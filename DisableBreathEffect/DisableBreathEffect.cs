using System;
using Harmony;

[HarmonyPatch(typeof(Breath), "PlayBreathEffect", new Type[0])]
public class DisableBreathEffect
{
	public static bool Prefix(Breath __instance)
	{
		bool suppress = (bool) AccessTools.Method(typeof(Breath), "ShouldSuppressBreathEffect").Invoke(__instance, new object[0]);
		if (suppress) return false;

		HeavyBreathingState heavyBreathingState = GameManager.GetFatigueComponent().GetHeavyBreathingState();
		if (heavyBreathingState == HeavyBreathingState.Light)
		{
			GameAudioManager.PlaySound(AK.EVENTS.PLAY_VOBREATHELOWINTENSITYNOLOOP, GameManager.GetPlayerObject());
		}
		else if (heavyBreathingState == HeavyBreathingState.Medium)
		{
			GameAudioManager.PlaySound(AK.EVENTS.PLAY_VOBREATHMEDIUMINTENSITYNOLOOP, GameManager.GetPlayerObject());
		}
		else if (heavyBreathingState == HeavyBreathingState.Heavy)
		{
			GameAudioManager.PlaySound(AK.EVENTS.PLAY_VOBREATHHIGHINTENSITYNOLOOP, GameManager.GetPlayerObject());
		}
		return false;
	}
}
