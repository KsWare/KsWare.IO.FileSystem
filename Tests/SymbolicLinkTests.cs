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
	public class SymbolicLinkTests {

		private Fixture _fixture;
		private const string TestDrive = @"Y:\";

		[TestMethod]
		public void CreateTest() {
			Helper.DemandTestDriveAvailable(TestDrive);
			//Helper.DemandElevated();

			var symbolicLink = PrepareSymbolicLink();

			Assert.AreEqual("Original", File.ReadAllText(symbolicLink));
		}

		[TestMethod]
		public void DeleteTest() {
			Helper.DemandTestDriveAvailable(TestDrive);
			//Helper.DemandElevated();

			var symbolicLink = PrepareSymbolicLink();

			SymbolicLink.Delete(symbolicLink);

			Assert.IsFalse(File.Exists(symbolicLink));
		}

		[TestMethod]
		public void ExistsTest() {
			Assert.Inconclusive("Not implemented.");
//			Helper.DemandTestDriveAvailable(TestDrive);
//			//Helper.DemandElevated();
//
//			var symbolicLink = PrepareSymbolicLink();
//
//			Assert.IsTrue(SymbolicLink.Exists(symbolicLink));
		}

		[TestMethod]
		public void GetTarget() {
			Helper.DemandTestDriveAvailable(TestDrive);
			//Helper.DemandElevated();

			var symbolicLink = PrepareSymbolicLink();

			var testFolder = Helper.GetTestFolder(TestDrive, "WriteTests", this);
			var originalFile = Path.Combine(testFolder, "Original.txt");

			Assert.AreEqual(originalFile, SymbolicLink.GetTarget(symbolicLink));
		}

		private string PrepareSymbolicLink([CallerMemberName] string testMethod = null) {

			var testFolder = Helper.GetTestFolder(TestDrive, "WriteTests", this,testMethod);
			_fixture = new Fixture(() => Helper.RobustDeleteDirectory(testFolder,true));

			Directory.CreateDirectory(testFolder);
			var originalFile = Path.Combine(testFolder, "Original.txt");
			File.WriteAllText(originalFile, "Original");

			var symbolicLink = Path.Combine(testFolder, "SymbolicLink.txt");
			SymbolicLink.Create(symbolicLink, originalFile, true);

			Assert.IsTrue(File.Exists(symbolicLink), "File 'SymbolicLink.txt' should exist, but does not!");
			return symbolicLink;
		}

		[TestCleanup]
		public void Fixture() {
			_fixture?.Cleanup();
			_fixture = null;
		}
	}
}