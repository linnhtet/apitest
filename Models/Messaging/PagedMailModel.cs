using System.Collections.Generic;

namespace WebApi.Models.Messaging
{
    public class PagedMailModel
    {
        public IEnumerable<MailModel> results { get; set; }
        public int totalRows { get; set; }
        public int pageNumber { get; set; }
        public int rowsOfPage { get; set; }
    }
}
