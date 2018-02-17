using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using KsWare.IO.FileSystem.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static KsWare.IO.FileSystem.Path;


namespace KsWare.IO.FileSystem.Tests {
	
	[TestClass]
	public class PathTests {

		[TestMethod]
		public void IsPrefixedTest() {
			Assert.AreEqual(true, PathHelper.HasPrefix(@"\\?\Volume{00000000-0000-0000-0000-000000000000}\"));
			Assert.AreEqual(true, PathHelper.HasPrefix(@"\\?\UNC\A\B\C"));
			Assert.AreEqual(true, PathHelper.HasPrefix(@"\\.\COM56"));
			Assert.AreEqual(true, PathHelper.HasPrefix(@"\??\any"));
		}

		[TestMethod]
		public void GetPrefixedTest() {
			Assert.AreEqual(@"\\?\", PathHelper.GetPrefix(@"\\?\Volume{00000000-0000-0000-0000-000000000000}\"));
			Assert.AreEqual(@"\\?\UNC\", PathHelper.GetPrefix(@"\\?\UNC\A\B\C"));
			Assert.AreEqual(@"\\.\", PathHelper.GetPrefix(@"\\.\COM56"));
			Assert.AreEqual(@"\??\", PathHelper.GetPrefix(@"\??\any"));
			Assert.AreEqual(null, PathHelper.GetPrefix(@"C:\"));
		}

		[TestMethod]
		public void IsVolumeTest() {
			Assert.AreEqual(true, IsVolume(@"\\?\Volume{00000000-0000-0000-0000-000000000000}\"));
			Assert.AreEqual(true, IsVolume(@"\\?\Volume{00000000-0000-0000-0000-000000000000}\path"));
			Assert.AreEqual(false, IsVolume(@"C:\"));
			Assert.AreEqual(false, IsVolume(@"C:\path"));
		}

		[TestMethod]
		public void IsUncTest() {
			Assert.AreEqual(false, IsUnc(@"C:\"));
			Assert.AreEqual(false, IsUnc(@"C:\D"));
			Assert.AreEqual(false, IsUnc(@"..\B"));
			Assert.AreEqual(false, IsUnc(@"C:rel"));
			Assert.AreEqual(true, IsUnc(@"\\SERVER\PATH\B"));
			Assert.AreEqual(true, IsUnc(@"\\SERVER\PATH\"));
			//			Assert.AreEqual(true, IsUnc(@"\\SERVER\PATH"));
			Assert.AreEqual(true, IsUnc(@"\\?\UNC\SERVER\PATH\B"));
			Assert.AreEqual(true, IsUnc(@"\\?\UNC\SERVER\PATH\"));
			//			Assert.AreEqual(true, IsUnc(@"\\?\UNC\SERVER\PATH"));
		}

		[TestMethod]
		public void RemovePrefixTest() {
			Assert.AreEqual(@"C:\", PathHelper.RemovePrefix(@"C:\"));
			Assert.AreEqual(@"C:\", PathHelper.RemovePrefix(@"\\?\C:\"));
			Assert.AreEqual(@"\\SERVER\SHARE", PathHelper.RemovePrefix(@"\\?\UNC\SERVER\SHARE"));
		}

		[TestMethod]
		public void IsAbsoluteTest() {
			Assert.ThrowsException<ArgumentNullException>(() => IsAbsolute(null));

			Assert.AreEqual(true, IsAbsolute(@"A:\"));
			Assert.AreEqual(true, IsAbsolute(@"A:\B"));
			Assert.AreEqual(true, IsAbsolute(@"A:\B\..\C"));
			Assert.AreEqual(true, IsAbsolute(@"A:\B\.\C"));
			Assert.AreEqual(true, IsAbsolute(@"\\SERVER\SHARE\B"));

			Assert.AreEqual(false, IsAbsolute(@"A:B"));
			Assert.AreEqual(false, IsAbsolute(@"A"));
			Assert.AreEqual(false, IsAbsolute(@"A\B"));
			Assert.AreEqual(false, IsAbsolute(@".."));
			Assert.AreEqual(false, IsAbsolute(@"..\B"));
			Assert.AreEqual(false, IsAbsolute(@"."));
			Assert.AreEqual(false, IsAbsolute(@"\"));
			Assert.AreEqual(false, IsAbsolute(@"A:"));
			Assert.AreEqual(false, IsAbsolute(@""));
		}

		[TestMethod]
		public void NormalizePathTest() {
			/*
			C:\A\..\B  => C:\B
			C:\A\B\..\..\C  => C:\
			C:\A\..  => C:\
			C:\A\.  => C:\A
			C:\A\.\B => C:\A\B
			\\SERVER\SHARE\			
			*/
			Assert.AreEqual(@"C:\B", NormalizePath(@"C:\A\..\B"));
			Assert.AreEqual(@"C:\C", NormalizePath(@"C:\A\B\..\..\C"));
			Assert.AreEqual(@"C:\", NormalizePath(@"C:\A\.."));
			Assert.AreEqual(@"C:\A", NormalizePath(@"C:\A\."));
			Assert.AreEqual(@"C:\A\B", NormalizePath(@"C:\A\.\B"));

			Assert.AreEqual(@"\\SERVER\SHARE\B", NormalizePath(@"\\SERVER\SHARE\A\..\B"));

			Assert.AreEqual(@"C:\A\B", NormalizePath(@"C:\A\.\B"));
			Assert.AreEqual(@"C:\A\B", NormalizePath(@"C:\A\B\."));
		}

		[TestMethod]
		public void MakeAbsoluteTest() {
			Assert.AreEqual(@"A:\B", MakeAbsolute("B", @"A:\"));
			Assert.AreEqual(@"A:\B\C", MakeAbsolute("C", @"A:\B"));
			Assert.AreEqual(@"A:\C", MakeAbsolute(@"..\C", @"A:\B"));
			Assert.AreEqual(@"A:\C", MakeAbsolute(@"..\..\C", @"A:\B\C"));

			Assert.AreEqual(@"A:\B", MakeAbsolute(@"\B", @"A:\"));
			Assert.AreEqual(@"A:\C", MakeAbsolute(@"\C", @"A:\B"));
		}

		[TestMethod]
		public void SplitPathTest() {
			var r = SplitPath(@"A:\B");
			Assert.AreEqual(2, r.Count);
			Assert.AreEqual("A:", r[0]);
			Assert.AreEqual("B", r[1]);

			r = SplitPath(@"\\A\B\C");
			Assert.AreEqual(2, r.Count);
			Assert.AreEqual(@"\\A\B", r[0]);
			Assert.AreEqual("C", r[1]);

			// trailing backslash
			r = SplitPath(@"A:\B\");
			Assert.AreEqual(2, r.Count);
			Assert.AreEqual("A:", r[0]);
			Assert.AreEqual("B", r[1]);

			// root only with trailing backslash
			r = SplitPath(@"A:\");
			Assert.AreEqual(1, r.Count);
			Assert.AreEqual("A:", r[0]);

			r = SplitPath(@"\\A\B\");
			Assert.AreEqual(1, r.Count);
			Assert.AreEqual(@"\\A\B", r[0]);

			// root only without trailing backslash
			r = SplitPath(@"A:");
			Assert.AreEqual(1, r.Count);
			Assert.AreEqual("A:", r[0]);

			r = SplitPath(@"\\A\B");
			Assert.AreEqual(1, r.Count);
			Assert.AreEqual(@"\\A\B", r[0]);

			//relative path
			r = SplitPath(@"..\A");
			Assert.AreEqual(2, r.Count);
			Assert.AreEqual("..", r[0]);
			Assert.AreEqual("A", r[1]);

			//relative path with root
			r = SplitPath(@"A:B");
			Assert.AreEqual(2, r.Count);
			Assert.AreEqual("A:", r[0]);
			Assert.AreEqual("B", r[1]);
		}

		[TestMethod()]
		public void AddTrailingBackslashTest() {
			Assert.ThrowsException<ArgumentNullException>(() => AddTrailingBackslash(null));

			Assert.AreEqual(@"A:\B\", AddTrailingBackslash(@"A:\B"));
			Assert.AreEqual(@"A:\B\", AddTrailingBackslash(@"A:\B\"));

			Assert.AreEqual(@"\", AddTrailingBackslash(@""));
		}

		[TestMethod()]
		public void RemoveTrailingBackslashTest() {
			Assert.AreEqual(@"A:\B", RemoveTrailingBackslash(@"A:\B\"));
			Assert.AreEqual(@"A:\B", RemoveTrailingBackslash(@"A:\B"));
			Assert.AreEqual(@"A:", RemoveTrailingBackslash(@"A:\"));
			Assert.AreEqual(@"", RemoveTrailingBackslash(@""));
			Assert.AreEqual(null, RemoveTrailingBackslash(null));
		}

		[TestMethod()]
		public void MakeRelativePathTest() {
			Assert.ThrowsException<ArgumentNullException>(() => MakeRelativePath(null, @"A:\"));
			Assert.ThrowsException<ArgumentNullException>(() => MakeRelativePath(@"A:\",null));
			Assert.ThrowsException<ArgumentException>(() => MakeRelativePath(@"..\", @"A:\"));
			Assert.ThrowsException<ArgumentException>(() => MakeRelativePath(@"A:\", @"..\"));

			Assert.AreEqual(@"B", MakeRelativePath(@"A:\B\",@"A:\"));
			Assert.AreEqual(@"B", MakeRelativePath(@"A:\B", @"A:\"));
			Assert.AreEqual(@"..", MakeRelativePath(@"A:\", @"A:\B"));
			Assert.AreEqual(@"..\C", MakeRelativePath(@"A:\C", @"A:\B"));

			Assert.ThrowsException<InvalidOperationException>(() => MakeRelativePath(@"B:\", @"A:\"));
			Assert.ThrowsException<InvalidOperationException>(() => MakeRelativePath(@"\\A\B\", @"A:\"));
		}

		[TestMethod()]
		public void ChangeFileNameTest() {
			Assert.AreEqual(@"A:\D.E", ChangeFileName(@"A:\B.C", @"D.E"));
		}

		[TestMethod()]
		public void ChangeFileNameWithoutExtensionTest() {
			Assert.AreEqual(@"A:\D.C", ChangeFileNameWithoutExtension(@"A:\B.C", @"D"));
		}

		[TestMethod]
		public void MemberTest() {
			Assert.AreEqual(true, Helper.HasAllSignatures(typeof(KsWare.IO.FileSystem.Path), typeof(Pri.LongPath.Path), "System.IO.Path."));
			Assert.AreEqual(true, Helper.HasAllSignatures(typeof(KsWare.IO.FileSystem.Path), typeof(System.IO.Path), "System.IO.Path."));
		}


	}

}