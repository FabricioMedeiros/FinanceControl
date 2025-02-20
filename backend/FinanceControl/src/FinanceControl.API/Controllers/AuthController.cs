using FinanceControl.Application.DTOs;
using FinanceControl.Application.Interfaces;
using FinanceControl.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinanceControl.API.Controllers
{
    [Route("api/auth")]
    public class AuthController : MainController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtTokenService jwtTokenService,
            INotificator notificator) : base(notificator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            if (!ModelState.IsValid)
                return CustomResponse();

            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    NotifyError(error.Description);

                return CustomResponse();
            }

            var token = await _jwtTokenService.GenerateTokenAsync(user);
            
            return CustomResponse(new { Token = token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return CustomResponse();

            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
            {
                NotifyError("Usuário ou senha inválidos.");
                return CustomResponse();
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);

            if (!result.Succeeded)
            {
                NotifyError("Usuário ou senha inválidos.");
                return CustomResponse();
            }

            var token = await _jwtTokenService.GenerateTokenAsync(user);

            return CustomResponse(new { Token = token });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return CustomResponse(new { message = "Logout realizado com sucesso!" });
        }
    }
}
