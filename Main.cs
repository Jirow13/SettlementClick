using HarmonyLib;
using System;
using System.Text;
using System.Windows.Forms;
using TaleWorlds.MountAndBlade;

namespace ExitSettlement
{
	public class ExceptionHandler
	{
		public static string ToString(Exception ex)
		{
			return GetString(ex);
		}

		private static string GetString(Exception ex)
		{
			StringBuilder stringBuilder = new StringBuilder();

			GetStringRecursive(ex, stringBuilder);

			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Stack trace: ");
			stringBuilder.AppendLine(ex.StackTrace);

			return stringBuilder.ToString();
		}

		private static void GetStringRecursive(Exception ex, StringBuilder sb)
		{
			sb.AppendLine(ex.GetType().Name + ": ");
			sb.AppendLine(ex.Message);
			if (ex.InnerException != null)
			{
				sb.AppendLine();
				GetStringRecursive(ex.InnerException, sb);
			}
		}
	}

	public class ExitSettlementSubModule : MBSubModuleBase
	{
		protected override void OnSubModuleLoad()
		{
			try
			{
				Harmony harmonyPatch = new Harmony("bannerlord.exitsettlement");
				harmonyPatch.PatchAll();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Failed hooking settlement exit on click code\n\n" + ExceptionHandler.ToString(ex));
			}
		}
	}
}
