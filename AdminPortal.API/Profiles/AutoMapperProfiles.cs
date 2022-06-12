using AutoMapper;
using AdminPortal.API.Entities;
using AdminPortal.API.Entities.Dto_s;
using AdminPortal.API.Entities.Dto;
using AdminPortal.API.Profiles.AfterMaps;

namespace AdminPortal.API.Profiles
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Student, StudentDto>()
                .ReverseMap();
            CreateMap<Address, AddressDto>()
                .ReverseMap();
            CreateMap<Gender, GenderDto>()
                .ReverseMap();
            CreateMap<UpdateStudentRequest, Student>()
                .AfterMap<UpdateStudentRequestAfterMap>();

            CreateMap<AddStudentRequest, Student>()
                .AfterMap<AddStudentRequestAfterMap>();

        }
    }
}
