using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JiraWhAzure.Models;
using JiraWhAzure.Stndr;

namespace JiraWhAzure.Stndr
{
    public class mpper
    {


        public IStandardInstance standarizedIssue(root rootIssue)
        {
            IStandardInstance strdMessage = new IStandardInstance();

            strdMessage.MessageOrigin = rootIssue.webhookEvent.Split(':')[0];
            strdMessage.Action = rootIssue.Issue_event_type_name.Split('_')[1];
            strdMessage.Summary = rootIssue.Issue.fields.Summary;
            strdMessage.Created = rootIssue.Issue.fields.created;
            strdMessage.Author =
                rootIssue.Issue.fields.creator.name + "_" + rootIssue.Issue.fields.creator.emailAddress;
            strdMessage.Type = rootIssue.Issue.fields.issueType.name;
            return strdMessage;

        }
        public IstandardComment standarizedComment(root rootComment)
        {
            IstandardComment strdMessage = new IstandardComment();

            strdMessage.MessageOrigin = rootComment.Issue_event_type_name.Split(':')[0];
            strdMessage.Action = rootComment.Issue_event_type_name.Split('_')[1];
            strdMessage.Created = rootComment.Comment.created;
            strdMessage.Author = rootComment.Comment.author.name + "_" + rootComment.Comment.author.emailAddress;
            strdMessage.Content = rootComment.Comment.body;

            return strdMessage;

        }


        public Topic ConvertToTopic(IStandardInstance stdmsg)
        {
            string[] c = stdmsg.Author.Split('_');

            Topic tpc = new Topic();

            tpc.Title = stdmsg.Summary;
            tpc.AuthorName = c[0];
            tpc.AuthorEmail = c[1];
            tpc.Id = "10ae4acd-eb20-4f8c-9ec9-4bca2aacc3ec";
            tpc.ProjectId = "636935113624010709";
            tpc.CreationDate = stdmsg.Created;

            return tpc;
        }

        public CommentEasyAccess ConvertCommentEasyAccess(IstandardComment stdmsg)
        {
            CommentEasyAccess cmtEA = new CommentEasyAccess();
            string[] c = stdmsg.Author.Split('_');

            cmtEA.Id = "b3e7f2d9-67c9-43ff-94d1-c133fa7bb26a";
            cmtEA.ProjectId= "636935113624010709";
            cmtEA.Date = stdmsg.Created;
            cmtEA.authorName= c[0];
            cmtEA.AuthorEmail = c[1];
            cmtEA.Content = stdmsg.Content;

            return cmtEA;

        }

    }
}
