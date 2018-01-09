using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using Dapper;

namespace SimpleFunctionApp
{
  public static class SleepFunction
  {
    private const int APP_VER = 1;
    private const string SqlAzureConnectionString = "REMOVED-put-yours-here";

    [FunctionName("SleepFunction")]
    public static async Task Run(
      [ServiceBusTrigger("myqueue", AccessRights.Listen, Connection = "MyAzureServiceBusConnectionString")]
        string correlationId, TraceWriter log)
    {
      int step = 0;

      try
      {
        var processId = Process.GetCurrentProcess().Id;
        var threadId = Thread.CurrentThread.ManagedThreadId;

        step++;
        using (var connection = new SqlConnection(SqlAzureConnectionString))
        {
          await connection.ExecuteAsync(
            $"INSERT INTO FunctionRun (ProcessId, ThreadId, RunKey, Step, InsertDate, FuncVer) Values (@ProcessId, @ThreadId, @RunKey, 1, GetDate(), {APP_VER});",
            new { ProcessId = processId, ThreadId = threadId, RunKey = correlationId }
          );
        }

        // it doesn't seem to matter whether we perform a syncrhonous sleep or an asyncrhonous sleep
        // in terms of how the host process kills are active executions
        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
        //await Task.Delay(TimeSpan.FromSeconds(5));

        step++;
        using (var connection = new SqlConnection(SqlAzureConnectionString))
        {
          await connection.ExecuteAsync(
            $"INSERT INTO FunctionRun (ProcessId, ThreadId, RunKey, Step, InsertDate, FuncVer) Values (@ProcessId, @ThreadId, @RunKey, 2, GetDate(), {APP_VER});",
            new { ProcessId = processId, ThreadId = threadId, RunKey = correlationId }
          );
        }
      }
      catch (Exception e)
      {
        // log any exceptions for tracking purposes
        using (var connection = new SqlConnection(SqlAzureConnectionString))
        {
          await connection.ExecuteAsync(
            $"INSERT INTO FunctionException (RunKey, Step, InsertDate, FuncVer, Details) Values (@RunKey, @Step, GetDate(), {APP_VER}, @Details);",
            new { RunKey = correlationId, Step = step, Details = e.ToString() }
          );
        }

        throw;
      }
    }
  }
}
