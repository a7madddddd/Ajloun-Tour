using Ajloun_Tour.DTOs2.JobApplicationDTOs;
using Ajloun_Tour.DTOs2.JobDTOs;
using Ajloun_Tour.Models;
using AutoMapper;

namespace Ajloun_Tour
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Job Mappings
            CreateMap<Job, JobDTO>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.JobImages));

            CreateMap<CreateJobDTO, Job>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.JobImages, opt => opt.Ignore());

            CreateMap<UpdateJobDTO, Job>()
                .ForMember(dest => dest.JobImages, opt => opt.Ignore());

            // Job Image Mappings
            CreateMap<JobImage, JobImageDTO>();

            // Job Application Mappings
            CreateMap<JobApplication, JobApplicationDTO>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Job.Title));

            CreateMap<CreateApplication, JobApplication>()
                .ForMember(dest => dest.ApplicationDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Pending"))
                .ForMember(dest => dest.Cvpath, opt => opt.Ignore());
        }
    }
}

