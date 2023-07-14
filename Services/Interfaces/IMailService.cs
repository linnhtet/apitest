using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Helpers.Enum;
using WebApi.Models.Messaging;

namespace WebApi.Services.Interfaces
{
    //interface should be in seperate folder to see Folders and File clear and easy to find
    public interface IMailService
    {
        Task<InternalResponse<IEnumerable<MailModel>>> GetAllMailsByFolderAsync(int folder, int userId);
        Task<InternalResponse<IEnumerable<MailModel>>> GetAllMailsByLabelAsync(int label,int userId);
        Task<InternalResponse<PagedMailModel>> GetPagedMailsByFolderIDAsync(int folder,int userId,int currentPage,int rowsPerPage);
        Mail CreateResendMail(Mail originMail, List<MailAttachment> originMailAttachments);
    }

}
