using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;

namespace Deprox
{
	internal static class ProxyController
	{
		[DllImport("wininet.dll", EntryPoint = "InternetSetOptionA", CharSet = CharSet.Ansi, SetLastError = true, PreserveSig = true)]
		private static extern bool InternetSetOption(IntPtr hInternet, uint dwOption, IntPtr pBuffer, int dwReserved);

		private const string REG_KEY = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Internet Settings";
		private const uint INTERNET_OPTION_REFRESH = 37;
		private const uint INTERNET_OPTION_SETTINGS_CHANGED = 39;

		public static bool Enabled
		{
			get => (int)Registry.GetValue(REG_KEY, "ProxyEnable", false) == 1;
			set
			{
				Registry.SetValue(REG_KEY, "ProxyEnable", value ? 1 : 0, RegistryValueKind.DWord);

				// broadcast proxy change to the system so that browsers can refresh themselves
				InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
				InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
			}
		}

		public static bool Toggle() => Enabled = !Enabled;
	}
}