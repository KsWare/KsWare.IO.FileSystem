using System;
using System.Runtime.InteropServices;
using KsWare.IO.FileSystem.Internal;

namespace KsWare.IO.FileSystem
{

	public static class HardLink
	{
		/// <summary>
		/// Creates a hard link.
		/// </summary>
		/// <param name="existingFileName">The path to the existing file.</param>
		/// <param name="fileName">The path to the new file</param>
		public static void Create(string fileName, string existingFileName )
		{
			var result = CreateHardLink(fileName, existingFileName, IntPtr.Zero);
			if (!result)
				throw Helper.IOExceptionForLastWin32Error($"Can not create hardlink {fileName} -> {existingFileName}");
		}

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern bool CreateHardLink(string lpFileName, string lpExistingFileName, IntPtr lpSecurityAttributes);


	}
}
