using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static KsWare.IO.FileSystem.Tests.SignatureHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KsWare.IO.FileSystem.Tests {

	internal static class Helper {

		public static bool HasAllSignatures(Type type, Type comparedType, string suffix = null) {
			var m0 = GetMethods(comparedType, BindingFlags.Static | BindingFlags.Public);
			var m1 = GetMethods(type,         BindingFlags.Static | BindingFlags.Public);

			//m1.Clear();

			var c = 0;
			foreach (var k in m0.Keys) {
				if (m1.ContainsKey(k)) continue;
				Debug.WriteLine("/// <inheritdoc cref=\"" + suffix + InheriteDoc.Sig(m0[k]) + "\"/>");
				Debug.WriteLine(ForCode.Sig(m0[k]) + " => "                                 + WrapperCall(m0[k]));
				c++;
			}
			return c == 0;
		}

		private static Dictionary<string, MethodInfo> GetMethods(Type type, BindingFlags flags) {
			var methods = type.GetMethods(flags);
//			var dic = methods.ToDictionary(ForCompareIgnoreReturnType.Sig, m => m);
			var dic = new Dictionary<string, MethodInfo>();
			foreach (var methodInfo in methods) {
				var sig = ForCompareIgnoreReturnType.Sig(methodInfo);
				dic.Add(sig, methodInfo);
			}
			return dic;
		}

		private static string WrapperCall(MethodInfo methodInfo) {
			var sb = new StringBuilder();

			if (methodInfo.IsStatic) {
				sb.Append(methodInfo.DeclaringType.FullName + "." + methodInfo.Name);
			}
			else {
				sb.Append("_wrapper." + methodInfo.Name);
			}

			sb.Append("(");
			sb.Append(WrapperCall(methodInfo.GetParameters()));
			sb.Append(");");
			return sb.ToString();
		}

		private static string WrapperCall(ParameterInfo[] parameters) {
			if (parameters.Length == 0) return string.Empty;
			var sb = new StringBuilder();

			foreach (var pi in parameters) {
				sb.Append(", " + pi.Name);
			}

			return sb.ToString(2, sb.Length - 2);
		}

		public static void DemandTestDriveAvailable(string testDrive) {
			if (!Directory.Exists(testDrive + "WriteTests"))
				Assert.Inconclusive($"Test volume not available. Mount 'KsWare.IO.FileSystem.Tests.vhdx' to {testDrive}.");

			Assert.AreEqual(true, Directory.Exists(testDrive));
			Assert.AreEqual(true, Directory.Exists(testDrive + @"ReadTests\Original"));
			Assert.AreEqual(true, Directory.Exists(testDrive + @"ReadTests\Volume"));
			Assert.AreEqual(true, Directory.Exists(testDrive + @"ReadTests\SymbolicLink"));
			Assert.AreEqual(true, Directory.Exists(testDrive + @"ReadTests\Junction"));
			Assert.AreEqual(true, File.Exists(testDrive      + @"ReadTests\Original.txt"));
			Assert.AreEqual(true, File.Exists(testDrive      + @"ReadTests\SymbolicLink.txt"));
		}

		public static void DemandElevated() {
			if (!KsWare.IO.FileSystem.Internal.Helper.IsElevated)
				Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Inconclusive(
					$"Test not possible. Administrator rights required.");
		}

		public static string GetTestFolder(string root,
			string readwrite,
			object testClass,
			[CallerMemberName] string testMethod = null) {
			return Path.Combine(root, readwrite, testClass.GetType().FullName + "." + testMethod);
		}

		public static void RobustDeleteDirectory(string path, bool recursive) {
			if (!recursive) {
				Directory.Delete(path);
				return;
			}
			var ex = DeleteDirectory(path);
			if (ex != null) throw new IOException("Delete directory failed.", ex);
		}

		private static Exception DeleteDirectory(string path) {
			if ((File.GetAttributes(path) & FileAttributes.ReparsePoint) != 0) {
				Directory.Delete(path);
				return null;
			}
			foreach (string directory in Directory.GetDirectories(path)) {
				var ex=DeleteDirectory(directory);
				if (ex != null) return ex;
			}
			foreach (string file in Directory.GetFiles(path)) {
				try {
					System.IO.File.Delete(file); // first call does not throw exception!!?
					System.IO.File.Delete(file); // UnauthorizedAccessException
				}
				catch (Exception ex) {
					return ex;
				}
			}

			try {
				Directory.Delete(path, true);
				return null;
			}
			catch (Exception ex) {
				return ex;
			}
		}
	}

}
