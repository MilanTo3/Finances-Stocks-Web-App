using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using api.DTOs;
using Mapster;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using api.Interfaces;
using api.Service;

namespace api.Controllers
{
    [Route("api/account")]
    public class AccountController : Controller
    {

        private readonly UserManager<AppUser> _user;
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService)
        {
           _user = userManager;
           _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> registerUser([FromBody] UserDTO dto){
            try{
                if(!ModelState.IsValid){
                    return BadRequest(ModelState);
                }

                var appUser = dto.Adapt<AppUser>();
                var createdUser = await _user.CreateAsync(appUser, dto.Password);
                if(createdUser.Succeeded){
                    var roleResult = await _user.AddToRoleAsync(appUser, "User");
                    if(roleResult.Succeeded){
                        return Ok(new NewUserDTO{ Username = appUser.UserName, Email = appUser.Email, Token = _tokenService.CreateToken(appUser) });
                    }else{
                        return StatusCode(500, roleResult.Errors);
                    }
                }else{
                    return StatusCode(500, createdUser.Errors);
                }
            
            }catch(Exception ex){
                return StatusCode(500, ex);
            }
        }

        
    }
}