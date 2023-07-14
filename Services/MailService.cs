using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Entities;
using WebApi.Helpers;

using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Options;
using System.IO;
using System.Net.Mail;
using CsvHelper;
using System.Text;
using System.Globalization;
using System.Net.Mime;
using Microsoft.Extensions.Logging;
using WebApi.Services.Interfaces;
using WebApi.Helpers.Enum;
using WebApi.Models.Messaging;
using WebApi.Repositories.Interfaces;
using System.Net;

namespace WebApi.Services
{
    public class MailService : IMailService, IDisposable
    {
        // Service Layer only handle business logic and data access or entry should handle seperatly in db layer
        // Service layer will retrieve or entry to db via Repository layer
        // using DB object or DB activity in Controller or Service layer will make the code hard to extensible later like if we need to use another db provider
        //by Seprating Repo layer , we can implement DB related logic inside its own layer even if we use another db via Common Interface

        // Flag: Has Dispose already been called?
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        private IMailRepository _mailRepository;
        private IMailAttachmentRepository _attachmentRepository;
        private readonly AppSettings _appSettings;
        private ILogger _log;

        public MailService(ILogger<MailService> log,DataContext context, IOptions<AppSettings> appSettings, IMailRepository mailRepository, IMailAttachmentRepository attachmentRepository)
        {
            _log = log;
            _appSettings = appSettings.Value;
            _mailRepository = mailRepository;
            _attachmentRepository = attachmentRepository;
        }

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                // Free any other managed objects here.
            }

            disposed = true;
        }

        // should also rewirte this , Database Entity should only be used by data read or write purpose
        // it will be better if we implement Mail sending function in Model class, it also has necesssary information to create and send a mail
        public Mail CreateResendMail(Mail originMail, List<MailAttachment> originMailAttachments)
        {
            Mail newMail = new Mail();
            List<MailAttachment> newMailAttachments = new List<MailAttachment>();

            newMail._appSettings = originMail._appSettings;
            newMail._log = originMail._log;

            newMail.OriginMailID = originMail.Id;
            newMail.ReceivingUser = originMail.ReceivingUser;
            newMail.SendingUser = originMail.SendingUser;

            newMail.Label = originMail.Label;
            newMail.SentTime = DateTime.Now;
            newMail.SendingUserID = originMail.SendingUserID;
            newMail.ReceivingUserID = originMail.ReceivingUserID;
            newMail.Subject = originMail.Subject;
            newMail.Message = originMail.Message;
            newMail.HasAttachments = originMail.HasAttachments;


            // using var transaction = _context.Database.BeginTransaction();
            // try
            // { 
            _mailRepository.AddMailToDB(newMail);

            if (newMail.HasAttachments)
            {
                foreach (var a in originMailAttachments)
                {
                    string filePath = a.SavedPath;
                    string fileName = a.Filename;
                    Attachment attachment = new Attachment(filePath);
                    attachment.Name = fileName;
                    newMail.attachments.Add(attachment);

                    MailAttachment mailAttachment = new MailAttachment();
                    mailAttachment.MailID = newMail.Id;
                    mailAttachment.Filename = fileName;
                    mailAttachment.SavedPath = filePath;
                    newMailAttachments.Add(mailAttachment);
                }
            }
            _attachmentRepository.AddMailAttachmentRangeToDB(newMailAttachments);

                // transaction.Commit();
            // }
            // catch (Exception ex)
            // {
            //     transaction.Rollback();
            // }

            return newMail;
        }
        public async Task<InternalResponse<PagedMailModel>> GetPagedMailsByFolderIDAsync(int folder, int userId, int currentPage, int rowsPerPage)
        {
            //validation before doing any db query
            if (!CheckMailFolder((MailFolder)folder))
            {
                return new InternalResponse<PagedMailModel>() 
                { 
                    status = false,
                    statusCode = HttpStatusCode.BadRequest,
                    message = "email folder not found!",
                    Value = null };
            }
            var mailTotalCount = await _mailRepository.GetMailTotalCountAsync((MailFolder)folder, userId);
            var pagedMails = await _mailRepository.GetPagedMailsByFolderAsync((MailFolder)folder,userId,currentPage,rowsPerPage);

            return new InternalResponse<PagedMailModel>() 
            { 
                status = true, 
                statusCode = HttpStatusCode.OK, 
                Value = new PagedMailModel
                {
                    pageNumber = currentPage,
                    rowsOfPage = rowsPerPage,
                    totalRows = mailTotalCount,
                    results = pagedMails
                }
            };
        }
        public async Task<InternalResponse<IEnumerable<MailModel>>> GetAllMailsByLabelAsync(int label, int userId)
        {
            if (!CheckMailLabel((MailLabels)label))
            {
                return new InternalResponse<IEnumerable<MailModel>>()
                {
                    status = false,
                    statusCode = HttpStatusCode.BadRequest,
                    message = "mail label not found!",
                    Value = null
                };

            }
            var mailsByLabel = await _mailRepository.GetAllMailsByLabelAsync((MailLabels)label, userId);
            return new InternalResponse<IEnumerable<MailModel>>()
            {
                status = true,
                statusCode = HttpStatusCode.OK,
                Value = mailsByLabel
            };
        }
        public async Task<InternalResponse<IEnumerable<MailModel>>> GetAllMailsByFolderAsync(int folder, int userId)
        {
            if (!CheckMailFolder((MailFolder)folder))
            {
                return new InternalResponse<IEnumerable<MailModel>>()
                {
                    status = false,
                    statusCode = HttpStatusCode.BadRequest,
                    message = "mail folder not found!",
                    Value = null
                };

            }
            var mailsByLabel = await _mailRepository.GetAllMailsByFolderAsync((MailFolder)folder, userId);
            return new InternalResponse<IEnumerable<MailModel>>()
            {
                status = true,
                statusCode = HttpStatusCode.OK,
                Value = mailsByLabel
            };
        }
        private bool CheckMailLabel(MailLabels label)
        {
            switch (label)
            {
                case MailLabels.ASSET_LOAN: return true;
                case MailLabels.ASSET_VERIFICATION: return true;
                case MailLabels.ASSET_SERVICING: return true;
                case MailLabels.ASSET_LOST_DAMAGED: return true;
                case MailLabels.ASSET_DONATED: return true;
                case MailLabels.ASSET_TRANSFER: return true;
                case MailLabels.OTHERS: return true;
                default: return false;
            }
        }

        private bool CheckMailFolder(MailFolder folder)
        {
            switch (folder)
            {
                case MailFolder.Sent: return true;
                case MailFolder.Receive: return true;
                default: return false;
            }
        }

       
    }
}