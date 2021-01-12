namespace Bumbo.Logic.Options
{
    public class MailOptions
    {
        public const string Mail = "Mail";

        public string Host { get; set; }
        public int Port { get; set; } = 465;
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool UseTls { get; set; } = true;

        public string FromName { get; set; } = "Bumbo";
        public string FromEmail { get; set; }
    }
}
