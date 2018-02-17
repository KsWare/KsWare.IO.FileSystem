using System;
using System.Diagnostics.CodeAnalysis;

namespace KsWare.IO.FileSystem {

	[SuppressMessage("ReSharper", "UnusedMember.Global")]
	public static partial class Path {

		/// <inheritdoc cref="System.IO.Path.Combine(string, string)"/>
		public static string Combine(string path1, string path2) => Pri.LongPath.Path.Combine(path1, path2);

		/// <inheritdoc cref="System.IO.Path.IsPathRooted(string)"/>
		public static bool IsPathRooted(string path) => Pri.LongPath.Path.IsPathRooted(path);

		/// <inheritdoc cref="System.IO.Path.Combine(string, string, string)"/>
		public static string Combine(string path1, string path2, string path3) =>
			Pri.LongPath.Path.Combine(path1, path2, path3);

		/// <inheritdoc cref="System.IO.Path.Combine(string, string, string, string)"/>
		public static string Combine(string path1, string path2, string path3, string path4) =>
			Pri.LongPath.Path.Combine(path1, path2, path3, path4);

		/// <inheritdoc cref="System.IO.Path.GetFileName(string)"/>
		public static string GetFileName(string path) => Pri.LongPath.Path.GetFileName(path);

		/// <inheritdoc cref="System.IO.Path.GetFullPath(string)"/>
		public static string GetFullPath(string path) => Pri.LongPath.Path.GetFullPath(path);

		/// <inheritdoc cref="System.IO.Path.GetDirectoryName(string)"/>
		public static string GetDirectoryName(string path) => Pri.LongPath.Path.GetDirectoryName(path);

		/// <inheritdoc cref="System.IO.Path.GetInvalidPathChars()"/>
		public static System.Char[] GetInvalidPathChars() => Pri.LongPath.Path.GetInvalidPathChars();

		/// <inheritdoc cref="System.IO.Path.GetInvalidFileNameChars()"/>
		public static System.Char[] GetInvalidFileNameChars() => Pri.LongPath.Path.GetInvalidFileNameChars();

		/// <inheritdoc cref="System.IO.Path.GetRandomFileName()"/>
		public static string GetRandomFileName() => Pri.LongPath.Path.GetRandomFileName();

		/// <inheritdoc cref="System.IO.Path.GetPathRoot(string)"/>
		public static string GetPathRoot(string path) => Pri.LongPath.Path.GetPathRoot(path);

		/// <inheritdoc cref="System.IO.Path.GetExtension(string)"/>
		public static string GetExtension(string path) => Pri.LongPath.Path.GetExtension(path);

		/// <inheritdoc cref="System.IO.Path.HasExtension(string)"/>
		public static bool HasExtension(string path) => Pri.LongPath.Path.HasExtension(path);

		/// <inheritdoc cref="System.IO.Path.GetTempPath()"/>
		public static string GetTempPath() => Pri.LongPath.Path.GetTempPath();

		/// <inheritdoc cref="System.IO.Path.GetTempFileName()"/>
		public static string GetTempFileName() => Pri.LongPath.Path.GetTempFileName();

		/// <inheritdoc cref="System.IO.Path.GetFileNameWithoutExtension(string)"/>
		public static string GetFileNameWithoutExtension(string path) => Pri.LongPath.Path.GetFileNameWithoutExtension(path);

		/// <inheritdoc cref="System.IO.Path.ChangeExtension(string, string)"/>
		public static string ChangeExtension(string filename, string extension) =>
			Pri.LongPath.Path.ChangeExtension(filename, extension);

		/// <inheritdoc cref="System.IO.Path.Combine(System.String[])"/>
		public static string Combine(System.String[] paths) => Pri.LongPath.Path.Combine(paths);

	}

}