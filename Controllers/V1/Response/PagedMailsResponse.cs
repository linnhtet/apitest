using System.Collections.Generic;
using WebApi.Models.Messaging;

namespace WebApi.Controllers.V1.Response
{
    /// <summary>
    /// Response class for PagedMail base on api version
    /// </summary>
    public class PagedMailsResponse
    {
        public IEnumerable<MailResponse> results { get; set; }
        public int totalRows { get; set; }
        public int pageNumber { get; set; }
        public int rowsOfPage { get; set; }
    }
}
