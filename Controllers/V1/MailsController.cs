using Autofac.Core;
using Autofac.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Win32.SafeHandles;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using WebApi.Controllers.V1.Response;
using WebApi.Helpers;
using WebApi.Helpers.ModelToResponseMapper;
using WebApi.Services;
using WebApi.Services.Interfaces;

namespace WebApi.Controllers.V1
{
    // 
    [Authorize]
    [ApiController]
    [Route("v1/[controller]")]
    public class MailsController : BaseController,IDisposable
    {
        // Flag: Has Dispose already been called?
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        private readonly IMailService _mailService;
        private readonly AppSettings _appSettings;
        private readonly ILogger<MailsController> _logger;
        readonly Disposable _disposable;
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
                //
            }

            disposed = true;
        }
        public MailsController(ILogger<MailsController> logger, IOptions<AppSettings> appSettings, IMailService mailService)
        {
            _logger = logger;
            _appSettings = appSettings.Value;
            _mailService = mailService;
            _disposable = new Disposable();
        }
        [HttpGet("folder/{paramFolderId}")]
        public async Task<IActionResult> GetFolderAsync(int paramFolderId)
        {
            HttpContext.Response.RegisterForDispose(_disposable);
            var token = Request.Headers["Authorization"];
            try
            {
                var result = await _mailService.GetAllMailsByFolderAsync(paramFolderId, TokenInfo.UserID);
                if (!result.status && result.statusCode == HttpStatusCode.BadRequest)
                {
                    return BadRequest(new ErrorResponse { errormessage = result.message });
                }
                return Ok(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured : Ex : {0} , Stack : {1}", ex.Message, ex.StackTrace);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Innder Exception : Ex : {0} , Stack : {1}", ex.InnerException.Message, ex.InnerException.StackTrace);
                }
                return BadRequest(new ErrorResponse { errormessage = ex.Message, errorstack = ex.StackTrace });
            }
        }
        [HttpGet("label/{paramLabelId}")]
        public async Task<IActionResult> GetLabelAsync(int paramLabelId)
        {
            HttpContext.Response.RegisterForDispose(_disposable);
            var token = Request.Headers["Authorization"];
            try
            {
                var result = await _mailService.GetAllMailsByLabelAsync(paramLabelId, TokenInfo.UserID);
                if (!result.status && result.statusCode == HttpStatusCode.BadRequest)
                {
                    return BadRequest(new ErrorResponse { errormessage = result.message });
                }
                return Ok(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured : Ex : {0} , Stack : {1}", ex.Message, ex.StackTrace);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Innder Exception : Ex : {0} , Stack : {1}", ex.InnerException.Message, ex.InnerException.StackTrace);
                }
                return BadRequest(new ErrorResponse { errormessage = ex.Message, errorstack = ex.StackTrace });
            }
        }
        [HttpGet("folderData/{paramFolderId}/{pageNumber}/{rowsOfPage}")]
        public async Task<IActionResult> GetSentFolderAsync(int paramFolderId, int pageNumber, int rowsOfPage)
        {
            HttpContext.Response.RegisterForDispose(_disposable);
            try
            {
                var result = await _mailService.GetPagedMailsByFolderIDAsync(paramFolderId, TokenInfo.UserID, pageNumber, rowsOfPage);
                if (!result.status && result.statusCode == HttpStatusCode.BadRequest)
                {
                    return BadRequest(new ErrorResponse { errormessage = result.message });
                }
                var response = new PagedMailsResponse
                {
                    pageNumber = result.Value.pageNumber,
                    rowsOfPage = result.Value.rowsOfPage,
                    totalRows = result.Value.totalRows,
                    results = result.Value.results.ToMailResponseList()

                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured : Ex : {0} , Stack : {1}", ex.Message, ex.StackTrace);
                if (ex.InnerException != null)
                {
                    _logger.LogError("Innder Exception : Ex : {0} , Stack : {1}", ex.InnerException.Message, ex.InnerException.StackTrace);
                }
                return BadRequest(new ErrorResponse { errormessage = ex.Message, errorstack = ex.StackTrace });
            }
        }
        [HttpPost("m/{paramMailId}/resend")]
        public IActionResult ResendMail(String paramMailId)
        {
            HttpContext.Response.RegisterForDispose(_disposable);
            var userId = UserService.GetUserIdFromToken(Request.Headers["Authorization"], _appSettings.Secret);
            return Ok();
        }
    }
}
