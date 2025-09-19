using AutoMapper;
using InvestmentApp.User.Dto;
using InvestmentApp.Investimento.Model;
using InvestmentApp.Investimento.Dto;
using InvestmentApp.Common.Models;
using InvestmentApp.Common.Models.Dto;
using UserModel = InvestmentApp.User.Model.User;

namespace InvestmentApp.Common.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // User mappings
            CreateMap<UserCreateDto, UserModel>();
            CreateMap<UserUpdateDto, UserModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CPF, opt => opt.Ignore())
                .ForMember(dest => dest.Senha, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
            CreateMap<UserModel, UserResponseDto>();

            // Investment mappings
            CreateMap<InvestmentCreateDto, Investment>();
            CreateMap<InvestmentUpdateDto, Investment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
            CreateMap<Investment, InvestmentResponseDto>();

            // UserInvestment mappings
            CreateMap<UserInvestmentCreateDto, UserInvestment>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore());
            CreateMap<UserInvestmentUpdateDto, UserInvestment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.InvestmentId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
            CreateMap<UserInvestment, UserInvestmentResponseDto>();
        }
    }
}
