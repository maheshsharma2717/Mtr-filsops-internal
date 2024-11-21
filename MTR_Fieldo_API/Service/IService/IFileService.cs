using Application.Models;
using MTR_Fieldo_API.Models.Dto;

namespace MTR_Fieldo_API.Service.IService
{
    public interface IFileService
    {
        Task<ResponseDto> FileUpload( UploadClientFileDto clientFileDto);
        Task<ResponseDto> GetFileUrl(string key);
    }
}
