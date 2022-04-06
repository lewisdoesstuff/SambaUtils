# SambaUtils


A C# .NET Library to handle mounting/unmounting Samba shares on Windows.

## Description

SambaUtils provides 2 methods (mount, unmount) to handle Samba shares on Windows, while reducing needed boilerplate code to handle `net use`.

Samba stdout/err can be enabled with `SambaUtils.Options.PrintOut = true`

## Getting Started

### Dependencies

* Project targets .NET 6. SDK required for building.
    - We don't call anything specific, so building for .NET 4.8 or .NET 5 isn't an issue.

### Building

* Building the library from scratch just requires you to download the solution, then build this through your .NET IDE of choice
* You can also use `MSBuild` from the CLI to build the file.

### Installing

* Add reference to SambaUtils.dll in your .NET Project.

* Alternatively, download the SambaUtils package from Nuget (.NET 6).

### Usage

C# Example
```c#
        public static void Main(string[] args)
        { 
            Share samba = new Share
            {
                Unc = @"\\nas\storage", // UNC path for Samba Share
                Mount = 'z', // requested mountpoint
                Credentials = new KeyValuePair<string, string>("username", "password") //samba credentials, could be read from file or pulled from another (safer) source.
            };
            
            // Enable stdout/err output for `net use`
            SambaUtils.Options.PrintOut = true;

            Samba.Mount(samba); // mount the network share with the given parameters
            Samba.Unmount(samba); // unmount the network share.
        }
```

## Authors

[lewisdoesstuff](https://github.com/lewisdoesstuff)

## Version History

* 0.2
    * .NET 6 port
* 0.1
    * Initial Release

## License

This project is licensed under the MIT License - see the LICENSE.md file for details
