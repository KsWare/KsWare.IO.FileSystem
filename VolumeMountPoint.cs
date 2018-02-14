using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using KsWare.IO.FileSystem.Internal;
using static KsWare.IO.FileSystem.Internal.VolumeMountPointHelper;
using static KsWare.IO.FileSystem.Internal.WinApi;

namespace KsWare.IO.FileSystem
{

	public static class VolumeMountPoint {

		private const int MaxVolumeNameLength = 100;

		public static void Create(string mountPoint, string volumeName) {
			CheckVolumeName(volumeName, true);

			mountPoint = Path.GetFullPath(mountPoint);

			// https://msdn.microsoft.com/en-us/library/windows/desktop/aa365561(v=vs.85).aspx

			if (!mountPoint.EndsWith("\\")) mountPoint += "\\"; // The mountPoint MUST ends with a backslash

			if (Directory.Exists(mountPoint)) {
				try {
					Directory.Delete(mountPoint);
				}
				catch (Exception ex) { }
				// the folder MUST be empty
				// the folder MUST NOT be a reparse point
			}
			{ // the folder MUST exist
				Directory.CreateDirectory(mountPoint);
			}

			if (Helper.IsElevated) {
				if (!SetVolumeMountPoint(mountPoint, volumeName)) {
					var err = Marshal.GetLastWin32Error();
					throw Helper.IOException($"Unable to create volume mount point '{volumeName}' -> '{mountPoint}'.",
						new Win32Exception(err));
				}
			}
			else {
				var ret = PrivilegedExecutor.Client.ExecuteService(typeof(VolumeMountPoint).FullName +"."+ nameof(SetVolumeMountPointConsole), mountPoint, volumeName);
				if (ret != 0)
					throw Helper.IOException($"Unable to create volume mount point '{volumeName}' -> '{mountPoint}'.",
						new Win32Exception(ret));
			}
		}

		internal static int SetVolumeMountPointConsole(string lpszVolumeMountPoint, string lpszVolumeName) {
			var success = SetVolumeMountPoint(lpszVolumeMountPoint, lpszVolumeName);
			var exitCode= success ? 0 : Marshal.GetLastWin32Error();
			return exitCode;
		}

		public static void Delete(string mountPoint) {
			// The path of the directory to be removed. This path must specify an empty directory, and the calling process must have delete access to the directory.
			// RemoveDirectory removes a directory junction, even if the contents of the target are not empty; the function removes directory junctions regardless of the state of the target object. 
			var path = Internal.PathHelper.LongPathSupport(mountPoint);
			var attr = WinApi.GetFileAttributes(path);
			if ((int) attr == INVALID_FILE_ATTRIBUTES) throw Helper.Win32Exception();

			GetVolumeName(mountPoint); // throws exception

			if (!RemoveDirectory(mountPoint))
				throw Helper.IOExceptionForLastWin32Error($"Error deleting mountpoint. {mountPoint}");
		}

		public static string GetVolumeName(string mountPoint) {
			var path = Path.AddTrailingBackslash(mountPoint);
			var sb   = new StringBuilder(MaxVolumeNameLength);
			if (!GetVolumeNameForVolumeMountPoint(path, sb, (uint) MaxVolumeNameLength)) {
				var err = Marshal.GetLastWin32Error();
				var ex = Helper.Win32Exception(err);
				Debug.WriteLine($"{err}: {ex.Message}");
				throw ex;
				// TODO 
			}
			return sb.ToString();
		}

		public static bool CheckVolumeName(string volumeName, bool throwException=false) {
		    const int MaxVolumeNameLength = 100;
		    var sb = new StringBuilder(MaxVolumeNameLength);
		    var result=GetVolumeNameForVolumeMountPoint(volumeName, sb, (uint) MaxVolumeNameLength);
			if(!result && throwException) Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
		    return result;
	    }

		

	}
}
