namespace SimpleMqttServer
{
    public class Config
    {
        public int Port { get; set; }

        public UserList Users { get; set; } = new UserList();
    }
}