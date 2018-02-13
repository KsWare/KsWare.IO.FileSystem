using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using KsWare.IO.FileSystem.Internal;
using Microsoft.Win32.SafeHandles;
using static KsWare.IO.FileSystem.Internal.WinApi;

namespace KsWare.IO.FileSystem {

// HowTo: Correctly read reparse data in Vista
// http://blog.kalmbach-software.de/2008/02/28/howto-correctly-read-reparse-data-in-vista/

	public static partial class ReparsePoint {

		/// <summary>
		///     Creates a junction point to the specified target directory.
		/// </summary>
		/// <remarks>
		///     Only works on NTFS.
		/// </remarks>
		/// <param name="directory">The directory to create.</param>
		/// <param name="target">The target directory.</param>
		/// <param name="overwrite">If true overwrites an existing reparse point or empty directory.</param>
		/// <exception cref="Helper.IOExceptionForLastWin32Error">
		///     Thrown when the junction point could not be created or when
		///     an existing directory was found and <paramref name="overwrite" /> if false
		/// </exception>
		public static void CreateMointpoint(string directory, string target, bool overwrite) {
			target = Path.GetFullPath(target);

			if (!Directory.Exists(target)) throw new IOException($"Source path does not exist or is not a directory.");

			if (Directory.Exists(directory)) throw new IOException($"Directory '{directory}' already exists.");

			Directory.CreateDirectory(directory);

			using (var handle = OpenReparsePoint(directory, EFileAccess.GenericWrite)) {
				var sourceDirBytes = Encoding.Unicode.GetBytes(NonInterpretedPathPrefix + Path.GetFullPath(target));

				var reparseDataBuffer = new REPARSE_DATA_BUFFER_MountPointReparseBuffer();

				reparseDataBuffer.ReparseTag           = IO_REPARSE_TAG_MOUNT_POINT;
				reparseDataBuffer.ReparseDataLength    = (ushort) (sourceDirBytes.Length + 12);
				reparseDataBuffer.SubstituteNameOffset = 0;
				reparseDataBuffer.SubstituteNameLength = (ushort) sourceDirBytes.Length;
				reparseDataBuffer.PrintNameOffset      = (ushort) (sourceDirBytes.Length + 2);
				reparseDataBuffer.PrintNameLength      = 0;
				reparseDataBuffer.PathBuffer           = new byte[0x3ff0];
				Array.Copy(sourceDirBytes, reparseDataBuffer.PathBuffer, sourceDirBytes.Length);

				var inBufferSize = Marshal.SizeOf(reparseDataBuffer);
				var inBuffer     = Marshal.AllocHGlobal(inBufferSize);

				try {
					Marshal.StructureToPtr(reparseDataBuffer, inBuffer, false);

					int bytesReturned;
					var result = DeviceIoControl(handle.DangerousGetHandle(), FSCTL_SET_REPARSE_POINT, inBuffer,
						sourceDirBytes.Length + 20,                              IntPtr.Zero,             0, out bytesReturned,
						IntPtr.Zero);

					if (!result)
						throw Helper.IOExceptionForLastWin32Error($"Unable to create junction point '{directory}' -> '{target}'.");
				}
				finally {
					Marshal.FreeHGlobal(inBuffer);
				}
			}
		}

		/// <summary>
		///     Creates a symbolic link to the specified target directory.
		/// </summary>
		/// <remarks>
		///     Only works on NTFS.
		/// </remarks>
		/// <param name="directory">The directory to create.</param>
		/// <param name="target">The target directory.</param>
		/// <param name="overwrite">If true overwrites an existing reparse point or empty directory.</param>
		/// <exception cref="Helper.IOExceptionForLastWin32Error">
		///     Thrown when the junction point could not be created or when
		///     an existing directory was found and <paramref name="overwrite" /> if false
		/// </exception>
		public static void CreateSymbolicLinkDirectory(string directory, string target, bool overwrite) {
			SymbolicLink.Create(directory, target, overwrite);
		}

		/// <summary>
		///     Creates a symbolic link from the specified file to the specified target file.
		/// </summary>
		/// <remarks>
		///     Only works on NTFS.
		/// </remarks>
		/// <param name="file">The file to create.</param>
		/// <param name="target">The target file.</param>
		/// <param name="overwrite">If true overwrites an existing reparse point or empty directory.</param>
		/// <exception cref="Helper.IOExceptionForLastWin32Error">
		///     Thrown when the junction point could not be created or when
		///     an existing directory was found and <paramref name="overwrite" /> if false
		/// </exception>
		public static void CreateSymbolicLinkFile(string file, string target, bool overwrite) {
			SymbolicLink.Create(file, target, overwrite);
		}


		//		/// <summary>
		//		///     Creates a symbolic link to the specified target directory.
		//		/// </summary>
		//		/// <remarks>
		//		///     Only works on NTFS.
		//		/// </remarks>
		//		/// <param name="directory">The directory to create.</param>
		//		/// <param name="target">The target directory.</param>
		//		/// <param name="overwrite">If true overwrites an existing reparse point or empty directory.</param>
		//		/// <exception cref="IOExceptionForLastWin32Error">
		//		///     Thrown when the junction point could not be created or when
		//		///     an existing directory was found and <paramref name="overwrite" /> if false
		//		/// </exception>
		//		public static void CreateSymbolicLinkDirectory(string directory, string target, bool overwrite) {
		//			target = Path.GetFullPath(target);
		//
		//			if (!Directory.Exists(target))
		//				throw new IOException($"Target path does not exist or is not a directory.");
		//
		//			if (Directory.Exists(directory) || File.Exists(directory))
		//				throw new IOException($"Directory '{directory}' already exists.");
		//
		//			Directory.CreateDirectory(directory);
		//
		//			using (var handle = OpenReparsePoint(directory, EFileAccess.GenericWrite)) {
		//				var sourceDirBytes = Encoding.Unicode.GetBytes(NonInterpretedPathPrefix + Path.GetFullPath(target));
		//
		//				var reparseDataBuffer = new REPARSE_DATA_BUFFER_SymbolicLinkReparseBuffer();
		//
		//				reparseDataBuffer.ReparseTag           = (uint) IO_REPARSE_TAG.SYMLINK;
		//				reparseDataBuffer.ReparseDataLength    = (ushort) (sourceDirBytes.Length + 12);
		//				reparseDataBuffer.SubstituteNameOffset = 0;
		//				reparseDataBuffer.SubstituteNameLength = (ushort) sourceDirBytes.Length;
		//				reparseDataBuffer.PrintNameOffset      = (ushort) (sourceDirBytes.Length + 2);
		//				reparseDataBuffer.PrintNameLength      = 0;
		//				reparseDataBuffer.Flags                = (uint) SYMLINK_FLAG.ABSOLUTE;
		//				reparseDataBuffer.PathBuffer           = new byte[0x3ff0];
		//				Array.Copy(sourceDirBytes, reparseDataBuffer.PathBuffer, sourceDirBytes.Length);
		//
		//				var inBufferSize = Marshal.SizeOf(reparseDataBuffer);
		//				var inBuffer     = Marshal.AllocHGlobal(inBufferSize);
		//
		//				try {
		//					Marshal.StructureToPtr(reparseDataBuffer, inBuffer, false);
		//
		//					int bytesReturned;
		//					var result = DeviceIoControl(handle.DangerousGetHandle(), FSCTL_SET_REPARSE_POINT, inBuffer,
		//						sourceDirBytes.Length + 20,                              IntPtr.Zero,             0, out bytesReturned,
		//						IntPtr.Zero);
		//
		//					if (!result)
		//						throw IOExceptionForLastWin32Error($"Unable to create symbolic link '{directory}' -> '{target}'.");
		//				}
		//				finally {
		//					Marshal.FreeHGlobal(inBuffer);
		//				}
		//			}
		//		}

		//		private static void CreateSymbolicLinkFileTest() {
		//			CreateSymbolicLinkFile(@"D:\ReparsePointTest\Original - SymbolicLink Test.txt", @"D:\ReparsePointTest\Original.txt", true);
		//		}

		//		/// <summary>
		//		///     Creates a symbolic link from the specified file to the specified target file.
		//		/// </summary>
		//		/// <remarks>
		//		///     Only works on NTFS.
		//		/// </remarks>
		//		/// <param name="file">The file to create.</param>
		//		/// <param name="target">The target file.</param>
		//		/// <param name="overwrite">If true overwrites an existing reparse point or empty directory.</param>
		//		/// <exception cref="IOExceptionForLastWin32Error">
		//		///     Thrown when the junction point could not be created or when
		//		///     an existing directory was found and <paramref name="overwrite" /> if false
		//		/// </exception>
		//		public static void CreateSymbolicLinkFile(string file, string target, bool overwrite) {
		//			target = Path.GetFullPath(target);
		//
		//			if (!File.Exists(target))
		//				throw new IOException($"Target path does not exist or is not a file.");
		//
		//			if (Directory.Exists(file) || File.Exists(file))
		//				throw new IOException($"File '{file}' already exists.");
		//
		//			//Directory.CreateDirectory(file);
		//			using (var f=File.Open(file, FileMode.CreateNew,FileAccess.ReadWrite,FileShare.None)) {
		//
		//			}
		//
		//			using (var handle = OpenReparsePoint(file, EFileAccess.GenericWrite)) {
		//				var sourceDirBytes = Encoding.Unicode.GetBytes(@"\\?\" + Path.GetFullPath(target));
		//
		//				var reparseDataBuffer = new REPARSE_DATA_BUFFER_SymbolicLinkReparseBuffer();
		//
		//				reparseDataBuffer.ReparseTag           = (uint) IO_REPARSE_TAG.SYMLINK;
		//				reparseDataBuffer.ReparseDataLength    = (ushort) (12 + sourceDirBytes.Length + 2 + sourceDirBytes.Length + 2);
		//				reparseDataBuffer.SubstituteNameOffset = 0;
		//				reparseDataBuffer.SubstituteNameLength = (ushort) sourceDirBytes.Length;
		//				reparseDataBuffer.PrintNameOffset      = (ushort) (sourceDirBytes.Length + 2);
		//				reparseDataBuffer.PrintNameLength      = (ushort) sourceDirBytes.Length;
		//				reparseDataBuffer.Flags                = (uint) SYMLINK_FLAG.ABSOLUTE;
		//				reparseDataBuffer.PathBuffer           = new byte[0x3ff0];
		//				Buffer.BlockCopy(sourceDirBytes,0, reparseDataBuffer.PathBuffer, reparseDataBuffer.SubstituteNameOffset, sourceDirBytes.Length);
		//				Buffer.BlockCopy(sourceDirBytes, 0, reparseDataBuffer.PathBuffer, reparseDataBuffer.PrintNameOffset, sourceDirBytes.Length);
		//
		//				var inBufferSize = Marshal.SizeOf(reparseDataBuffer);
		//				var inBuffer     = Marshal.AllocHGlobal(inBufferSize);
		//
		//				try {
		//					Marshal.StructureToPtr(reparseDataBuffer, inBuffer, false);
		//
		//					int bytesReturned;
		//					var result = DeviceIoControl(handle.DangerousGetHandle(), FSCTL_SET_REPARSE_POINT, inBuffer,
		//						sourceDirBytes.Length + 20,                              IntPtr.Zero,             0, out bytesReturned,
		//						IntPtr.Zero);
		//
		//					if (!result)
		//						throw IOExceptionForLastWin32Error($"Unable to create symbolic link '{file}' -> '{target}'.");
		//				}
		//				finally {
		//					Marshal.FreeHGlobal(inBuffer);
		//				}
		//			}
		//		}

		/// <summary>
		///     Deletes a junction point at the specified source directory along with the directory itself.
		///     Does nothing if the junction point does not exist.
		/// </summary>
		/// <remarks>
		///     Only works on NTFS.
		/// </remarks>
		/// <param name="junctionPoint">The junction point path</param>
		public static void DeleteMountpoint(string junctionPoint) {
			if (!Directory.Exists(junctionPoint)) {
				if (File.Exists(junctionPoint)) throw new IOException("Path is not a junction point.");
				return;
			}

			using (var handle = OpenReparsePoint(junctionPoint, EFileAccess.GenericWrite)) {
				var reparseDataBuffer = new REPARSE_DATA_BUFFER_MountPointReparseBuffer();

				reparseDataBuffer.ReparseTag        = IO_REPARSE_TAG_MOUNT_POINT;
				reparseDataBuffer.ReparseDataLength = 0;
				reparseDataBuffer.PathBuffer        = new byte[0x3ff0];

				var inBufferSize = Marshal.SizeOf(reparseDataBuffer);
				var inBuffer     = Marshal.AllocHGlobal(inBufferSize);
				try {
					Marshal.StructureToPtr(reparseDataBuffer, inBuffer, false);

					int bytesReturned;
					var result = DeviceIoControl(handle.DangerousGetHandle(), FSCTL_DELETE_REPARSE_POINT, inBuffer, 8, IntPtr.Zero, 0,
						out bytesReturned,                                       IntPtr.Zero);

					if (!result) throw Helper.IOExceptionForLastWin32Error("Unable to delete junction point.");
				}
				finally {
					Marshal.FreeHGlobal(inBuffer);
				}

				try {
					Directory.Delete(junctionPoint);
				}
				catch (IOException ex) {
					throw new IOException("Unable to delete junction point.", ex);
				}
			}
		}

		/// <summary>
		///     Determines whether the specified path exists and refers to a junction point.
		/// </summary>
		/// <param name="path">The junction point path</param>
		/// <returns>True if the specified path represents a junction point</returns>
		/// <exception cref="Helper.IOExceptionForLastWin32Error">
		///     Thrown if the specified path is invalid
		///     or some other error occurs
		/// </exception>
		public static bool Exists(string path) {
			if (!Directory.Exists(path)) return false;

			using (var handle = OpenReparsePoint(path, EFileAccess.GenericRead)) {
				var data = GetReparseData(handle, true);
				return data != null;
			}
		}

		/// <summary>
		///     Gets the target of the specified reparse point.
		/// </summary>
		/// <remarks>
		///     Only works on NTFS.
		/// </remarks>
		/// <param name="reparsePoint">The junction point path</param>
		/// <returns>The target of the junction point</returns>
		/// <exception cref="Helper.IOExceptionForLastWin32Error">
		///     Thrown when the specified path does not
		///     exist, is invalid, is not a junction point, or some other error occurs
		/// </exception>
		public static string GetTarget(string reparsePoint) {
			using (var handle = OpenReparsePoint(reparsePoint, EFileAccess.GenericRead)) {
				var target = InternalGetTarget(handle);
				if (target == null) throw new IOException("Path is not a reparse point.");

				return target;
			}
		}

	}

	public static partial class ReparsePoint {
		

		private static void ReadTest() {
			string f;
			//			using (var h = OpenReparsePoint(@"D:\ReparsePointTest\Original", EFileAccess.GenericRead)) {
			//				var d = GetReparseData(h);
			//			}
			f = @"D:\ReparsePointTest\Original - Junction";
			using (var h = OpenReparsePoint(f, EFileAccess.GenericRead)) {
				Debug.WriteLine(f);
				var d = GetReparseData(h);
				Debug.WriteLine((object) d?.ReparseTag);
				Debug.WriteLine(d?.SubstituteName);
			}
			f = @"D:\ReparsePointTest\D - Mountpoint";
			using (var h = OpenReparsePoint(f, EFileAccess.GenericRead)) {
				Debug.WriteLine(f);
				var d = GetReparseData(h);
				Debug.WriteLine((object) d?.ReparseTag);
				Debug.WriteLine(d?.SubstituteName);
			}
			f = @"D:\ReparsePointTest\Original - SymbolicLink";
			using (var h = OpenReparsePoint(f, EFileAccess.GenericRead)) {
				Debug.WriteLine(f);
				var d = GetReparseData(h);
				Debug.WriteLine((object) d?.ReparseTag);
				Debug.WriteLine(d?.SubstituteName);
				Debug.WriteLine((object) d?.Flags);
			}
			//			using (var h = OpenReparsePoint(@"D:\ReparsePointTest\Original.txt", EFileAccess.GenericRead)) {
			//				var d = GetReparseData(h);
			//			}
			f = @"D:\ReparsePointTest\Original - SymbolicLink.txt";
			using (var h = OpenReparsePoint(f, EFileAccess.GenericRead)) {
				Debug.WriteLine(f);
				var d = GetReparseData(h);
				Debug.WriteLine((object) d?.ReparseTag);
				Debug.WriteLine(d?.SubstituteName);
				Debug.WriteLine((object) d?.Flags);
			}
			//			using (var h = OpenReparsePoint(@"D:\ReparsePointTest\Original - Hardlink.txt", EFileAccess.GenericRead)) {
			//				var d = GetReparseData(h);
			//				Debug.WriteLine(d.ReparseTag);
			//				Debug.WriteLine(d.SubstituteName);
			//			}
		}

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool DeviceIoControl(IntPtr hDevice,
			uint dwIoControlCode,
			IntPtr InBuffer,
			int nInBufferSize,
			IntPtr OutBuffer,
			int nOutBufferSize,
			out int pBytesReturned,
			IntPtr lpOverlapped);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr CreateFile(string lpFileName,
			EFileAccess dwDesiredAccess,
			EFileShare dwShareMode,
			IntPtr lpSecurityAttributes,
			ECreationDisposition dwCreationDisposition,
			EFileAttributes dwFlagsAndAttributes,
			IntPtr hTemplateFile);

		private static string InternalGetTarget(SafeFileHandle handle) {
			var outBufferSize = Marshal.SizeOf(typeof(REPARSE_DATA_BUFFER_MountPointReparseBuffer));
			var outBuffer     = Marshal.AllocHGlobal(outBufferSize);

			try {
				int bytesReturned;
				var result = DeviceIoControl(handle.DangerousGetHandle(), FSCTL_GET_REPARSE_POINT, IntPtr.Zero, 0, outBuffer,
					outBufferSize,                                           out bytesReturned,       IntPtr.Zero);

				if (!result) {
					var error = Marshal.GetLastWin32Error();
					if (error == ERROR_NOT_A_REPARSE_POINT)
						return null;

					throw Helper.IOExceptionForLastWin32Error("Unable to get information about reparse point.");
				}

				var reparseDataBuffer =
					(REPARSE_DATA_BUFFER_MountPointReparseBuffer) Marshal.PtrToStructure(outBuffer,
						typeof(REPARSE_DATA_BUFFER_MountPointReparseBuffer));

				if (reparseDataBuffer.ReparseTag != IO_REPARSE_TAG_MOUNT_POINT)
					return null;

				var targetDir = Encoding.Unicode.GetString(reparseDataBuffer.PathBuffer, reparseDataBuffer.SubstituteNameOffset,
					reparseDataBuffer.SubstituteNameLength);

				if (targetDir.StartsWith(NonInterpretedPathPrefix))
					targetDir = targetDir.Substring(NonInterpretedPathPrefix.Length);

				return targetDir;
			}
			finally {
				Marshal.FreeHGlobal(outBuffer);
			}
		}

		public static ReparseData GetReparseData(string path) {
			ReparseData data;

			var attr = WinApi.GetFileAttributes(path);
			if ((int) attr == INVALID_FILE_ATTRIBUTES) return new ReparseData{Type = ReparsePointType.None};

			using (var handle = OpenReparsePoint(path, EFileAccess.GenericRead)) {
				data = GetReparseData(handle, true);
			}
			switch (data.ReparseTag) {
				case IO_REPARSE_TAG.MOUNT_POINT: {
					if(data.SubstituteName.StartsWith(@"\\?\Volume{") || data.SubstituteName.StartsWith(@"\??\Volume{"))
						data.Type = ReparsePointType.VolumeMountPoint;
					else 
						data.Type = ReparsePointType.MountPoint;
					break;
				}
				case IO_REPARSE_TAG.SYMLINK: {
					if((attr & FILE_ATTRIBUTE.DIRECTORY) !=0)
						data.Type = ReparsePointType.SymbolicDirectoryLink;
					else
						data.Type = ReparsePointType.SymbolicFileLink;
					break;
				}
				default:
					data.Type = ReparsePointType.Other;
					break;
			}

			return data;
		}

		private static ReparseData GetReparseData(SafeFileHandle handle, bool noException = false) {
			var outBufferSize = Marshal.SizeOf(typeof(REPARSE_DATA_BUFFER_SymbolicLinkReparseBuffer));
			var outBuffer     = Marshal.AllocHGlobal(outBufferSize);

			try {
				int bytesReturned;
				var result = DeviceIoControl(handle.DangerousGetHandle(), FSCTL_GET_REPARSE_POINT, IntPtr.Zero, 0, outBuffer,
					outBufferSize,                                           out bytesReturned,       IntPtr.Zero);

				if (!result) {
					var error = Marshal.GetLastWin32Error();
					if (error == ERROR_NOT_A_REPARSE_POINT) {
						if (!noException)
							throw Helper.Win32Exception(error, nameof(GetReparseData), null);
						return null;
					}
					if (!noException)
						throw Helper.IOExceptionForLastWin32Error("Unable to get information about reparse point.");
					return null;
				}

				var managed        = new ReparseData();
				managed.ReparseTag = (IO_REPARSE_TAG) Marshal.ReadInt32(outBuffer); // read ReparseTag

				switch (managed.ReparseTag) {
					case IO_REPARSE_TAG.MOUNT_POINT: {
						var reparseDataBuffer =
							(REPARSE_DATA_BUFFER_MountPointReparseBuffer) Marshal.PtrToStructure(outBuffer,
								typeof(REPARSE_DATA_BUFFER_MountPointReparseBuffer));
						if (reparseDataBuffer.SubstituteNameLength > 0) {
							managed.SubstituteName = Encoding.Unicode.GetString(reparseDataBuffer.PathBuffer,
								reparseDataBuffer.SubstituteNameOffset, reparseDataBuffer.SubstituteNameLength);
						}
						if (reparseDataBuffer.PrintNameLength > 0) {
							managed.PrintName = Encoding.Unicode.GetString(reparseDataBuffer.PathBuffer, reparseDataBuffer.PrintNameOffset,
								reparseDataBuffer.PrintNameLength);
						}
						break;
					}
					case IO_REPARSE_TAG.SYMLINK: {
						var reparseDataBuffer =
							(REPARSE_DATA_BUFFER_SymbolicLinkReparseBuffer) Marshal.PtrToStructure(outBuffer,
								typeof(REPARSE_DATA_BUFFER_SymbolicLinkReparseBuffer));
						if (reparseDataBuffer.SubstituteNameLength > 0) {
							managed.SubstituteName = Encoding.Unicode.GetString(reparseDataBuffer.PathBuffer,
								reparseDataBuffer.SubstituteNameOffset, reparseDataBuffer.SubstituteNameLength);
						}
						if (reparseDataBuffer.PrintNameLength > 0) {
							managed.PrintName = Encoding.Unicode.GetString(reparseDataBuffer.PathBuffer, reparseDataBuffer.PrintNameOffset,
								reparseDataBuffer.PrintNameLength);
						}
						managed.Flags = (SYMLINK_FLAG) Marshal.ReadInt32(outBuffer, 16);
						break;
					}
					default: {
						var reparseDataLength = Marshal.ReadInt32(outBuffer, 4); // read ReparseDataLength
						managed.Generic       = new byte[reparseDataLength];
						Marshal.Copy(new IntPtr(outBuffer.ToInt64() + 8), managed.Generic, 0, reparseDataLength);
						break;
					}
				}

				//				if (managed.SubstituteName!=null && managed.SubstituteName.StartsWith(NonInterpretedPathPrefix))
				//					managed.SubstituteName = managed.SubstituteName.Substring(NonInterpretedPathPrefix.Length);
				//				if (managed.PrintName!=null && managed.PrintName.StartsWith(NonInterpretedPathPrefix))
				//					managed.PrintName = managed.PrintName.Substring(NonInterpretedPathPrefix.Length);
				return managed;
			}
			finally {
				Marshal.FreeHGlobal(outBuffer);
			}
		}

		private static bool IsReparseTagMicrosoft(uint tag) { return (tag & 0x8000) != 0; }

		private static bool IsReparseTagNameSurrogate(uint tag) { return (tag & 0x2000) != 0; }

		public static SafeFileHandle OpenReparsePoint(string reparsePoint, EFileAccess accessMode) {
			var reparsePointHandle = new SafeFileHandle(
				CreateFile(reparsePoint, accessMode, EFileShare.Read | EFileShare.Write | EFileShare.Delete,
					IntPtr.Zero,
					ECreationDisposition.OpenExisting, EFileAttributes.BackupSemantics | EFileAttributes.OpenReparsePoint,
					IntPtr.Zero), true);

			if (Marshal.GetLastWin32Error() != 0)
				throw Helper.IOExceptionForLastWin32Error("Unable to open reparse point.");

			return reparsePointHandle;
		}

		public enum IO_REPARSE_TAG : uint {
			MOUNT_POINT = 0xA0000003,
			HSM = 0xC0000004,
			SIS = 0x80000007,
			DFS = 0x8000000A,
			SYMLINK = 0xA000000C,
			DFSR = 0x80000012
		}

		[Flags]
		public enum EFileAccess : uint {
			GenericRead = 0x80000000,
			GenericWrite = 0x40000000,
			GenericExecute = 0x20000000,
			GenericAll = 0x10000000
		}

		[Flags]
		private enum EFileShare : uint {
			None = 0x00000000,
			Read = 0x00000001,
			Write = 0x00000002,
			Delete = 0x00000004
		}

		private enum ECreationDisposition : uint {
			New = 1,
			CreateAlways = 2,
			OpenExisting = 3,
			OpenAlways = 4,
			TruncateExisting = 5
		}

		[Flags]
		private enum EFileAttributes : uint {
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

		/*typedef struct _REPARSE_DATA_BUFFER {
				ULONG  ReparseTag;
				USHORT  ReparseDataLength;
				USHORT  Reserved;
				union {
				struct {
					USHORT  SubstituteNameOffset;
					USHORT  SubstituteNameLength;
					USHORT  PrintNameOffset;
					USHORT  PrintNameLength;
					ULONG   Flags; // it seems that the docu is missing this entry (at least 2008-03-07)
					WCHAR  PathBuffer[1];
					} SymbolicLinkReparseBuffer;
				struct {
					USHORT  SubstituteNameOffset;
					USHORT  SubstituteNameLength;
					USHORT  PrintNameOffset;
					USHORT  PrintNameLength;
					WCHAR  PathBuffer[1];
					} MountPointReparseBuffer;
				struct {
					UCHAR  DataBuffer[1];
				} GenericReparseBuffer;
				};
			} REPARSE_DATA_BUFFER, *PREPARSE_DATA_BUFFER;
		*/

		[StructLayout(LayoutKind.Sequential)]
		public struct REPARSE_DATA_BUFFER_MountPointReparseBuffer {

			/// <summary>
			///     Reparse point tag. Must be a Microsoft reparse point tag.
			/// </summary>
			public uint ReparseTag;

			/// <summary>
			///     Size, in bytes, of the data after the Reserved member. This can be calculated by:
			///     (4 * sizeof(ushort)) + SubstituteNameLength + PrintNameLength +
			///     (namesAreNullTerminated ? 2 * sizeof(char) : 0);
			/// </summary>
			public ushort ReparseDataLength;

			/// <summary>
			///     Reserved; do not use.
			/// </summary>
			public readonly ushort Reserved;

			/// <summary>
			///     Offset, in bytes, of the substitute name string in the PathBuffer array.
			/// </summary>
			public ushort SubstituteNameOffset;

			/// <summary>
			///     Length, in bytes, of the substitute name string. If this string is null-terminated,
			///     SubstituteNameLength does not include space for the null character.
			/// </summary>
			public ushort SubstituteNameLength;

			/// <summary>
			///     Offset, in bytes, of the print name string in the PathBuffer array.
			/// </summary>
			public ushort PrintNameOffset;

			/// <summary>
			///     Length, in bytes, of the print name string. If this string is null-terminated,
			///     PrintNameLength does not include space for the null character.
			/// </summary>
			public ushort PrintNameLength;

			/// <summary>
			///     A buffer containing the unicode-encoded path string. The path string contains
			///     the substitute name string and print name string.
			/// </summary>
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x3FF0)]
			public byte[] PathBuffer;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct REPARSE_DATA_BUFFER_SymbolicLinkReparseBuffer {

			/// <summary>
			///     Reparse point tag. Must be a Microsoft reparse point tag.
			/// </summary>
			public uint ReparseTag;

			/// <summary>
			///     Size, in bytes, of the data after the Reserved member. This can be calculated by:
			///     (4 * sizeof(ushort)) + SubstituteNameLength + PrintNameLength +
			///     (namesAreNullTerminated ? 2 * sizeof(char) : 0);
			/// </summary>
			public ushort ReparseDataLength;

			/// <summary>
			///     Reserved; do not use.
			/// </summary>
			public readonly ushort Reserved;

			/// <summary>
			///     Offset, in bytes, of the substitute name string in the PathBuffer array.
			/// </summary>
			public ushort SubstituteNameOffset;

			/// <summary>
			///     Length, in bytes, of the substitute name string. If this string is null-terminated,
			///     SubstituteNameLength does not include space for the null character.
			/// </summary>
			public ushort SubstituteNameLength;

			/// <summary>
			///     Offset, in bytes, of the print name string in the PathBuffer array.
			/// </summary>
			public ushort PrintNameOffset;

			/// <summary>
			///     Length, in bytes, of the print name string. If this string is null-terminated,
			///     PrintNameLength does not include space for the null character.
			/// </summary>
			public ushort PrintNameLength;

			public uint Flags;

			/// <summary>
			///     A buffer containing the unicode-encoded path string. The path string contains
			///     the substitute name string and print name string.
			/// </summary>
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x3FF0)]
			public byte[] PathBuffer;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct REPARSE_DATA_BUFFER_GenericReparseBuffer {
			/// <summary>
			///     A buffer containing the unicode-encoded path string. The path string contains
			///     the substitute name string and print name string.
			/// </summary>
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x3FF0)]
			public byte[] DataBuffer;
		}

		#region constants

		private const int SYMLINK_FLAG_ABSOLUTE = 0x00000000;
		private const int SYMLINK_FLAG_RELATIVE = 0x00000001;

		/// <summary>
		///     The file or directory is not a reparse point.
		/// </summary>
		private const int ERROR_NOT_A_REPARSE_POINT = 4390;

		/// <summary>
		///     The reparse point attribute cannot be set because it conflicts with an existing attribute.
		/// </summary>
		private const int ERROR_REPARSE_ATTRIBUTE_CONFLICT = 4391;

		/// <summary>
		///     The data present in the reparse point buffer is invalid.
		/// </summary>
		private const int ERROR_INVALID_REPARSE_DATA = 4392;

		/// <summary>
		///     The tag present in the reparse point buffer is invalid.
		/// </summary>
		private const int ERROR_REPARSE_TAG_INVALID = 4393;

		/// <summary>
		///     There is a mismatch between the tag specified in the request and the tag present in the reparse point.
		/// </summary>
		private const int ERROR_REPARSE_TAG_MISMATCH = 4394;

		/// <summary>
		///     Command to set the reparse point data block.
		/// </summary>
		public const int FSCTL_SET_REPARSE_POINT = 0x000900A4;

		/// <summary>
		///     Command to get the reparse point data block.
		/// </summary>
		private const int FSCTL_GET_REPARSE_POINT = 0x000900A8;

		/// <summary>
		///     Command to delete the reparse point data base.
		/// </summary>
		public const int FSCTL_DELETE_REPARSE_POINT = 0x000900AC;

		/// <summary>
		///     Reparse point tag used to identify mount points and junction points.
		/// </summary>
		public const uint IO_REPARSE_TAG_MOUNT_POINT = 0xA0000003;

		/// <summary>
		///     This prefix indicates to NTFS that the path is to be treated as a non-interpreted
		///     path in the virtual file system.
		/// </summary>
		public const string NonInterpretedPathPrefix = @"\??\";

		#endregion

		public enum SYMLINK_FLAG {
			ABSOLUTE = 0,
			RELATIVE = 1
		}

		
	}

}