namespace FewBox.Service.Mail.Model.Configs
{
    public class SmtpConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public int Timeout { get; set; }
        public bool EnableSsl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}