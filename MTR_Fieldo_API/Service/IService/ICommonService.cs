using Application.Models;
using MTR_Fieldo_API.Models.Dto;

namespace MTR_Fieldo_API.Service.IService
{
    public interface ICommonService
    {
        Task<ResponseDto> UploadFile(UploadClientFileDto uploadClientFileDTO, Fieldo_UserDetails user);
        Task<ResponseDto> UploadFile1(UploadClientFileDto uploadClientFileDTO);
        Task<(string key, bool isSuccess)> UploadFile(UploadClientFileDto uploadClientFileDTO);
        string GeneratePreSignedURL(string awsBucketName, string key, int expireInSeconds);
        (bool Succeeded, string Error) SendEmail(string TemplateName, EmailInformationDto emailInformation, string recieverEmail = "");
    }
}
