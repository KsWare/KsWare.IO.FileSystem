using KsWare.IO.FileSystem.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KsWare.IO.FileSystem.Tests.Internal {

	[TestClass]
	public class PrivilegedExecutorTests {

		[TestMethod]
		public void Execute() {
			var method = typeof(PrivilegedExecutor).FullName+".TestConsole";
			
			Assert.AreEqual(0, PrivilegedExecutor.Execute(method));
			Assert.AreEqual(1, PrivilegedExecutor.Execute(method, "1"));
			Assert.AreEqual(2, PrivilegedExecutor.Execute(method, "1","2"));
		}

	}
}
