using System;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Server;
using MQTTnet.Client.Publishing;

namespace SimpleMqttServer
{
    public class MqttService : BaseService, IMqttService
    {
        private IMqttServer _server;

        public MqttService()
        {
            _server = new MqttFactory().CreateMqttServer();
            
        }


        public async Task<MqttClientPublishResult> PublishAsync(string topic, string message)
        {
            var publishResult = await _server.PublishAsync((b) => b
                .WithTopic(topic)
                .WithUserProperty("kind", "iot")
                .WithPayload(message));

            return publishResult;
            
        }

        public async void Run()
        {
            var opts = MqttHelpers.GetOptionsBuilder().Build();

            await _server.StartAsync(opts);

        }
    }
}
