# SambaUtils


A C# .NET Library to handle mounting/unmounting Samba shares on Windows.

## Description

SambaUtils provides 2 methods (mount, unmount) to handle Samba shares on Windows, while reducing needed boilerplate code to handle `net use`.

Samba stdout/err can be enabled with `SambaUtils.Options.PrintOut = true`

## Getting Started

### Dependencies

* Project targets .NET Framework 4.7. There shouldn't be any issues building for NET 5.0, if needed.

### Building

* Building the library from scratch just requires you to download the solution, then build this through your .NET IDE of choice
* You can also use `dotnet build` from the CLI to build the file.

### Installing

* Add reference to SambaUtils.dll in your .NET Project.
* You can also add the `SambaUtils` NuGet package from the CLI or your IDE.

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

* 0.1
    * Initial Release

## License

This project is licensed under the MIT License - see the LICENSE.md file for details
