using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TN.Setup.Application.Setup.Queries.Municipality;
using TN.Setup.Application.Setup.Queries.MunicipalityById;
using TN.Setup.Application.Setup.Queries.District;
using TN.Setup.Application.Setup.Queries.DistrictById;
using TN.Setup.Application.Setup.Queries.GetDistrictByProvinceId;
using TN.Setup.Application.Setup.Queries.Province;
using TN.Setup.Application.Setup.Queries.ProvinceById;
using TN.Setup.Domain.Entities;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Setup.Application.Setup.Queries.GetMunicipalityByDistrictId;
using TN.Setup.Application.Setup.Command.AddModule;
using TN.Setup.Application.Setup.Queries.GetVdcByDistrictId;
using TN.Setup.Application.Setup.Queries.Vdc;
using TN.Setup.Application.Setup.Queries.VdcById;
using TN.Setup.Application.Setup.Command.AddSubModules;
using TN.Setup.Application.Setup.Command.AddMenu;
using TN.Setup.Application.Setup.Command.UpdateModules;
using TN.Setup.Application.Setup.Command.AddOrganization;
using TN.Setup.Application.Setup.Command.UpdateOrganization;
using TN.Setup.Application.Setup.Queries.Organization;
using TN.Setup.Application.Setup.Queries.OrganizationById;
using TN.Setup.Application.Setup.Queries.GetOrganizationByProvinceId;
using TN.Setup.Application.Setup.Command.AddInstitution;
using TN.Setup.Application.Setup.Command.UpdateInstitution;
using TN.Setup.Application.Setup.Queries.Institution;
using TN.Setup.Application.Setup.Queries.InstitutionById;
using TN.Setup.Application.Setup.Queries.InstitutionByOrganizationId;
using TN.Setup.Application.Setup.Queries.Company;
using TN.Setup.Application.Setup.Command.AddCompany;
using TN.Setup.Application.Setup.Command.UpdateCompany;
using TN.Setup.Application.Setup.Queries.CompanyById;
using TN.Setup.Application.Setup.Queries.CompanyByInstitutionId;
using TN.Setup.Application.Setup.Queries.Modules;
using TN.Setup.Application.Setup.Queries.SubModules;
using TN.Setup.Application.Setup.Queries.Menu;
using TN.Setup.Application.Setup.Command.DeleteModule;

using TN.Setup.Application.Setup.Command.UpdateSubModules;
using TN.Setup.Application.Setup.Command.UpdateMenu;
using TN.Setup.Application.Setup.Command.DeleteSubModule;
using TN.Setup.Application.Setup.Queries.ModulesById;
using TN.Setup.Application.Setup.Queries.GetSubModulesById;
using TN.Setup.Application.Setup.Queries.MenuById;
using TN.Setup.Application.Setup.Command.DeleteOrganization;
using TN.Setup.Application.Setup.Command.DeleteCompany;
using TN.Setup.Application.Setup.Command.DeleteInstitution;
using TN.Authentication.Domain.Entities;
using TN.Authentication.Application.Authentication.Commands.AddPermission;
using TN.Setup.Application.Setup.Command.UpdateBillNumberForPurchase;
using TN.Setup.Application.Setup.Queries.GetCompanyDetailsByCompanyId;


namespace TN.Setup.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Province
            CreateMap<GetAllProvinceResponse, Province>().ReverseMap();
            CreateMap<PagedResult<Province>, PagedResult<GetAllProvinceResponse>>().ReverseMap();
            CreateMap<GetProvinceByIdResponse, Province>().ReverseMap();
            #endregion

            #region District
            CreateMap<GetAllDistrictResponse, District>().ReverseMap();
            CreateMap<PagedResult<District>, PagedResult<GetAllDistrictResponse>>().ReverseMap();
            CreateMap<GetDistrictByIdResponse, District>().ReverseMap();
            CreateMap<GetDistrictByProvinceIdResponse, District>().ReverseMap();
            #endregion

            #region Municipality
            CreateMap<GetAllMunicipalityResponse, Municipality>().ReverseMap();
            CreateMap<PagedResult<Municipality>, PagedResult<GetAllMunicipalityResponse>>().ReverseMap();
            CreateMap<GetMunicipalityByIdResponse, Municipality>().ReverseMap();
            CreateMap<PagedResult<Municipality>, PagedResult<GetMunicipalityByIdResponse>>().ReverseMap();
            CreateMap<GetMunicipalityByDistrictIdResponse,  Municipality>().ReverseMap();
            #endregion

            #region VDC
            CreateMap<GetVdcByDistrictIdResponse, Vdc>().ReverseMap();
            CreateMap<GetVdcByIdResponse, Vdc>().ReverseMap();
            CreateMap<PagedResult<Vdc>, PagedResult<GetVdcByIdResponse>>().ReverseMap();
            CreateMap<GetAllVdcResponse,Vdc>().ReverseMap();
            CreateMap<PagedResult<Vdc>, PagedResult<GetAllVdcResponse>>().ReverseMap();
            #endregion

            #region Modules

            CreateMap<GetAllModulesResponse, Modules>().ReverseMap();
            CreateMap<PagedResult<Modules>, PagedResult<GetAllModulesResponse>>().ReverseMap();
            CreateMap<AddModuleResponse, Modules>().ReverseMap();
            CreateMap<AddModuleResponse, AddModuleCommand>().ReverseMap();
            CreateMap<Modules, UpdateModulesCommand>().ReverseMap();
            CreateMap<Modules, DeleteModuleCommand>().ReverseMap();
            CreateMap<GetModulesByIdResponse, Modules>().ReverseMap();
            #endregion

            #region SubModules
            CreateMap<GetAllSubModulesResponse, SubModules>().ReverseMap();
            CreateMap<PagedResult<SubModules>, PagedResult<GetAllSubModulesResponse>>().ReverseMap();
            CreateMap<AddSubmodulesResponse,SubModules>().ReverseMap();
            CreateMap<AddSubModulesCommand, AddSubmodulesResponse>().ReverseMap();
            CreateMap<SubModules,UpdateSubModulesCommand>().ReverseMap();
            CreateMap<SubModules, DeleteSubModuleCommand>().ReverseMap();
            CreateMap<GetSubModulesByIdResponse, SubModules>().ReverseMap();
           

            #endregion

            #region Menu
            CreateMap<GetAllMenuResponse, Menu>().ReverseMap();
            CreateMap<PagedResult<Menu>, PagedResult<GetAllMenuResponse>>().ReverseMap();
            CreateMap<AddMenuResponse, Menu>().ReverseMap();
            CreateMap<AddMenuCommand, AddMenuResponse>().ReverseMap();
            CreateMap<Menu, UpdateMenuCommand>().ReverseMap();
            CreateMap<GetMenuByIdResponse, Menu>().ReverseMap();
            CreateMap<Menu,DeleteModuleCommand>().ReverseMap();

            #endregion

            #region Organization
            CreateMap<AddOrganizationResponse, Organization>().ReverseMap();
            CreateMap<AddOrganizationResponse, AddOrganizationCommand>().ReverseMap();
            CreateMap<Organization, UpdateOrganizationCommand>().ReverseMap();
            CreateMap<GetAllOrganizationResponse, Organization>().ReverseMap();
            CreateMap<PagedResult<Organization>, PagedResult<GetAllOrganizationResponse>>().ReverseMap();
            CreateMap<GetOrganizationByIdQueryResponse, Organization>().ReverseMap();
            CreateMap<GetOrganizationByProvinceIdResponse, Organization>().ReverseMap();
            CreateMap<DeleteOrganizationCommand,Organization>().ReverseMap();
            #endregion

            #region Institution
            CreateMap<AddInstitutionResponse, Institution>().ReverseMap();
            CreateMap<AddInstitutionResponse, AddInstitutionCommand>().ReverseMap();
            CreateMap<Institution, UpdateInstitutionCommand>().ReverseMap();
            CreateMap<GetAllInstitutionResponse, Institution>().ReverseMap();
            CreateMap<PagedResult<Institution>, PagedResult<GetAllInstitutionResponse>>().ReverseMap();
            CreateMap<GetInstitutionByIdResponse, Institution>().ReverseMap();
            CreateMap<GetInstitutionByOrganizationIdResponse, Institution>().ReverseMap();
            CreateMap<DeleteInstitutionCommand, Institution>().ReverseMap();
            #endregion

            #region Comapny
            CreateMap<GetAllCompanyQueryResponse, Company>().ReverseMap();
            CreateMap<PagedResult<Company>, PagedResult<GetAllCompanyQueryResponse>>().ReverseMap();
 
            CreateMap<AddCompanyResponse, Company>().ReverseMap();
            CreateMap<AddCompanyResponse, AddCompanyCommand>().ReverseMap();
            CreateMap<Company, UpdateCompanyCommand>().ReverseMap();
            CreateMap<GetCompanyByIdResponse, Company>().ReverseMap();
            CreateMap<GetCompanyByInstitutionIdResponse, Company>().ReverseMap();
            CreateMap<DeleteCompanyCommand, Company>().ReverseMap();
            CreateMap<UpdateBillNumberStatusForPurchaseCommand, Company>().ReverseMap();
            CreateMap<GetCompanyDetailsByCompanyIdQueryResponse, Company>().ReverseMap();


            #endregion

            #region Permission
            CreateMap<AddPermissionResponse, Permission>().ReverseMap();
            CreateMap<AddPermissionCommand, AddPermissionResponse>().ReverseMap();
            CreateMap<AddPermissionRequest, AddPermissionResponse>().ReverseMap();
            #endregion
        }
    }
}
