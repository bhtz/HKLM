using System;
using AutoMapper;
using Microscope.Application.Core.Queries.Analytic;
using Microscope.Domain.Entities;

namespace Microscope.Application.Common.Mappings
{
    public class AnalyticProfile : Profile
    {
        public AnalyticProfile()
        {
            CreateMap<Analytic, AnalyticQueryResult>().ReverseMap();
        }
    }
}
