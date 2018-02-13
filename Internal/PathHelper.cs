namespace KsWare.IO.FileSystem.Internal {
	using static KsWare.IO.FileSystem.Path;

	public static class PathHelper {

		public static bool HasPrefix(string path) {
			if (path == null) return false;
			if (path.StartsWith(@"\\?\")) return true;
			if (path.StartsWith(@"\\?\UNC\")) return true;
			if (path.StartsWith(@"\\.\")) return true; // Win32 Device Namespaces
			if (path.StartsWith(@"\??\")) return true;
			return false;
		}

		public static string GetPrefix(string path) {
			if (path == null) return null;
//			if (path.StartsWith(@"\\?\Volume{")) return @"\\?\Volume; // prefix for volumes
			if (path.StartsWith(@"\\?\UNC\")) return @"\\?\UNC\";
			if (path.StartsWith(@"\\?\")) return @"\\?\";
			if (path.StartsWith(@"\\.\")) return @"\\.\"; // Win32 Device Namespaces
			if (path.StartsWith(@"\??\")) return @"\??\";
			return null;

		}

		public static string RemovePrefix(string path) {
			if (path == null) return null;
			if (path.StartsWith(@"\\?\Volume{")) return path; // keep prefix for volumes
			if (path.StartsWith(@"\\?\UNC\")) return @"\\" + path.Substring(8);
			if (path.StartsWith(@"\\?\")) return path.Substring(4);
			if (path.StartsWith(@"\\.\")) return path.Substring(4); // Win32 Device Namespaces
			if (path.StartsWith(@"\??\")) return path.Substring(4);
			return path;
		}

		public static string LongPathSupport(string path) {
			// TODO revise name and functionality
			// 
			if (HasPrefix(path)) return path;
			if (!IsAbsolute(path)) return path;
			return LongPath(path);
		}

		
		public static string LongPath(string path) {
			// TODO
			// if(!IsAbsolute(path)) checked by NormalizePath
			path = Path.NormalizePath(path); //removes .. and .
			if (path.StartsWith(@"\\")) path = @"\\?\UNC\" + path.Substring(2);
			else                        path = @"\\?\" + path;
			return path;
		}

	}
}
