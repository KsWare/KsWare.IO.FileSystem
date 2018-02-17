using Microsoft.VisualStudio.TestTools.UnitTesting;
using static KsWare.IO.FileSystem.Tests.SignatureHelper;
namespace KsWare.IO.FileSystem.Tests {

	[TestClass]
	public class FileTests {

		[TestMethod]
		public void MemberTest() {
			Assert.AreEqual(true, Helper.HasAllSignatures(typeof(KsWare.IO.FileSystem.File), typeof(Pri.LongPath.File), "System.IO.File."));
			Assert.AreEqual(true, Helper.HasAllSignatures(typeof(KsWare.IO.FileSystem.File), typeof(System.IO.File), "System.IO.File."));
		}
	}

}