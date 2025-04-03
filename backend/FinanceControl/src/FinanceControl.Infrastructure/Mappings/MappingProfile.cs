using AutoMapper;
using FinanceControl.Application.DTOs;
using FinanceControl.Domain.Entities;

namespace FinanceControl.Infrastructure.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<PaymentMethod, PaymentMethodDto>().ReverseMap();
            CreateMap<Transaction, TransactionDto>().ReverseMap();

            CreateMap<TransactionDto, Transaction>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Category.Id))
                .ForMember(dest => dest.PaymentMethodId, opt => opt.MapFrom(src => src.PaymentMethod.Id)) 
                .ForMember(dest => dest.Category, opt => opt.Ignore()) 
                .ForMember(dest => dest.PaymentMethod, opt => opt.Ignore()); 
        }
    }
}