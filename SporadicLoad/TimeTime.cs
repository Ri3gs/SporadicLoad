using System;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace SporadicLoad
{
	public static class TimeTime
	{
		[FunctionName("TimeTime")]
		public static void Run([TimerTrigger("0 */3 * * * *")]TimerInfo myTimer, TraceWriter log, ExecutionContext context)
		{
			log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
			TelemetryConfiguration.Active.InstrumentationKey = Environment.GetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY");
			var instanceId = Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID", EnvironmentVariableTarget.Process);
			var writer = new TelemetryClient();
			var traceTelemetry = new TraceTelemetry($"(Id={context.InvocationId})", SeverityLevel.Information);
			traceTelemetry.Context.Operation.Id = context.InvocationId.ToString();
			traceTelemetry.Context.Operation.ParentId = context.InvocationId.ToString();
			traceTelemetry.Properties["WEBSITE_INSTANCE_ID"] = instanceId;
			writer.TrackTrace(traceTelemetry);
		}
	}
}

