﻿using Newtonsoft.Json;
using Shopping.Shared.Dtos;
using Shopping.Shared.Dtos.RolesAndUsers;
using Shopping.Web.Portal.Services.Interfaces;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;


namespace Shopping.Web.Portal.Services
{
    public class AdministrationService : IAdministrationService
    {

        #region Fields and CTOR

        private readonly HttpClient httpClient;
        private readonly JsonSerializerOptions options;
        public List<UserRoleDto> UserRoleDtos { get; set; } = new List<UserRoleDto>();
        public List<UserClaimsDto> UserClaimsDtos { get; set; } = new List<UserClaimsDto>();

        public AdministrationService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        }

        #endregion

        #region Role Services

        public async Task<EditRoleDto> GetRole(string Id)
        {

            HttpResponseMessage response = await httpClient.GetAsync($"api/administration/editrole/{Id}");

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return default(EditRoleDto);
                }

                return await response.Content.ReadFromJsonAsync<EditRoleDto>();
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception(message);
            }

        }

        public async Task<List<RoleDto>> GetRoles()
        {
         
            HttpResponseMessage response = await httpClient.GetAsync($"api/administration/Roles");

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return Enumerable.Empty<RoleDto>().ToList();
                }
                return await response.Content.ReadFromJsonAsync<List<RoleDto>>();
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status code: {response.StatusCode} Message: {message}");
            }
                   
        }

        public async Task<CreateRoleDto> CreateRole(CreateRoleDto createRoleDto)
        {

            var jsonRequest = JsonConvert.SerializeObject(createRoleDto);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");

            HttpResponseMessage response = await httpClient.PostAsync($"api/administration/CreateRole", content);


            var responseContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(responseContent);
            }

            return createRoleDto;
                       
        }

        public async Task<EditRoleDto> EditRole(EditRoleDto editRoleDto)
        {

            var jsonRequest = JsonConvert.SerializeObject(editRoleDto);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");

            HttpResponseMessage response = await httpClient.PostAsync($"api/administration/EditRole", content);


            var responseContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(responseContent);
            }

            return editRoleDto;

        }

        public async Task<RoleDto> DeleteRole(string id)
        {
            
            HttpResponseMessage response = await httpClient.DeleteAsync($"api/administration/deleterole/{id}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<RoleDto>();
            }
                
            return default(RoleDto);
                    
        }

        #endregion

        #region User Services

        public async Task<List<UserDto>> GetUsers()
        {
            
            HttpResponseMessage response = await httpClient.GetAsync($"api/administration/Users");

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return Enumerable.Empty<UserDto>().ToList();
                }
                return await response.Content.ReadFromJsonAsync<List<UserDto>>();
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status code: {response.StatusCode} Message: {message}");
            }
                    
        }

        public async Task<EditUserDto> EditUser(string Id)
        {

            HttpResponseMessage response = await httpClient.GetAsync($"api/administration/edituser/{Id}");


            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return default(EditUserDto);
                }
                return await response.Content.ReadFromJsonAsync<EditUserDto>();
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status code: {response.StatusCode} Message: {message}");
            }
                    
        }

        public async Task<EditUserDto> EditUser(EditUserDto editUserDto)
        {

            
            var jsonRequest = JsonConvert.SerializeObject(editUserDto);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");

            HttpResponseMessage response = await httpClient.PostAsync($"api/administration/EditUser", content);


            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(responseContent);
            }

            return editUserDto;
                     
        }

        public async Task<UserDto> DeleteUser(string id)
        {

            
                HttpResponseMessage response = await httpClient.DeleteAsync($"api/administration/deleteuser/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<UserDto>();
                }
                return default(UserDto);
                     
        }

        #endregion

        # region Additional Services

        public async Task<UserDto> GetRolesForUser(UserDto userDto)
        {
           
            var jsonRequest = JsonConvert.SerializeObject(userDto);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");

            HttpResponseMessage response = await httpClient.PostAsync($"api/administration/userRoles", content);


            var responseContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(responseContent);
            }

            return userDto;

        }              
        public async Task<List<UserRoleDto>> GetUsersInRole(string Id)

        {
            
            HttpResponseMessage response = await httpClient.GetAsync($"api/administration/GetUsersInRole/{Id}");


            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return Enumerable.Empty<UserRoleDto>().ToList();
                }
                return await response.Content.ReadFromJsonAsync<List<UserRoleDto>>();
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status code: {response.StatusCode} Message: {message}");
            }

        }
              
        public async Task<List<UserRolesDto>> ManageUserRoles(string id)

        {
            
            HttpResponseMessage response = await httpClient.GetAsync($"api/administration/ManageUserRoles/{id}");


            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return Enumerable.Empty<UserRolesDto>().ToList();
                }
                return await response.Content.ReadFromJsonAsync<List<UserRolesDto>>();
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status code: {response.StatusCode} Message: {message}");
            }

        }                        
        public async Task<List<UserRolesDto>> ManageUserRoles(string id, List<UserRolesDto> userRolesDtos)

        { 
                 
                var jsonRequest = JsonConvert.SerializeObject(userRolesDtos);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");

                HttpResponseMessage response = await httpClient.PatchAsync($"api/administration/manageuserroles/{id}", content);


                var patchContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    throw new ApplicationException(patchContent);
                }

                return userRolesDtos;

        }
             
        public async Task<List<UserRoleDto>> ManageRoleMemberShip(string Id)

        {
            
            HttpResponseMessage response = await httpClient.GetAsync($"api/administration/managerolemembership/{Id}");


            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return Enumerable.Empty<UserRoleDto>().ToList();
                }
                return await response.Content.ReadFromJsonAsync<List<UserRoleDto>>();
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status code: {response.StatusCode} Message: {message}");
            }

        }           
        public async Task<List<UserRoleDto>> ManageRoleMemberShip(string Id, List<UserRoleDto> userRoleDtos)

        {
           
            var jsonRequest = JsonConvert.SerializeObject(userRoleDtos);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");

            HttpResponseMessage response = await httpClient.PatchAsync($"api/administration/managerolemembership/{Id}", content);


            var patchContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(patchContent);
            }

            return UserRoleDtos;

        }
            
        public async Task<UserClaimsDto> ManageUserClaims(string id)
        {
           
            HttpResponseMessage response = await httpClient.GetAsync($"api/administration/ManageUserClaims/{id}");


            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return default(UserClaimsDto);
                }

                return await response.Content.ReadFromJsonAsync<UserClaimsDto>();

            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status code: {response.StatusCode} Message: {message}");
            }

        }         
        public async Task<UserClaimsDto> ManageUserClaims(string Id, UserClaimsDto userClaimsDto) 

        {
            var jsonRequest = JsonConvert.SerializeObject(userClaimsDto);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync($"api/administration/ManageUserClaims/{Id}", content);


            var postContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(postContent);
            }

            return userClaimsDto;

        }

        # endregion

    }
}
