using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace KsWare.IO.FileSystem.Internal {

	internal static class Helper {

		private static void dummy() {
			var p = nameof(System.IO.Path);      // 'Find Usages' should return only this line
			var d = nameof(System.IO.Directory); // 'Find Usages' should return only this line
			var f = nameof(System.IO.File);		 // 'Find Usages' should return only this line
		}

		public static bool IsElevated {
			get {
				bool isElevated;
				using (var identity = WindowsIdentity.GetCurrent())
				{
					var principal = new WindowsPrincipal(identity);
					isElevated = principal.IsInRole(WindowsBuiltInRole.Administrator);
				}
				return isElevated;
			}
		}

		public static string RemoveQuotes(string s) {
			if (s.StartsWith("\"") && s.EndsWith("\"")) {
				s = s.Substring(1, s.Length - 2);// remove quotes
				s = s.Replace("\"\"", "\"");
			}
			return s;
		}

		public static Exception Win32Exception() {
			var err = Marshal.GetLastWin32Error();
			return new Win32Exception(err);
		}

		public static Exception Win32Exception(int error) {
			return new Win32Exception(error);
		}

		public static Exception Win32Exception(int error, string method, string file) {
			var message = $"{new Win32Exception(error).Message} on {method} {file}";
			return new Win32Exception(error, message);
		}

		public static Exception IOExceptionForLastWin32Error(string message) {
			return new System.IO.IOException(message, Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error()));
		}

		public static Exception ExceptionForLastWin32Error() {
			var ex = Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());
			return ex;
		}

		public static Exception IOException(string message) {
			return new System.IO.IOException(message);
		}

		public static Exception IOException(string message, Exception innerException) { return new System.IO.IOException(message, innerException); }
	}
}
