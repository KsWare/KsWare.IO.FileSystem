using System;
using System.Runtime.InteropServices;
using KsWare.IO.FileSystem.Internal;
using static KsWare.IO.FileSystem.Internal.PathHelper;
using static KsWare.IO.FileSystem.Internal.FileManagementApi;

namespace KsWare.IO.FileSystem {

	public static class HardLink {

		/// <summary>
		/// Creates a hard link.
		/// </summary>
		/// <param name="fileName">The path to the new file</param>
		/// <param name="existingFileName">The path to the existing file.</param>
		public static void Create(string fileName, string existingFileName) {
			var result = CreateHardLink(LongPathSupport(fileName), LongPathSupport(existingFileName), IntPtr.Zero);
			if (!result) throw Helper.IOExceptionForLastWin32Error($"Can not create hardlink {fileName} -> {existingFileName}");
		}

		public static void CreateUnchecked(string fileName, string existingFileName) {
			var result = CreateHardLink(fileName, existingFileName, IntPtr.Zero);
			if (!result) throw Helper.IOExceptionForLastWin32Error($"Can not create hardlink {fileName} -> {existingFileName}");
		}

		/// <summary>
		/// Gets the number of links to the specified file.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns>The number of links.</returns>
		public static int GetNumberOfLinks(string path) {
			using (var handle = FileManagementApi.CreateFile(LongPathSupport(path), 0, 2, System.IntPtr.Zero,
				CREATION_DISPOSITION_OPEN_EXISTING, 0, System.IntPtr.Zero)) {
				if(handle.IsInvalid) throw Helper.ExceptionForLastWin32Error();
				BY_HANDLE_FILE_INFORMATION info;
				if (!GetFileInformationByHandle(handle, out info)) throw Helper.ExceptionForLastWin32Error();
				return (int) info.NumberOfLinks;
			}
		}

		/// <inheritdoc cref="System.IO.File.Delete(string)"/>
		/// <param name="keepMinimumLinks">The minimum number of links to keep. The default valaue is 1.</param>
		/// <returns><c>true</c> if the file could be deleted; else <c>false</c>.</returns>
		/// <remarks><para>If <paramref name="keepMinimumLinks"/> is larger the 0, the file is deleted only if the number of the remaining links is greater or equal <paramref name="keepMinimumLinks"/>.</para></remarks>
		/// <seealso cref="System.IO.File.Delete(string)"/>
		public static bool Delete(string path, int keepMinimumLinks = 1) {
			if (keepMinimumLinks == 0) {
				File.Delete(path);
			}
			else {
				var numberOfLinks = GetNumberOfLinks(path);
				if(numberOfLinks<= keepMinimumLinks ) return false;
				File.Delete(path);
			}
			return true;
		}

		#region WinApi

		// https://msdn.microsoft.com/en-us/library/windows/desktop/aa363860.aspx
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern bool CreateHardLink(string lpFileName, string lpExistingFileName, IntPtr lpSecurityAttributes);

		#endregion

	}

}
