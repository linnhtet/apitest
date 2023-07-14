using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Repositories.Abstract;
using WebApi.Repositories.Interfaces;

namespace WebApi.Repositories
{
    public class MailAttachmentRepository : BaseRepository<MailAttachment>, IMailAttachmentRepository
    {
        readonly DataContext _db;
        readonly ILogger<MailRepository> _logger;

        public MailAttachmentRepository(DataContext db, ILogger<MailRepository> logger) : base(db)
        {
            _db = db;
            _logger = logger;
        }

        public void AddMailAttachmentRangeToDB(IEnumerable<MailAttachment> entity)
        {
            try
            {
                base.AddRangeToDB(entity);
                base.CommitToDB();
            }
            catch (Exception ex)
            {
                _logger.LogError("AddMailAttachmentRangeToDB Exception : Ex {0} , StackTrace : {1}", ex.Message, ex.StackTrace);
                if (ex.InnerException != null)
                {
                    _logger.LogError("AddMailAttachmentRangeToDB Exception : Ex {0} , StackTrace : {1}", ex.InnerException.Message, ex.InnerException.StackTrace);
                }
                throw;
            }
        }
    }
}

