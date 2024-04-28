namespace Lumbre.Frontend.Web.Configuration
{
    public record Configuration(IWebHostBuilder WebHost, IApplicationBuilder ApplicationBuilder) {
        public string BasePath { get; set; } = "FHIR";
    };

    public class SelfHostedConfiguration 
    {
        public string IP { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 5000; 
    
    };
}
