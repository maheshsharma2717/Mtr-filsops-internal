using Application.Models;
using AutoMapper;
using MTR_Fieldo_API.Models.Dto;
using MTR_Fieldo_API.ViewModels;

namespace MTR_Fieldo_API
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<TaskViewModel, Fieldo_Task>().ReverseMap();
                config.CreateMap<Fieldo_WorkerBankDetails, BankDetailsRequestDto>().ReverseMap();
                config.CreateMap<Fieldo_WorkerBankDetails, UserBankDetailsViewModel>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
