using System;
using MQTTnet;
using MQTTnet.Server;
using Serilog;

namespace SimpleMqttServer
{
    public class BaseService
    {
        protected static readonly ILogger Logger = Log.ForContext<BaseService>();

        /// <summary> 
        ///     Logs the message from the MQTT subscription interceptor context. 
        /// </summary> 
        /// <param name="context">The MQTT subscription interceptor context.</param> 
        /// <param name="successful">A <see cref="bool"/> value indicating whether the subscription was successful or not.</param> 
        protected static void LogMessage(MqttSubscriptionInterceptorContext context, bool successful)
        {
            if (context == null)
                return;
            var clientId = context.ClientId;
            var topicFilter = context.TopicFilter;
            string msg = successful
                    ? $"New subscription: ClientId = {clientId}, TopicFilter = {topicFilter}"
                    : $"Subscription failed for clientId = {clientId}, TopicFilter = {topicFilter}";
            Console.WriteLine(msg);
            Logger.Information(msg);
        }

        /// <summary>
        ///     Logs the message from the MQTT message interceptor context.
        /// </summary>
        /// <param name="context">The MQTT message interceptor context.</param>
        protected static void LogMessage(MqttApplicationMessageInterceptorContext context)
        {
            if (context == null)
                return;

            var msg = context.ApplicationMessage;

            var payload = $"\aTopic: {msg.Topic};\tMsg:{msg?.ConvertPayloadToString() ?? string.Empty}";

            Logger.Information(
                "Message: ClientId = {clientId}, Topic = {topic}, Payload = {payload}, QoS = {qos}, Retain-Flag = {retainFlag}",
                context.ClientId,
                "KS.LOG",
                payload,
                msg?.QualityOfServiceLevel,
                msg?.Retain);
        }

        /// <summary> 
        ///     Logs the message from the MQTT connection validation context. 
        /// </summary> 
        /// <param name="context">The MQTT connection validation context.</param> 
        /// <param name="showPassword">A <see cref="bool"/> value indicating whether the password is written to the log or not.</param> 
        protected static void LogMessage(MqttConnectionValidatorContext context, bool showPassword)
        {
            if (context == null)
            {
                return;
            }

            if (showPassword)
            {
                var msg = $"New connection:\n" +
                    $"ClientId = {context.ClientId}, " +
                    $"Endpoint = {context.Endpoint}, " +
                    $"Username = {context.Username}, " +
                    $"Password = {context.Password}, " +
                    $"CleanSession = {context.CleanSession}";
                Console.WriteLine(msg);
                Logger.Information(msg);
            }
            else
            {
                var msg = $"New connection:\n" +
                    $"ClientId = {context.ClientId}, " +
                    $"Endpoint = {context.Endpoint}, " +
                    $"Username = {context.Username}, " +
                    $"CleanSession = {context.CleanSession}";
                Console.WriteLine(msg);
                Logger.Information(msg);
                    
            }
        }
    }
}