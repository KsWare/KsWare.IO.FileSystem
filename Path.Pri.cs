using System;

namespace KsWare.IO.FileSystem {

	public static partial class Path {

		public static string ChangeExtension(string path, string extension) => Pri.LongPath.Path.ChangeExtension(path, extension);

		public static string GetDirectoryName(string path) => Pri.LongPath.Path.GetDirectoryName(path);
		public static Char[] GetInvalidPathChars() => Pri.LongPath.Path.GetInvalidPathChars();
		public static Char[] GetInvalidFileNameChars() => Pri.LongPath.Path.GetInvalidFileNameChars();
		public static string GetExtension(string path) => Pri.LongPath.Path.GetExtension(path);
		public static string GetFullPath(string path) => Pri.LongPath.Path.GetFullPath(path);
		public static string GetFileName(string path) => Pri.LongPath.Path.GetFileName(path);
		public static string GetFileNameWithoutExtension(string path) => Pri.LongPath.Path.GetFileNameWithoutExtension(path);
		public static string GetPathRoot(string path) => Pri.LongPath.Path.GetPathRoot(path);
		public static string GetTempPath() => Pri.LongPath.Path.GetTempPath();
		public static string GetRandomFileName() => Pri.LongPath.Path.GetRandomFileName();
		public static string GetTempFileName() => Pri.LongPath.Path.GetTempFileName();
		public static Boolean HasExtension(string path) => Pri.LongPath.Path.HasExtension(path);
		public static Boolean IsPathRooted(string path) => Pri.LongPath.Path.IsPathRooted(path);
		public static string Combine(string path1, string path2) => Pri.LongPath.Path.Combine(path1, path2);

		public static string Combine(string path1, string path2, string path3) =>
			Pri.LongPath.Path.Combine(path1, path2, path3);

		public static string Combine(string path1, string path2, string path3, string path4) =>
			Pri.LongPath.Path.Combine(path1, path2, path3, path4);

		public static string Combine(string[] paths) => Pri.LongPath.Path.Combine(paths);

	}

}