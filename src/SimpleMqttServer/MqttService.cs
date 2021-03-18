using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Server;
using MQTTnet.Client.Publishing;
using System;
using System.Linq;

namespace SimpleMqttServer
{
    public class MqttService : BaseService
    {
        private IMqttServer _server;

        public MqttService()
        {
            _server = new MqttFactory().CreateMqttServer();
        }

        public async Task Run()
        {
            var opts = MqttHelpers.GetOptionsBuilder().Build();
            await _server.StartAsync(opts);

            await ProcessCommands();
        }

        public async Task<MqttClientPublishResult> PublishAsync(string topic, string message)
        {
            var publishResult = await _server.PublishAsync(b => b
                .WithTopic(topic)
                .WithUserProperty("kind", "iot")
                .WithPayload(message));

            return publishResult;
            
        }
        public async Task ProcessCommands()
        {
            Console.WriteLine($"Server started.\n");
            string cmd;
            while ((cmd = Console.ReadLine()) != "exit")
            {
                switch (cmd)
                {
                    case "info":
                        var ses = await _server.GetRetainedApplicationMessagesAsync();
                        string m = "";
                        if (ses.Any())
                            m = string.Join("\n", ses.Select(i => $"Topic:\t{i.Topic}\tPayload:{i.ConvertPayloadToString()}"));

                        Console.WriteLine(m);
                        Console.WriteLine("Kek.");
                        break;
                    case "list":
                        var re = await _server.PublishAsync("KS.LOG",
                            string.Join('\n',MqttHelpers.Users.Select(i => $"{i.UserName}")));
                        break;
                    default:
                        var many = cmd.Split(" ");
                        if(many.Any())
                        {
                            if(many[0] == "pub")
                            {
                                var res = await PublishAsync(many[1], many[2]);
                                Console.WriteLine(res.ReasonCode);
                            }
                        }
                        break;
                }

            }
        }
    }
}
