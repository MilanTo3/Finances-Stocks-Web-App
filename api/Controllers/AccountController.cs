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
    [ApiController]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _user;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signinManager;

        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
           _user = userManager;
           _tokenService = tokenService;
           _signinManager = signInManager;
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
                        return Ok(new NewUserDTO{ UserName = appUser.UserName, Email = appUser.Email, Token = _tokenService.CreateToken(appUser) });
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

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _user.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.UserName.ToLower());

            if (user == null) return Unauthorized("Invalid username!");

            var result = await _signinManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized("Username not found and/or password incorrect");

            return Ok(
                new NewUserDTO
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                }
            );
        }

        
    }
}