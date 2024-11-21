using Application.Models;
using MTR_Fieldo_API.Models.Dto;
using MTR_Fieldo_API.Service.IService;
using Stripe;

namespace MTR_Fieldo_API.Service
{
    public class FileService: IFileService
    {
        private readonly ILogService _logService;
        private readonly ICommonService _commonService; 
        private readonly MtrContext _db;
        private readonly ResponseDto _responseDto;
        private static string bucketName;
        private readonly IConfiguration _configuration;
        public FileService(MtrContext db, ICommonService commonService, IConfiguration configuration, ILogService logService)
        {
            _db = db;
            _responseDto = new ResponseDto();
            _commonService = commonService;
            _configuration = configuration;
            bucketName = (Convert.ToBoolean(_configuration.GetSection("Environment:Staging").Value) ? _configuration.GetSection("AWS:DEVBucketName").Value.ToString() : _configuration.GetSection("AWS:BucketName").Value.ToString());
            _logService = logService;
        }
        public async Task<ResponseDto> FileUpload(UploadClientFileDto clientFileDto)
        {
            try
            {
                //_logService.writeLog("","file service line 29",User.Id);
              var result=   await _commonService.UploadFile1(clientFileDto);
                _responseDto.Result = result.Result;
                _responseDto.IsSuccess= result.IsSuccess;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"{ex.Message}\n {ex.InnerException.Message}";
            }
            return _responseDto;
        }

        public async Task<ResponseDto> GetFileUrl(string key)
        {
            try
            {
                var result =  _commonService.GeneratePreSignedURL(bucketName, key, 7200);
                _responseDto.Result = result ;
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"{ex.Message}\n {ex.InnerException.Message}";
            }
            return _responseDto;
        }
    }
}
