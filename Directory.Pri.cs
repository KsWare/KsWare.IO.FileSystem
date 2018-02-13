namespace KsWare.IO.FileSystem {

	public static partial class Directory {

		#region Pri.LongPath.Directory

		public static Pri.LongPath.DirectoryInfo GetParent(string path) => Pri.LongPath.Directory.GetParent(path);
		public static Pri.LongPath.DirectoryInfo CreateDirectory(string path) => Pri.LongPath.Directory.CreateDirectory(path);

		public static Pri.LongPath.DirectoryInfo CreateDirectory(string path,
			System.Security.AccessControl.DirectorySecurity directorySecurity) =>
			Pri.LongPath.Directory.CreateDirectory(path, directorySecurity);

		public static System.Boolean Exists(string path) => Pri.LongPath.Directory.Exists(path);

		public static void SetCreationTime(string path, System.DateTime creationTime) =>
			Pri.LongPath.Directory.SetCreationTime(path, creationTime);

		public static void SetCreationTimeUtc(string path, System.DateTime creationTimeUtc) =>
			Pri.LongPath.Directory.SetCreationTimeUtc(path, creationTimeUtc);

		public static System.DateTime GetCreationTime(string path) => Pri.LongPath.Directory.GetCreationTime(path);
		public static System.DateTime GetCreationTimeUtc(string path) => Pri.LongPath.Directory.GetCreationTimeUtc(path);

		public static void SetLastWriteTime(string path, System.DateTime lastWriteTime) =>
			Pri.LongPath.Directory.SetLastWriteTime(path, lastWriteTime);

		public static void SetLastWriteTimeUtc(string path, System.DateTime lastWriteTimeUtc) =>
			Pri.LongPath.Directory.SetLastWriteTimeUtc(path, lastWriteTimeUtc);

		public static System.DateTime GetLastWriteTime(string path) => Pri.LongPath.Directory.GetLastWriteTime(path);
		public static System.DateTime GetLastWriteTimeUtc(string path) => Pri.LongPath.Directory.GetLastWriteTimeUtc(path);

		public static void SetLastAccessTime(string path, System.DateTime lastAccessTime) =>
			Pri.LongPath.Directory.SetLastAccessTime(path, lastAccessTime);

		public static void SetLastAccessTimeUtc(string path, System.DateTime lastAccessTimeUtc) =>
			Pri.LongPath.Directory.SetLastAccessTimeUtc(path, lastAccessTimeUtc);

		public static System.DateTime GetLastAccessTime(string path) => Pri.LongPath.Directory.GetLastAccessTime(path);
		public static System.DateTime GetLastAccessTimeUtc(string path) => Pri.LongPath.Directory.GetLastAccessTimeUtc(path);

		public static System.Security.AccessControl.DirectorySecurity GetAccessControl(string path) =>
			Pri.LongPath.Directory.GetAccessControl(path);

		public static System.Security.AccessControl.DirectorySecurity GetAccessControl(string path,
			System.Security.AccessControl.AccessControlSections includeSections) =>
			Pri.LongPath.Directory.GetAccessControl(path, includeSections);

		public static void SetAccessControl(string path, System.Security.AccessControl.DirectorySecurity directorySecurity) =>
			Pri.LongPath.Directory.SetAccessControl(path, directorySecurity);

		public static System.String[] GetFiles(string path) => Pri.LongPath.Directory.GetFiles(path);

		public static System.String[] GetFiles(string path, string searchPattern) =>
			Pri.LongPath.Directory.GetFiles(path, searchPattern);

		public static System.String[] GetFiles(string path, string searchPattern, System.IO.SearchOption searchOption) =>
			Pri.LongPath.Directory.GetFiles(path, searchPattern, searchOption);

		public static System.String[] GetDirectories(string path) => Pri.LongPath.Directory.GetDirectories(path);

		public static System.String[] GetDirectories(string path, string searchPattern) =>
			Pri.LongPath.Directory.GetDirectories(path, searchPattern);

		public static System.String[]
			GetDirectories(string path, string searchPattern, System.IO.SearchOption searchOption) =>
			Pri.LongPath.Directory.GetDirectories(path, searchPattern, searchOption);

		public static System.String[] GetFileSystemEntries(string path) => Pri.LongPath.Directory.GetFileSystemEntries(path);

		public static System.String[] GetFileSystemEntries(string path, string searchPattern) =>
			Pri.LongPath.Directory.GetFileSystemEntries(path, searchPattern);

		public static System.String[] GetFileSystemEntries(string path,
			string searchPattern,
			System.IO.SearchOption searchOption) =>
			Pri.LongPath.Directory.GetFileSystemEntries(path, searchPattern, searchOption);

		public static System.Collections.Generic.IEnumerable<string> EnumerateDirectories(string path) =>
			Pri.LongPath.Directory.EnumerateDirectories(path);

		public static System.Collections.Generic.IEnumerable<string>
			EnumerateDirectories(string path, string searchPattern) =>
			Pri.LongPath.Directory.EnumerateDirectories(path, searchPattern);

		public static System.Collections.Generic.IEnumerable<string> EnumerateDirectories(string path,
			string searchPattern,
			System.IO.SearchOption searchOption) =>
			Pri.LongPath.Directory.EnumerateDirectories(path, searchPattern, searchOption);

		public static System.Collections.Generic.IEnumerable<string> EnumerateFiles(string path) =>
			Pri.LongPath.Directory.EnumerateFiles(path);

		public static System.Collections.Generic.IEnumerable<string> EnumerateFiles(string path, string searchPattern) =>
			Pri.LongPath.Directory.EnumerateFiles(path, searchPattern);

		public static System.Collections.Generic.IEnumerable<string> EnumerateFiles(string path,
			string searchPattern,
			System.IO.SearchOption searchOption) =>
			Pri.LongPath.Directory.EnumerateFiles(path, searchPattern, searchOption);

		public static System.Collections.Generic.IEnumerable<string> EnumerateFileSystemEntries(string path) =>
			Pri.LongPath.Directory.EnumerateFileSystemEntries(path);

		public static System.Collections.Generic.IEnumerable<string>
			EnumerateFileSystemEntries(string path, string searchPattern) =>
			Pri.LongPath.Directory.EnumerateFileSystemEntries(path, searchPattern);

		public static System.Collections.Generic.IEnumerable<string> EnumerateFileSystemEntries(string path,
			string searchPattern,
			System.IO.SearchOption searchOption) =>
			Pri.LongPath.Directory.EnumerateFileSystemEntries(path, searchPattern, searchOption);

		public static System.String[] GetLogicalDrives() => Pri.LongPath.Directory.GetLogicalDrives();
		public static string GetDirectoryRoot(string path) => Pri.LongPath.Directory.GetDirectoryRoot(path);
		public static string GetCurrentDirectory() => Pri.LongPath.Directory.GetCurrentDirectory();
		public static void SetCurrentDirectory(string path) => Pri.LongPath.Directory.SetCurrentDirectory(path);

		public static void Move(string sourceDirName, string destDirName) =>
			Pri.LongPath.Directory.Move(sourceDirName, destDirName);

		public static void Delete(string path) => Pri.LongPath.Directory.Delete(path);
		public static void Delete(string path, System.Boolean recursive) => Pri.LongPath.Directory.Delete(path, recursive);

		#endregion

	}

}