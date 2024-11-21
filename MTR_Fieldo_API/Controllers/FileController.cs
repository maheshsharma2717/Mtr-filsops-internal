using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MTR_Fieldo_API.Models.Dto;
using MTR_Fieldo_API.Service;
using MTR_Fieldo_API.Service.IService;
using System.Security.Claims;

namespace MTR_Fieldo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly ResponseDto _responseDto;
        private readonly IFileService _fileService;
        private readonly IAuthenticateService _authenticateService;
        public FileController(IFileService fileService, IAuthenticateService authenticateService)
        {
            _fileService = fileService;
            _authenticateService = authenticateService;
            _responseDto = new ResponseDto();
        }
        [HttpPost("uploadfile")]
        [Authorize]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> UploadFile(UploadClientFileDto clientFileDto, int domainId)
        {
            //Claim claim = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email).FirstOrDefault();
            //if (claim != null)
            //{
              //  Fieldo_UserDetails UserDetail = await _authenticateService.GetUserDetailssAsync(claim.Value.ToString());
                //Fieldo_UserDetails UserDetail = await _authenticateService.GetUserDetailsAsync(claim.Value.ToString());
                return await _fileService.FileUpload(clientFileDto);
            //}
            //else
            //{
            //    _responseDto.IsSuccess = false;
            //    _responseDto.Message = "Claims not found";
            //    return _responseDto;
            //}
        }

        [HttpPost("getfileurl")]
        [Authorize]
        [ValidateDomainIdFilter]
        public async Task<ResponseDto> GetFileUrl(string key, int domainId)
        {
            Claim claim = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email).FirstOrDefault();
            if (claim != null)
            {
                Fieldo_UserDetails UserDetail = await _authenticateService.GetUserDetailsAsync(claim.Value.ToString());
                return await _fileService.GetFileUrl(key);
            }
            else
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Claims not found";
                return _responseDto;
            }
        }

    }
}
