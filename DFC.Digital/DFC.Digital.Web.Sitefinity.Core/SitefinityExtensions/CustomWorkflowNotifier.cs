using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Sitefinity.Security;
using Telerik.Sitefinity.Security.Model;
using Telerik.Sitefinity.Services.Notifications;
using Telerik.Sitefinity.Workflow;
using Telerik.Sitefinity.Workflow.Activities;

namespace DFC.Digital.Web.Sitefinity.Core.SitefinityExtensions
{
    public class CustomWorkflowNotifier : WorkflowNotifier
    {
        private const string RejectionSubject = "Your #itemType was rejected";

        private const string RejectionBody = "#itemTitle that you submitted was rejected by #rejectorName for reason: <strong>#rejectionReason</strong>";

        public override void SendNotification(WorkflowNotificationContext context)
        {
            var item = context;

            // Here we could do some cool stuff like sending SMS or adding dashboard notification.
            base.SendNotification(context);
        }

        protected override void BeforeEmailSend(
                                        WorkflowNotificationContext context,
                                        ref string subjectTemplate,
                                        ref string bodyTemplate,
                                        ref IDictionary<string, string> tokens,
                                        ref IEnumerable<ISubscriberRequest> subscribers)
        {
            // If we want to override the default email that is sent when an item is rejected.
            if (context.ApprovalStatus == ApprovalStatusConstants.Rejected)
            {
                this.ComposeCustomRejectionEmail(context, ref subjectTemplate, ref bodyTemplate, ref tokens);
            }

            // If we want to notify admin@example.com about every workflow event that happens on example.com,
            // then we add them to the subscribers.
            if (context.SiteName == "example.com")
            {
                var admin = new SubscriberRequestProxy
                {
                    FirstName = "Example",
                    LastName = "Administrator",
                    Email = "admin@example.com"
                };

                subscribers = subscribers.Concat(new List<ISubscriberRequest> { admin });
            }
        }

        private void ComposeCustomRejectionEmail(
            WorkflowNotificationContext context,
            ref string subjectTemplate,
            ref string bodyTemplate,
            ref IDictionary<string, string> tokens)
        {
            subjectTemplate = RejectionSubject;
            bodyTemplate = RejectionBody;

            User rejectorUser = UserManager.FindUser(context.LastStateChangerId);

            tokens = new Dictionary<string, string>
            {
                { "#itemType", context.ItemTypeLabel },
                { "#itemTitle", context.ItemTitle },
                { "#rejectorName", rejectorUser == null ? "N/A" : rejectorUser.UserName },
                { "#rejectionReason", context.ApprovalNote }
            };
        }
    }
}