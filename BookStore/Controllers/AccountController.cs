using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models;
using BookStore.Models.Dto;
using BookStore.Models.Dto.ResultDto;
using BookStore.Models.Entities;
using BookStore.Services.Implementation;
using BookStore.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationContext ctx;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtTokenService _jwtTokenService;

        public AccountController(
                ApplicationContext context,
                UserManager<User> userManager,
                SignInManager<User> signInManager,
                IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            ctx = context;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
        }

        [HttpGet]
        public ResultDto ok()
        {
            return new ResultDto
            {
                IsSuccessful = true
            };
        }

        [HttpPost("Register")]
        public async Task<ResultDto> Register([FromBody] RegisterDto model)
        {
            try
            {
                User user = new User()
                {
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    UserName = model.Email
                };
                await _userManager.CreateAsync(user, model.Password);
                UserAdditionalInfo ui = new UserAdditionalInfo()
                {
                    Id = user.Id,
                    Age = model.Age,
                    FullName = model.FullName,
                    Image = model.Image
                };

                var result = _userManager.AddToRoleAsync(user, "Guest").Result;

                await ctx.UserAdditionalInfo.AddAsync(ui);
                await ctx.SaveChangesAsync();

                return new ResultDto
                {
                    IsSuccessful = true
                };
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        [HttpPost("login")]
        public async Task<ResultDto> Login(LoginDto model)
        {
            var res = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            if (!res.Succeeded)
                return new ResultDto
                {
                    IsSuccessful = false
                };

            var user = await _userManager.FindByEmailAsync(model.Email);
            await _signInManager.SignInAsync(user, isPersistent: false);

            return new ResultLoginDto
            {
                IsSuccessful = true,
                Token = _jwtTokenService.CreateToken(user)
            };
        }

    }
}
