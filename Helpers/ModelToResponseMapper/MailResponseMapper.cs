using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using WebApi.Controllers.V1.Response;
using WebApi.Models.Messaging;

namespace WebApi.Helpers.ModelToResponseMapper
{
    public static class MailResponseMapper
    {
        public static IEnumerable<MailResponse> ToMailResponseList(this IEnumerable<MailModel> models)
        {
            return models.Select(model => new MailResponse
            {
                id = model.Id,
                sendingStaffName = model.SendingStaffName,
                sendingStaffEmail = model.SendingStaffEmail,
                receivingStaffName = model.ReceivingStaffName,
                receivingStaffEmail = model.ReceivingStaffEmail,
                subject = model.Subject,
                message = model.Message,
                sentTime = model.SentTime,
                sentSuccessToSMTPServer = model.SentSuccessToSMTPServer,
                read = model.Read,
                starred = model.Starred,
                important = model.Important,
                hasAttachments = model.HasAttachments,
                label = model.Label,
                folder = model.Folder,
            }).ToList();
        }
    }
}
