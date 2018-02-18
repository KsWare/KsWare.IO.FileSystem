using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace KsWare.IO.FileSystem.Internal {

	public static class WinApi {

		public const int MAX_PATH = 260; // but is 256!?

		/// <summary>
		/// If the <see cref="GetFileAttributes"/> function fails, the return value is INVALID_FILE_ATTRIBUTES. To get extended error information, call <see cref="Marshal.GetLastWin32Error"/>.
		/// </summary>
		public static int INVALID_FILE_ATTRIBUTES = -1;

		public const int CREATION_DISPOSITION_OPEN_EXISTING = 3;

		public const int FILE_FLAG_BACKUP_SEMANTICS = 0x02000000;

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern FILE_ATTRIBUTE GetFileAttributes(string lpFileName);
		
		[DllImport("kernel32.dll", EntryPoint = "CreateFileW", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern SafeFileHandle CreateFile(string lpFileName,
			int dwDesiredAccess,
			int dwShareMode,
			IntPtr SecurityAttributes,
			int dwCreationDisposition,
			int dwFlagsAndAttributes,
			IntPtr hTemplateFile);


	}
}
