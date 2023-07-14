using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Entities;
using WebApi.Helpers.Enum;
using WebApi.Models.Messaging;

namespace WebApi.Repositories.Interfaces
{
    public interface IMailRepository
    {
        Task<int> GetMailTotalCountAsync(MailFolder folder,int userId);

        Task<IEnumerable<MailModel>> GetAllMailsByLabelAsync(MailLabels label,int userId);
        Task<IEnumerable<MailModel>> GetAllMailsByFolderAsync(MailFolder folder, int userId);
        Task<IEnumerable<MailModel>> GetPagedMailsByFolderAsync(MailFolder folder, int userId, int currentPage,int rowsPerPage);

        void AddMailToDB(Mail mail);
    }
}
