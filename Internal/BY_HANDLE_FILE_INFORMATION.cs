using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;

namespace KsWare.IO.FileSystem.Internal {

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct BY_HANDLE_FILE_INFORMATION {
		public uint FileAttributes;
		public System.Runtime.InteropServices.ComTypes.FILETIME CreationTime;
		public System.Runtime.InteropServices.ComTypes.FILETIME LastAccessTime;
		public FILETIME LastWriteTime;
		public uint VolumeSerialNumber;
		public uint FileSizeHigh;
		public uint FileSizeLow;
		public uint NumberOfLinks;
		public uint FileIndexHigh;
		public uint FileIndexLow;
	}
}
