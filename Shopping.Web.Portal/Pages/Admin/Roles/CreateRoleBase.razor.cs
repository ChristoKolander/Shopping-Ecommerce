﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Shopping.Shared.Dtos.RolesAndUsers;
using Shopping.Web.Portal.Services.Interfaces;

namespace Shopping.Web.Portal.Pages.Admin.Roles
{
    public class CreateRoleBase: ComponentBase
    {
        public bool Result { get; set; }
        public bool IsVisible { get; set; }
        public string RecordName { get; set; }
        public bool Edit { get; set; }


        public CreateRoleDto CreateRoleDto = new CreateRoleDto();

        [Inject]
        public IAdministrationService administrationService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        IAuthorizationService AuthService { get; set; } = default!;

        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; } = default!;

        protected async override Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            var CheckAdminPolicy = await AuthService.AuthorizeAsync(authState.User, "AdminRolePolicy");

            if (CheckAdminPolicy.Succeeded)
            {
                StateHasChanged();
            }
        }

              
        public async Task CreateRole()
        {
            CreateRoleDto Success;

            Success = await administrationService.CreateRole(CreateRoleDto);

            if (Success.Name.Length > 0)
            {
                IsVisible = true;
                RecordName = CreateRoleDto.Name;
                Result = true;
                Edit = true;
                
            }
        }
    }
}
