using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace KsWare.IO.FileSystem {

	/// <inheritdoc cref="System.IO.File"/>
	[SuppressMessage("ReSharper", "UnusedMember.Global")]
	public static partial class File {

		/// <inheritdoc cref="System.IO.File.OpenText(string)"/>
		public static System.IO.StreamReader OpenText(string path) => Pri.LongPath.File.OpenText(path);
		/// <inheritdoc cref="System.IO.File.CreateText(string)"/>
		public static System.IO.StreamWriter CreateText(string path) => Pri.LongPath.File.CreateText(path);
		/// <inheritdoc cref="System.IO.File.AppendText(string)"/>
		public static System.IO.StreamWriter AppendText(string path) => Pri.LongPath.File.AppendText(path);
		/// <inheritdoc cref="System.IO.File.Copy(string, string)"/>
		public static void Copy(string sourceFileName, string destFileName) => Pri.LongPath.File.Copy(sourceFileName, destFileName);
		/// <inheritdoc cref="System.IO.File.Copy(string, string, bool)"/>
		public static void Copy(string sourcePath, string destinationPath, bool overwrite) => Pri.LongPath.File.Copy(sourcePath, destinationPath, overwrite);
		/// <inheritdoc cref="System.IO.File.Create(string)"/>
		public static System.IO.FileStream Create(string path) => Pri.LongPath.File.Create(path);
		/// <inheritdoc cref="System.IO.File.Create(string, int)"/>
		public static System.IO.FileStream Create(string path, int bufferSize) => Pri.LongPath.File.Create(path, bufferSize);
		/// <inheritdoc cref="System.IO.File.Create(string, int, System.IO.FileOptions)"/>
		public static System.IO.FileStream Create(string path, int bufferSize, System.IO.FileOptions options) => Pri.LongPath.File.Create(path, bufferSize, options);
		/// <inheritdoc cref="System.IO.File.Create(string, int, System.IO.FileOptions, System.Security.AccessControl.FileSecurity)"/>
		public static System.IO.FileStream Create(string path, int bufferSize, System.IO.FileOptions options, System.Security.AccessControl.FileSecurity fileSecurity) => Pri.LongPath.File.Create(path, bufferSize, options, fileSecurity);
		/// <inheritdoc cref="System.IO.File.Delete(string)"/>
		public static void Delete(string path) => Pri.LongPath.File.Delete(path);
		/// <inheritdoc cref="System.IO.File.Decrypt(string)"/>
		public static void Decrypt(string path) => Pri.LongPath.File.Decrypt(path);
		/// <inheritdoc cref="System.IO.File.Encrypt(string)"/>
		public static void Encrypt(string path) => Pri.LongPath.File.Encrypt(path);
		/// <inheritdoc cref="System.IO.File.Exists(string)"/>
		public static bool Exists(string path) => Pri.LongPath.File.Exists(path);
		/// <inheritdoc cref="System.IO.File.Open(string, System.IO.FileMode)"/>
		public static System.IO.FileStream Open(string path, System.IO.FileMode mode) => Pri.LongPath.File.Open(path, mode);
		/// <inheritdoc cref="System.IO.File.Open(string, System.IO.FileMode, System.IO.FileAccess)"/>
		public static System.IO.FileStream Open(string path, System.IO.FileMode mode, System.IO.FileAccess access) => Pri.LongPath.File.Open(path, mode, access);
		/// <inheritdoc cref="System.IO.File.Open(string, System.IO.FileMode, System.IO.FileAccess, System.IO.FileShare)"/>
		public static System.IO.FileStream Open(string path, System.IO.FileMode mode, System.IO.FileAccess access, System.IO.FileShare share) => Pri.LongPath.File.Open(path, mode, access, share);
		/// <inheritdoc cref="System.IO.File.SetCreationTime(string, System.DateTime)"/>
		public static void SetCreationTime(string path, System.DateTime creationTime) => Pri.LongPath.File.SetCreationTime(path, creationTime);
		/// <inheritdoc cref="System.IO.File.SetCreationTimeUtc(string, System.DateTime)"/>
		public static void SetCreationTimeUtc(string path, System.DateTime creationTimeUtc) => Pri.LongPath.File.SetCreationTimeUtc(path, creationTimeUtc);
		/// <inheritdoc cref="System.IO.File.GetCreationTime(string)"/>
		public static System.DateTime GetCreationTime(string path) => Pri.LongPath.File.GetCreationTime(path);
		/// <inheritdoc cref="System.IO.File.GetCreationTimeUtc(string)"/>
		public static System.DateTime GetCreationTimeUtc(string path) => Pri.LongPath.File.GetCreationTimeUtc(path);
		/// <inheritdoc cref="System.IO.File.SetLastWriteTime(string, System.DateTime)"/>
		public static void SetLastWriteTime(string path, System.DateTime lastWriteTime) => Pri.LongPath.File.SetLastWriteTime(path, lastWriteTime);
		/// <inheritdoc cref="System.IO.File.SetLastWriteTimeUtc(string, System.DateTime)"/>
		public static void SetLastWriteTimeUtc(string path, System.DateTime lastWriteTimeUtc) => Pri.LongPath.File.SetLastWriteTimeUtc(path, lastWriteTimeUtc);
		/// <inheritdoc cref="System.IO.File.GetLastWriteTime(string)"/>
		public static System.DateTime GetLastWriteTime(string path) => Pri.LongPath.File.GetLastWriteTime(path);
		/// <inheritdoc cref="System.IO.File.GetLastWriteTimeUtc(string)"/>
		public static System.DateTime GetLastWriteTimeUtc(string path) => Pri.LongPath.File.GetLastWriteTimeUtc(path);
		/// <inheritdoc cref="System.IO.File.SetLastAccessTime(string, System.DateTime)"/>
		public static void SetLastAccessTime(string path, System.DateTime lastAccessTime) => Pri.LongPath.File.SetLastAccessTime(path, lastAccessTime);
		/// <inheritdoc cref="System.IO.File.SetLastAccessTimeUtc(string, System.DateTime)"/>
		public static void SetLastAccessTimeUtc(string path, System.DateTime lastAccessTimeUtc) => Pri.LongPath.File.SetLastAccessTimeUtc(path, lastAccessTimeUtc);
		/// <inheritdoc cref="System.IO.File.GetLastAccessTime(string)"/>
		public static System.DateTime GetLastAccessTime(string path) => Pri.LongPath.File.GetLastAccessTime(path);
		/// <inheritdoc cref="System.IO.File.GetLastAccessTimeUtc(string)"/>
		public static System.DateTime GetLastAccessTimeUtc(string path) => Pri.LongPath.File.GetLastAccessTimeUtc(path);
		/// <inheritdoc cref="System.IO.File.GetAttributes(string)"/>
		public static System.IO.FileAttributes GetAttributes(string path) => Pri.LongPath.File.GetAttributes(path);
		/// <inheritdoc cref="System.IO.File.SetAttributes(string, System.IO.FileAttributes)"/>
		public static void SetAttributes(string path, System.IO.FileAttributes fileAttributes) => Pri.LongPath.File.SetAttributes(path, fileAttributes);
		/// <inheritdoc cref="System.IO.File.OpenRead(string)"/>
		public static System.IO.FileStream OpenRead(string path) => Pri.LongPath.File.OpenRead(path);
		/// <inheritdoc cref="System.IO.File.OpenWrite(string)"/>
		public static System.IO.FileStream OpenWrite(string path) => Pri.LongPath.File.OpenWrite(path);
		/// <inheritdoc cref="System.IO.File.ReadAllText(string)"/>
		public static string ReadAllText(string path) => Pri.LongPath.File.ReadAllText(path);
		/// <inheritdoc cref="System.IO.File.ReadAllText(string, System.Text.Encoding)"/>
		public static string ReadAllText(string path, Encoding encoding) => Pri.LongPath.File.ReadAllText(path, encoding);
		/// <inheritdoc cref="System.IO.File.WriteAllText(string, string)"/>
		public static void WriteAllText(string path, string contents) => Pri.LongPath.File.WriteAllText(path, contents);
		/// <inheritdoc cref="System.IO.File.WriteAllText(string, string, Encoding)"/>
		public static void WriteAllText(string path, string contents, Encoding encoding) => Pri.LongPath.File.WriteAllText(path, contents, encoding);
		/// <inheritdoc cref="System.IO.File.ReadAllBytes(string)"/>
		public static System.Byte[] ReadAllBytes(string path) => Pri.LongPath.File.ReadAllBytes(path);
		/// <inheritdoc cref="System.IO.File.WriteAllBytes(string, System.Byte[])"/>
		public static void WriteAllBytes(string path, System.Byte[] bytes) => Pri.LongPath.File.WriteAllBytes(path, bytes);
		/// <inheritdoc cref="System.IO.File.ReadAllLines(string)"/>
		public static System.String[] ReadAllLines(string path) => Pri.LongPath.File.ReadAllLines(path);
		/// <inheritdoc cref="System.IO.File.ReadAllLines(string, System.Text.Encoding)"/>
		public static System.String[] ReadAllLines(string path, System.Text.Encoding encoding) => Pri.LongPath.File.ReadAllLines(path, encoding);
		/// <inheritdoc cref="System.IO.File.ReadLines(string)"/>
		public static System.Collections.Generic.IEnumerable<string> ReadLines(string path) => Pri.LongPath.File.ReadLines(path);
		/// <inheritdoc cref="System.IO.File.ReadLines(string, System.Text.Encoding)"/>
		public static System.Collections.Generic.IEnumerable<string> ReadLines(string path, System.Text.Encoding encoding) => Pri.LongPath.File.ReadLines(path, encoding);
		/// <inheritdoc cref="System.IO.File.WriteAllLines(string, System.String[])"/>
		public static void WriteAllLines(string path, System.String[] contents) => Pri.LongPath.File.WriteAllLines(path, contents);
		/// <inheritdoc cref="System.IO.File.WriteAllLines(string, System.String[], System.Text.Encoding)"/>
		public static void WriteAllLines(string path, System.String[] contents, System.Text.Encoding encoding) => Pri.LongPath.File.WriteAllLines(path, contents, encoding);
		/// <inheritdoc cref="System.IO.File.WriteAllLines(string, System.Collections.Generic.IEnumerable{string})"/>
		public static void WriteAllLines(string path, System.Collections.Generic.IEnumerable<string> contents) => Pri.LongPath.File.WriteAllLines(path, contents);
		/// <inheritdoc cref="System.IO.File.WriteAllLines(string, System.Collections.Generic.IEnumerable{string}, System.Text.Encoding)"/>
		public static void WriteAllLines(string path, System.Collections.Generic.IEnumerable<string> contents, System.Text.Encoding encoding) => Pri.LongPath.File.WriteAllLines(path, contents, encoding);
		/// <inheritdoc cref="System.IO.File.AppendAllText(string, string)"/>
		public static void AppendAllText(string path, string contents) => Pri.LongPath.File.AppendAllText(path, contents);
		/// <inheritdoc cref="System.IO.File.AppendAllText(string, string, System.Text.Encoding)"/>
		public static void AppendAllText(string path, string contents, System.Text.Encoding encoding) => Pri.LongPath.File.AppendAllText(path, contents, encoding);
		/// <inheritdoc cref="System.IO.File.AppendAllLines(string, System.Collections.Generic.IEnumerable{string})"/>
		public static void AppendAllLines(string path, System.Collections.Generic.IEnumerable<string> contents) => Pri.LongPath.File.AppendAllLines(path, contents);
		/// <inheritdoc cref="System.IO.File.AppendAllLines(string, System.Collections.Generic.IEnumerable{string}, System.Text.Encoding)"/>
		public static void AppendAllLines(string path, System.Collections.Generic.IEnumerable<string> contents, System.Text.Encoding encoding) => Pri.LongPath.File.AppendAllLines(path, contents, encoding);
		/// <inheritdoc cref="System.IO.File.Move(string, string)"/>
		public static void Move(string sourcePath, string destinationPath) => Pri.LongPath.File.Move(sourcePath, destinationPath);
		/// <inheritdoc cref="System.IO.File.Replace(string, string, string)"/>
		public static void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName) => Pri.LongPath.File.Replace(sourceFileName, destinationFileName, destinationBackupFileName);
		/// <inheritdoc cref="System.IO.File.Replace(string, string, string, bool)"/>
		public static void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors) => Pri.LongPath.File.Replace(sourceFileName, destinationFileName, destinationBackupFileName, ignoreMetadataErrors);
		/// <inheritdoc cref="System.IO.File.SetAccessControl(string, System.Security.AccessControl.FileSecurity)"/>
		public static void SetAccessControl(string path, System.Security.AccessControl.FileSecurity fileSecurity) => Pri.LongPath.File.SetAccessControl(path, fileSecurity);
		/// <inheritdoc cref="System.IO.File.GetAccessControl(string)"/>
		public static System.Security.AccessControl.FileSecurity GetAccessControl(string path) => Pri.LongPath.File.GetAccessControl(path);
		/// <inheritdoc cref="System.IO.File.GetAccessControl(string, System.Security.AccessControl.AccessControlSections)"/>
		public static System.Security.AccessControl.FileSecurity GetAccessControl(string path, System.Security.AccessControl.AccessControlSections includeSections) => Pri.LongPath.File.GetAccessControl(path, includeSections);

	}

}