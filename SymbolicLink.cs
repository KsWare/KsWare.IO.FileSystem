using System.ComponentModel;
using System.Runtime.InteropServices;
using KsWare.IO.FileSystem.Internal;

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
			var result = false;
			if (Directory.Exists(target)) {
				result = CreateSymbolicLink(path, target, SYMBOLIC_LINK_FLAG_DIRECTORY);
			}
			else if (File.Exists(target)) {
				result = CreateSymbolicLink(path, target, 0);
			}
			else
				throw Helper.IOException("Target not found");
			if (!result)
				throw new Win32Exception(Marshal.GetLastWin32Error());
		}

		/// <summary>
		/// Löscht einen symbolischen Link.
		/// </summary>
		/// <param name="path">Der Pfad zum symbolischen Link.</param>
		public static void Delete(string path) {
			if (Directory.Exists(path)) Directory.Delete(path);
			if (File.Exists(path)) File.Delete(path);
		}

		#region WinApi

		/// <summary>
		/// The link target is a directory.
		/// </summary>
		private const int SYMBOLIC_LINK_FLAG_DIRECTORY = 0x1;

		/// <summary>
		/// Specify this flag to allow creation of symbolic links when the process is not elevated.
		/// </summary>
		private const int SYMBOLIC_LINK_FLAG_ALLOW_UNPRIVILEGED_CREATE = 0x2;


		// Minimum supported client: Windows Vista[desktop apps only]
		// Minimum supported server: Windows Server 2008 [desktop apps only]
		[DllImport("kernel32.dll", EntryPoint = "CreateSymbolicLinkW", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern bool CreateSymbolicLink(
			[In] string lpSymlinkFileName,
			[In] string lpTargetFileName,
			[In] int dwFlags);

		#endregion
	}

}
