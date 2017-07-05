using System;
using System.Collections.Generic;
using Harmony;

static class RememberBreakDownItem
{
	private static readonly Dictionary<string, int> rememberedToolIDs = new Dictionary<string, int>();
	private static int lastUsedID = -1;

	[HarmonyPatch(typeof(Panel_BreakDown), "Update", new Type[0])]
	static class KeyboardShortcuts
	{
		static void Postfix(Panel_BreakDown __instance)
		{
			if (!__instance.IsEnabled() || InterfaceManager.ShouldImmediatelyExitOverlay()
				|| InputManager.GetEscapePressed() || __instance.IsBreakingDown()) return;

			int framesInPanel = (int)AccessTools.Field(typeof(Panel_BreakDown), "m_FramesInPanel").GetValue(__instance);
			if (framesInPanel > 1 && Utils.IsMouseActive())
			{
				float movement = Utils.GetMenuMovementHorizontal(true, true);
				if (movement < 0)
				{
					__instance.OnPrevTool();
				}
				else if (movement > 0)
				{
					__instance.OnNextTool();
				}
				else if (InputManager.GetRadialButton())
				{
					__instance.OnBreakDown();
				}
			}
		}
	}

	[HarmonyPatch(typeof(Panel_BreakDown), "OnBreakDown", new Type[0])]
	static class Store
	{
		static void Prefix(Panel_BreakDown __instance)
		{
			BreakDown breakDown = __instance.m_BreakDown;
			if (breakDown == null) return;
			string breakDownName = breakDown.m_LocalizedDisplayName.m_LocalizationID;

			GearItem selectedTool = (GearItem)AccessTools.Method(typeof(Panel_BreakDown), "GetSelectedTool").Invoke(__instance, new object[0]);
			int instanceId = (selectedTool == null) ? 0 : selectedTool.m_InstanceID;
			string toolName = (selectedTool == null) ? "<hands>" : selectedTool.m_DisplayName;

			rememberedToolIDs[breakDownName] = instanceId;
			lastUsedID = instanceId;
		}
	}

	[HarmonyPatch(typeof(Panel_BreakDown), "MakeDefaultSelections", new Type[0])]
	static class Load
	{
		static void Postfix(Panel_BreakDown __instance)
		{
			BreakDown breakDown = __instance.m_BreakDown;
			if (breakDown == null) return;
			string breakDownName = breakDown.m_LocalizedDisplayName.m_LocalizationID;

			if (rememberedToolIDs.ContainsKey(breakDownName))
			{
				Use(__instance, rememberedToolIDs[breakDownName]);
			}
			else
			{
				Use(__instance, lastUsedID);
			}
		}
	}

	private static void Use(Panel_BreakDown panel, int toolInstanceID)
	{
		if (toolInstanceID == 0)
		{
			AccessTools.Field(typeof(Panel_BreakDown), "m_SelectedToolItemIndex").SetValue(panel, 0);
			return;
		}

		List<GearItem> tools = (List<GearItem>)AccessTools.Field(typeof(Panel_BreakDown), "m_Tools").GetValue(panel);
		int index = -1;
		for (int i = 0; i < tools.Count; ++i)
		{
			GearItem tool = tools[i];
			if (tool != null && tool.m_InstanceID == toolInstanceID)
			{
				index = i;
				break;
			}
		}
		if (index == -1) return;

		AccessTools.Field(typeof(Panel_BreakDown), "m_SelectedToolItemIndex").SetValue(panel, index);
	}
}
