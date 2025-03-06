using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using ExamWeb.AlumniImageService;
using ExamWeb.AlumniService;
using ExamWeb.FacultyService;
using ExamWeb.JobAttachmentService;
using ExamWeb.JobHistoryService;
using ExamWeb.JobPostingService;
using ExamWeb.MajorService;
using ExamWeb.EventService;
using ExamWeb.PhotoAlbumService;
using ExamWeb.PhotoService;
using ExamWeb.Models;
using ExamWeb.UserManagementService;

namespace ExamWeb
{
    public static class Mapping
    {
        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                cfg.AddProfile<ModelMapping>();
            });
            var mapper = config.CreateMapper();
            return mapper;
        });
        public static IMapper Mapper => Lazy.Value;
    }
    public class ModelMapping : Profile
    {
        public ModelMapping()
        {
            CreateMap<FacultyDTO, FacultyModel>().ReverseMap();
            CreateMap<MajorDTO, MajorModel>().ReverseMap();
            CreateMap<AlumniDTO, AlumniModel>().ReverseMap();
            CreateMap<JobHistoryDTO, JobHistoryModel>().ReverseMap();
            CreateMap<AlumniImageDTO, AlumniImageModel>().ReverseMap();
            CreateMap<HobbyDTO, HobbyModel>().ReverseMap();
            CreateMap<JobPostingDTO, JobPostingModel>().ReverseMap();
            CreateMap<JobPostingService.JobAttachmentDTO, JobAttachmentModel>().ReverseMap();
            CreateMap<JobCandidateDTO, JobCandidateModel>().ReverseMap();
            CreateMap<EventDTO, EventModel>().ReverseMap();
            CreateMap<PhotoDTO, PhotoModel>().ReverseMap();
            CreateMap<PhotoAlbumDTO, PhotoAlbumModel>().ReverseMap();
            CreateMap<UserManagementService.AspNetUserDTORoleDTO, AspNetUserModel.RoleModel>().ReverseMap();
            CreateMap<UserManagementService.AspNetUserDTOUserDTO, AspNetUserModel.UserModel>().ReverseMap();
            CreateMap<UserManagementService.AspNetUserDTOPermissionDTO, AspNetUserModel.PermissionModel>().ReverseMap();
            CreateMap<UserManagementService.AspNetUserDTORolePermissionDTO, AspNetUserModel.RolePermissionModel>().ReverseMap();
            CreateMap<UserManagementService.AspNetUserDTOUserRoleDTO, AspNetUserModel.UserRoleModel>().ReverseMap();
            CreateMap<UserManagementService.AspNetUserDTOUserClaimDTO, AspNetUserModel.UserClaimModel>().ReverseMap();
            CreateMap<UserManagementService.AspNetUserDTOUserLoginDTO, AspNetUserModel.UserLoginModel>().ReverseMap();
        }
    }
}