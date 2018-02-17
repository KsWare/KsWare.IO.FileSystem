using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using KsWare.IO.FileSystem.Internal;
using KsWare.PrivilegedExecutor;
using Microsoft.Win32.SafeHandles;

namespace KsWare.IO.FileSystem {

	public static class SymbolicLink {

		/// <summary>
		/// Erstellt einen symbolischen Link.
		/// </summary>
		/// <param name="target">Das Ziel auf das der Link verweißen soll.</param>
		/// <param name="path">Der Pfad des symbolischen Links.</param>
		/// <param name="replaceExisting"><c>True</c>, wenn eine bereits existierende Datei bzw. ein bereits existierender symbolischer Link überschrieben werden soll.</param>
		public static void Create(string path, string target, bool replaceExisting) {
			if (replaceExisting) Delete(path);
//			var target1= Path.IsAbsolute(target) ? Path.MakeRelativePath(target, path) : target;
			var target1 = target;

			int flags;

			if (Directory.Exists(target)) flags=SYMBOLIC_LINK_FLAG_DIRECTORY;
			else if (File.Exists(target)) flags = 0;
			else throw Helper.IOException("Target not found");

			int result;
			if (!Helper.IsElevated) {
				result = PrivilegedExecutor.Client.Execute(
					typeof(SymbolicLink), nameof(CreateSymbolicLinkConsole), path, target, flags.ToString(CultureInfo.InvariantCulture));
			}
			else {
				result = CreateSymbolicLinkFix(path, target1, flags);
			}
			if (result != 0 && Enum.IsDefined(typeof(PrivilegedExecutor.ExitCode), result))
				throw Helper.IOException($"Unable to create symbolic link '{path}' -> '{target}'.",
					new TargetException(((ExitCode) result).ToString()));
			if (result != 0)
				throw Helper.IOException($"Unable to create symbolic link '{path}' -> '{target}'.", new Win32Exception(result));

			// https://stackoverflow.com/questions/33010440/createsymboliclink-on-windows-10
		}

		internal static int CreateSymbolicLinkConsole(string lpSymlinkFileName,string lpTargetFileName,string dwFlags) { 
			return CreateSymbolicLinkFix(lpSymlinkFileName, lpTargetFileName, int.Parse(dwFlags));
		}

		private static int CreateSymbolicLinkFix(string lpSymlinkFileName, string lpTargetFileName, int dwFlags) {
			var result = CreateSymbolicLink(lpSymlinkFileName, lpTargetFileName, dwFlags);
			// WORKAROUND result == 1 SUCCESS, != 1 FAIL 
			// in Win10 CreateSymbolicLink returns NOT 0 on failure, it returns a (negative) number
			// TEST:  result == 1 the symbolic link was created successfully.
			if (result == 1) return 0;
			return Marshal.GetLastWin32Error();
		}

		/// <summary>
		/// Deleted a symbolic link
		/// </summary>
		/// <param name="path">The symbolic link path.</param>
		public static void Delete(string path) {
			if (Directory.Exists(path)) Directory.Delete(path);
			else if (File.Exists(path)) File.Delete(path);
		}

		/// <summary>
		/// Gibt den Pfad eines Ordners oder einer Datei zurück wobei symbolische Links und bereit gestellte NTFS Ordner aufgelöst wurden.
		/// </summary>
		/// <param name="path">Der Pfad zu einem Ordner oder einer Datei.</param>
		/// <returns>Der wahre Pfad von <paramref name="path"/>.</returns>
		/// <remarks>Sollte kein Laufwerkpfad für den NTFS-Ordner verfügbar sein, so wird der Bereitstellungspfad zurück gegeben.</remarks>
		public static string GetTarget(string path) {
			if (!Directory.Exists(path) && !File.Exists(path))
				throw new IOException("Path not found");

			var symlink = PathHelper.LongPathSupport(Path.GetFullPath(path));
			using (var directoryHandle = CreateFile(symlink, 0, 2, System.IntPtr.Zero, CREATION_DISPOSITION_OPEN_EXISTING,
				FILE_FLAG_BACKUP_SEMANTICS,                  System.IntPtr.Zero)) {
				if (directoryHandle.IsInvalid)
					throw new Win32Exception(Marshal.GetLastWin32Error());

				var result = new StringBuilder(512);
				int mResult = GetFinalPathNameByHandle(directoryHandle.DangerousGetHandle(), result, result.Capacity, 0);
				if (mResult < 0)
					throw new Win32Exception(Marshal.GetLastWin32Error());
				if (result.Length >= 4 && result[0] == '\\' && result[1] == '\\' && result[2] == '?' && result[3] == '\\')
					return result.ToString().Substring(4);// "\\?\" entfernen
				else
					return result.ToString();
			}
		}

		#region WinApi

		// https://dotnet-snippets.de/snippet/symbolischen-link-erstellen-loeschen-und-auslesen/3791

		/// <summary>
		/// The link target is a directory.
		/// </summary>
		private const int SYMBOLIC_LINK_FLAG_DIRECTORY = 0x1;

		/// <summary>
		/// Specify this flag to allow creation of symbolic links when the process is not elevated.
		/// </summary>
		private const int SYMBOLIC_LINK_FLAG_ALLOW_UNPRIVILEGED_CREATE = 0x2;

		private const int CREATION_DISPOSITION_OPEN_EXISTING = 3;

		private const int FILE_FLAG_BACKUP_SEMANTICS = 0x02000000;

		// https://msdn.microsoft.com/de-de/library/windows/desktop/aa363866(v=vs.85).aspx
		// Minimum supported client: Windows Vista[desktop apps only]
		// Minimum supported server: Windows Server 2008 [desktop apps only]
//		[DllImport("kernel32.dll", EntryPoint = "CreateSymbolicLinkW", CharSet = CharSet.Unicode, SetLastError = true)]
//		private static extern bool CreateSymbolicLink(
//			[In] string lpSymlinkFileName,
//			[In] string lpTargetFileName,
//			[In] int dwFlags);

		[DllImport("kernel32.dll", EntryPoint = "CreateSymbolicLinkW", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern int CreateSymbolicLink(
			[In] string lpSymlinkFileName,
			[In] string lpTargetFileName,
			[In] int dwFlags);

		[DllImport("kernel32.dll", EntryPoint = "GetFinalPathNameByHandleW", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern int GetFinalPathNameByHandle([In] IntPtr hFile,
			[Out] StringBuilder lpszFilePath,
			[In] int cchFilePath,
			[In] int dwFlags);

		[DllImport("kernel32.dll", EntryPoint = "CreateFileW", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern SafeFileHandle CreateFile(string lpFileName,
			int dwDesiredAccess,
			int dwShareMode,
			IntPtr securityAttributes,
			int dwCreationDisposition,
			int dwFlagsAndAttributes,
			IntPtr hTemplateFile);



		#endregion
	}

}
