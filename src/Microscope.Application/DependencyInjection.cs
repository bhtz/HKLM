﻿using AutoMapper;
using FluentValidation;
using MediatR;
using Microscope.Application.Core;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Microscope.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(typeof(IMicroscopeApplicationCoreModule).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(IMicroscopeApplicationCoreModule).GetTypeInfo().Assembly);
            services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }   
    }
}
