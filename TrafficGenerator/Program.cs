using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Azure.ServiceBus;

namespace TrafficGenerator
{
  class Program
  {
    const string ServiceBusConnectionString = "REMOVED-put-yours-here";
    const string QueueName = "myqueue";
    private const string AzureSqlConnectionString = "REMOVED-put-yours-here";

    static void Main(string[] args)
    {
      int threads = 1;
      int loops = 4;
      int sleepSeconds = 5;
      int interations = 1;

      if (args.Length > 0)
      {
        threads = int.Parse(args[0]);
      }

      if (args.Length > 1)
      {
        loops = int.Parse(args[1]);
      }

      if (args.Length > 2)
      {
        sleepSeconds = int.Parse(args[2]);
      }

      if (args.Length > 3)
      {
        interations = int.Parse(args[3]);
      }

      for (int i = 0; i < interations; i++)
      {
        SendMessagesAsync(threads, loops).Wait();
        Thread.Sleep(TimeSpan.FromSeconds(sleepSeconds));
      }

      Console.WriteLine("Done");
    }

    static async Task SendMessagesAsync(int threadCount, int numberOfMessagesToSend)
    {
      List<Task> tasks = new List<Task>();

      var queueClient = new QueueClient(ServiceBusConnectionString, QueueName);

      for (var i = 0; i < threadCount; i++)
      {
        tasks.Add(Task.Run(async () =>
        {
          for (var j = 0; j < numberOfMessagesToSend; j++)
          {
            try
            {
              var correlationId = Guid.NewGuid().ToString();

              using (var connection = new SqlConnection(AzureSqlConnectionString))
              {
                connection.Execute("INSERT INTO BusMessage (MessageKey, InsertDate) Values (@Key, GetDate());",
                  new { Key = correlationId }
                );
              }

              // Create a new message to send to the queue
              var message = new Message(Encoding.UTF8.GetBytes(correlationId));
              message.ContentType = "application/json";

              // Send the message to the queue
              await queueClient.SendAsync(message);

              // Write the body of the message to the console
              Console.WriteLine($"{DateTime.Now}: Sent message: {correlationId}");
            }
            catch (Exception exception)
            {
              Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
          }
        }));
      }

      await Task.WhenAll(tasks);

      Console.WriteLine("Done with interation.");
    }
  }
}