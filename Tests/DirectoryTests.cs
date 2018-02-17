using Microsoft.VisualStudio.TestTools.UnitTesting;
using static KsWare.IO.FileSystem.Tests.SignatureHelper;

namespace KsWare.IO.FileSystem.Tests {

	[TestClass]
	public class DirectoryTests {

		[TestMethod]
		public void MemberTest() {
			Assert.AreEqual(true, Helper.HasAllSignatures(typeof(KsWare.IO.FileSystem.Directory), typeof(Pri.LongPath.Directory), "System.IO.Directory."));
			Assert.AreEqual(true, Helper.HasAllSignatures(typeof(KsWare.IO.FileSystem.Directory), typeof(System.IO.Directory), "System.IO.Directory"));
		}
	}

}