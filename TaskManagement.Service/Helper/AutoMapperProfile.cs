using AutoMapper;
using DotnetTask.Model;
using DotnetTask.Model.DTOs;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TaskManagement.Repository.Entities;


namespace Backend.Helper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<TaskEntity, TaskDTO>().ReverseMap();
        }
    }
}