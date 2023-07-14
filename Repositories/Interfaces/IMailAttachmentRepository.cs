using System.Collections.Generic;
using WebApi.Entities;

namespace WebApi.Repositories.Interfaces
{
    public interface IMailAttachmentRepository
    {
        void AddMailAttachmentRangeToDB(IEnumerable<MailAttachment> entity);
        void CommitToDB();
    }
}
