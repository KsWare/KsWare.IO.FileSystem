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
		private const string TestDrive = @"Y:\";

		[TestMethod]
		public void CreateTest() {
			Helper.DemandTestDriveAvailable(TestDrive);
			//Helper.DemandElevated();

			var testFolder = Helper.GetTestFolder(TestDrive, "WriteTests", this);
			_fixture =new Fixture(()=>Directory.Delete(testFolder,true));

			Directory.CreateDirectory(testFolder);
			var originalFile = Path.Combine(testFolder, "original.txt");
			File.WriteAllText(originalFile,"Original");

			var hardLink = Path.Combine(testFolder, "hardlink.txt");
			HardLink.Create(hardLink, originalFile);

			Assert.AreEqual("Original", File.ReadAllText(hardLink));
		}

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
			var testFolder = Helper.GetTestFolder(TestDrive, "WriteTests", testClass, testMethod);
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