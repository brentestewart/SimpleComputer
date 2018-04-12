
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

namespace ProcessTweet
{
    public static class ProcessTweetFunction
    {
        [FunctionName("ProcessTweet")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequest req, CloudBlockBlob outputBlob, IBinder binder, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

	        //req.Content.ReadAsStringAsync();
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            var name = data?.name;

	        var webClient = new WebClient();
	        byte[] imageBytes = webClient.DownloadData("http://www.google.com/images/logos/ps_logo2.png");

	        outputBlob.Uri.ToString();
			outputBlob.UploadFromByteArrayAsync(imageBytes,0,1000).RunSynchronously();

	        var date = DateTime.Now.ToString("yyyyMMddhhmmss");
	        using (var writer = binder.BindAsync<BinaryWriter>(new TableAttribute("Post", DateTime.Now.Year.ToString(), date)).Result)
	        {
				writer.Write(imageBytes);
	        }

				return name != null
					? (ActionResult)new OkObjectResult($"Hello, {name}")
					: new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
