using System;
using Harmony;

[HarmonyPatch(typeof(Breath), "PlayBreathEffect", new Type[0])]
public class DisableBreathEffect
{
	public static bool Prefix()
	{
		return false;
	}
}
