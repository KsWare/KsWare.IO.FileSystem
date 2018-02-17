namespace KsWare.IO.FileSystem {


	/// <summary>
	/// Type of reparse point.
	/// </summary>
	public enum ReparsePointType {

		/// <summary>
		/// None.
		/// </summary>
		None,

		/// <summary>
		/// Mount point/junction.
		/// </summary>
		MountPoint,

		/// <summary>
		/// Volume mount point.
		/// </summary>
		VolumeMountPoint,

		/// <summary>
		/// Symbolic link.
		/// </summary>
		SymbolicLink,

		/// <summary>
		/// Other.
		/// </summary>
		Other
	}

}
 