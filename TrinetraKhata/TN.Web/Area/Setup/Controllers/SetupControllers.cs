using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TN.Authentication.Domain.Entities;
using TN.Setup.Application.Setup.Command.AddCompany;
using TN.Setup.Application.Setup.Command.AddCompany.RequestCommandMapper;
using TN.Setup.Application.Setup.Command.AddInstitution;
using TN.Setup.Application.Setup.Command.AddInstitution.RequestCommandMapper;
using TN.Setup.Application.Setup.Command.AddMenu.RequestCommandMapper;
using TN.Setup.Application.Setup.Command.AddModule.RequestCommandMapper;
using TN.Setup.Application.Setup.Command.AddOrganization;
using TN.Setup.Application.Setup.Command.AddOrganization.RequestCommandMapper;
using TN.Setup.Application.Setup.Command.AddSubModules.RequestCommandMapper;
using TN.Setup.Application.Setup.Command.AssignModulesToRole.RequestCommandMapper;
using TN.Setup.Application.Setup.Command.DeleteCompany;
using TN.Setup.Application.Setup.Command.DeleteInstitution;
using TN.Setup.Application.Setup.Command.DeleteOrganization;
using TN.Setup.Application.Setup.Command.UpdateBillNumberForPurchase;
using TN.Setup.Application.Setup.Command.UpdateBillNumberForPurchase.RequestCommandMapper;
using TN.Setup.Application.Setup.Command.UpdateCompany;
using TN.Setup.Application.Setup.Command.UpdateCompany.RequestCommandMapper;
using TN.Setup.Application.Setup.Command.UpdateInstitution;
using TN.Setup.Application.Setup.Command.UpdateInstitution.RequestCommandMapper;
using TN.Setup.Application.Setup.Command.UpdateMenu.RequestCommandMapper;
using TN.Setup.Application.Setup.Command.UpdateModules.RequestCommandMapper;
using TN.Setup.Application.Setup.Command.UpdateOrganization;
using TN.Setup.Application.Setup.Command.UpdateOrganization.RequestCommandMapper;
using TN.Setup.Application.Setup.Command.UpdateSubModules.RequestCommandMapper;
using TN.Setup.Application.Setup.Queries.Company;
using TN.Setup.Application.Setup.Queries.CompanyById;
using TN.Setup.Application.Setup.Queries.CompanyByInstitutionId;
using TN.Setup.Application.Setup.Queries.District;
using TN.Setup.Application.Setup.Queries.DistrictById;
using TN.Setup.Application.Setup.Queries.FilterCompanyByDate;
using TN.Setup.Application.Setup.Queries.GetCompanyDetailsByCompanyId;
using TN.Setup.Application.Setup.Queries.GetDistrictByProvinceId;
using TN.Setup.Application.Setup.Queries.GetMunicipalityByDistrictId;
using TN.Setup.Application.Setup.Queries.GetOrganizationByProvinceId;
using TN.Setup.Application.Setup.Queries.GetVdcByDistrictId;
using TN.Setup.Application.Setup.Queries.Institution;
using TN.Setup.Application.Setup.Queries.InstitutionById;
using TN.Setup.Application.Setup.Queries.InstitutionByOrganizationId;
using TN.Setup.Application.Setup.Queries.Municipality;
using TN.Setup.Application.Setup.Queries.MunicipalityById;
using TN.Setup.Application.Setup.Queries.Organization;
using TN.Setup.Application.Setup.Queries.OrganizationById;
using TN.Setup.Application.Setup.Queries.Province;
using TN.Setup.Application.Setup.Queries.ProvinceById;
using TN.Setup.Application.Setup.Queries.Vdc;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Web.Controllers;

namespace TN.Web.Area.Setup.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    public class SetupControllers : BaseController
    {
        private readonly IMediator _mediator;
        public SetupControllers(IMediator mediator, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : base(userManager, roleManager)
        {
            _mediator = mediator;
           
        }

        #region Province
        #region AllProvince
        [HttpGet("all-province")]
        public async Task<IActionResult> AllProvince([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetAllProvinceQuery(paginationRequest);
            var provinceResult = await _mediator.Send(query);
            #region Switch Statement
            return provinceResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(provinceResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = provinceResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(provinceResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion
        #region ProvinceById
        [HttpGet("Province/{ProvinceId}")]
        public async Task<IActionResult> GetByProvinceId([FromRoute] int ProvinceId)
        {
            var query = new GetProvinceByIdQuery(ProvinceId);
            var provinceResult = await _mediator.Send(query);
            #region Switch Statement
            return provinceResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(provinceResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = provinceResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(provinceResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion
        #endregion

        #region District
        #region AllDistrict
        [HttpGet("all-district")]
        public async Task<IActionResult> AllDistrict([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetAllDistrictQuery(paginationRequest);
            var districtResult = await _mediator.Send(query);
            #region Switch Statement
            return districtResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(districtResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = districtResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(districtResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };

            #endregion
        }

        #endregion
        #region DistrictById
        [HttpGet("{DistrictId}")]
        public async Task<IActionResult> GetByDistrictId([FromRoute] int DistrictId)
        {
            var query = new GetDistrictByIdQuery(DistrictId);
            var districtResult = await _mediator.Send(query);
            #region Switch Statement
            return districtResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(districtResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = districtResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(districtResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion
        #region DistrictByProvinceId
        [HttpGet("District/{ProvinceId}")]
        public async Task<IActionResult> GetDistrictByProvinceId([FromRoute] int ProvinceId)
        {
            var query = new GetDistrictByProvinceIdQuery(ProvinceId);
            var districtResult = await _mediator.Send(query);
            #region Switch Statement
            return districtResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(districtResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = districtResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(districtResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion
        #endregion

        #region Municipality
        #region AllMunicipality
        [HttpGet("all-municipality")]
        public async Task<IActionResult> AllMunicipality([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetAllMunicipalityQuery(paginationRequest);
            var municipalityResult = await _mediator.Send(query);
            #region Switch Statement
            return municipalityResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(municipalityResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = municipalityResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(municipalityResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };

            #endregion
        }

        #endregion
        #region MunicipalityById
        [HttpGet("Municipality/{MunicipalityId}")]
        public async Task<IActionResult> GetByMunicipalityId([FromRoute] int MunicipalityId)
        {
            var query = new GetMunicipalityByIdQuery(MunicipalityId);
            var municipalityResult = await _mediator.Send(query);
            #region Switch Statement
            return municipalityResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(municipalityResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = municipalityResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(municipalityResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion
        #region MunicipalityByDistrictId
        [HttpGet("GetMunicipality/{DistrictId}")]
        public async Task<IActionResult> GetMunicipalityByDistrictId([FromRoute] int DistrictId)
        {
            var query = new GetMunicipalityByDistrictIdQuery(DistrictId);
            var municipalityResult = await _mediator.Send(query);
            #region Switch Statement
            return municipalityResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(municipalityResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = municipalityResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(municipalityResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion
        #endregion

        #region VDC
        #region AllVdc
        [HttpGet("all-Vdc")]
        public async Task<IActionResult> AllVdc([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetAllVdcQuery(paginationRequest);
            var vdcResult = await _mediator.Send(query);
            #region Switch Statement
            return vdcResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(vdcResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = vdcResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(vdcResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion
        }
        #endregion

        #region VdcByDistrictId
        [HttpGet("GetVdc/{DistrictId}")]
        public async Task<IActionResult> GetVdcByDistrictId([FromRoute] int DistrictId) 
        {
            var query=new GetVdcByDistrictIdQuery(DistrictId);
            var vdcResult = await _mediator.Send(query);
            #region Switch Statement
            return vdcResult switch
            {

                { IsSuccess: true, Data: not null } => new JsonResult(vdcResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = vdcResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(vdcResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion
        }

        #endregion
        #endregion

        #region Organization

        #region AddOrganization
        [HttpPost("AddOrganization")]
        public async Task<IActionResult> AddOrganization([FromBody] AddOrganizationRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var addOrganizationResult = await _mediator.Send(command);
            #region Switch Statement
            return addOrganizationResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddOrganization), addOrganizationResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addOrganizationResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addOrganizationResult.Errors),
                _ => BadRequest("Invalid Fields for Add organization")
            };

            #endregion
        }
        #endregion

        #region UpdateOrganization
        [HttpPatch("UpdateOrganization/{Id}")]

        public async Task<IActionResult> UpdateOrganization([FromRoute] string Id, [FromBody] UpdateOrganizationRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(Id);
            var updateOrganizationResult = await _mediator.Send(command);
            #region Switch Statement
            return updateOrganizationResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateOrganizationResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateOrganizationResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateOrganizationResult.Errors),
                _ => BadRequest("Invalid Fields for Update Organization")
            };

            #endregion


        }
        #endregion

        #region AllOrganization
        [HttpGet("all-organization")]
        public async Task<IActionResult> AllOrganization([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetAllOrganizationQuery(paginationRequest);
            var organizationResult = await _mediator.Send(query);
            #region Switch Statement
            return organizationResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(organizationResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = organizationResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(organizationResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };

            #endregion
        }

        #endregion

        #region OrganizationById
        [HttpGet("Organization/{OrganizationId}")]
        public async Task<IActionResult> GetByOrganizationId([FromRoute] string OrganizationId)
        {
            var query = new GetOrganizationByIdQuery(OrganizationId);
            var organizationResult = await _mediator.Send(query);
            #region Switch Statement
            return organizationResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(organizationResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = organizationResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(organizationResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region OrganizationByProvinceId
        [HttpGet("GetOrganization/{ProvinceId}")]
        public async Task<IActionResult> GetOrganizationByProvinceId([FromRoute] int ProvinceId)
        {
            var query = new GetOrganizationByProvinceIdQuery(ProvinceId);
            var organizationResult = await _mediator.Send(query);
            #region Switch Statement
            return organizationResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(organizationResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = organizationResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(organizationResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region DeleteOrganization
        [HttpDelete("DeleteOrganization/{Id}")]

        public async Task<IActionResult> DeleteOrganization([FromRoute] string Id, CancellationToken cancellationToken)
        {
            var command = new DeleteOrganizationCommand(Id);
            var deleteOrganizationResult = await _mediator.Send(command);
            #region Switch Statement
            return deleteOrganizationResult switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { Message = deleteOrganizationResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteOrganizationResult.Errors),
                _ => BadRequest("Invalid Fields for Add Organization")
            };

            #endregion
        }
        #endregion
        #endregion

        #region Institution

        #region AddInstitution
        [HttpPost("AddInstitution")]
        public async Task<IActionResult> AddInstitution([FromBody] AddInstitutionRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var addInstitutionResult = await _mediator.Send(command);
            #region Switch Statement
            return addInstitutionResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddInstitution), addInstitutionResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addInstitutionResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addInstitutionResult.Errors),
                _ => BadRequest("Invalid Fields for Add Institution")
            };

            #endregion
        }
        #endregion

        #region UpdateInstitution
        [HttpPatch("UpdateInstitution/{Id}")]

        public async Task<IActionResult> UpdateInstitution([FromRoute] string Id, [FromBody] UpdateInstitutionRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(Id);
            var updateInstitutionResult = await _mediator.Send(command);
            #region Switch Statement
            return updateInstitutionResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateInstitutionResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateInstitutionResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateInstitutionResult.Errors),
                _ => BadRequest("Invalid Fields for Update Institution")
            };

            #endregion


        }
        #endregion

        #region AllInstitution
        [HttpGet("all-institution")]
        public async Task<IActionResult> AllInstitution([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetAllInstitutionQuery(paginationRequest);
            var institutionResult = await _mediator.Send(query);
            #region Switch Statement
            return institutionResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(institutionResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = institutionResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(institutionResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region InstitutionById
        [HttpGet("Institution/{InstitutionId}")]
        public async Task<IActionResult> GetByInstiutionId([FromRoute] string InstitutionId)
        {
            var query = new GetInstitutionByIdQuery(InstitutionId);
            var instiutionResult = await _mediator.Send(query);
            #region Switch Statement
            return instiutionResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(instiutionResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = instiutionResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(instiutionResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region InstitutionByOrganizationId
        [HttpGet("GetInstitution/{OrganizationId}")]
        public async Task<IActionResult> GetInstitutionByOrganizationId([FromRoute] string OrganizationId)
        {
            var query = new GetInstitutionByOrganizationIdQuery(OrganizationId);
            var institutionResult = await _mediator.Send(query);
            #region Switch Statement
            return institutionResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(institutionResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = institutionResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(institutionResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region DeleteInstitution
        [HttpDelete("DeleteInstitution/{Id}")]

        public async Task<IActionResult> DeleteInstitution([FromRoute] string Id, CancellationToken cancellationToken)
        {
            var command = new DeleteInstitutionCommand(Id);
            var deleteInstitutionResult = await _mediator.Send(command);
            #region Switch Statement
            return deleteInstitutionResult switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { Message = deleteInstitutionResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteInstitutionResult.Errors),
                _ => BadRequest("Invalid Fields for Add Insitution")
            };

            #endregion
        }
        #endregion
        #endregion 

        #region Company
        #region AllCompany
        [HttpGet("all-company")]
        public async Task<IActionResult> AllCompany([FromQuery] PaginationRequest paginationRequest)
        {
            var query = new GetAllCompanyQuery(paginationRequest);
            var companyResult = await _mediator.Send(query);
            #region Switch Statement
            return companyResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(companyResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = companyResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(companyResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }

        #endregion

        #region AddCompany
        [HttpPost("AddCompany")]

        public async Task<IActionResult> AddCompany([FromBody] AddCompanyRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand();
            var addCompanyResult = await _mediator.Send(command);
            #region Switch Statement
            return addCompanyResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddCompany), addCompanyResult.Data),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = addCompanyResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addCompanyResult.Errors),
                _ => BadRequest("Invalid Fields for Add Company")
            };

            #endregion
        }
        #endregion

        #region UpdateCompany
        [HttpPatch("UpdateCompany/{Id}")]

        public async Task<IActionResult> UpdateCompany([FromRoute] string Id, [FromBody] UpdateCompanyRequest request)
        {
            //Mapping command and request
            var command = request.ToCommand(Id);
            var updateCompanyResult = await _mediator.Send(command);
            #region Switch Statement
            return updateCompanyResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateCompanyResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateCompanyResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateCompanyResult.Errors),
                _ => BadRequest("Invalid Fields for Update Company")
            };

            #endregion


        }
        #endregion

        #region CompanyById
        [HttpGet("Company/{CompanyId}")]
        public async Task<IActionResult> GetByCompanyId([FromRoute] string CompanyId)
        {
            var query = new GetCompanyByIdQuery(CompanyId);
            var companyResult = await _mediator.Send(query);
            #region Switch Statement
            return companyResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(companyResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = companyResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(companyResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region CompanyByInstitutionId
        [HttpGet("GetCompany/{InstitutionId}")]
        public async Task<IActionResult> GetCompanyByInstitutionId([FromRoute] string InstitutionId)
        {
            var query = new GetCompanyByInstitutionIdQuery(InstitutionId);
            var companyResult = await _mediator.Send(query);
            #region Switch Statement
            return companyResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(companyResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = companyResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(companyResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #region DeleteCompany
        [HttpDelete("DeleteCompany/{id}")]

        public async Task<IActionResult> DeleteCompany([FromRoute] string id, CancellationToken cancellationToken)
        {
            var command = new DeleteCompanyCommand(id);
            var deleteCompanyResult = await _mediator.Send(command);
            #region Switch Statement
            return deleteCompanyResult switch
            {
                { IsSuccess: true, Data: true } => NoContent(),
                { IsSuccess: true, Message: not null } => new JsonResult(new { Message = deleteCompanyResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteCompanyResult.Errors),
                _ => BadRequest("Invalid Fields for Add Modules")
            };

            #endregion
        }
        #endregion

        #region UpdateBillNumberStatusForPurchase
        [HttpPatch("UpdateBillNumberStatusForPurchase/{companyId}")]

        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateBillNumberStatusForPurchaseRequest request)
        {
            //Mapping command and request
            var command =request.ToCommand(id);
            var updateBillNumberResult = await _mediator.Send(command);
            #region Switch Statement
            return updateBillNumberResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateBillNumberResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = updateBillNumberResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateBillNumberResult.Errors),
                _ => BadRequest("Invalid Fields for Update bill number status")
            };

            #endregion


        }
        #endregion

        #region FilterCompanyByDate
        [HttpGet("FilterCompanyByDate")]
        public async Task<IActionResult> GetCompanyFilter([FromQuery] FilterCompanyDTOs filterCompanyDTOs )
        {
            var query = new FilterCompanyByDateQuery(filterCompanyDTOs);
            var filterCompanyResult = await _mediator.Send(query);

            #region Switch Statement
            return filterCompanyResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(filterCompanyResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = filterCompanyResult.Message }),
                { IsSuccess: false, Errors: not null } => BadRequest(new { Errors = filterCompanyResult.Errors }),
                _ => BadRequest(new { Message = "Invalid request or unexpected error occurred." })
            };
            #endregion
        }
        #endregion


        #region CompanyDetailsByInstitutionId
        [HttpGet("GetCompanyDetails/{institutionId}")]
        public async Task<IActionResult> GetCompanyDetailsByCompanyId([FromRoute] string institutionId)
        {
            var query = new GetCompanyDetailsByCompanyIdQuery(institutionId);
            var companyDetailsResult = await _mediator.Send(query);
            #region Switch Statement
            return companyDetailsResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(companyDetailsResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: true, Data: null, Message: not null } => new JsonResult(new { Message = companyDetailsResult.Message }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(companyDetailsResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields")
            };
            #endregion

        }
        #endregion

        #endregion
    }

}