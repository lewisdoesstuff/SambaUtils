using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SambaUtils
{
    /// <summary>
    /// Options for the program.
    /// PrintOut will print net use output to the console.
    /// </summary>
    public static class Options
    {
        public static bool PrintOut = false;
    }
    
    
    /// <summary>
    /// Define the Samba server
    /// Unc = UNC mapped path of samba share (e.g. "\\server\share")
    /// Mount = Requested mount point of share (e.g. Z)
    /// Credentials = KeyValuePair: Username, Password.
    /// </summary>
    public class Share
    {
        public string Unc { get; set; }
        public char Mount { get; set; }
        public KeyValuePair<string,string> Credentials { get; set; }
    }

    public class Samba
    {
        /// <summary>
        /// Mount the given samba share.
        /// </summary>
        /// <param name="samba">Share to mount.</param>
        /// <returns>Exit code of `net use`. Returns 254 if UNC is invalid.</returns>
        public static int Mount(Share samba)
        {
            if (!Validation.Validate(samba)) return 254;
            // parse the Share into a `net use` command, then execute it.
            var command = $"net use {samba.Mount}: {samba.Unc} /user:{samba.Credentials.Key} {samba.Credentials.Value}";
            return Cmd.Execute(command);
        }

        /// <summary>
        /// Unmount the given samba share.
        /// </summary>
        /// <param name="samba">Share to unmount.</param>
        /// <returns>Exit code of `net use`. Returns 254 if UNC is invalid.</returns>
        public static int Unmount(Share samba)
        {
            if (!Validation.Validate(samba)) return 254;
            // parse the Share into a `net use` command, then execute it.
            var command = $"net use {samba.Mount}: /D /Y";
            return Cmd.Execute(command);
        }
    }

    /// <summary>
    /// For now, this just contains the method to validate that the share passed is a (mostly) valid UNC path.
    /// </summary>
    internal class Validation
    {
        public static bool Validate(Share samba)
        {
            // We could almost definitely do this with a RegEx expression. 
            // We only need to do this once, so I think this is fine.
            return samba.Unc.StartsWith(@"\\") && !string.IsNullOrWhiteSpace(samba.Unc.Split('\\')[3]);
        }
    }
    
    internal class Cmd
    {
        /// <summary>
        /// Run a given command using cmd.exe with the /c argument.
        /// </summary>
        /// <param name="command">Command to be executed</param>
        /// <returns>Returns process exit code. If process is null, return 1.</returns>
        public static int Execute(string command)
        {
            // .NET has no native methods for dealing with network shares of any kind (hence this library).
            
            // Create a new process with the given arguments
            var startInfo = new ProcessStartInfo("cmd.exe", "/C " + command)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                WorkingDirectory = @"C:\",
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };
            var process = Process.Start(startInfo);
            // Run the process and redirect outputs to the below vars.
            var stdout = process?.StandardOutput.ReadToEnd();
            var stderr = process?.StandardError.ReadToEnd();
            
            // if requested, print them to the console. A logger of sorts would definitely be the better implementation here.
            if (Options.PrintOut)
            {
                Console.WriteLine("stdout: " + stdout);
                Console.WriteLine("stderr: " + stderr);
            }

            // process should never really be null, but just to be sure.
            // If not, close the process, then return the exit code.
            if (process == null) return 1;
            var exitCode = process.ExitCode;
            process.Close();
            return exitCode;
        }
    }
}