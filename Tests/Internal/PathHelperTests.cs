using Microsoft.VisualStudio.TestTools.UnitTesting;
using KsWare.IO.FileSystem.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KsWare.IO.FileSystem.Tests;

namespace KsWare.IO.FileSystem.Internal.Tests {

	[TestClass()]
	public class PathHelperTests {

		[TestMethod()]
		public void HasPrefixTest() {
			Assert.Inconclusive();
		}

		[TestMethod()]
		public void GetPrefixTest() {
			Assert.Inconclusive();
		}

		[TestMethod()]
		public void RemovePrefixTest() {
			Assert.Inconclusive();
		}

		[TestMethod()]
		public void LongPathSupportTest() {
			Assert.AreEqual(@"C:\", PathHelper.LongPathSupport(@"C:\"));
			Assert.AreEqual(@"\\server\share\", PathHelper.LongPathSupport(@"\\server\share\"));

			// relative paths are unchanged, but limited to MAX_PATH
			Assert.AreEqual(@"path", PathHelper.LongPathSupport(@"path"));
			Assert.AreEqual(@"..", PathHelper.LongPathSupport(@".."));

			Assert.AreEqual(@"\\?\"+ TestDrive.ReadTests.LongPath.FullName, PathHelper.LongPathSupport(TestDrive.ReadTests.LongPath.FullName));

			Assert.ThrowsException<PathTooLongException>(() => PathHelper.LongPathSupport(@"..\"+new string('!',255)+"\\!"));
		}

		[TestMethod()]
		public void LongPathTest() {
			Assert.AreEqual(@"\\?\C:\", PathHelper.LongPath(@"C:\"));
			Assert.AreEqual(@"\\?\UNC\server\share\", PathHelper.LongPath(@"\\server\share\"));

			Assert.AreEqual(@"\\?\C:\", PathHelper.LongPath(@"C:\path\.."));

			//relative
			Assert.ThrowsException<ArgumentException>(() => PathHelper.LongPath(@"path"));
			Assert.ThrowsException<ArgumentException>(() => PathHelper.LongPath(@"path\path"));
			Assert.ThrowsException<ArgumentException>(() => PathHelper.LongPath(@".."));
			Assert.ThrowsException<ArgumentException>(() => PathHelper.LongPath(@"..\path"));
			Assert.ThrowsException<ArgumentException>(() => PathHelper.LongPath(@"."));
			Assert.ThrowsException<ArgumentException>(() => PathHelper.LongPath(@".\path"));
			Assert.ThrowsException<ArgumentException>(() => PathHelper.LongPath(@"C:path"));
			Assert.ThrowsException<ArgumentException>(() => PathHelper.LongPath(@"C:"));

			Assert.ThrowsException<ArgumentNullException>(() => PathHelper.LongPath(null));
		}

		[TestMethod()]
		public void StartsWithVolumeNameTest() {
			Assert.IsTrue(PathHelper.StartsWithVolumeName(@"\\?\Volume{7B4BE779-27AF-48C6-91CC-DA21C9E78FBC}\path"));
			Assert.IsTrue(PathHelper.StartsWithVolumeName(@"\\?\Volume{7B4BE779-27AF-48C6-91CC-DA21C9E78FBC}\"));
			Assert.IsTrue(PathHelper.StartsWithVolumeName(@"\??\Volume{7B4BE779-27AF-48C6-91CC-DA21C9E78FBC}\"));

			Assert.IsFalse(PathHelper.StartsWithVolumeName(@"\\?\Volume{"));
			Assert.IsFalse(PathHelper.StartsWithVolumeName(@"Volume{"));
			Assert.IsFalse(PathHelper.StartsWithVolumeName(string.Empty));
			Assert.IsFalse(PathHelper.StartsWithVolumeName(null));
		}

		[TestMethod()]
		public void IsValidVolumeNameTest() {
			Assert.IsTrue(PathHelper.IsValidVolumeName(@"\\?\Volume{7B4BE779-27AF-48C6-91CC-DA21C9E78FBC}\"));
			Assert.IsTrue(PathHelper.IsValidVolumeName(@"\??\Volume{7B4BE779-27AF-48C6-91CC-DA21C9E78FBC}\"));

			Assert.IsFalse(PathHelper.IsValidVolumeName(@"\\?\Volume{7B4BE779-27AF-48C6-91CC-DA21C9E78FBC}"));
			Assert.IsFalse(PathHelper.IsValidVolumeName(@"\\?\Volume{"));
			Assert.IsFalse(PathHelper.IsValidVolumeName(@"\??\Volume{"));

			Assert.IsFalse(PathHelper.IsValidVolumeName(@"Volume{"));
			Assert.IsFalse(PathHelper.IsValidVolumeName(string.Empty));
			Assert.IsFalse(PathHelper.IsValidVolumeName(null));
		}
	}
}