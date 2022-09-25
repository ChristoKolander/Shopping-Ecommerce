﻿using Microsoft.AspNetCore.Components;
using Shopping.Models.Dtos;
using Shopping.Models.Dtos.RolesAndUsers;
using Shopping.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopping.Web.Pages.Admin.Claims
{
    public class ManageUserClaimsBase: ComponentBase
    {
        public bool Result { get; set; }
        public bool IsVisible { get; set; }
        public string RecordName { get; set; }
        public bool Edit { get; set; }
        public bool Selected { get; set; }
        public int Number { get; set; }

        [Parameter]
        public string Id { get; set; }

        public EditUserDto EditUserDto = new EditUserDto();

        public UserClaim UserClaim = new UserClaim(); 

        public UserClaimsDto UserClaimsDto { get; set; } = new UserClaimsDto();

        [Inject]
        public IAdministrationService administrationService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected async override Task OnInitializedAsync()
        {

            EditUserDto = await administrationService.EditUser(Id);
            UserClaimsDto = await administrationService.ManageUserClaims(EditUserDto.Id);
  
        }


        public async Task EditUserClaims()
        {

            UserClaimsDto Success;

            Success = await administrationService.ManageUserClaims(EditUserDto.Id, UserClaimsDto);


            if (Success.Claims.Count >= 0)
            {
                IsVisible = true;
                RecordName = EditUserDto.UserName;
                Result = true;
                Edit = true;

            }

            StateHasChanged();

        }


        public void Cancel_Click()
        {
            NavigationManager.NavigateTo($"/admin/edituser/{EditUserDto.Id}");

        }
    }
}