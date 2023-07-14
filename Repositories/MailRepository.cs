using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Helpers.EntitytoModelMapper;
using WebApi.Helpers.Enum;
using WebApi.Models.Messaging;
using WebApi.Repositories.Abstract;
using WebApi.Repositories.Interfaces;

namespace WebApi.Repositories
{
    public class MailRepository :BaseRepository<Mail>,IMailRepository
    {
        readonly DataContext _db;
        readonly ILogger<MailRepository> _logger;

        public MailRepository(DataContext db, ILogger<MailRepository> logger):base(db) 
        {
            _db = db;
            _logger = logger;
        }
        /// <summary>
        /// get paged mails from database by FolderId, we can get paged Send or Received Mails from this method
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="userid"></param>
        /// <param name="currentPage"></param>
        /// <param name="rowsPerPage"></param>
        /// <returns></returns>
        public async Task<IEnumerable<MailModel>> GetPagedMailsByFolderAsync(MailFolder folder, int userId, int currentPage, int rowsPerPage)
        {
            var mails = new List<MailModel>();
            try
            {
                if (folder.Equals(MailFolder.Sent))
                {
                    mails = await _db.Mail.Where(m => m.Folder == (int)folder && m.SendingUserID == userId).Include(m => m.SendingUser).Include(m => m.ReceivingUser)
                                        .Skip((currentPage - 1) * rowsPerPage)
                                        .Take(rowsPerPage)
                                        .Select(m => m.ToMailModel()).ToListAsync();
                }
                else
                {
                    mails = await _db.Mail.Where(m => m.Folder == (int)folder && m.ReceivingUserID == userId).Include(m => m.SendingUser).Include(m => m.ReceivingUser)
                                        .Skip((currentPage - 1) * rowsPerPage)
                                        .Take(rowsPerPage)
                                        .Select(m => m.ToMailModel()).ToListAsync();
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError("GetPagedMailsByFolder Exception : Ex {0} , StackTrace : {1}", ex.Message, ex.StackTrace);
                if (ex.InnerException != null)
                {
                    _logger.LogError("GetPagedMailsByFolder Exception : Ex {0} , StackTrace : {1}", ex.InnerException.Message, ex.InnerException.StackTrace);
                }
                throw;
            }
            
            return mails;
        }

        /// <summary>
        /// Reterieve Mail total count by Mail Folder and logged in userid
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<int> GetMailTotalCountAsync(MailFolder folder, int userId)
        {
            int count = folder.Equals(MailFolder.Sent) ?  await _db.Mail.CountAsync(m => m.Folder == (int)folder && m.SendingUserID == userId) :
                await _db.Mail.CountAsync(m => m.Folder == (int)folder && m.ReceivingUserID == userId);
            return count;
        }

        public async Task<IEnumerable<MailModel>> GetAllMailsByLabelAsync(MailLabels label, int userId)
        {
            try
            {
                
                return await _db.MailModel.FromSqlRaw("[dbo].[GetMailsByLabel] {0},{1}",
                    (int)label,userId).ToListAsync();
            }
            catch (System.Exception ex)
            {
                _logger.LogError("GetAllMailsByLabel Exception : Ex {0} , StackTrace : {1}", ex.Message, ex.StackTrace);
                if (ex.InnerException != null)
                {
                    _logger.LogError("GetAllMailsByLabel Exception : Ex {0} , StackTrace : {1}", ex.InnerException.Message, ex.InnerException.StackTrace);
                }
                throw;
            }
        }
        public async Task<IEnumerable<MailModel>> GetAllMailsByFolderAsync(MailFolder folder, int userId)
        {
            try
            {

                return await _db.MailModel.FromSqlRaw("[dbo].[GetAllMailsByFolder] {0},{1}",
                    (int)folder, userId).ToListAsync();
            }
            catch (System.Exception ex)
            {
                _logger.LogError("GetAllMailsByFolder Exception : Ex {0} , StackTrace : {1}", ex.Message, ex.StackTrace);
                if (ex.InnerException != null)
                {
                    _logger.LogError("GetAllMailsByFolder Exception : Ex {0} , StackTrace : {1}", ex.InnerException.Message, ex.InnerException.StackTrace);
                }
                throw;
            }
        }
        public void AddMailToDB(Mail mail)
        {
            try
            {
                base.AddToDB(mail);
                base.CommitToDB();
            }
            catch (Exception ex)
            {
                _logger.LogError("AddMailToDB Exception : Ex {0} , StackTrace : {1}", ex.Message, ex.StackTrace);
                if (ex.InnerException != null)
                {
                    _logger.LogError("AddMailToDB Exception : Ex {0} , StackTrace : {1}", ex.InnerException.Message, ex.InnerException.StackTrace);
                }
                throw;
            }
        }

       
    }
}
