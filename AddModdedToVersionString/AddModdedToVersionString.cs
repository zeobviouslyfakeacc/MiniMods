using System;
using UnityEngine;
using Harmony;

[HarmonyPatch(typeof(GameManager), "ReadVersionFile", new Type[0])]
class AddModdedToVersionString
{
	static void Postfix()
	{
		GameManager.m_GameVersionString = "Modded " + GameManager.m_GameVersionString;
		Debug.Log(" === This game is MODDED. Do not report any issues to Hinterland! === ");
	}
}
