using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Protocol;
using MQTTnet.Server;
using Newtonsoft.Json;
using Serilog;

namespace SimpleMqttServer
{
    public class Program
    {
        private const string ARG_PORT = "port";

        private static Action<MqttServerClientConnectedEventArgs> ClientConnectionHandler => clientConn =>
        {
            //TODO:
            
            Console.WriteLine($"Connected. Client ID: {clientConn.ClientId}");
        };

        public static void Main(string[] args)
        {
            if (args.Any())
                if (args.Any(i => i.StartsWith(ARG_PORT)))
                    if (int.TryParse(args.FirstOrDefault(i => i.StartsWith("port")).Skip(ARG_PORT.Length).ToString(), out int port))
                        MqttHelpers.Port = port;


            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(Path.Combine(Directory.GetCurrentDirectory(), "MqttServerLog.txt"),
                rollingInterval: RollingInterval.Day)
                .WriteTo.Console()
                .CreateLogger();
            Stopwatch sw = new Stopwatch();
            var srv = new MqttService();
            sw.Start();
            srv.Run().ContinueWith(r =>
            {
                
                Console.WriteLine("Server stopped.");
                Console.WriteLine($"Time from start: {sw.Elapsed}");
            });
            //ProcessCommands();

            
        }
    }
}