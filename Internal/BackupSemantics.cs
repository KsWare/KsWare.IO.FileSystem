using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;


namespace KsWare.IO.FileSystem.Internal {

	public static class BackupSemantics {

		public static bool SetPrivilege(IntPtr hToken, string lpszPrivilege, bool bEnablePrivilege) {
			const uint ERROR_NOT_ALL_ASSIGNED = 1300;
			const uint SE_PRIVILEGE_ENABLED = 0x2;

			var luid = new LUID();
			if (!LookupPrivilegeValue(null, lpszPrivilege, ref luid)) {
				System.Console.WriteLine(new Win32Exception(Marshal.GetLastWin32Error()).Message);
				return false;
			}

			var tp = new TOKEN_PRIVILEGES1 {
				PrivilegeCount = 1,
				Privileges = {
					Luid = luid,
					Attributes = bEnablePrivilege ? (int) SE_PRIVILEGE_ENABLED : 0
				}
			};

			// Enable the privilege or disable all privileges.
			if (!AdjustTokenPrivileges(hToken, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero)) {
				var err = Marshal.GetLastWin32Error();
				switch (err) {
					case (int)ERROR_NOT_ALL_ASSIGNED:
						System.Console.WriteLine("The token does not have the specified privilege.");
						return false;
					default:
						System.Console.WriteLine(new Win32Exception(err).Message);
						return false;
				}
			}
			return true;
		}

		public static SafeFileHandle OpenBackupRead(string fileName, bool securityInformation) {
			// To get the security info using the BackupRead function, you have to pass 
			// the READ_CONTROL as the dwDesiredAccess parameter of CreateFile. 
			// See http://support.microsoft.com/kb/240184 for more details.

			var hFile = CreateFile(
				fileName,
				EFileAccess.FILE_GENERIC_READ /*| EFileAccess.ACCESS_SYSTEM_SECURITY*/ | EFileAccess.READ_CONTROL,
				EFileShare.None,
				IntPtr.Zero,
				ECreationDisposition.OPEN_EXISTING,
				EFileAttributes.FILE_FLAG_BACKUP_SEMANTICS,
				IntPtr.Zero
			);
			if (hFile.IsInvalid)
				throw Helper.Win32Exception(Marshal.GetLastWin32Error(), $"{nameof(OpenBackupRead)}.CreateFile", fileName);
			return hFile;
		}

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool CreateDirectory(string lpPathName, IntPtr lpSecurityAttributes);

		static void CreateDirectoryManaged(string lpPathName) {
			var path = lpPathName.Length > 248 ? lpPathName = @"\\?\" : lpPathName;
			if (!CreateDirectory(path, IntPtr.Zero))
				throw Helper.Win32Exception(Marshal.GetLastWin32Error(), nameof(CreateDirectory), lpPathName);
		}

		public static SafeFileHandle OpenBackupWrite(string fileName, bool securityInformation) {
			SafeFileHandle hFile;
			if (fileName.EndsWith("\\") || fileName.EndsWith("/")) {
				fileName = fileName.Substring(0, fileName.Length - 1);
				CreateDirectoryManaged(fileName);
				hFile = CreateFile(
					@"\\?\" + fileName,
					EFileAccess.GENERIC_WRITE | EFileAccess.WRITE_DAC | EFileAccess.WRITE_OWNER,
					EFileShare.None,
					//					EFileShare.Read|EFileShare.Write,
					IntPtr.Zero,
					ECreationDisposition.OPEN_EXISTING,
					EFileAttributes.FILE_ATTRIBUTE_DIRECTORY | EFileAttributes.FILE_FLAG_BACKUP_SEMANTICS,
					IntPtr.Zero
				);
			}
			else {
				hFile = CreateFile(
					@"\\?\" + fileName,
					EFileAccess.GENERIC_WRITE | EFileAccess.WRITE_DAC | EFileAccess.WRITE_OWNER,
					EFileShare.None,
					IntPtr.Zero,
					ECreationDisposition.CREATE_ALWAYS,
					//				ECreationDisposition.OPEN_ALWAYS, 
					EFileAttributes.FILE_ATTRIBUTE_NORMAL | EFileAttributes.FILE_FLAG_BACKUP_SEMANTICS | EFileAttributes.FILE_FLAG_SEQUENTIAL_SCAN,
					IntPtr.Zero
				);
			}
			if (hFile.IsInvalid)
				throw Helper.Win32Exception(Marshal.GetLastWin32Error(), $"{nameof(OpenBackupWrite)}.CreateFile", fileName);
			return hFile;
		}

		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool OpenProcessToken(IntPtr processHandle, uint desiredAccess, out IntPtr tokenHandle);

		private struct LUID {

			public uint LowPart;

			public uint HighPart;

		}

		private struct LUID_AND_ATTRIBUTES {

			public LUID Luid;

			public int Attributes;

		}

		private struct TOKEN_PRIVILEGES1 {

			public int PrivilegeCount;

			public LUID_AND_ATTRIBUTES Privileges;

		}

		private const Int32 ANYSIZE_ARRAY = 1;

		private struct TOKEN_PRIVILEGES {
			public UInt32 PrivilegeCount;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = ANYSIZE_ARRAY)]
			public LUID_AND_ATTRIBUTES[] Privileges;
		}

		[DllImport("advapi32.dll", SetLastError = true)]
		static extern bool LookupPrivilegeValue(string lpSystemName, string lpName, ref LUID lpLuid);

		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool AdjustTokenPrivileges(
			IntPtr tokenHandle,
			[MarshalAs(UnmanagedType.Bool)]bool disableAllPrivileges,
			ref TOKEN_PRIVILEGES newState,
			UInt32 bufferLengthInBytes,
			ref TOKEN_PRIVILEGES previousState,
			out UInt32 returnLengthInBytes);

		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool AdjustTokenPrivileges(
			IntPtr tokenHandle,
			[MarshalAs(UnmanagedType.Bool)] bool disableAllPrivileges,
			ref TOKEN_PRIVILEGES1 newState,
			UInt32 null1,
			IntPtr zero1,
			IntPtr zero2);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr GetCurrentProcess();

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool CloseHandle(IntPtr hObject);

		/// <summary>
		/// Gets the security entity value.
		/// </summary>
		/// <param name="securityEntity">The security entity.</param>
		private static string GetSecurityEntityValue(SecurityEntity securityEntity) {
			switch (securityEntity) {
				case SecurityEntity.SE_ASSIGNPRIMARYTOKEN_NAME:
					return "SeAssignPrimaryTokenPrivilege";
				case SecurityEntity.SE_AUDIT_NAME:
					return "SeAuditPrivilege";
				case SecurityEntity.SE_BACKUP_NAME:
					return "SeBackupPrivilege";
				case SecurityEntity.SE_CHANGE_NOTIFY_NAME:
					return "SeChangeNotifyPrivilege";
				case SecurityEntity.SE_CREATE_GLOBAL_NAME:
					return "SeCreateGlobalPrivilege";
				case SecurityEntity.SE_CREATE_PAGEFILE_NAME:
					return "SeCreatePagefilePrivilege";
				case SecurityEntity.SE_CREATE_PERMANENT_NAME:
					return "SeCreatePermanentPrivilege";
				case SecurityEntity.SE_CREATE_SYMBOLIC_LINK_NAME:
					return "SeCreateSymbolicLinkPrivilege";
				case SecurityEntity.SE_CREATE_TOKEN_NAME:
					return "SeCreateTokenPrivilege";
				case SecurityEntity.SE_DEBUG_NAME:
					return "SeDebugPrivilege";
				case SecurityEntity.SE_ENABLE_DELEGATION_NAME:
					return "SeEnableDelegationPrivilege";
				case SecurityEntity.SE_IMPERSONATE_NAME:
					return "SeImpersonatePrivilege";
				case SecurityEntity.SE_INC_BASE_PRIORITY_NAME:
					return "SeIncreaseBasePriorityPrivilege";
				case SecurityEntity.SE_INCREASE_QUOTA_NAME:
					return "SeIncreaseQuotaPrivilege";
				case SecurityEntity.SE_INC_WORKING_SET_NAME:
					return "SeIncreaseWorkingSetPrivilege";
				case SecurityEntity.SE_LOAD_DRIVER_NAME:
					return "SeLoadDriverPrivilege";
				case SecurityEntity.SE_LOCK_MEMORY_NAME:
					return "SeLockMemoryPrivilege";
				case SecurityEntity.SE_MACHINE_ACCOUNT_NAME:
					return "SeMachineAccountPrivilege";
				case SecurityEntity.SE_MANAGE_VOLUME_NAME:
					return "SeManageVolumePrivilege";
				case SecurityEntity.SE_PROF_SINGLE_PROCESS_NAME:
					return "SeProfileSingleProcessPrivilege";
				case SecurityEntity.SE_RELABEL_NAME:
					return "SeRelabelPrivilege";
				case SecurityEntity.SE_REMOTE_SHUTDOWN_NAME:
					return "SeRemoteShutdownPrivilege";
				case SecurityEntity.SE_RESTORE_NAME:
					return "SeRestorePrivilege";
				case SecurityEntity.SE_SECURITY_NAME:
					return "SeSecurityPrivilege";
				case SecurityEntity.SE_SHUTDOWN_NAME:
					return "SeShutdownPrivilege";
				case SecurityEntity.SE_SYNC_AGENT_NAME:
					return "SeSyncAgentPrivilege";
				case SecurityEntity.SE_SYSTEM_ENVIRONMENT_NAME:
					return "SeSystemEnvironmentPrivilege";
				case SecurityEntity.SE_SYSTEM_PROFILE_NAME:
					return "SeSystemProfilePrivilege";
				case SecurityEntity.SE_SYSTEMTIME_NAME:
					return "SeSystemtimePrivilege";
				case SecurityEntity.SE_TAKE_OWNERSHIP_NAME:
					return "SeTakeOwnershipPrivilege";
				case SecurityEntity.SE_TCB_NAME:
					return "SeTcbPrivilege";
				case SecurityEntity.SE_TIME_ZONE_NAME:
					return "SeTimeZonePrivilege";
				case SecurityEntity.SE_TRUSTED_CREDMAN_ACCESS_NAME:
					return "SeTrustedCredManAccessPrivilege";
				case SecurityEntity.SE_UNDOCK_NAME:
					return "SeUndockPrivilege";
				default:
					throw new ArgumentOutOfRangeException(typeof(SecurityEntity).Name);
			}
		}

		public enum SecurityEntity {
			SE_CREATE_TOKEN_NAME,
			SE_ASSIGNPRIMARYTOKEN_NAME,
			SE_LOCK_MEMORY_NAME,
			SE_INCREASE_QUOTA_NAME,
			SE_UNSOLICITED_INPUT_NAME,
			SE_MACHINE_ACCOUNT_NAME,
			SE_TCB_NAME,
			SE_SECURITY_NAME,
			SE_TAKE_OWNERSHIP_NAME,
			SE_LOAD_DRIVER_NAME,
			SE_SYSTEM_PROFILE_NAME,
			SE_SYSTEMTIME_NAME,
			SE_PROF_SINGLE_PROCESS_NAME,
			SE_INC_BASE_PRIORITY_NAME,
			SE_CREATE_PAGEFILE_NAME,
			SE_CREATE_PERMANENT_NAME,
			SE_BACKUP_NAME,
			SE_RESTORE_NAME,
			SE_SHUTDOWN_NAME,
			SE_DEBUG_NAME,
			SE_AUDIT_NAME,
			SE_SYSTEM_ENVIRONMENT_NAME,
			SE_CHANGE_NOTIFY_NAME,
			SE_REMOTE_SHUTDOWN_NAME,
			SE_UNDOCK_NAME,
			SE_SYNC_AGENT_NAME,
			SE_ENABLE_DELEGATION_NAME,
			SE_MANAGE_VOLUME_NAME,
			SE_IMPERSONATE_NAME,
			SE_CREATE_GLOBAL_NAME,
			SE_CREATE_SYMBOLIC_LINK_NAME,
			SE_INC_WORKING_SET_NAME,
			SE_RELABEL_NAME,
			SE_TIME_ZONE_NAME,
			SE_TRUSTED_CREDMAN_ACCESS_NAME
		}

		/*
CreateFile Function
http://msdn.microsoft.com/en-us/library/aa363858(VS.85).aspx
BackupRead Function
http://msdn.microsoft.com/en-us/library/aa362509(VS.85).aspx
BackupWrite Function
http://msdn.microsoft.com/en-us/library/aa362511(VS.85).aspx

SACL Access Right
http://msdn.microsoft.com/en-us/library/aa379321(VS.85).aspx

Privilege Constants
http://msdn.microsoft.com/en-us/library/bb530716(VS.85).aspx

Note: You can ignore the below if you don't care about encrypted files.

OpenEncryptedFileRaw Function
http://msdn.microsoft.com/en-us/library/aa365429(VS.85).aspx

ReadEncryptedFileRaw Function
http://msdn.microsoft.com/en-us/library/aa365466(VS.85).aspx

WriteEncryptedFileRaw Function
http://msdn.microsoft.com/en-us/library/aa365746(VS.85).aspx

CloseEncryptedFileRaw Function 
http://msdn.microsoft.com/en-us/library/aa363839(VS.85).aspx
		 */

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool BackupWrite(SafeFileHandle hFile, IntPtr lpBuffer,
			uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten, bool bAbort,
			bool bProcessSecurity, ref IntPtr lpContext);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool BackupRead(SafeFileHandle hFile, IntPtr lpBuffer,
			uint nNumberOfBytesToRead, out uint lpNumberOfBytesRead, bool bAbort,
			bool bProcessSecurity, ref IntPtr lpContext);

		[DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		private static extern SafeFileHandle CreateFile(
			string fileName,
			[MarshalAs(UnmanagedType.U4)] FileAccess fileAccess,
			[MarshalAs(UnmanagedType.U4)] FileShare fileShare,
			IntPtr securityAttributes, // optional SECURITY_ATTRIBUTES structure can be passed
			[MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
			[MarshalAs(UnmanagedType.U4)] FileAttributes flagsAndAttributes,
			IntPtr template);

		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		private static extern SafeFileHandle CreateFile(
			string lpFileName,
			EFileAccess dwDesiredAccess,
			EFileShare dwShareMode,
			IntPtr lpSecurityAttributes,
			ECreationDisposition dwCreationDisposition,
			EFileAttributes dwFlagsAndAttributes,
			IntPtr hTemplateFile);


		[Flags]
		private enum EFileAccess : uint {
			//
			// Standart Section
			//

			ACCESS_SYSTEM_SECURITY = 0x1000000,   // AccessSystemAcl access type
			MaximumAllowed = 0x2000000,     // MaximumAllowed access type

			Delete = 0x10000,
			READ_CONTROL = 0x20000,
			WRITE_DAC = 0x40000,
			WRITE_OWNER = 0x80000,
			Synchronize = 0x100000,

			StandardRightsRequired = 0xF0000,
			StandardRightsRead = READ_CONTROL,
			StandardRightsWrite = READ_CONTROL,
			StandardRightsExecute = READ_CONTROL,
			StandardRightsAll = 0x1F0000,
			SpecificRightsAll = 0xFFFF,

			FILE_READ_DATA = 0x0001,        // file & pipe
			FILE_LIST_DIRECTORY = 0x0001,       // directory
			FILE_WRITE_DATA = 0x0002,       // file & pipe
			FILE_ADD_FILE = 0x0002,         // directory
			FILE_APPEND_DATA = 0x0004,      // file
			FILE_ADD_SUBDIRECTORY = 0x0004,     // directory
			FILE_CREATE_PIPE_INSTANCE = 0x0004, // named pipe
			FILE_READ_EA = 0x0008,          // file & directory
			FILE_WRITE_EA = 0x0010,         // file & directory
			FILE_EXECUTE = 0x0020,          // file
			FILE_TRAVERSE = 0x0020,         // directory
			FILE_DELETE_CHILD = 0x0040,     // directory
			FILE_READ_ATTRIBUTES = 0x0080,      // all
			FILE_WRITE_ATTRIBUTES = 0x0100,     // all

			//
			// Generic Section
			//

			GENERIC_READ = 0x80000000,
			GENERIC_WRITE = 0x40000000,
			GENERIC_EXECUTE = 0x20000000,
			GENERIC_ALL = 0x10000000,

			SPECIFIC_RIGHTS_ALL = 0x00FFFF,
			FILE_ALL_ACCESS =
			StandardRightsRequired |
			Synchronize |
			0x1FF,

			FILE_GENERIC_READ =
			StandardRightsRead |
			FILE_READ_DATA |
			FILE_READ_ATTRIBUTES |
			FILE_READ_EA |
			Synchronize,

			FILE_GENERIC_WRITE =
			StandardRightsWrite |
			FILE_WRITE_DATA |
			FILE_WRITE_ATTRIBUTES |
			FILE_WRITE_EA |
			FILE_APPEND_DATA |
			Synchronize,

			FILE_GENERIC_EXECUTE =
			StandardRightsExecute |
			  FILE_READ_ATTRIBUTES |
			  FILE_EXECUTE |
			  Synchronize
		}

		[Flags]
		private enum EFileShare : uint {
			/// <summary>
			/// 
			/// </summary>
			None = 0x00000000,
			/// <summary>
			/// Enables subsequent open operations on an object to request read access. 
			/// Otherwise, other processes cannot open the object if they request read access. 
			/// If this flag is not specified, but the object has been opened for read access, the function fails.
			/// </summary>
			Read = 0x00000001,
			/// <summary>
			/// Enables subsequent open operations on an object to request write access. 
			/// Otherwise, other processes cannot open the object if they request write access. 
			/// If this flag is not specified, but the object has been opened for write access, the function fails.
			/// </summary>
			Write = 0x00000002,
			/// <summary>
			/// Enables subsequent open operations on an object to request delete access. 
			/// Otherwise, other processes cannot open the object if they request delete access.
			/// If this flag is not specified, but the object has been opened for delete access, the function fails.
			/// </summary>
			Delete = 0x00000004
		}

		private enum ECreationDisposition : uint {
			/// <summary>
			/// Creates a new file. The function fails if a specified file exists.
			/// </summary>
			NEW = 1,
			/// <summary>
			/// Creates a new file, always. 
			/// If a file exists, the function overwrites the file, clears the existing attributes, combines the specified file attributes, 
			/// and flags with FILE_ATTRIBUTE_ARCHIVE, but does not set the security descriptor that the SECURITY_ATTRIBUTES structure specifies.
			/// </summary>
			CREATE_ALWAYS = 2,
			/// <summary>
			/// Opens a file. The function fails if the file does not exist. 
			/// </summary>
			OPEN_EXISTING = 3,
			/// <summary>
			/// Opens a file, always. 
			/// If a file does not exist, the function creates a file as if dwCreationDisposition is CREATE_NEW.
			/// </summary>
			OPEN_ALWAYS = 4,
			/// <summary>
			/// Opens a file and truncates it so that its size is 0 (zero) bytes. The function fails if the file does not exist.
			/// The calling process must open the file with the GENERIC_WRITE access right. 
			/// </summary>
			TRUNCATE_EXISTING = 5
		}

		[Flags]
		private enum EFileAttributes : uint {
			Readonly = 0x00000001,
			Hidden = 0x00000002,
			System = 0x00000004,
			FILE_ATTRIBUTE_DIRECTORY = 0x00000010,
			Archive = 0x00000020,
			Device = 0x00000040,
			FILE_ATTRIBUTE_NORMAL = 0x00000080,
			Temporary = 0x00000100,
			SparseFile = 0x00000200,
			ReparsePoint = 0x00000400,
			Compressed = 0x00000800,
			Offline = 0x00001000,
			NotContentIndexed = 0x00002000,
			Encrypted = 0x00004000,
			Write_Through = 0x80000000,
			Overlapped = 0x40000000,
			NoBuffering = 0x20000000,
			RandomAccess = 0x10000000,
			FILE_FLAG_SEQUENTIAL_SCAN = 0x08000000,
			DeleteOnClose = 0x04000000,
			FILE_FLAG_BACKUP_SEMANTICS = 0x02000000,
			PosixSemantics = 0x01000000,
			OpenReparsePoint = 0x00200000,
			OpenNoRecall = 0x00100000,
			FirstPipeInstance = 0x00080000
		}

		[Flags]
		public enum EFileAttributesEx : uint {
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
			Write_Through = 0x80000000,
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
		
		public enum FILE_INFO_BY_HANDLE_CLASS : int {
			FileBasicInfo = 0,
			FileStandardInfo = 1,
			FileNameInfo = 2,
			FileRenameInfo = 3,
			FileDispositionInfo = 4,
			FileAllocationInfo = 5,
			FileEndOfFileInfo = 6,
			FileStreamInfo = 7,
			FileCompressionInfo = 8,
			FileAttributeTagInfo = 9,
			FileIdBothDirectoryInfo = 10, // 0xA
			FileIdBothDirectoryRestartInfo = 11, // 0xB
			FileIoPriorityHintInfo = 12, // 0xC
			FileRemoteProtocolInfo = 13, // 0xD
			FileFullDirectoryInfo = 14, // 0xE
			FileFullDirectoryRestartInfo = 15, // 0xF
			FileStorageInfo = 16, // 0x10
			FileAlignmentInfo = 17, // 0x11
			FileIdInfo = 18, // 0x12
			FileIdExtdDirectoryInfo = 19, // 0x13
			FileIdExtdDirectoryRestartInfo = 20, // 0x14
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct FILE_BASIC_INFO {
			public LARGE_INTEGER CreationTime;
			public LARGE_INTEGER LastAccessTime;
			public LARGE_INTEGER LastWriteTime;
			public LARGE_INTEGER ChangeTime;
			public FILE_ATTRIBUTE FileAttributes;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct FILE_DISPOSITION_INFO {
			public bool DeleteFile;
		}
		[StructLayout(LayoutKind.Explicit)]
		public struct FileInformation {
			[FieldOffset(0)]
			public FILE_BASIC_INFO FILE_BASIC_INFO;
			[FieldOffset(0)]
			public FILE_DISPOSITION_INFO FILE_DISPOSITION_INFO;
		}

		[DllImport("Kernel32.dll", SetLastError = true)]
		public static extern bool SetFileInformationByHandle(
			SafeFileHandle hFile,
			FILE_INFO_BY_HANDLE_CLASS fileInformationClass,
			ref FileInformation fileInformation,
			Int32 dwBufferSize
		);

		public static bool SetFileInformationByHandle(SafeFileHandle hFile, FILE_BASIC_INFO fileInformation) {
			var fih = new FileInformation { FILE_BASIC_INFO = fileInformation };
			return SetFileInformationByHandle(
				hFile,
				FILE_INFO_BY_HANDLE_CLASS.FileBasicInfo,
				ref fih,
				Marshal.SizeOf(fileInformation));
		}

		[StructLayout(LayoutKind.Explicit, Size = 8)]
		public struct LARGE_INTEGER {

			[FieldOffset(0)] public int Low;
			[FieldOffset(4)] public int High;
			[FieldOffset(0)] public long QuadPart;

			// use only when QuadPart canot be passed
			public long ToInt64() { return ((long)this.High << 32) | (uint)this.Low; }

			// just for demonstration
			public static LARGE_INTEGER FromInt64(long value) {
				return new LARGE_INTEGER { Low = (int)(value), High = (int)((value >> 32)) };
			}

		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct FILE_ID_BOTH_DIR_INFO {
			public uint NextEntryOffset;
			public uint FileIndex;
			public LARGE_INTEGER CreationTime;
			public LARGE_INTEGER LastAccessTime;
			public LARGE_INTEGER LastWriteTime;
			public LARGE_INTEGER ChangeTime;
			public LARGE_INTEGER EndOfFile;
			public LARGE_INTEGER AllocationSize;
			public uint FileAttributes;
			public uint FileNameLength;
			public uint EaSize;
			public char ShortNameLength;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
			public string ShortName;
			public LARGE_INTEGER FileId;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
			public string FileName;
		}

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool GetFileInformationByHandleEx(
			SafeFileHandle hFile,
			FILE_INFO_BY_HANDLE_CLASS infoClass,
			out FILE_BASIC_INFO dirInfo,
			uint dwBufferSize);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool GetFileInformationByHandleEx(
			SafeFileHandle hFile,
			FILE_INFO_BY_HANDLE_CLASS infoClass,
			out FILE_ID_BOTH_DIR_INFO dirInfo,
			uint dwBufferSize);

		public static bool GetFileInformationByHandleEx(
			SafeFileHandle hFile,
			out FILE_BASIC_INFO info
		) {
			info = new FILE_BASIC_INFO();
			return GetFileInformationByHandleEx(hFile, FILE_INFO_BY_HANDLE_CLASS.FileBasicInfo, out info, (uint)Marshal.SizeOf(info));
		}

		public static bool GetFileInformationByHandleEx(
			SafeFileHandle hFile,
			out FILE_ID_BOTH_DIR_INFO info
			) {
			var fileStruct = new FILE_ID_BOTH_DIR_INFO();
			return GetFileInformationByHandleEx(hFile, FILE_INFO_BY_HANDLE_CLASS.FileIdBothDirectoryInfo, out info, (uint)Marshal.SizeOf(fileStruct));
		}

	}
}
