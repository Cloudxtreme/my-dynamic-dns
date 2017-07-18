using System.Web.Configuration;

namespace MyDynamicDns
{
    public static class Settings
    {
        public static string UserName { get; }
        public static string Password { get; }
        public static string HostedZoneId { get; }
        public static string AwsAccessKeyId { get; }
        public static string AwsSecretAccessKey { get; }
        public static string OpenDnsUserName { get; }
        public static string OpenDnsPassword { get; }

        static Settings()
        {
            UserName = WebConfigurationManager.AppSettings["UserName"];
            Password = WebConfigurationManager.AppSettings["Password"];
            HostedZoneId = WebConfigurationManager.AppSettings["HostedZoneId"];
            AwsAccessKeyId = WebConfigurationManager.AppSettings["AWSAccessKeyId"];
            AwsSecretAccessKey = WebConfigurationManager.AppSettings["AWSSecretAccessKey"];
            OpenDnsUserName = WebConfigurationManager.AppSettings["OpenDnsUserName"];
            OpenDnsPassword = WebConfigurationManager.AppSettings["OpenDnsPassword"];
        }
    }
}