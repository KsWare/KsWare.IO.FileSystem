using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace KsWare.IO.FileSystem.Internal {

	public static class FileManagementApi {

		public static readonly IntPtr NULL=IntPtr.Zero;

		public const int CREATION_DISPOSITION_OPEN_EXISTING = 3;

		public const int FILE_FLAG_BACKUP_SEMANTICS = 0x02000000;

		[DllImport("kernel32.dll", EntryPoint = "CreateFileW", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern SafeFileHandle CreateFile(string lpFileName,
			int dwDesiredAccess,
			int dwShareMode,
			IntPtr securityAttributes,
			int dwCreationDisposition,
			int dwFlagsAndAttributes,
			IntPtr hTemplateFile);

		[DllImport("kernel32.dll", EntryPoint = "CreateFileW", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern SafeFileHandle CreateFile(string lpFileName,
			EFileAccess dwDesiredAccess,
			EFileShare dwShareMode,
			IntPtr lpSecurityAttributes,
			ECreationDisposition dwCreationDisposition,
			EFileAttributes dwFlagsAndAttributes,
			IntPtr hTemplateFile);

		[Flags]
		internal enum EFileAccess : uint {

			GenericRead = 0x80000000,
			GenericWrite = 0x40000000,
			GenericExecute = 0x20000000,
			GenericAll = 0x10000000

		}

		[Flags]
		internal enum EFileShare : uint {

			None = 0x00000000,
			Read = 0x00000001,
			Write = 0x00000002,
			Delete = 0x00000004

		}

		internal enum ECreationDisposition : uint {

			New = 1,
			CreateAlways = 2,
			OpenExisting = 3,
			OpenAlways = 4,
			TruncateExisting = 5

		}

		[Flags]
		internal enum EFileAttributes : uint {

			Readonly = 0x00000001,
			Hidden = 0x00000002,
			System = 0x00000004,
			Directory = 0x00000010,
			Archive = 0x00000020,
			Device = 0x00000040,
			Normal = 0x00000080,
			Temporary = 0x00000100,
			SparseFile = 0x00000200,
			ReparsePoint = 0x00000400,
			Compressed = 0x00000800,
			Offline = 0x00001000,
			NotContentIndexed = 0x00002000,
			Encrypted = 0x00004000,
			WriteThrough = 0x80000000,
			Overlapped = 0x40000000,
			NoBuffering = 0x20000000,
			RandomAccess = 0x10000000,
			SequentialScan = 0x08000000,
			DeleteOnClose = 0x04000000,
			BackupSemantics = 0x02000000,
			PosixSemantics = 0x01000000,
			OpenReparsePoint = 0x00200000,
			OpenNoRecall = 0x00100000,
			FirstPipeInstance = 0x00080000

		}

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool GetFileInformationByHandle(SafeFileHandle hFile,
			out BY_HANDLE_FILE_INFORMATION lpFileInformation);

	}
}
