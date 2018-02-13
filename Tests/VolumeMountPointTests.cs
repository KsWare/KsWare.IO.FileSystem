using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KsWare.IO.FileSystem.Tests {

	[TestClass]
	public class VolumeMountPointTests {

		private const string TestDrive = @"Y:\";

		[TestMethod]
		public void CompleteTest() {
			Helper.DemandTestDriveAvailable(TestDrive);
			//Helper.DemandElevated();

			var testFolder= Path.Combine(TestDrive,"WriteTests", typeof(VolumeMountPointTests).FullName);
			var mountpoint = Path.Combine(testFolder, "Volume");

			var vn= VolumeMountPoint.GetVolumeName(TestDrive);

			VolumeMountPoint.Create(mountpoint, vn);

			var vn1 = VolumeMountPoint.GetVolumeName(mountpoint);
			Assert.AreEqual(vn,vn1);

			VolumeMountPoint.Delete(mountpoint);
			Assert.AreEqual(false, Directory.Exists(mountpoint));
		}

//		[TestMethod]
//		public void CreateTest() {
//			VolumeMountPoint.Create(@"V:\Tests\D", @"\\?\Volume{4f630c88-0000-0000-0000-f01500000000}\");
//		}

		[TestMethod]
		public void GetVolumeNameTest() {
			Helper.DemandTestDriveAvailable(TestDrive);

			VolumeMountPoint.GetVolumeName(TestDrive).Should().StartWith(@"\\?\Volume{");
			VolumeMountPoint.GetVolumeName(TestDrive + @"ReadTests\Volume\").Should().StartWith(@"\\?\Volume{");
			VolumeMountPoint.GetVolumeName(TestDrive + @"ReadTests\Volume").Should().StartWith(@"\\?\Volume{");

			// junction not a volume mountpoint throws System.ArgumentException: Der Wert liegt außerhalb des erwarteten Bereichs.
			// Win32Exception: 'Falscher Parameter'
//			87: Falscher Parameter
			Assert.ThrowsException<Win32Exception>(() => VolumeMountPoint.GetVolumeName(TestDrive + @"ReadTests\Junction"));

			// Die Datei oder das Verzeichnis ist kein Analysepunkt. (Ausnahme von HRESULT: 0x80071126)
			// Die Datei oder das Verzeichnis ist kein Analysepunkt
//			4390: Die Datei oder das Verzeichnis ist kein Analysepunkt
			Assert.ThrowsException<Win32Exception>(() => VolumeMountPoint.GetVolumeName(TestDrive + @"ReadTests\Original"));

			// Win32Exception: 'Falscher Parameter'
//			87: Falscher Parameter
			Assert.ThrowsException<Win32Exception>(() => VolumeMountPoint.GetVolumeName(TestDrive + @"ReadTests\SymbolicLink"));

			// Win32Exception: Das System kann die angegebene Datei nicht finden
//			2: Das System kann die angegebene Datei nicht finden
			Assert.ThrowsException<Win32Exception>(() => VolumeMountPoint.GetVolumeName(TestDrive + @"ReadTests\NotExists"));

			// Die Syntax für den Dateinamen, Verzeichnisnamen oder die Datenträgerbezeichnung ist falsch
//			123: Die Syntax für den Dateinamen, Verzeichnisnamen oder die Datenträgerbezeichnung ist falsch
			Assert.ThrowsException<Win32Exception>(() => VolumeMountPoint.GetVolumeName(TestDrive + @"ReadTests\Original.txt"));

			// Die Syntax für den Dateinamen, Verzeichnisnamen oder die Datenträgerbezeichnung ist falsch
//			123: Die Syntax für den Dateinamen, Verzeichnisnamen oder die Datenträgerbezeichnung ist falsch
			Assert.ThrowsException<Win32Exception>(() => VolumeMountPoint.GetVolumeName(TestDrive + @"ReadTests\SymbolicLink.txt"));
		}
	}

}