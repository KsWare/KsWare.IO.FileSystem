using System;

namespace KsWare.IO.FileSystem.Internal {

	/// <summary>
	/// Provides data of a reparse point.
	/// </summary>
	public class ReparseData {

		public ReparsePointType Type;

		/// <summary>
		///     Reparse point tag. 
		/// </summary>
		public ReparsePoint.IO_REPARSE_TAG? ReparseTag;

		/// <summary>
		///     The substitute name.
		/// </summary>
		public string SubstituteName;

		/// <summary>
		/// The unchanged substitute name.
		/// </summary>
		public string SubstituteNameOriginal;

		/// <summary>
		///     The print name.
		/// </summary>
		public string PrintName;

		/// <summary>
		///     The unchanged print name.
		/// </summary>
		public string PrintNameOriginal;

		//			/// <summary>
		//			/// Flags (Symbolic Link only)
		//			/// </summary>
		//			/// <remarks>A 32-bit bit field that specifies whether the substitute is an absolute target path name or a path name relative to the directory containing the symbolic link.
		//			/// 0x00000000 The substitute name is an absolute target path name.
		//			/// SYMLINK_FLAG_RELATIVE 0x00000001 When this Flags value is set, the substitute name is a path name relative to the directory containing the symbolic link.
		//			/// </remarks>
		//			public int Flags;

		public ReparsePoint.SYMLINK_FLAG? Flags;

		public Guid? ReparseGuid;

		public byte[] Generic;
	}

}
 