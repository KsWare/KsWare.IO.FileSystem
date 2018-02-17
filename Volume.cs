using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using KsWare.IO.FileSystem.Internal;

namespace KsWare.IO.FileSystem {

	public static class Volume {

		/// <summary>
		/// Gets a list of drive letters and mounted folder paths for the specified volume.
		/// </summary>
		/// <param name="volumeDeviceName">A volume GUID path for the volume. A volume GUID path is of the form "\\?\Volume{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}\".</param>
		/// <returns>List&lt;System.String&gt;.</returns>
		/// <exception cref="Win32Exception"></exception>
		public static string[] GetMountPoints(string volumeDeviceName) {
			// GetVolumePathNamesForVolumeName function
			// https://msdn.microsoft.com/en-us/library/windows/desktop/aa364998(v=vs.85).aspx

			// support for path names. like C:\ or C:\mnt\hd1
			if (volumeDeviceName.Length >= 2 && volumeDeviceName[1] == ':') {
				volumeDeviceName = VolumeMountPoint.GetVolumeName(volumeDeviceName);
			}

			var result = new List<string>();

			// GetVolumePathNamesForVolumeName is only available on Windows XP/2003 and above
			var osVersionMajor = Environment.OSVersion.Version.Major;
			var osVersionMinor = Environment.OSVersion.Version.Minor;
			if (osVersionMajor < 5 || (osVersionMajor == 5 && osVersionMinor < 1)) {
				return result.ToArray();
			}

			try {
				uint lpcchReturnLength = 0;
				var  buffer            = "";

				GetVolumePathNamesForVolumeNameW(volumeDeviceName, buffer, (uint) buffer.Length, ref lpcchReturnLength);
				if (lpcchReturnLength == 0) {
					return result.ToArray();
				}

				buffer = new string(new char[lpcchReturnLength]); // buffer = "Y:\\\0Y:\\ReadTests\\Volume\\\0\0"

				if (!GetVolumePathNamesForVolumeNameW(volumeDeviceName, buffer, lpcchReturnLength, ref lpcchReturnLength)) {
					throw new Win32Exception(Marshal.GetLastWin32Error());
				}

				var mounts = buffer.Split(new[]{'\0'},StringSplitOptions.RemoveEmptyEntries);
				foreach (var mount in mounts) {
					if (mount.Length > 0) {
						result.Add(mount);
					}
				}
			}
			catch (Exception ex) {
				Console.WriteLine(ex.ToString());
			}

			return result.ToArray();
		}

		/// <summary>Determines whether the specified file exists.</summary>
		/// <param name="volumeName">The volume name to check. </param>
		/// <returns>
		/// <see langword="true" /> if the caller has the required permissions and <paramref name="volumeName" /> contains the name of an existing volume; otherwise, <see langword="false" />. This method also returns <see langword="false" /> if <paramref name="volumeName" /> is <see langword="null" />, an invalid path, or a zero-length string. If the caller does not have sufficient permissions to read the specified file, no exception is thrown and the method returns <see langword="false" /> regardless of the existence of <paramref name="volumeName" />.</returns>
		public static bool Exists(string volumeName) {
			/* if the caller has the required permissions
			 * contains the name of an existing volume;
			 * else false
			 * "volumeName" == null" or a zero-length string. 
			 * an invalid path,  
			 * the caller does not have sufficient permissions to read the specified file, no exception is thrown regardless of the existence of "volumeName"
			 * */
			if (string.IsNullOrEmpty(volumeName)) return false;
			if (!PathHelper.StartsWithVolumeName(volumeName)) return false;
			var mps = GetMountPoints(volumeName);
			if (mps.Length == 0) return false; // TODO revise if there is no mountpoint it could be exist nevertheless.
			return Directory.Exists(mps[0]);
		}


		#region winapi

		// https://msdn.microsoft.com/de-de/library/windows/desktop/aa364998.aspx
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool GetVolumePathNamesForVolumeNameW(
			[MarshalAs(UnmanagedType.LPWStr)] string lpszVolumeName,
			[MarshalAs(UnmanagedType.LPWStr)] string lpszVolumePathNames,
			uint cchBuferLength,
			ref UInt32 lpcchReturnLength);

		/// <summary>
		/// The GetDriveType function determines whether a disk drive is a removable, fixed, CD-ROM, RAM disk, or network drive
		/// </summary>
		/// <param name="lpRootPathName">A pointer to a null-terminated string that specifies the root directory and returns information about the disk.A trailing backslash is required. If this parameter is NULL, the function uses the root of the current directory.</param>
		[DllImport("kernel32.dll")]
		public static extern DriveType GetDriveType([MarshalAs(UnmanagedType.LPStr)] string lpRootPathName);

		#endregion
	}

}
 