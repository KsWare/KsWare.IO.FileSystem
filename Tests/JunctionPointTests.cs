using Microsoft.VisualStudio.TestTools.UnitTesting;
using KsWare.IO.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KsWare.IO.FileSystem.Tests {

	[TestClass]
	public class JunctionPointTests {
		private Fixture _fixture;
		private const string TestDrive = @"Y:\";

		[TestMethod]
		public void CreateTest() {
			Helper.DemandTestDriveAvailable(TestDrive);
			//Helper.DemandElevated();

			var testFolder = Helper.GetTestFolder(TestDrive, "WriteTests", this);
			_fixture = new Fixture(() => Directory.Delete(testFolder, true));

			Directory.CreateDirectory(testFolder);
			var originalFolder = Path.Combine(testFolder, "original");
			Directory.CreateDirectory(originalFolder);
			var originalFile = Path.Combine(originalFolder, "original.txt");
			File.WriteAllText(originalFile, "Original");

			var junction = Path.Combine(testFolder, "junction");
			JunctionPoint.Create(junction, originalFolder, true);

			Assert.AreEqual("Original", File.ReadAllText(Path.Combine(junction, "original.txt")));
		}

		[TestMethod]
		public void DeleteTest() {
			Helper.DemandTestDriveAvailable(TestDrive);
			//Helper.DemandElevated();

			var testFolder = Helper.GetTestFolder(TestDrive, "WriteTests", this);
			_fixture = new Fixture(() => Directory.Delete(testFolder, true));

			Directory.CreateDirectory(testFolder);
			var originalFolder = Path.Combine(testFolder, "original");
			Directory.CreateDirectory(originalFolder);
			var junction = Path.Combine(testFolder, "junction");
			JunctionPoint.Create(junction, originalFolder,true);
			Assert.IsTrue(Directory.Exists(junction));

			JunctionPoint.Delete(junction);

			Assert.IsFalse(Directory.Exists(junction));
		}

		[TestMethod]
		public void ExistsTest() {
			Helper.DemandTestDriveAvailable(TestDrive);
			//Helper.DemandElevated();

			var testFolder = Helper.GetTestFolder(TestDrive, "WriteTests", this);
			_fixture = new Fixture(() => Directory.Delete(testFolder, true));

			Directory.CreateDirectory(testFolder);
			var originalFolder = Path.Combine(testFolder, "original");
			Directory.CreateDirectory(originalFolder);
			var junction = Path.Combine(testFolder, "junction");
			JunctionPoint.Create(junction, originalFolder,true);
			Assert.IsTrue(Directory.Exists(junction));

			Assert.IsTrue(JunctionPoint.Exists(junction));
		}

		[TestMethod]
		public void GetTarget() {
			Helper.DemandTestDriveAvailable(TestDrive);
			//Helper.DemandElevated();

			var testFolder = Helper.GetTestFolder(TestDrive, "WriteTests", this);
			_fixture = new Fixture(() => Directory.Delete(testFolder, true));

			Directory.CreateDirectory(testFolder);
			var originalFolder = Path.Combine(testFolder, "original");
			Directory.CreateDirectory(originalFolder);
			var junction = Path.Combine(testFolder, "junction");
			JunctionPoint.Create(junction, originalFolder, true);
			Assert.IsTrue(Directory.Exists(junction));

			Assert.AreEqual(originalFolder, JunctionPoint.GetTarget(junction));
		}

		[TestCleanup]
		public void Fixture() {
			_fixture?.Cleanup();
			_fixture = null;
		}
	}
}