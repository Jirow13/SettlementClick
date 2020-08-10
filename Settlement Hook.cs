using HarmonyLib;
using SandBox.View.Map;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameMenus;

namespace ExitSettlement
{
	[HarmonyPatch(typeof(MapScreen), "HandleLeftMouseButtonClick")]
	public class MapScreenHandleLeftMouseButtonClickPatch
	{
		private static AccessTools.FieldRef<MapScreen, MapState> mapState = AccessTools.FieldRefAccess<MapScreen, MapState>("_mapState");

		private static void Prefix(MapScreen __instance, PathFaceRecord mouseOverFaceIndex)
		{
			if (!mouseOverFaceIndex.IsValid())
				return;

			if (PlayerSiege.PlayerSiegeEvent != null || MobileParty.MainParty.SiegeEvent != null)
				return;

			if (mapState(__instance) != null && mapState(__instance).AtMenu)
			{
				Settlement currentSettlement = MobileParty.MainParty.CurrentSettlement;
				if (currentSettlement != null && (currentSettlement.IsCastle || currentSettlement.IsFortification || currentSettlement.IsTown || currentSettlement.IsVillage))
				{
					PlayerEncounter.LeaveSettlement();
					GameMenu.ExitToLast();
					PlayerEncounter.Finish(true);
				}
				else
				if (MobileParty.MainParty.TargetSettlement != null)
				{
					PlayerEncounter.Finish(true);
				}
			}
		}
	}
}
