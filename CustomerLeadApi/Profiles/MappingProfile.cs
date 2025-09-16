using AutoMapper;
using CustomerLeadApi.Models;
using CustomerLeadApi.DTOs;

namespace CustomerLeadApi.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerDto>();
            CreateMap<CreateCustomerDto, Customer>();
            CreateMap<CustomerImage, CustomerImageDto>();
            CreateMap<UploadImageDto, CustomerImage>();
        }
    }
}