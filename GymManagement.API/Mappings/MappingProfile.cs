using AutoMapper;
using GymManagement.API.DTOs.Request;
using GymManagement.API.DTOs.Response;
using GymManagement.Domain.Entities;

namespace GymManagement.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Member mappings
            CreateMap<MemberRequestDTO, Member>();
            CreateMap<Member, MemberResponseDTO>();

            // Trainer mappings
            CreateMap<TrainerRequestDTO, Trainer>();
            CreateMap<Trainer, TrainerResponseDTO>();

            // GymClass mappings
            CreateMap<GymClassRequestDTO, GymClass>();
            CreateMap<GymClass, GymClassResponseDTO>()
                .ForMember(dest => dest.TrainerFullName,
                    opt => opt.MapFrom(src =>
                        src.Trainer.FirstName + " " + src.Trainer.LastName))
                .ForMember(dest => dest.EnrolledCount,
                    opt => opt.MapFrom(src =>
                        src.Enrollments != null ? src.Enrollments.Count : 0));

            // Membership mappings
            CreateMap<MembershipRequestDTO, Membership>();
            CreateMap<Membership, MembershipResponseDTO>()
                .ForMember(dest => dest.MemberFullName,
                    opt => opt.MapFrom(src =>
                        src.Member.FirstName + " " + src.Member.LastName));

            // Enrollment mappings
            CreateMap<EnrollmentRequestDTO, Enrollment>();
            CreateMap<Enrollment, EnrollmentResponseDTO>()
                .ForMember(dest => dest.MemberFullName,
                    opt => opt.MapFrom(src =>
                        src.Member.FirstName + " " + src.Member.LastName))
                .ForMember(dest => dest.GymClassName,
                    opt => opt.MapFrom(src => src.GymClass.Name))
                .ForMember(dest => dest.TrainerFullName,
                    opt => opt.MapFrom(src =>
                        src.GymClass.Trainer.FirstName + " " + src.GymClass.Trainer.LastName));
        }
    }
}