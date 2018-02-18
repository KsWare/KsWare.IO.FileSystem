using Microsoft.VisualStudio.TestTools.UnitTesting;
using static KsWare.IO.FileSystem.Tests.SignatureHelper;

namespace KsWare.IO.FileSystem.Tests {

	[TestClass]
	public class DirectoryTests {
		private Fixture _fixture;

		[TestMethod]
		public void DotTest() {
			Helper.DemandTestDriveAvailable(TestDrive.Root);
			//Helper.DemandElevated();

			var testFolder = TestDrive.WriteTests.CreateTestFolder(this);
			_fixture = new Fixture(() => Helper.RobustDeleteDirectory(testFolder, true));

			var t2 = testFolder + "\\subfolder\\..\\subfolder2";
			//Directory.CreateDirectory(t2);
			System.IO.Directory.CreateDirectory(t2);
			Assert.IsTrue(Directory.Exists(Path.GetFullPath(t2)));
		}

		[TestMethod]
		public void MemberTest() {
			Assert.AreEqual(true, Helper.HasAllSignatures(typeof(KsWare.IO.FileSystem.Directory), typeof(Pri.LongPath.Directory), "System.IO.Directory."));
			Assert.AreEqual(true, Helper.HasAllSignatures(typeof(KsWare.IO.FileSystem.Directory), typeof(System.IO.Directory), "System.IO.Directory"));
		}
	}

}