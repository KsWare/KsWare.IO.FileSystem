using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using KsWare.IO.FileSystem.Internal;
using static KsWare.IO.FileSystem.Internal.WinApi;

namespace KsWare.IO.FileSystem {

	public static partial class Path {

		public static readonly Dictionary<char, char> SafeFileNameReplacementChars = new Dictionary<char, char>();

		public static string GetSafeFileName(string name) {
			var result = Path.GetInvalidFileNameChars()
				.Aggregate(name, (current, c) => current.Replace(c, GetReplacementChar(c)));
			return result;
		}

		private static char GetReplacementChar(char c) {
			char r;
			return SafeFileNameReplacementChars.TryGetValue(c, out r) ? r : '_';
		}


		/// <summary>
		/// Gibt den Pfad eines Ordners oder einer Datei zurück wobei symbolische Links und bereit gestellte NTFS Ordner aufgelöst wurden.
		/// </summary>
		/// <param name="path">Der Pfad zu einem Ordner oder einer Datei.</param>
		/// <returns>Der wahre Pfad von <paramref name="path"/>.</returns>
		/// <remarks>Sollte kein Laufwerkpfad für den NTFS-Ordner verfügbar sein, so wird der Bereitstellungspfad zurück gegeben.</remarks>
		public static string GetRealPath(string path) {
			if (!Directory.Exists(path) && !File.Exists(path))
				throw Helper.IOException("Path not found");

			var symlink = new DirectoryInfo(path); //Es ist egel ob es eine Datei oder ein Ordner ist
			var directoryHandle = CreateFile(symlink.FullName, 0, 2, System.IntPtr.Zero, CREATION_DISPOSITION_OPEN_EXISTING, FILE_FLAG_BACKUP_SEMANTICS, System.IntPtr.Zero); //Handle zur Datei/Ordner
			if (directoryHandle.IsInvalid)
				throw Helper.Win32Exception();

			var result = new StringBuilder(512);
			var mResult = GetFinalPathNameByHandle(directoryHandle.DangerousGetHandle(), result, result.Capacity, 0);
			if (mResult < 0)
				throw new Win32Exception(Marshal.GetLastWin32Error());
			if (result.Length >= 4 && result[0] == '\\' && result[1] == '\\' && result[2] == '?' && result[3] == '\\')
				return result.ToString().Substring(4); // "\\?\" entfernen
			else
				return result.ToString();
		}

		/// <summary>
		/// Determines whether the specified path is absolute.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns><c>true</c> if the specified path is absolute; otherwise, <c>false</c>.</returns>
		/// <remarks>A path is absolute if <see cref="System.Environment.CurrentDirectory"/> is not requiered.</remarks>
		public static bool IsAbsolute(string path) {
			if(path==null) throw new ArgumentNullException(nameof(path));
			if (IsVolume(path)) return true;
			path = PathHelper.RemovePrefix(path); //path must not be prefixed
			if (path.StartsWith(@"\\")) return true;
			if (Regex.IsMatch(path, @"(?inx-ms:^[A-Z]:[\\])")) return true;

			// C:path  => C:\%CurentDirOnC%\path
			return false;
		}

		public static bool IsVolume(string path) => path != null && path.StartsWith(@"\\?\Volume{");

		public static bool IsUnc(string path) => path != null && (path.StartsWith(@"\\") || path.StartsWith(@"\\?\UNC\"));

		public static List<string> SplitPath(string path) {
			var m = Regex.Match(path, @"(?inx-ms:((?<unc>\\\\[^\\]+\\[^\\]+)|(?<drive>[A-Z]\:))?\\?(?<path>.*))");
			var tokens = m.Groups["path"].Value.Split('\\').ToList(); // split path
			if(tokens[tokens.Count-1]==string.Empty) tokens.RemoveAt(tokens.Count - 1); // A:\B\ <trailing backslash

			if (m.Groups["drive"].Success) {
				tokens.Insert(0, m.Groups["drive"].Value);
			}
			else if (m.Groups["unc"].Success)
				tokens.Insert(0, m.Groups["unc"].Value);
			
			return tokens;
		}

		public static string NormalizePath(string path) {
			if(!IsAbsolute(path)) throw new ArgumentException("Path must be absolute",nameof(path));
			//TODO implement support for relative path

			var p = path;
			p     = PathHelper.RemovePrefix(p);
			p     = p.Replace("/", "\\");
			p     = RemoveTrailingBackslash(p);

			var tokens = SplitPath(p);
			// .
			// ..
			// 
			// C:\A\..\B  => C:\B
			// C:\A\B\..\..\C  => C:\C
			// C:\A\..  => C:\
			// C:\A\.  => C:\A
			// C:\A\.\B => C:\A\B
			// \\SERVER\SHARE\

			for (int i = 1; i < tokens.Count; i++) {
				if (tokens[i]    == ".") {
					tokens.RemoveAt(i);
					i--;
				}
			}

			for (int i = 1; i < tokens.Count; i++) {
				if (tokens[i]    == "..") {
					if (i           == 1)
						throw new ArgumentException("Invalid path.", nameof(path));
					tokens.RemoveAt(i);
					tokens.RemoveAt(i - 1);
					i = 0;
				}
			}
			if (tokens.Count == 1) {
				return tokens[0] + "\\"; 
			}
			p = string.Join("\\", tokens);
			return p;
		}

		public static string MakeAbsolute(string path, string anchor) {
			string p;
			if (IsAbsolute(path))
				p = path;
			else if (path.StartsWith(@"\"))
				p = AddTrailingBackslash(SplitPath(anchor)[0]) + path.Substring(1); 
			else if (path.StartsWith(".."))
				p = AddTrailingBackslash(anchor) + path;
			else
				p = AddTrailingBackslash(anchor) + path;

			return NormalizePath(p);
		}

		public static string AddTrailingBackslash(string path) {
			if(path==null) throw new ArgumentNullException(nameof(path));
			return !path.EndsWith("\\") ? path + "\\" : path;
		}

		public static string RemoveTrailingBackslash(string path) {
			return path != null && path.EndsWith("\\") ? path.Substring(0,path.Length-1) : path;
		}

		public static string MakeRelativePath(string path, string anchor) {
			if (path == null) throw new ArgumentNullException(nameof(path));
			if (anchor == null) throw new ArgumentNullException(nameof(anchor));
			if (!IsAbsolute(path)) throw new ArgumentException("Path must be absolute.", nameof(path));
			if (!IsAbsolute(anchor)) throw new ArgumentException("Path must be absolute.", nameof(anchor));
			//TODO
//			throw new NotImplementedException();

			//		A: B C		A: C		A  B C		A: B
			//		A:			A: B        A  D		A: C  D
			//		   B C		.. B		.. B C		.. .. B
			// c	1			1			1			1

			var p = SplitPath(path);
			var a = SplitPath(anchor);
			var r = new List<string>();

			var c = 0;
			while (c < p.Count && c < a.Count && p[c]==a[c]) c++;
			if(c==0) throw new InvalidOperationException("Relative path not possible.");

			for (var i = 0; i < a.Count - c; i++) r.Add("..");
			for (var i = c; i < p.Count; i++) r.Add(p[i]);

			return string.Join("\\", r);
		}

		public static string ChangeFileName(string path, string fileName) {
			if(path==null) throw new ArgumentNullException(nameof(path));
			if(fileName ==null) throw new ArgumentNullException(nameof(fileName));
			var p = path.LastIndexOf("\\");
			var path0 = path.Substring(0, p);
			var path1 = path.Substring(p+1);
			return path0 + "\\" + fileName;
		}

		public static string ChangeFileNameWithoutExtension(string path, string fileName) {
			if(path==null) throw new ArgumentNullException(nameof(path));
			if(fileName ==null) throw new ArgumentNullException(nameof(fileName));
			var p     = path.LastIndexOf("\\");
			var path0 = path.Substring(0, p);
			var ext   = GetExtension(path);
			return path0 + "\\" + fileName + ext;
		}

		#region WinApi

		[DllImport("kernel32.dll", EntryPoint = "GetFinalPathNameByHandleW", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern int GetFinalPathNameByHandle([In] IntPtr hFile,
			[Out] StringBuilder lpszFilePath,
			[In] int cchFilePath,
			[In] int dwFlags);

		#endregion

	}
}
