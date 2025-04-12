using AutoMapper;
using IssueManager.Application.DTOs;
using IssueManager.Domain.Entities.GitHub;
using IssueManager.Domain.Entities.GitLab;

namespace IssueManager.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GitHubIssue, IssueDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Number))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Body, opt => opt.MapFrom(src => src.Body))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State));

            CreateMap<GitLabIssue, IssueDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Iid))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Body, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State));
        }
    }
}
