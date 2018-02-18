using Microsoft.VisualStudio.TestTools.UnitTesting;
using KsWare.IO.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KsWare.IO.FileSystem.Tests {

	[TestClass]
	public class HardLinkTests {
		private Fixture _fixture;

		[TestMethod]
		public void CreateTest() {
			Helper.DemandTestDriveAvailable(TestDrive.Root);
			//Helper.DemandElevated();

			var testFolder = Helper.GetTestFolder(TestDrive.Root, "WriteTests", this);
			_fixture =new Fixture(()=>Directory.Delete(testFolder,true));

			Directory.CreateDirectory(testFolder);
			var originalFile = Path.Combine(testFolder, "original.txt");
			File.WriteAllText(originalFile,"Original");

			var hardLink = Path.Combine(testFolder, "hardlink.txt");
			var c0 = HardLink.GetNumberOfLinks(originalFile);
			HardLink.Create(hardLink, originalFile);
			var c1 = HardLink.GetNumberOfLinks(hardLink);

			Assert.AreEqual("Original", File.ReadAllText(hardLink));
			Assert.AreEqual(c0+1,c1);
		}

		[TestMethod]
		public void CreateLongTest() {
			Helper.DemandTestDriveAvailable(TestDrive.Root);
			//Helper.DemandElevated();

			var testFolder = TestDrive.WriteTests.LongPath.CreateTestFolder(this);
			_fixture = new Fixture(() => Directory.Delete(testFolder, true));

			var originalFile = Path.Combine(testFolder, "Original.txt");
			File.WriteAllText(originalFile, "Original");

			var hardLink = Path.Combine(testFolder, "HardLink.txt");
			HardLink.Create(hardLink, originalFile);

			Assert.AreEqual(TestDrive.ReadTests.OriginalTxt.Content, File.ReadAllText(hardLink));
		}

		[TestMethod]
		public void CreateLongTargetTest() {
			Helper.DemandTestDriveAvailable(TestDrive.Root);
			//Helper.DemandElevated();

			var testFolder = TestDrive.WriteTests.CreateTestFolder(this);
			_fixture = new Fixture(() => Directory.Delete(testFolder, true));

			var originalFile = TestDrive.ReadTests.LongPath.OriginalTxt.FullName;
			File.WriteAllText(originalFile, "Original");

			var hardLink = Path.Combine(testFolder, "HardLink.txt");
			HardLink.Create(hardLink, originalFile);

			Assert.AreEqual(TestDrive.ReadTests.LongPath.OriginalTxt.Content, File.ReadAllText(hardLink));
		}

		private void CreateUncheckedTest(int length,[CallerMemberName] string callerName=null) {
			Helper.DemandTestDriveAvailable(TestDrive.Root);
			//Helper.DemandElevated();

			var testFolder = TestDrive.WriteTests.CreateTestFolder(this, callerName);
			_fixture = new Fixture(() => Directory.Delete(testFolder, true));

			var longPathFile = Path.Combine(testFolder, new string('!', length - testFolder.Length -1 - 4))+".txt";
			Assert.AreEqual(length,longPathFile.Length);
			var originalFile = TestDrive.ReadTests.OriginalTxt.FullName;

			HardLink.Create(longPathFile, originalFile);
			Assert.AreEqual(TestDrive.ReadTests.OriginalTxt.Content, File.ReadAllText(longPathFile));
		}

		[TestMethod]
		public void CreateUnchecked259Test() => CreateUncheckedTest(259);

		[TestMethod]
		public void CreateUnchecked260Test() => CreateUncheckedTest(260);


		[TestMethod]
		public void DeleteTest() {
			Assert.Inconclusive("Not implemented.");
//			Helper.DemandTestDriveAvailable(TestDrive);
//			//Helper.DemandElevated();
//
//			var hardLink = PrepareHardLink(this);
//
//			HardLink.Delete(hardLink);
//
//			Assert.IsFalse(File.Exists(hardLink));
		}

		[TestMethod]
		public void ExistsTest() {
			Assert.Inconclusive("Not implemented.");
//			Helper.DemandTestDriveAvailable(TestDrive);
//			//Helper.DemandElevated();
//
//			var hardLink=PrepareHardLink(this);
//
//			Assert.IsTrue(SymbolicLink.Exists(hardLink));
		}

		private string PrepareHardLink(object testClass, [CallerMemberName] string testMethod = null) {
			var testFolder = Helper.GetTestFolder(TestDrive.Root, "WriteTests", testClass, testMethod);
			_fixture = new Fixture(() => Directory.Delete(testFolder, true));

			Directory.CreateDirectory(testFolder);
			var originalFile = Path.Combine(testFolder, "Original.txt");
			File.WriteAllText(originalFile, "Original");
			var hardLink = Path.Combine(testFolder, "HardLink.txt");
			HardLink.Create(hardLink, originalFile);
			Assert.IsTrue(File.Exists(hardLink));
			return hardLink;
		}

		[TestCleanup]
		public void Fixture() {
			_fixture?.Cleanup();
			_fixture = null;
		}
	}
}