

namespace Evolve.Utils
{
    internal class WebSocketContent
    {
        public string userId { get; set; }
        public User user { get; set; }
        public Moderation moderation { get; set; }
        public string location { get; set; }
        public string instance { get; set; }
        public World world { get; set; }
        public bool canRequestInvite { get; set; }
    }
}