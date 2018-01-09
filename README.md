# UngracefulUnloadOfFunctions
This is a quick repro to show how Azure Functions do not shut down gracefully but rather kill any actively running functions when stopped, restarted, deployed, or deployment slots are swapped.

# Setup
1. Create an Azure Service Bus with a Queue named "myqueue".
2. Update `Program.cs` (line 15) with the connection string to the Azure Service Bus you created in #1.
3. Create `local.settings.json` and add the connection string to the Azure Service Bus you created in #1 (if you want to run locally).
4. Create an Azure SQL Database and run the `database.sql` script to create the tables needed.
5. Update `SleepFunction.cs` (line 16) and `Program.cs` (line 17) with the connection string to the Azure SQL DB you created in #3.
6. Publish the SimpleFunctionApp to an App Service Plan host
7. Create a new Application setting for the function app named `MyAzureServiceBusConnectionString` and set it to the connection string from #1.

# Spot check
1. Run `TrafficGenerator.exe 1 1 0 1` to send 1 message to the `myqueue` queue.
2. Validate the function picks it up and process the message
3. Run the `query.sql` queries to see the results (everything is good)
4. Use the `delete` statements in the `query.sql` comments to clear out the data

# Run test
1. Run `TrafficGenerator.exe 5 4 2 9999` to start dumping load on the queue.
2. Run `query.sql` to monitor what's going on (specifically watch the table set with 1 column named MISSING as this shows the backlog of messages).
3. Once the `MISSING` list is pretty static (not growing/shrinking) or keeps growing (about 20-30 seconds; you just want constant traffic; you can crank up the first 2 numbers if you need more load), stop the Function App.
4. Start the Function App.
5. Wait for the Function to start picking up messages again.
6. CTRL+C in the cmd prompt running TrafficManager to stop new messages.
7. Wait for the MISSING table set to be empty (finish processing).

# See the problem
1. Run `query.sql` and you _should_ see the table set with the TOO_MANY column having records that indicate functions were killed mid-flight and reprocessed.
2. If you don't, try the Run Test a couple more times until you do.