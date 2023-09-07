﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TestUp.DataAccess.IRepositories;
using TestUp.DataAccess.Repositories;
using TestUp.DataAccess.Repository;
using TestUp.Service.Interfaces;
using TestUp.Service.Mappers;
using TestUp.Service.Services;

namespace TestUp.WebApi.Extensions;

public static class ServiceCollections
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IExamService, ExamService>();
        services.AddScoped<IUserService/*, UserService*/>();
        services.AddScoped<IAnswerService, AnswerService>();
        services.AddScoped<IQuestionService, QuestionService>();
        services.AddScoped<IResultService/*, ResultService*/>();
        services.AddScoped<IUserAnswerService, UserAnswerService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IAttachmentService, AttachmentService>();
        services.AddScoped<IQuestionPackService, QuestionPackService>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddAutoMapper(typeof(MappingProfile));
    }

    public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            var key = Encoding.UTF8.GetBytes(configuration["JWT:Key"]);
            o.SaveToken = true;
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["JWT:Issuer"],
                ValidAudience = configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });
    }
}