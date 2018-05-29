using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace SporadicLoad
{
	public static class Participant
	{
		[FunctionName("Participant")]
		public static async Task<HttpResponseMessage> Run(
			[HttpTrigger(AuthorizationLevel.Function, "post", Route = "participant")]HttpRequestMessage req,
			TraceWriter log,
			ExecutionContext context)
		{
			log.Info("C# HTTP trigger function processed a request.");

			TelemetryConfiguration.Active.InstrumentationKey = Environment.GetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY");
			var instanceId = Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID", EnvironmentVariableTarget.Process);
			var writer = new TelemetryClient();
			var traceTelemetry = new TraceTelemetry($"(Id={context.InvocationId})", SeverityLevel.Information);
			traceTelemetry.Context.Operation.Id = context.InvocationId.ToString();
			traceTelemetry.Context.Operation.ParentId = context.InvocationId.ToString();
			traceTelemetry.Properties["WEBSITE_INSTANCE_ID"] = instanceId;
			writer.TrackTrace(traceTelemetry);

			await Task.Delay(500);
			return req.CreateResponse(HttpStatusCode.OK, "Participant");
		}
	}
}
