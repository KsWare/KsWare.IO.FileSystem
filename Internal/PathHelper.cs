using System;
using System.IO;
using System.Text.RegularExpressions;

namespace KsWare.IO.FileSystem.Internal {
	using static KsWare.IO.FileSystem.Path;

	public static class PathHelper {

		public const int MaxPathInFileExplorer = 247; // (Windows 10)
		public const int MaxDirectoryNameLength = 255;
		public const int MaxPath = 32767;
		public const int MaxPathLimit = WinApi.MAX_PATH-1; // MAX_PATH=260;  D:\some 256-character path string<NUL>

		public static bool HasPrefix(string path) {
			if (path == null) return false;
			if (path.StartsWith(@"\\?\")) return true;
			if (path.StartsWith(@"\\?\UNC\")) return true;
			if (path.StartsWith(@"\\.\")) return true; // Win32 Device Namespaces
			if (path.StartsWith(@"\??\")) return true;
			return false;
		}

		public static string GetPrefix(string path) {
			if (path == null) return null;
//			if (path.StartsWith(@"\\?\Volume{")) return @"\\?\Volume; // prefix for volumes
			if (path.StartsWith(@"\\?\UNC\")) return @"\\?\UNC\";
			if (path.StartsWith(@"\\?\")) return @"\\?\";
			if (path.StartsWith(@"\\.\")) return @"\\.\"; // Win32 Device Namespaces
			if (path.StartsWith(@"\??\")) return @"\??\";
			return null;

		}

		public static string RemovePrefix(string path) {
			if (path == null) return null;
			if (path.StartsWith(@"\\?\Volume{")) return path; // keep prefix for volumes
			if (path.StartsWith(@"\??\Volume{")) return @"\\?\" + path.Substring(4); // keep prefix for volumes
			if (path.StartsWith(@"\\?\UNC\")) return @"\\" + path.Substring(8);
			if (path.StartsWith(@"\\?\")) return path.Substring(4);
			if (path.StartsWith(@"\??\")) return path.Substring(4);
			if (path.StartsWith(@"\\.\")) return path.Substring(4); // Win32 Device Namespaces
			return path;
		}

		public static string LongPathSupport(string path) {
			// TODO revise name and functionality
	
			if (HasPrefix(path)) return path;
			if (!IsAbsolute(path)) {
				if(path.Length> MaxPathLimit) throw new PathTooLongException();
				return path; // TODO check max length
			}
			return path.Length > MaxPathLimit ? LongPath(path) : path;
		}
		
		public static string LongPath(string path) {
			// TODO check maxlength
			// if(!IsAbsolute(path)) checked by NormalizePath
			path = Path.NormalizePath(path); //removes .. and .
			if (path.StartsWith(@"\\")) path = @"\\?\UNC\" + path.Substring(2);
			else                        path = @"\\?\" + path;
			return path;
		}

		public static bool StartsWithVolumeName(string path) {
			if (path == null) return false;
			// \\?\ and \??\ are valid
			return Regex.IsMatch(path, @"^\\[\\\?]\?\\Volume\{[0-9A-F]{8}(-[0-9A-F]{4}){3}-[0-9A-F]{12}\}\\",
				RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
		}

		/// <summary>
		/// Determines whether volume name is valid.
		/// </summary>
		/// <param name="maybeVolumeName">The path.</param>
		/// <returns><c>true</c> if volume name is valid; otherwise, <c>false</c>.</returns>
		/// <remarks><para>Example: <c>\\?\Volume{7B4BE779-27AF-48C6-91CC-DA21C9E78FBC}\</c></para></remarks>
		public static bool IsValidVolumeName(string maybeVolumeName) {
			if (maybeVolumeName == null) return false;
			// \\?\ and \??\ are valid
			return Regex.IsMatch(maybeVolumeName, @"^\\[\\\?]\?\\Volume\{[0-9A-F]{8}(-[0-9A-F]{4}){3}-[0-9A-F]{12}\}\\$",
				RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
		}

		/// <summary>
		/// Adds a trailing backslash if no one exists.
		/// </summary>
		/// <param name="path">The path to be changed.</param>
		/// <exception cref="ArgumentNullException">path</exception>
		public static void AddTrailingBackslash(ref string path) {
			if (path == null) throw new ArgumentNullException(nameof(path));
			if (!path.EndsWith("\\")) path += "\\";
		}

		/// <summary>
		/// Adds a trailing backslash if no one exists.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns>A path with a trailing backslash.</returns>
		/// <exception cref="ArgumentNullException">path</exception>
		public static string AddTrailingBackslash(string path) {
			if(path==null) throw new ArgumentNullException(nameof(path));
			return !path.EndsWith("\\") ? path += "\\" : path;
		}

		
	}
}
