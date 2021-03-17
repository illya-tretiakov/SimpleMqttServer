using System;
using System.IO;
using MQTTnet.Protocol;
using MQTTnet.Server;
using Newtonsoft.Json;

namespace SimpleMqttServer
{
    public class MqttHelpers : BaseService
    {
        private static Config _config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "config.json")));

        public static MqttServerOptionsBuilder GetOptionsBuilder()
            => new MqttServerOptionsBuilder()
                .WithDefaultEndpoint()
                .WithDefaultEndpointPort(_config.Port)
                .WithConnectionValidator(ConnectionValidator)
                .WithSubscriptionInterceptor(SubscriptionInterceptor)
                .WithApplicationMessageInterceptor(MessageInterceptor);

        private static Action<MqttConnectionValidatorContext> ConnectionValidator => c =>
        {
            var currentUser = _config.Users[c.Username];

            if (currentUser == null)
            {
                c.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                LogMessage(c, true);
                return;
            }

            if (c.Username != currentUser.UserName)
            {
                c.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                LogMessage(c, true);
                return;
            }

            if (c.Password != currentUser.Password)
            {
                c.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                LogMessage(c, true);
                return;
            }

            c.ReasonCode = MqttConnectReasonCode.Success;
            LogMessage(c, false);
        };

        private static Action<MqttSubscriptionInterceptorContext> SubscriptionInterceptor => c =>
        {
            c.AcceptSubscription = true;
            LogMessage(c, true);
        };

        /// <summary>
        /// app message handling
        /// </summary>
        private static Action<MqttApplicationMessageInterceptorContext> MessageInterceptor => c =>
        {
            c.AcceptPublish = true;
            LogMessage(c);
        };
    }
}
