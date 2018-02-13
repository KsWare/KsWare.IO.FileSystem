using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KsWare.IO.FileSystem.Internal {

	public static class VolumeMountPointHelper {

		/// <summary>
		/// Gets a list of drive letters and mounted folder paths for the specified volume.
		/// </summary>
		/// <param name="volumeDeviceName">A volume GUID path for the volume. A volume GUID path is of the form "\\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\".</param>
		/// <returns>List&lt;System.String&gt;.</returns>
		/// <exception cref="Win32Exception"></exception>
		public static List<string> GetMountPointsForVolume(string volumeDeviceName) {
			// GetVolumePathNamesForVolumeName function
			// https://msdn.microsoft.com/en-us/library/windows/desktop/aa364998(v=vs.85).aspx

			var result = new List<string>();

			// GetVolumePathNamesForVolumeName is only available on Windows XP/2003 and above
			var osVersionMajor = Environment.OSVersion.Version.Major;
			var osVersionMinor = Environment.OSVersion.Version.Minor;
			if (osVersionMajor < 5 || (osVersionMajor == 5 && osVersionMinor < 1)) { return result; }

			try {
				uint lpcchReturnLength = 0;
				var buffer = "";

				GetVolumePathNamesForVolumeNameW(volumeDeviceName, buffer, (uint)buffer.Length, ref lpcchReturnLength);
				if (lpcchReturnLength == 0) { return result; }

				buffer = new string(new char[lpcchReturnLength]);

				if (!GetVolumePathNamesForVolumeNameW(volumeDeviceName, buffer, lpcchReturnLength, ref lpcchReturnLength)) { throw new Win32Exception(Marshal.GetLastWin32Error()); }

				var mounts = buffer.Split('\0');
				foreach (var mount in mounts) {
					if (mount.Length > 0) { result.Add(mount); }
				}
			}
			catch (Exception ex) { Console.WriteLine(ex.ToString()); }

			return result;
		}

		private static List<string> GetMountPointsForVolumeTest() {
			return GetMountPointsForVolume(@"\\?\Volume{bd9d0236-4d50-11e3-824f-2cd05a81f67a}\");
		}

		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool GetVolumeNameForVolumeMountPoint(string lpszVolumeMountPoint, [Out] StringBuilder lpszVolumeName, uint cchBufferLength);

		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool SetVolumeMountPoint(string lpszVolumeMountPoint, string lpszVolumeName);

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool GetVolumePathNamesForVolumeNameW(
			[MarshalAs(UnmanagedType.LPWStr)]
			string lpszVolumeName,
			[MarshalAs(UnmanagedType.LPWStr)]
			string lpszVolumePathNames,
			uint cchBuferLength,
			ref UInt32 lpcchReturnLength);

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool RemoveDirectory(string lpPathName);
	}
}
