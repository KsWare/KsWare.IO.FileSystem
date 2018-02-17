using Microsoft.VisualStudio.TestTools.UnitTesting;
using KsWare.IO.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KsWare.IO.FileSystem.Tests {
	[TestClass()]
	public class ReparsePointTests {

		private Fixture _fixture;
		private const string TestDrive = @"Y:\";

//		[TestMethod()]
//		public void CreateMointpointTest() {
//			Assert.Fail();
//		}
//
//		[TestMethod()]
//		public void CreateSymbolicLinkTest() {
//			Assert.Fail();
//		}

		[TestMethod()]
		public void ExistsTest() {
			Helper.DemandTestDriveAvailable(TestDrive);

			Assert.IsTrue(ReparsePoint.Exists(@"Y:\ReadTests\Junction"));
			Assert.IsTrue(ReparsePoint.Exists(@"Y:\ReadTests\SymbolicLink"));
			Assert.IsTrue(ReparsePoint.Exists(@"Y:\ReadTests\Volume"));
			Assert.IsTrue(ReparsePoint.Exists(@"Y:\ReadTests\SymbolicLink.txt"));

			Assert.IsFalse(ReparsePoint.Exists(@"Y:\ReadTests\Hardlink.txt"));
			Assert.IsFalse(ReparsePoint.Exists(@"Y:\ReadTests\Original"));
			Assert.IsFalse(ReparsePoint.Exists(@"Y:\ReadTests\Original.txt"));

			Assert.IsFalse(ReparsePoint.Exists(@"Y:\ReadTests\NotExists"));
		}

		[TestMethod()]
		public void TargetExistsTest() {
			Helper.DemandTestDriveAvailable(TestDrive);

			Assert.IsTrue(ReparsePoint.TargetExists(@"Y:\ReadTests\Junction"));
			Assert.IsTrue(ReparsePoint.TargetExists(@"Y:\ReadTests\SymbolicLink"));
			Assert.IsTrue(ReparsePoint.TargetExists(@"Y:\ReadTests\Volume"));
			Assert.IsTrue(ReparsePoint.TargetExists(@"Y:\ReadTests\SymbolicLink.txt"));

			Assert.IsFalse(ReparsePoint.TargetExists(@"Y:\ReadTests\Hardlink.txt"));
			Assert.IsFalse(ReparsePoint.TargetExists(@"Y:\ReadTests\Original"));
			Assert.IsFalse(ReparsePoint.TargetExists(@"Y:\ReadTests\Original.txt"));

			Assert.IsFalse(ReparsePoint.TargetExists(@"Y:\ReadTests\NotExists"));
		}
	}
}