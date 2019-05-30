using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AzureFunctionWithSerilogAndDi
{
    public class Function
    {
        private readonly ILogger _logger;

        public Function(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("Function");
        }

        [FunctionName("Function")]
        public async Task Run([ServiceBusTrigger("$queueName$", Connection = "ConnectionName")]string myQueueItem, ExecutionContext context)
        {
            try
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(context.FunctionAppDirectory)
                    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();

                _logger.LogInformation($"C# ServiceBus queue trigger function processed message");

                //var setting = config["Setting"];
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error in Run");
                throw;
            }
        }
    }
}
