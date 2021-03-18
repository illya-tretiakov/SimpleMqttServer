using System.Linq;
using System.Collections.Generic;
namespace SimpleMqttServer
{
    public class User
    {
        /// <summary>
        ///     Gets or sets the user name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///     Gets or sets the password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// user props (string key, string val)
        /// </summary>
        public Dictionary<string, string> UserProperties { get; set; }
    }
    public static class UserExtensions
    {
        public static bool IsIot(this User user)
        {
            if (user?.UserProperties is null)
                return false;

            var props = user.UserProperties;

            return props.Any() && props.ContainsKey("kind") && props["kind"] == "iot";   

        }
    }
    public class UserList : List<User>
    {
        public User this[string username]
        {
            get => this.FirstOrDefault(i => i.UserName == username) ?? this.FirstOrDefault();
            set
            {
                var v = this.FirstOrDefault(i => i.UserName == username);
                v = value;
            }
        }
    }
}