namespace Itan.Wrappers
{
    public class EmailSenderSettings
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}