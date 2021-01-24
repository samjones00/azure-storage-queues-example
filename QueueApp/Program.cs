using System;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QueueApp.Interfaces;
using QueueApp.Services;

namespace QueueApp
{
    class Program
    {
        static IHostBuilder CreateHostBuilder(string[] args) =>
          Host.CreateDefaultBuilder(args)
              .ConfigureServices((_, services) =>
                  services.AddTransient<IQueueService, StorageQueueService>());

        static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();

            var service = host.Services.GetService<IQueueService>();

            string connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");

            QueueClient queue = new QueueClient(connectionString, "queue-demo");

            if(args.Length > 0)
            {

                string value = string.Join(" ", args);
                await service.Insert(value);
                Console.WriteLine($"Sent: {value}");
            }
            else{
                string value = await service.Retrieve();
                Console.Write($"Received: {value}");
            }

            Console.Write("Press enter...");
            Console.ReadLine();
        
        }
    }
}
