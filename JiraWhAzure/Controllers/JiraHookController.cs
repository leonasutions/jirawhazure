using System;
using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JiraWhAzure.Hubs;
using JiraWhAzure.Models;
using JiraWhAzure.Stndr;
using Newtonsoft.Json.Linq;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Azure.ServiceBus.Management;
using Newtonsoft.Json;

namespace JiraWhAzure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JiraHookController : Controller
    {

        public const string connectionString = "Endpoint=sb://issueconnector-dev.servicebus.windows.net/;SharedAccessKeyName=secq;SharedAccessKey=cpi0DLY02+JKKPhM+z8UANLsz5XhzkWsFac5k/SyM1I=";
        public const string QueName = "secQue";
        static IQueueClient queueClient;
        private readonly IJiraHookService _jiraHookService;
        private string msgs = "test";
        private string issueCreated = "issue_created";
        private string issueUpdated = "issue_updated";
        private string issueDeleted = "issue_deleted";
        private string issueCommented = "issue_commented";
        string TrimbleProjectId = "636935113624010709";
        private string tpcId = "9b5500c0-6f86-4df4-bc68-104c31a50dea";
        string token = "71WxFStkbstp6zijMBYiYgXecPQ56zMmhuvYno0BYelQv9_EFy-65N3qbmhncezT7k3jyurN9D6P8qCsVVGuy3aqZ8fg__m2pYeYEjSpRI7jnDWVdKA4hR0pb5Tv34qqqwZFxKSDl2HnhxIGSXSQm_4Wzi_6E8LG5wWApYMlziJDA_LxPFDPj66L5sPTjyDngWHWNzBxCGR4aoCu41G1TTmk9KRV9Oiy6pNOBH4rufpb71T-njq6RrxfoRAV6cFmRsptM0m2s_FFHbngh3TtuDCwsN3eVsXfTxRT2h2RhY012C3QiM2k8ElXZNxdqyvLL2p_E3yf30CfIOUjHdQOggE4m6OvI8IW6D-oFDVlLWLxKve0jxhHoEL7pRo2aIxCc24WQgvv8QaDX5FJoBEhPosnlhUP-jnoaRtxhoZYuy3IOuSXjZ-e25zBqW7SFleUur3rBA";

        mpper mapx = new mpper();

        public JiraHookController(IJiraHookService jiraHookService)
        {
            _jiraHookService = jiraHookService;
        }

        [HttpPost]
        public async void JiraHubHandler(JObject data)
        {

           await sendToServiceBus(data);
            ConsumeServiceBus();
        }

        public async Task sendToServiceBus(JObject data)
        {
            string dataJsonFormat = "";
            queueClient = new QueueClient(connectionString, QueName);
            IStandardInstance StandarizedIssue;
            IstandardComment standarizedComment;

            root dataDeserializeObject = JsonConvert.DeserializeObject<root>(data.ToString());


            if (dataDeserializeObject.Issue_event_type_name == issueCreated)
            {
                StandarizedIssue = mapx.standarizedIssue(dataDeserializeObject);
                msgs = JsonConvert.SerializeObject(StandarizedIssue);
            }
            else if (dataDeserializeObject.Issue_event_type_name == issueUpdated)
            {
                StandarizedIssue = mapx.standarizedIssue(dataDeserializeObject);
                msgs = JsonConvert.SerializeObject(StandarizedIssue);
            }
            else if (dataDeserializeObject.Issue_event_type_name == issueCommented)
            {
                standarizedComment = mapx.standarizedComment(dataDeserializeObject);
                msgs = JsonConvert.SerializeObject(standarizedComment);
            }


            await SendMessagesAsync(msgs);
        }

        public async Task ConsumeServiceBus()
        {
            var managementClient = new ManagementClient(connectionString);
            var queue = await managementClient.GetQueueRuntimeInfoAsync(QueName);
            var x = (int)queue.MessageCount;

            var receiver = new MessageReceiver(connectionString, QueName, ReceiveMode.ReceiveAndDelete,
                RetryPolicy.NoRetry, x);

            IList<Message> messagesFromSrvcBus = await receiver.ReceiveAsync(x);
            for (var i = 0; i < x; i++)
            {
                
                string msgContent = Encoding.UTF8.GetString(messagesFromSrvcBus[i].Body);

                IStandardMessage strdMsg = JsonConvert.DeserializeObject<IStandardMessage>(msgContent);

                Topic tpc;
                CommentEasyAccess cmtEa;
                await receiver.CloseAsync();
                if (strdMsg.Action == "created")
                {
                    IStandardInstance strdInstance = JsonConvert.DeserializeObject<IStandardInstance>(msgContent);

                    tpc = mapx.ConvertToTopic(strdInstance);

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
                    var response = await client.PostAsync("projects/" + tpc.ProjectId + "/topics", content);
                }
                else if (strdMsg.Action == "updated")
                {
                    IStandardInstance strdInstance = JsonConvert.DeserializeObject<IStandardInstance>(msgContent);

                    tpc = mapx.ConvertToTopic(strdInstance);

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
                    var response = await client.PutAsync("projects/" + tpc.ProjectId + "/topics/" + tpc.Id, content);
                }
                else if (strdMsg.Action == "commented")
                {
                    IstandardComment strdComment = JsonConvert.DeserializeObject<IstandardComment>(msgContent);


                    cmtEa = mapx.ConvertCommentEasyAccess(strdComment);

                    var values = new Dictionary<string, string>
                {
                    {"topicGuid", cmtEa.Id},
                    {"projectId", cmtEa.ProjectId},
                    {"content",cmtEa.Content },
                    {"date", cmtEa.Date},
                };
                    var client = new HttpClient();
                    client.BaseAddress = new Uri("https://topics.quadridcm.com/api/");
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var content = new FormUrlEncodedContent(values);
                    var response = await client.PostAsync("projects/" + cmtEa.ProjectId + "/topics/" + cmtEa.Id + "/comments", content);
                }


            }

         


        }

        static async Task SendMessagesAsync(string msg)
        {

            // Create a new message to send to the queue.

            var message = new Message(Encoding.UTF8.GetBytes(msg));


            // Send the message to the queue.
            await queueClient.SendAsync(message);



        }

    }
}

/*
 *  string dataJsonFormat = "";
            queueClient = new QueueClient(connectionString, QueName);

            root a = Newtonsoft.Json.JsonConvert.DeserializeObject<root>(data.ToString());



            if (a.Issue_event_type_name == issueCreated)
            {
                root dataDeserializeObject = JsonConvert.DeserializeObject<root>(data.ToString());
                IStandardInstance StandarizedIssue = mapx.standarizedIssue(dataDeserializeObject);
                msgs = JsonConvert.SerializeObject(StandarizedIssue);
            }
            else if (a.Issue_event_type_name == issueUpdated)
            {

            }
            else if (a.Issue_event_type_name == issueDeleted)
            {

            }
            else if (a.Issue_event_type_name == issueCommented)
            {

            }
            



            await SendMessagesAsync(msgs);
 *
 *
 *
 */
