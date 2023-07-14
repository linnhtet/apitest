using WebApi.Entities;
using WebApi.Models.Messaging;

namespace WebApi.Helpers.EntitytoModelMapper
{
    /// <summary>
    /// Mapper class to map Mail Entity to Mail Model class
    /// </summary>
    public static class MailMapper
    {
        //using customer mapper instead of IMapper for simple mapping
        public static MailModel ToMailModel(this Mail entity)
        {
            return new MailModel
            {
                Id = entity.Id,
                SendingStaffName = entity.SendingUser.StaffName,
                SendingStaffEmail = entity.SendingUser.StaffEmail,
                ReceivingStaffName = entity.ReceivingUser.StaffName,
                ReceivingStaffEmail = entity.ReceivingUser.StaffEmail,
                Subject = entity.Subject,
                Message = entity.Message,
                SentTime = entity.SentTime,
                SentSuccessToSMTPServer = entity.SentSuccessToSMTPServer,
                Read = entity.Read,
                Starred = entity.Starred,
                Important = entity.Important,
                HasAttachments = entity.HasAttachments,
                Label = entity.Label,
                Folder = entity.Folder,
            };
        }
    }
}
