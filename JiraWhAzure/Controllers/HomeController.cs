using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JiraWhAzure.Models;
using JiraWhAzure.Stndr;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Azure.ServiceBus.Management;
using Newtonsoft.Json;

namespace JiraWhAzure.Controllers
{
    public class HomeController : Controller
    {
        const string ServiceBusConnectionString = "Endpoint=sb://issueconnector-dev.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Kb/9VMVKyZTNLXoBXphbEut2btlyWbXZhMIf+IbtCjc=";
        const string QueueName = "firstqueue";
        static IQueueClient queueClient;
        public const string connectionString = "Endpoint=sb://issueconnector-dev.servicebus.windows.net/;SharedAccessKeyName=secq;SharedAccessKey=cpi0DLY02+JKKPhM+z8UANLsz5XhzkWsFac5k/SyM1I=";
        public const string QueName = "secQue";
        private string msgs = "test";
        private string issueCreated = "issue_created";
        private string issueUpdated = "issue_updated";
        private string issueDeleted = "issue_deleted";
        private string issueCommented = "issue_commented";
        string TrimbleProjectId = "636935113624010709";
        string token = "71WxFStkbstp6zijMBYiYgXecPQ56zMmhuvYno0BYelQv9_EFy-65N3qbmhncezT7k3jyurN9D6P8qCsVVGuy3aqZ8fg__m2pYeYEjSpRI7jnDWVdKA4hR0pb5Tv34qqqwZFxKSDl2HnhxIGSXSQm_4Wzi_6E8LG5wWApYMlziJDA_LxPFDPj66L5sPTjyDngWHWNzBxCGR4aoCu41G1TTmk9KRV9Oiy6pNOBH4rufpb71T-njq6RrxfoRAV6cFmRsptM0m2s_FFHbngh3TtuDCwsN3eVsXfTxRT2h2RhY012C3QiM2k8ElXZNxdqyvLL2p_E3yf30CfIOUjHdQOggE4m6OvI8IW6D-oFDVlLWLxKve0jxhHoEL7pRo2aIxCc24WQgvv8QaDX5FJoBEhPosnlhUP-jnoaRtxhoZYuy3IOuSXjZ-e25zBqW7SFleUur3rBA";

        mpper mapx = new mpper();

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public string GetMessage()
        {
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
    
             var managementClient = new ManagementClient(ServiceBusConnectionString);
          

            return "done";

        }
        public async Task<string> GetMessage1()
        {

            var managementClient = new ManagementClient(ServiceBusConnectionString);
            var queue = await managementClient.GetQueueRuntimeInfoAsync(QueueName);
            var messageCount = queue.MessageCount.ToString();

            return messageCount;
        }
        public async Task<string> ConsumeServiceBus1()
        {
            var receiver = new MessageReceiver(connectionString, QueName, ReceiveMode.ReceiveAndDelete,
                RetryPolicy.NoRetry, 1);

            IList<Message> messagesFromSrvcBus = await receiver.ReceiveAsync(1);

            string msgContent = Encoding.UTF8.GetString(messagesFromSrvcBus[0].Body);

            IStandardInstance strdMsg = JsonConvert.DeserializeObject<IStandardInstance>(msgContent);

            Topic tpc= new Topic();
            HttpResponseMessage response = new HttpResponseMessage();

            if (strdMsg.Action == "created")
            {
                tpc = mapx.ConvertToTopic(strdMsg);

                var values = new Dictionary<string, string>
                {
                    {"title", tpc.Title},
                    {"qguid", tpc.QGuid},
                    {"projectId", tpc.ProjectId},
                    {"AuthorName", tpc.AuthorName},
                    {"AuthorEmail", tpc.AuthorEmail},
                    {"CreationDate", tpc.CreationDate}
                };
                var client = new HttpClient();
                client.BaseAddress = new Uri("https://topics.quadridcm.com/api/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var content = new FormUrlEncodedContent(values);
                 response = await client.PostAsync("projects/" + tpc.ProjectId + "/topics", content);
            }
            else if (strdMsg.Action == "updated")
            {
                tpc = mapx.ConvertToTopic(strdMsg);

                var values = new Dictionary<string, string>
                {
                    {"title", tpc.Title},
                    {"qguid", tpc.QGuid},
                    {"id",tpc.Id },
                    {"projectId", tpc.ProjectId},
                    {"AuthorName", tpc.AuthorName},
                    {"AuthorEmail", tpc.AuthorEmail},
                    {"CreationDate", tpc.CreationDate}
                };
                var client = new HttpClient();
                client.BaseAddress = new Uri("https://topics.quadridcm.com/api/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var content = new FormUrlEncodedContent(values);
                  response = await client.PutAsync("projects/" + tpc.ProjectId + "/topics/" + tpc.Id , content);
            }



            return response.ToString();

        }

        private void RegisterOnMessageHandlerAndReceiveMessages()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                // Maximum number of concurrent calls to the callback ProcessMessagesAsync(), set to 1 for simplicity.
                // Set it according to how many messages the application wants to process in parallel.
                MaxConcurrentCalls = 1,

                // Indicates whether the message pump should automatically complete the messages after returning from user callback.
                // False below indicates the complete operation is handled by the user callback as in ProcessMessagesAsync().
                AutoComplete = false
            };

            // Register the function that processes messages.
            queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);

        }

        static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            // Process the message.
            Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");

            // Complete the message so that it is not received again.
            // This can be done only if the queue Client is created in ReceiveMode.PeekLock mode (which is the default).
            await queueClient.CompleteAsync(message.SystemProperties.LockToken);

            // Note: Use the cancellationToken passed as necessary to determine if the queueClient has already been closed.
            // If queueClient has already been closed, you can choose to not call CompleteAsync() or AbandonAsync() etc.
            // to avoid unnecessary exceptions.
        }
        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {

            return Task.CompletedTask;
        }
    }
}
