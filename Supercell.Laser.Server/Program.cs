namespace Supercell.Laser.Server
{
    using Masuda.Net;
    using Masuda.Net.HelpMessage;
    using Masuda.Net.Models;
    using Supercell.Laser.Server.Database.Models;
    using Supercell.Laser.Server.Database;
    using Supercell.Laser.Server.Handler;
    using Supercell.Laser.Server.Settings;
    using Supercell.Laser.Titan.Debug;
    using System.Drawing;
    using Supercell.Laser.Server.Networking.Session;

    static class Program
    {
        public const string SERVER_VERSION = "1.2";
        public const string BUILD_TYPE = "Alpha";

        private static void Main(string[] args)
        {
            Console.Title = "Server";
            Directory.SetCurrentDirectory(AppContext.BaseDirectory);


            Colorful.Console.WriteWithGradient(
                @"
██████╗ ███████╗██████╗     ██╗   ██╗
██╔══██╗██╔════╝██╔══██╗    ██║   ██║
██████╔╝███████╗██████╔╝    ██║   ██║
██╔══██╗╚════██║██╔═══╝     ╚██╗ ██╔╝
██████╔╝███████║██║          ╚████╔╝    
╚═════╝ ╚══════╝╚═╝           ╚═══╝      
                                                     
                                               
 " + "\n\n\n", Color.DarkRed, Color.Red, 10);

            Logger.Print("Server is now starting...");

            Logger.Init();
            Configuration.Instance = Configuration.LoadFromFile("config.json");

            Resources.InitDatabase();
            Resources.InitLogic();
            Resources.InitNetwork();

            Logger.Print("Server started!");

            Logger.Print("alpha");
            ExitHandler.Init();
            CmdHandler.Start();

        }
    }
}