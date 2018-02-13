# KsWare.IO.FileSystem # 

A NTFS file system library.

Provides static NTFS object classes like SymbolicLink, JunctionPoint, HardLink, ReparsePoint with long path support. Long path support for normal filesystem objects is realized by using Pri.LongPath library by Peter Ritchie. It is scheduled to implement the new objects into Pro.LongPath. So this library can be used as extension for Pri.LongPath. 

## Usage ##

Create volume mountpoint fpr drive X:

    var vn= VolumeMountPoint.GetVolumeName(@"X:\");
    VolumeMountPoint.Create(@"C:\mnt\X", vn);

SymbolicLink

    SymbolicLink.Create(@"C:\Links\X", @"X:\anypath", true);

ReparsePoint

    string target = ReparsePoint.GetTarget(@"C:\Links\X");
    ReparseData data = ReparsePoint.GetReparseData(string path);
    Debug.WriteLine(data.ReparseTag);
    Debug.WriteLine(data.SubstituteName);

JunctionPoint

    JunctionPoint.Create(@"C:\Links\directory", @"X:\anydirectory", true);

HardLink

    JunctionPoint.Create(@"C:\Links\file", @"X:\anyfile", true);

## License ##

Licensed under the MIT license.