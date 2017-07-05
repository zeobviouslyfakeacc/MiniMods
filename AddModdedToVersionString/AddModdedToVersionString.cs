using UnityEngine;

class AddModdedToVersionString
{
	public static void OnLoad()
	{
		GameManager.m_GameVersionString = "Modded " + GameManager.m_GameVersionString;
		Debug.Log(" === This game is MODDED. Do not report any issues to Hinterland! === ");
	}
}
