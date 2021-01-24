using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using QueueApp.Interfaces;
using System;
using System.Threading.Tasks;

namespace QueueApp.Services
{
    public class StorageQueueService : IQueueService
    {
        QueueClient client;

        public StorageQueueService()
        {
            string connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");

            client = new QueueClient(connectionString, "queue-demo");
        }

        public async Task Insert(string message)
        {
            var ttl = -1;

            if (client.CreateIfNotExistsAsync() != null)
            {
            }

            await client.SendMessageAsync(message, default, TimeSpan.FromSeconds(ttl), default);
        }

        public async Task<string> Retrieve()
        {
            if (await client.ExistsAsync())
            {
                QueueProperties properties = await client.GetPropertiesAsync();

                if (properties.ApproximateMessagesCount > 0)
                {
                    QueueMessage[] retrievedMessage = await client.ReceiveMessagesAsync(1);
                    var message = retrievedMessage[0].MessageText;
                    await client.DeleteMessageAsync(retrievedMessage[0].MessageId, retrievedMessage[0].PopReceipt);
                    return message;
                }
                else
                {
                    await DeleteEmptyQueue();
                }
            }

            return "The queue does not exist";

        }

        public async Task DeleteEmptyQueue()
        {
            QueueProperties properties = await client.GetPropertiesAsync();

            if (properties.ApproximateMessagesCount == 0)
            {
                await client.DeleteIfExistsAsync();
            }
        }
    }
}
