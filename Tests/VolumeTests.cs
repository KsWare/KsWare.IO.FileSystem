using Microsoft.VisualStudio.TestTools.UnitTesting;
using KsWare.IO.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KsWare.IO.FileSystem.Tests {

	[TestClass()]
	public class VolumeTests {

		private Fixture _fixture;
		private const string TestDrive = @"Y:\";

		[TestMethod()]
		public void GetMountPointsTest() {
			Helper.DemandTestDriveAvailable(TestDrive);

			var mountpoint = TestDrive + @"ReadTests\Volume\";
			var volumeName = VolumeMountPoint.GetVolumeName(mountpoint);

			CollectionAssert.AreEqual(new[] {TestDrive, mountpoint}, Volume.GetMountPoints(volumeName));
			CollectionAssert.AreEqual(new []{TestDrive, mountpoint},Volume.GetMountPoints(mountpoint));
		}

		[TestMethod()]
		public void ExistsTest() {
			Helper.DemandTestDriveAvailable(TestDrive);

			var mountpoint = TestDrive + @"ReadTests\Volume\";
			var volumeName = VolumeMountPoint.GetVolumeName(mountpoint);

			Assert.IsTrue(Volume.Exists(volumeName));
			Assert.IsFalse(Volume.Exists(@"\\?\Volume{6213B9DD-5BC7-4605-BEBB-4362CB224194}"));
			Assert.IsFalse(Volume.Exists(mountpoint));
			Assert.IsFalse(Volume.Exists(TestDrive + "NotExist"));
		}
	}
}