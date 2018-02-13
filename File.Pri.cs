namespace KsWare.IO.FileSystem {

	public static partial class File {

		public static System.IO.StreamReader OpenText(string path) => Pri.LongPath.File.OpenText(path);
		public static System.IO.StreamWriter CreateText(string path) => Pri.LongPath.File.CreateText(path);
		public static System.IO.StreamWriter AppendText(string path) => Pri.LongPath.File.AppendText(path);

		public static void Copy(string sourceFileName, string destFileName) =>
			Pri.LongPath.File.Copy(sourceFileName, destFileName);

		public static void Copy(string sourceFileName, string destFileName, bool overwrite) =>
			Pri.LongPath.File.Copy(sourceFileName, destFileName, overwrite);

		public static System.IO.FileStream Create(string path) => Pri.LongPath.File.Create(path);
		public static System.IO.FileStream Create(string path, int bufferSize) => Pri.LongPath.File.Create(path, bufferSize);

		public static System.IO.FileStream Create(string path, int bufferSize, System.IO.FileOptions options) =>
			Pri.LongPath.File.Create(path, bufferSize, options);

		public static System.IO.FileStream Create(string path,
			int bufferSize,
			System.IO.FileOptions options,
			System.Security.AccessControl.FileSecurity fileSecurity) =>
			Pri.LongPath.File.Create(path, bufferSize, options, fileSecurity);

		public static void Delete(string path) => Pri.LongPath.File.Delete(path);
		public static void Decrypt(string path) => Pri.LongPath.File.Decrypt(path);
		public static void Encrypt(string path) => Pri.LongPath.File.Encrypt(path);
		public static System.Boolean Exists(string path) => Pri.LongPath.File.Exists(path);
		public static System.IO.FileStream Open(string path, System.IO.FileMode mode) => Pri.LongPath.File.Open(path, mode);

		public static System.IO.FileStream Open(string path, System.IO.FileMode mode, System.IO.FileAccess access) =>
			Pri.LongPath.File.Open(path, mode, access);

		public static System.IO.FileStream Open(string path,
			System.IO.FileMode mode,
			System.IO.FileAccess access,
			System.IO.FileShare share) =>
			Pri.LongPath.File.Open(path, mode, access, share);

		public static void SetCreationTime(string path, System.DateTime creationTime) =>
			Pri.LongPath.File.SetCreationTime(path, creationTime);

		public static void SetCreationTimeUtc(string path, System.DateTime creationTimeUtc) =>
			Pri.LongPath.File.SetCreationTimeUtc(path, creationTimeUtc);

		public static System.DateTime GetCreationTime(string path) => Pri.LongPath.File.GetCreationTime(path);
		public static System.DateTime GetCreationTimeUtc(string path) => Pri.LongPath.File.GetCreationTimeUtc(path);

		public static void SetLastAccessTime(string path, System.DateTime lastAccessTime) =>
			Pri.LongPath.File.SetLastAccessTime(path, lastAccessTime);

		public static void SetLastAccessTimeUtc(string path, System.DateTime lastAccessTimeUtc) =>
			Pri.LongPath.File.SetLastAccessTimeUtc(path, lastAccessTimeUtc);

		public static System.DateTime GetLastAccessTime(string path) => Pri.LongPath.File.GetLastAccessTime(path);
		public static System.DateTime GetLastAccessTimeUtc(string path) => Pri.LongPath.File.GetLastAccessTimeUtc(path);

		public static void SetLastWriteTime(string path, System.DateTime lastWriteTime) =>
			Pri.LongPath.File.SetLastWriteTime(path, lastWriteTime);

		public static void SetLastWriteTimeUtc(string path, System.DateTime lastWriteTimeUtc) =>
			Pri.LongPath.File.SetLastWriteTimeUtc(path, lastWriteTimeUtc);

		public static System.DateTime GetLastWriteTime(string path) => Pri.LongPath.File.GetLastWriteTime(path);
		public static System.DateTime GetLastWriteTimeUtc(string path) => Pri.LongPath.File.GetLastWriteTimeUtc(path);
		public static System.IO.FileAttributes GetAttributes(string path) => Pri.LongPath.File.GetAttributes(path);

		public static void SetAttributes(string path, System.IO.FileAttributes fileAttributes) =>
			Pri.LongPath.File.SetAttributes(path, fileAttributes);

		public static System.Security.AccessControl.FileSecurity GetAccessControl(string path) =>
			Pri.LongPath.File.GetAccessControl(path);

		public static System.Security.AccessControl.FileSecurity GetAccessControl(string path,
			System.Security.AccessControl.AccessControlSections includeSections) =>
			Pri.LongPath.File.GetAccessControl(path, includeSections);

		public static void SetAccessControl(string path, System.Security.AccessControl.FileSecurity fileSecurity) =>
			Pri.LongPath.File.SetAccessControl(path, fileSecurity);

		public static System.IO.FileStream OpenRead(string path) => Pri.LongPath.File.OpenRead(path);
		public static System.IO.FileStream OpenWrite(string path) => Pri.LongPath.File.OpenWrite(path);
		public static string ReadAllText(string path) => Pri.LongPath.File.ReadAllText(path);

		public static string ReadAllText(string path, System.Text.Encoding encoding) =>
			Pri.LongPath.File.ReadAllText(path, encoding);

		public static void WriteAllText(string path, string contents) => Pri.LongPath.File.WriteAllText(path, contents);

		public static void WriteAllText(string path, string contents, System.Text.Encoding encoding) =>
			Pri.LongPath.File.WriteAllText(path, contents, encoding);

		public static System.Byte[] ReadAllBytes(string path) => Pri.LongPath.File.ReadAllBytes(path);
		public static void WriteAllBytes(string path, System.Byte[] bytes) => Pri.LongPath.File.WriteAllBytes(path, bytes);
		public static System.String[] ReadAllLines(string path) => Pri.LongPath.File.ReadAllLines(path);

		public static System.String[] ReadAllLines(string path, System.Text.Encoding encoding) =>
			Pri.LongPath.File.ReadAllLines(path, encoding);

		public static System.Collections.Generic.IEnumerable<string> ReadLines(string path) =>
			Pri.LongPath.File.ReadLines(path);

		public static System.Collections.Generic.IEnumerable<string> ReadLines(string path, System.Text.Encoding encoding) =>
			Pri.LongPath.File.ReadLines(path, encoding);

		public static void WriteAllLines(string path, System.String[] contents) =>
			Pri.LongPath.File.WriteAllLines(path, contents);

		public static void WriteAllLines(string path, System.String[] contents, System.Text.Encoding encoding) =>
			Pri.LongPath.File.WriteAllLines(path, contents, encoding);

		public static void WriteAllLines(string path, System.Collections.Generic.IEnumerable<string> contents) =>
			Pri.LongPath.File.WriteAllLines(path, contents);

		public static void WriteAllLines(string path,
			System.Collections.Generic.IEnumerable<string> contents,
			System.Text.Encoding encoding) =>
			Pri.LongPath.File.WriteAllLines(path, contents, encoding);

		public static void AppendAllText(string path, string contents) => Pri.LongPath.File.AppendAllText(path, contents);

		public static void AppendAllText(string path, string contents, System.Text.Encoding encoding) =>
			Pri.LongPath.File.AppendAllText(path, contents, encoding);

		public static void AppendAllLines(string path, System.Collections.Generic.IEnumerable<string> contents) =>
			Pri.LongPath.File.AppendAllLines(path, contents);

		public static void AppendAllLines(string path,
			System.Collections.Generic.IEnumerable<string> contents,
			System.Text.Encoding encoding) =>
			Pri.LongPath.File.AppendAllLines(path, contents, encoding);

		public static void Move(string sourceFileName, string destFileName) =>
			Pri.LongPath.File.Move(sourceFileName, destFileName);

		public static void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName) =>
			Pri.LongPath.File.Replace(sourceFileName, destinationFileName, destinationBackupFileName);

		public static void Replace(string sourceFileName,
			string destinationFileName,
			string destinationBackupFileName,
			System.Boolean ignoreMetadataErrors) =>
			Pri.LongPath.File.Replace(sourceFileName, destinationFileName, destinationBackupFileName, ignoreMetadataErrors);

	}

}