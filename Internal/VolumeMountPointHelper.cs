using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KsWare.IO.FileSystem.Internal {

	public static class VolumeMountPointHelper {

		// Retrieves a volume GUID path for the volume that is associated with the specified volume mount point ( drive letter, volume GUID path, or mounted folder).
		// https://msdn.microsoft.com/de-de/library/windows/desktop/aa364994(v=vs.85).aspx
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool GetVolumeNameForVolumeMountPoint(string lpszVolumeMountPoint, [Out] StringBuilder lpszVolumeName, uint cchBufferLength);

		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool SetVolumeMountPoint(string lpszVolumeMountPoint, string lpszVolumeName);


		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool RemoveDirectory(string lpPathName);
	}
}
