using Ajloun_Tour.DTOs2.EmployeeDTOs;
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
            // ✅ Corrected Job Mappings
            CreateMap<Job, JobDTO>()
                .ForMember(dest => dest.MainImage, opt => opt.MapFrom(src =>
                    src.JobImages.OrderBy(img => img.ImageId)  // Ensure consistent order
                        .Select(img => img.ImageUrl)
                        .FirstOrDefault() ?? "/JobsImages/default.jpg"  // Default if no images
                ))
                .ForMember(dest => dest.SubImages, opt => opt.MapFrom(src =>
                    src.JobImages.OrderBy(img => img.ImageId)  // Ensure consistent order
                        .Skip(1)  // Exclude the first image
                        .Select(img => img.ImageUrl)
                        .ToList()
                ));

            CreateMap<CreateJobDTO, Job>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.JobImages, opt => opt.Ignore());

            CreateMap<UpdateJobDTO, Job>()
                .ForMember(dest => dest.JobImages, opt => opt.Ignore());

            // ✅ Fix Job Image Mappings
            CreateMap<JobImage, JobImageDTO>();

            // ✅ Fix Job Application Mappings
            CreateMap<JobApplication, JobApplicationDTO>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Job.Title));

            CreateMap<CreateApplication, JobApplication>()
                .ForMember(dest => dest.ApplicationDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Pending"))
                .ForMember(dest => dest.Cvpath, opt => opt.Ignore());



            CreateMap<Employee, EmployeeDTO>();
            CreateMap<CreateEmployee, Employee>();
        }
    }
}

