﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Core.Interfaces;
using Shopping.Infrastructure.Identity;
using Shopping.Shared.Claims;
using Shopping.Shared.Dtos;
using Shopping.Shared.Dtos.RolesAndUsers;
using System.Security.Claims;

namespace Shopping.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminRolePolicy")]
    public class AdministrationController : ControllerBase
    {

        #region Fields and CTOR

        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITokenService tokenService;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILoggerManager logger;

        public AdministrationController(UserManager<ApplicationUser> userManager, 
                                        ITokenService tokenService,RoleManager<IdentityRole> roleManager,
                                        ILoggerManager logger)
        {
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.roleManager = roleManager;
            this.logger = logger;
        }

        #endregion

       
        #region Role Actions

        [HttpGet("Roles")]
        public IActionResult Roles()
        {
            var roles = roleManager.Roles;
            return Ok(roles);
        }


        [HttpGet("CreateRole")]  
        public IActionResult CreateRole()
        {
            return Ok();

        }


        [HttpPost("CreateRole")]
        public async Task<ActionResult> CreateRole(CreateRoleDto createRoleDto)
        {

            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = createRoleDto.Name
                };

                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return Ok(); 
                }


                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    logger.LogError("Could not Register new Role");
                }
            }

            return null;

        }


        [HttpGet]
        [Route("EditRole/{id}")]
        public async Task<ActionResult> EditRole(string id)
        {

            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                logger.LogError("Role could not be found in database");
                return NotFound();
               
            }

            var editRoleDto = new EditRoleDto
            {
                Id = role.Id,
                Name = role.Name
            };


            return Ok(editRoleDto);
        }


        [HttpPost("EditRole")]
        public async Task<ActionResult> EditRole(EditRoleDto editRoleDto)
        {
            var role = await roleManager.FindByIdAsync(editRoleDto.Id);

            if (role == null)
            {
                logger.LogError("Edit Role could not be found in database");
                return NotFound();
            }
            else
            {
                role.Name = editRoleDto.Name;

                var result = await roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("Roles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    logger.LogError("Could not Edit Role");
                }

                return null;
            }
        }

 

        [HttpDelete("DeleteRole/{id}")]
        [Authorize(Policy = "DeleteRolePolicy")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                logger.LogError("Delete Role could not be found in database");
                return NotFound();

            }
            else
            {
                var result = await roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("Roles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    logger.LogError("Could not Delete Role");
                }

                return Ok("Roles");
            }
        }


        [HttpGet("GetUsersInRole/{id}")]
        public async Task<ActionResult<IEnumerable<UserRoleDto>>> GetUsersInRole(string id)
        {

            var role = await roleManager.FindByIdAsync(id);


            if (role == null)
            {
                logger.LogError("GetUsersInRole; Role could not be found in database");
                return NoContent();
            }

            var userRoleDtoList = new List<UserRoleDto>();

            foreach (var user in await userManager.Users.ToListAsync())
            {
                var userRoleDto = new UserRoleDto
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Selected = true
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleDtoList.Add(userRoleDto);
                }

            }

            return Ok(userRoleDtoList);


        }


        [HttpGet("ManageRoleMemberShip/{id}")]
        public async Task<ActionResult<IEnumerable<UserRoleDto>>> ManageRoleMemberShip(string id)
        {

            var role = await roleManager.FindByIdAsync(id);


            if (role == null)
            {
                logger.LogError("ManageRoleMemberShip; Role could not be found in database");
                return NoContent();
            }

            var userRoleDtoList = new List<UserRoleDto>();
            var allUsers = await userManager.Users.ToListAsync();


            foreach (var user in allUsers)
            {
                var userRoleDto = new UserRoleDto
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };


                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleDto.Selected = true;
                }
                else
                {
                    userRoleDto.Selected = false;
                }

                    userRoleDtoList.Add(userRoleDto);
                

            }

            return Ok(userRoleDtoList);


        }

        [HttpPatch("ManageRoleMemberShip/{id}")]
        public async Task<ActionResult<IEnumerable<UserRoleDto>>> ManageRoleMemberShip(string Id, List<UserRoleDto> userRoleDtos)
        {
            var role = await roleManager.FindByIdAsync(Id);
             

            if (role == null)
            {
                logger.LogError("ManageRolesMembership; Role could not be found in database");
                return NoContent();
            }


            for (int i = 0; i < userRoleDtos.Count; i++)
            {
                var user = await userManager.FindByIdAsync(userRoleDtos[i].UserId);

                IdentityResult result = null;

                if (userRoleDtos[i].Selected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!userRoleDtos[i].Selected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (userRoleDtos.Count - 1))
                        continue;
                    else

                        return Ok(userRoleDtos);

                   
                }
                return RedirectToAction("HandleRoleMemberShip", new { Id = Id });

            }

            return null;

        }



        #endregion


        #region User Actions

        [HttpGet("Users")]
        public async Task<ActionResult<UserDto>> Users()
        {
            var list = new List<UserDto>();

            foreach (var user in userManager.Users.ToList())
            {
                list.Add(new UserDto()
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Roles = await userManager.GetRolesAsync(user)

                });
            }
            return Ok(list);
        }

        [HttpGet("EditUser/{id}")]
        public async Task<ActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                logger.LogError("EditUser; User could not be found in database");
                return NotFound();
            }


            var userClaims = await userManager.GetClaimsAsync(user);

            var userRoles = await userManager.GetRolesAsync(user);

            var editUserDto = new EditUserDto
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                City = user.City,
                Claims = userClaims.Select(c => c.Type + ": " + c.Value).ToList(),
                Roles = userRoles
            };

            return Ok(editUserDto);
        }

        [HttpPost("EditUser")]
        public async Task<ActionResult<EditUserDto>> EditUser(EditUserDto editUserDto)
        {
            var user = await userManager.FindByIdAsync(editUserDto.Id);       

            if (user == null)
            {
                logger.LogError("EditUser; User could not be found in database");
                return NotFound();
            }
            else
            {
                user.Email = editUserDto.Email;
                user.UserName = editUserDto.UserName;
                user.City = editUserDto.City;
                user.FirstName = editUserDto.FirstName;
                user.LastName = editUserDto.LastName;

                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Users");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    logger.LogError("EditUser; User could NOT be updated");
                }

                    return editUserDto;
            }
        }


        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                logger.LogError("DeleteUser; User could not be found in database");
                return NotFound();

            }
            else
            {
                var result = await userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Users");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    logger.LogError("DeleteUser; User could not be deleted");
                }

                return Ok("Users");
            }
        }

        #endregion


        [HttpGet("ManageUserRoles/{id}")]
        public async Task<ActionResult<IEnumerable<UserRolesDto>>> ManageUserRoles(string Id)
        {

            var user = await userManager.FindByIdAsync(Id);


            if (user == null)
            {
                logger.LogError("ManageUserRoles; User could not be found in database");
                return NoContent();
            }

            var userRolesDtoList = new List<UserRolesDto>();
            var allRoles= await roleManager.Roles.ToListAsync();


            foreach (var role in allRoles)
            {
                var userRolesDto = new UserRolesDto
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };


                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesDto.Selected = true;
                }
                else
                {
                    userRolesDto.Selected = false;
                }

                userRolesDtoList.Add(userRolesDto);


            }

            return Ok(userRolesDtoList);


        }


        [HttpPatch("ManageUserRoles/{id}")]
        public async Task<ActionResult<IEnumerable<UserRolesDto>>> ManageUserRoles(string id, List<UserRolesDto> userRolesDtos)
        {

            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                logger.LogError("ManageUserRoles; User could not be found in database");
                return NoContent();
            }


            for (int i = 0; i < userRolesDtos.Count; i++)
            {
                var role = await roleManager.FindByIdAsync(userRolesDtos[i].RoleId);

                IdentityResult result = null;

                if (userRolesDtos[i].Selected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!userRolesDtos[i].Selected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (userRolesDtos.Count - 1))
                        continue;
                    else

                        return Ok(userRolesDtos);


                }
                return RedirectToAction("ManageUserRoles", new { Id = id });

            }

            return null;

        }


        [HttpGet("ManageUserClaims/{id}")]
        public async Task<ActionResult<UserClaimsDto>> ManageUserClaims(string id)
        {

            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                logger.LogError("ManageUserClaims; User could not be found in database");
                return NoContent();
            }

            var existingUserClaims = await tokenService.GetClaims(user);

            var userClaimsDto = new UserClaimsDto
            {
                UserId = id
            };


            foreach (Claim claim in CustomClaims.AllClaims)
            {
                UserClaim userClaim = new UserClaim
                {
                    ClaimType = claim.Type
                };


                if (existingUserClaims.Any(c => c.Type == claim.Type && c.Value=="true"))
                {
                    userClaim.Selected = true;
                }

                userClaimsDto.Claims.Add(userClaim);
            }

            return Ok(userClaimsDto);
        }


        [HttpPost("ManageUserClaims/{id}")]
        public async Task<ActionResult<UserClaimsDto>> ManageUserClaims(string id, UserClaimsDto userClaimsDto)
        {

            var user = await userManager.FindByIdAsync(id);


            if (user == null)
            {
                logger.LogError("ManageUserClaims; User could not be found in database");
                return NoContent();
            }

            var claims = await userManager.GetClaimsAsync(user);
            var result = await userManager.RemoveClaimsAsync(user, claims);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing claims");
                logger.LogError("ManageUserClaims; Cannot remove user existing claims");

                return (userClaimsDto);
            }


            result = await userManager.AddClaimsAsync(user,
                  userClaimsDto.Claims.Select(c => new Claim(c.ClaimType, c.Selected ? "true": "false")));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected claims to user");
                logger.LogError("ManageUserClaims; Cannot add selected claims to user");

                return (userClaimsDto);
            }

            return RedirectToAction("EditUser", new { Id = userClaimsDto.UserId });

        }

    }
} 

