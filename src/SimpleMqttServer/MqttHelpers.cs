using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using MQTTnet.Protocol;
using MQTTnet.Server;
using Newtonsoft.Json;

namespace SimpleMqttServer
{
    public class MqttHelpers : BaseService
    {
        private static Config _config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "config.json")));
        public static int? Port { get; set; } = null;
        public static new UserList Users
        {
            get => _config?.Users;
            set => _config.Users = value;
        }

        public static MqttServerOptionsBuilder GetOptionsBuilder()
            => new MqttServerOptionsBuilder()
                .WithDefaultEndpoint()
                .WithDefaultEndpointPort(Port ?? _config.Port)
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

            if (!Users.Any(i => i.UserName == c.Username))
                Users.Add(currentUser);

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
