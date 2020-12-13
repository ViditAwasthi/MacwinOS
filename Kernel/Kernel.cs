using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;

namespace Kernel
{
    public class Kernel : Sys.Kernel
    {
        string pass = "Vidit";
        string error = "Unknown command. For complete list of commands, use HELP";
        public static string file;
        public bool FS = false;
        string current_path = @"0:\";
        public bool SudoY = false;
        public void deleteFile(string fname)
        {
            if (File.Exists(fname))
            {
                File.Delete(fname);
            }
            else
            {
                Console.WriteLine("File doesn't exist!");
            }
        }
        public void deleteDirectory(string fname)
        {
            if (Directory.Exists(fname))
            {
                Directory.Delete(fname);
            }
            else
            {
                Console.WriteLine("Directory doesn't exist!");
            }
        }

        public void copyFile(string path, string fname, string destination)
        {
            if (File.Exists(fname) && Directory.Exists(destination))
            {
                File.Copy(path + fname, destination);
            }
            else
            {
                Console.WriteLine("File or Directory doesn't exist!");
            }
        }
        public void moveFile(string path, string fname, string destination)
        {
            copyFile(path, fname, destination);
            deleteFile(path + fname);
        }
        private string FileExists(string directory, string filename)
        {
            //Console.WriteLine(directory);
            string exists = null;
            var fileNameToCheck = Path.Combine(directory, filename);
            if (Directory.Exists(directory))
            {
                //check directory for file
                //exists = Directory.GetFiles(directory).Any(x => x.Equals(fileNameToCheck, StringComparison.OrdinalIgnoreCase));
                foreach (var dir in Directory.GetFiles(directory))
                {
                    if (dir == filename)
                    {
                        exists = Path.Combine(directory, dir);
                    }
                }
                //check subdirectories for file
                if (exists == null)
                {
                    foreach (var dir in Directory.GetDirectories(directory))
                    {
                        var dirNew = Path.Combine(directory, dir);
                        exists = FileExists(dirNew, filename);

                        if (exists != null) break;
                    }
                }
            }
            return exists;
        protected override void BeforeRun()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.BackgroundColor = ConsoleColor.Black;
        file_system:
            Console.Write("Do you want to enable the file system?(Y/N)");
            var filesys = Console.ReadLine();
            if (filesys == "Y")
            {
                FS = true;
                Console.Write("File system will be Initialized");
                var fs = new Sys.FileSystem.CosmosVFS(); //in-built function to make a new file system
                Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);
            }
            else if (filesys == "N")
            {
                Console.WriteLine("File System will not be Initiated");
            }
            else
                goto file_system;
            try
            {
                filesystem.createDir("0:\\System1"); //initial directory creation
                filesystem.createDir("0:\\User");
                filesystem.createDir("0:\\User\\Documents");
                Sys.KeyboardManager.SetKeyLayout(new Sys.ScanMaps.US_Standard()); //keeping keyboard layout as US standarad
            }
            catch (Exception exc)
            {
                goto fatto;
            }
        fatto:
            Console.Clear();
            logo.Logo();
            Console.WriteLine("                                                  ");
            Console.WriteLine("           Successfully Booted                    ");
            Console.WriteLine("                                                  ");
        inizia:
            Console.WriteLine("Please enter password to log in! (Type N to shutdown)");
            var sino = Console.ReadLine();
            if (sino == pass)
            {
                Console.Clear();
                Console.Write("Welcome to MacwinOS!!\n");
            }
            else if (sino == "N" || sino == "n")
            {
                Stop();
            }
            else { goto inizia; }


        }

        protected override void Run()
        {
            if (!Directory.Exists(@"0:\RecycleBin"))
            {
                Directory.CreateDirectory(@"0:\RecycleBin");
            }
            Console.Write(current_path + "@root: ");
            var input = Console.ReadLine();
            var co = input;
            var vars = "";
            if (input.ToLower().IndexOf('/') != -1)
            {

                string[] parts = input.Split('/');
                co = parts[0];
                vars = parts[1];
            }
            try
            {
                switch (co)
                {

                    case "reboot":    //Reboots the machine
                        Cosmos.System.Power.Reboot();
                        break;

                    case "shutdown":   //Shuts down the machine
                        Console.WriteLine("now you can power off your system");
                        Stop();
                        break;
                    case "clear":   //Clears the screen
                        Console.Clear();
                        break;

                }
                catch (Exception e) //BlueScreenOfDeath-like thing I wanted to make noerror false but it bugs
            {
            }
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Clear();
        spegni:
            Console.Write("   Do you want to reboot or shutdown?(R/S)");
            var risp = Console.ReadLine();
            if (risp == "R" || risp == "r")
            {
                Sys.Power.Reboot();
            }
            else if (risp == "S" || risp == "s")
            {
                Stop();
            }
            else
            {
                goto spegni;
            }
        }
