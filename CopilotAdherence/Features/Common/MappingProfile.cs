using AutoMapper;
using CopilotAdherence.Database.Entities;
using CopilotAdherence.Features.Metrics.Common;

namespace CopilotAdherence.Features.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LanguageBreakdown, CopilotLanguageDetail>().ReverseMap();
            CreateMap<DailyStatistics, CopilotDailyStatistic>()
                   .ForMember(dest => dest.DailyBreakdown, opt => opt.MapFrom(src => src.Breakdown)).ReverseMap();
        }
    }
}
