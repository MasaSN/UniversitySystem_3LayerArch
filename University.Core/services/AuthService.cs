using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using University.Core.dtos;
using University.Core.Exceptions;
using University.Core.forms;
using University.Core.Validations;
using University.Data.Entities.Identity;

namespace University.Core.services
{
    public class AuthService : IauthService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ILogger<AuthService> _logger;


        public AuthService(SignInManager<User> signInManager,
                UserManager<User> userManager,
                RoleManager<Role> roleManager,
                ILogger<AuthService> logger) {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }
        public async Task<UserDTO> Register(RegisterForm form)
        {
            if (form == null) throw new ArgumentNullException("form");
            var validation = FormValidator.Validate(form);
            if (!validation.isValid)
            {
                throw new BussinessException(validation.Errors);
            }
            var userExists = await _userManager.FindByEmailAsync(form.Email);
            if (userExists != null)
            {
                throw new BussinessException("User Already exist with this email");
            }
            var user = new User()
            {
                FirstName = form.FirstName,
                LastName = form.LastName,
                Email = form.Email,
                UserName = form.Email
            };
            var result = await _userManager.CreateAsync(user, form.Password);
            _logger.LogInformation("Role that isn't being returned: {Role}, ", form.Role);
            if (!result.Succeeded)
            {
                _logger.LogError("❌ Failed to create role {Role}. Errors: {Errors}", form.Role);

                throw new BussinessException(result.Errors
                    .GroupBy(x => x.Code)
                    .ToDictionary(x => x.Key, x => x.Select(e => e.Description).ToList()));

            }
            if (!await _roleManager.RoleExistsAsync(form.Role))
            {
                 await _roleManager.CreateAsync(new Role { Name = form.Role});
                

            }
            await _userManager.AddToRoleAsync(user, form.Role);
            return new UserDTO()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed
            };
        }
        public async Task<UserDTO> Login(LoginForm form)
        {
            if(form == null)
            {
                throw new ArgumentNullException(nameof(form), "Login request cannot be null.");
            }
            var validation = FormValidator.Validate(form);
            if (!validation.isValid)
            {
                throw new BussinessException(validation.Errors);
            }
            var result = await _signInManager.PasswordSignInAsync(
                form.Email,
                form.Password,
                true, // Remember me
                lockoutOnFailure    : false
            );
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(form.Email);
                if (user == null)
                {
                    throw new BussinessException("User not found.");
                }
                var dto = new UserDTO()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                    EmailConfirmed = user.EmailConfirmed,
                    PhoneNumberConfirmed = user.PhoneNumberConfirmed
                };
                return dto;
            }
            else if (result.IsLockedOut)
            {
                throw new BussinessException("User account is locked out.");
            }
            else if (result.IsNotAllowed)
            {
                throw new BussinessException("User is not allowed to login.");
            }
            else
            {
                throw new BussinessException("Invalid login attempt.");
            }
        }
    }
    public interface IauthService
    {
        Task<UserDTO> Login(LoginForm form);
        Task<UserDTO> Register(RegisterForm form);
    }
    }
