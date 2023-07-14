using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace WebApi.Controllers.V1.Response
{
    public class MailResponse
    {
        public Guid id { get; set; }

        public string sendingStaffName { get; set; }
        public string sendingStaffEmail { get; set; }
        public string receivingStaffName { get; set; }
        public string receivingStaffEmail { get; set; }

        [Column(TypeName = "text")]
        public string subject { get; set; }
        [Column(TypeName = "text")]
        public string message { get; set; }

        [DisplayFormat(DataFormatString = "{yyyy-MM-dd H:mm:ss}",
        ApplyFormatInEditMode = true)]
        public DateTime sentTime { get; set; }

        public bool sentSuccessToSMTPServer { get; set; }  

        public bool read { get; set; }
        public bool starred { get; set; }
        public bool important { get; set; }
        public bool hasAttachments { get; set; }
        public int label { get; set; }
        public int folder { get; set; }
    }
}
