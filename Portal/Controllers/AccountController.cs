using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using Portal.Database;
using Portal.Intefaces;
using Portal.Models;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Portal.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class  AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userMenager;
        private  SignInManager<IdentityUser> _signInManager;
        private INewsRepository _newsRepository;
        private IMailServices _mailServices;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly TokenValidationParameters _tokenValidationParameters;
        public AccountController(UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager,
            IConfiguration config,
            AppDbContext context,
            INewsRepository newsRepository,
            IMailServices mailServices,
            TokenValidationParameters tokenValidationParameters)
        {

            _userMenager = userManager;
            _signInManager = signInManager;
            _configuration = config;
            _context = context;
            _tokenValidationParameters = tokenValidationParameters;
            _newsRepository = newsRepository;
            _mailServices = mailServices;
        }
        [HttpPost]
        public async Task<ActionResult<UserViewModel>> Register(RegisterViewModel model)
        {
            if(_userMenager.Users.Any(x=> x.Email == model.Email)){
                return BadRequest("email is taken");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = new IdentityUser() { Email=model.Email,UserName=model.Email.Split("@")[0] };
            

            var result = await _userMenager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var WelcomeRequest = new WelcomeRequest() { ToEmail = model.Email, UserName = user.UserName };
                var mailRequest = new MailRequest()
                {
                    ToEmail = user.Email,
                    Subject = "Welcome",
                    Body = "New acc has been created in news portal with this email"

                };

         
                var currentregister =  _userMenager.Users.FirstOrDefault(x=> x.Email==model.Email);
                     await   _userMenager.AddToRoleAsync(currentregister, "Registered");

                var Usertoreturn = new UserViewModel()
                {
                    UserId = currentregister.Id,
                    Email = currentregister.Email,
                    Role = "Registered"

                };
               await _mailServices.SendEmailAsync(mailRequest);
                return Ok(Usertoreturn);
            }
            return BadRequest("cant create user");

        }
        [HttpPost]
        [Route("/login")]
        public async Task<ActionResult<UserViewModel>> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem();
            }
            var user = await _userMenager.FindByEmailAsync(model.Email);

           var resault= await  _signInManager.CheckPasswordSignInAsync(user,model.Password,false);
            if (resault.Succeeded)
            {
                _newsRepository.updateFingerPrint(model.FingerPrintId, user.Id);
                var token = await GenerateJWTTOkenAsync(user,null);
                return Ok(token);
               
               
            }

            var roles = await _userMenager.GetRolesAsync(user);

            if (resault.Succeeded==true)
            {
                if (_signInManager.IsSignedIn(User))
                {


                    var usertoreturn = new UserViewModel()
                    {
                        UserId = user.Id,
                        Email = user.Email,
                        Role = roles.FirstOrDefault()
                    };
                    return usertoreturn;
                }
            }
            return BadRequest();
        }
        [HttpPost]
         [Route("/refresh-token")]

        public async Task<ActionResult> refresh(TokenRequestVM model)
        {

            var result = await VerifyAndGenerateTokenAsync(model);
            return Ok(result);

        }
        [HttpPost]
        [Route("/addAdmin")]
        [Authorize]
        public async Task<ActionResult<UserViewModel>> RegisterAdmin(RegisterViewModel model)
        {
            if (_userMenager.Users.Any(x => x.Email == model.Email))
            {
                return BadRequest("email is taken");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = new IdentityUser() { Email = model.Email, UserName = model.Email.Split("@")[0] };


            var result = await _userMenager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var WelcomeRequest = new WelcomeRequest() { ToEmail = model.Email, UserName = user.UserName };
                var mailRequest = new MailRequest()
                {
                    ToEmail = user.Email,
                    Subject = "Welcome",
                    Body = "New acc has been created in news portal with this email"

                };

               

                var currentregister = _userMenager.Users.FirstOrDefault(x => x.Email == model.Email);
                await _userMenager.AddToRoleAsync(currentregister, "Admin");
                await _mailServices.SendEmailAsync(mailRequest);
                var Usertoreturn = new UserViewModel()
                {
                    UserId = currentregister.Id,
                    Email = currentregister.Email,
                    Role = "Admin"

                };
                return Ok(Usertoreturn);
            }
            return BadRequest("cant create user");

        }
        [HttpGet]
        public async Task<ActionResult<UserViewModel>> GetCurrentUser()
        {
            if (User.Identity.IsAuthenticated) 
            {
                var user = await _userMenager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var roles = await _userMenager.GetRolesAsync(user);

                var usertoreturn = new UserViewModel()
                {
                    Email = user.Email,
                    UserId = user.Id,
                    Role = roles.FirstOrDefault()
                };

                return usertoreturn;



            }
            return NoContent();
        }
        [HttpPost]
        [Route("/logout")]
        public async Task<ActionResult> Logout()
        {
            await  _signInManager.SignOutAsync();

            return Ok();
        }
        private async Task<AuthResaultVMcs> GenerateJWTTOkenAsync(IdentityUser user,RefershToken rtoken)
        {
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.Sub,user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            var userRoles =await  _userMenager.GetRolesAsync(user);
            foreach(var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));
             

            var token = new JwtSecurityToken(
                claims: authClaims,
               expires: DateTime.UtcNow.AddMinutes(1),
               signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
               );
            IdentityModelEventSource.ShowPII = true;
           
            
                var jwtTOken = new JwtSecurityTokenHandler().WriteToken(token);
            
           

            if (rtoken != null)
            {
                var reTokenRsponse= new AuthResaultVMcs()
                {
                    token = jwtTOken,
                    refreshToken = rtoken.Token,
                    ExpiresAt = token.ValidTo
                };
                return reTokenRsponse;
            }

            var refreshToken = new RefershToken()
            {
                Jwtid = token.Id,
                IsRevoked = false,
                Id = user.Id,
                DateAded = DateTime.UtcNow,
                DateExpires = DateTime.UtcNow.AddMonths(6),
                Token = new Guid().ToString()+"-"+Guid.NewGuid().ToString()

            };

            await _context.refreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            var response = new AuthResaultVMcs()
            {
                token = jwtTOken,
                refreshToken = refreshToken.Token,
                ExpiresAt = token.ValidTo,
                Username = user.UserName,
                UserEmail = user.Email,
                UserRole = userRoles.FirstOrDefault()

            };

            return response;
        }

        private async Task<AuthResaultVMcs> VerifyAndGenerateTokenAsync(TokenRequestVM tokenRequestVM)
        {
            var jwtTokenHeandler = new JwtSecurityTokenHandler();
            var storedToken = await _context.refreshTokens.FirstOrDefaultAsync(x => x.Token == tokenRequestVM.RefreshToken);
            var dbUser = await _userMenager.FindByIdAsync(storedToken.Id);
            try
            {
                var tokenCheckResault = jwtTokenHeandler.ValidateToken(tokenRequestVM.Token, _tokenValidationParameters, out var validatedToken);
                return await GenerateJWTTOkenAsync(dbUser, storedToken);
            }
            catch (SecurityTokenExpiredException ex)
            {
                if (storedToken.DateExpires >= DateTime.UtcNow)
                {
                    return await GenerateJWTTOkenAsync(dbUser, storedToken);
                }
                else
                {
                    return await GenerateJWTTOkenAsync(dbUser, null);
                }
            }

        }
        [HttpGet]
        [Route("/getUsers")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<UserViewModel>>> GetUsers()
        {

            var users = await _userMenager.Users.ToListAsync();
            var listofUsers = new List<UserViewModel>();
  
            foreach(var user in users)
            {
                var roles = await _userMenager.GetRolesAsync(user);
                listofUsers.Add(new UserViewModel() { UserId=user.Id,Email=user.Email,UserName=user.UserName,Role=roles.FirstOrDefault() });

            }

            return Ok(listofUsers);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteUser(string Id)
        {
            var user = _userMenager.Users.Where(x => x.Id == Id).FirstOrDefault();
              var result= await  _userMenager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok();
;            }
            return BadRequest();
        }
        [HttpPost]
        [Route("/forgot-password")]
        public async Task<IdentityUser> ForgotPassword(ForgetPasswordViewModel forgetPasswordViewModel)
        {
            var user = await _userMenager.FindByEmailAsync(forgetPasswordViewModel.Email);

            if (user != null)
            {
                var token = await _userMenager.GeneratePasswordResetTokenAsync(user);
                token = HttpUtility.UrlEncode(token);

                await SendResetPasswordEmail(forgetPasswordViewModel.Email, token);
            }

            return user;
        }
        [HttpPost]
        [Route("/editUser")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userMenager.FindByIdAsync(model.userId);

                user.UserName = model.userName;
                user.Email = model.email;
                if (model.password != null)
                {
                     await _userMenager.RemovePasswordAsync(user);
                   var result = await _userMenager.AddPasswordAsync(user, model.password);
                    if (result.Succeeded)
                    {
                       
                    }
                    else
                    {
                        return ValidationProblem("Password could not change");
                    }
                }
                var roles = await _userMenager.GetRolesAsync(user);
                if (roles.FirstOrDefault() != model.role)
                {
                    var result = await _userMenager.RemoveFromRoleAsync(user, roles.FirstOrDefault());
                    if (result.Succeeded)
                    {
                        result = await _userMenager.AddToRoleAsync(user, model.role);
                        if (result.Succeeded)
                        {

                        }
                        else
                        {
                            return ValidationProblem("Role could not change");
                        }
                    }
                    else
                    {
                        return ValidationProblem("Role could not change");
                    }
                }
             var useruppdateResult=  await _userMenager.UpdateAsync(user);
                if(useruppdateResult.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return ValidationProblem("User could not updated");
                }

            }
            return BadRequest();
        }

        private async Task<bool> SendResetPasswordEmail(string email, string token)
        {

            var emailTemplateFile = Directory.GetCurrentDirectory() + "\\Templates\\SendResetPasswordTemplate.html";
            StreamReader str = new StreamReader(emailTemplateFile);
            string emailTemplate = str.ReadToEnd();
            var builder = new BodyBuilder();
            builder.HtmlBody = emailTemplate;

            string clientBaseUrl = "http://localhost:3000";

            if (emailTemplate != null)
            {
                string resetLink = $"{clientBaseUrl}/reset-password?&token=" + token;

                builder.HtmlBody = emailTemplate.Replace("{{LINK}}", resetLink);
                var mailRequest = new MailRequest()
                {
                    Subject = "Forgot Password Reset Link",
                    ToEmail = email,
                    Body = builder.HtmlBody
                };

                var result = await _mailServices.SendEmailAsync(mailRequest);

                return result;
            }

            return false;
        }
        [HttpPost]
        [Route("/reset-password")]
        public async Task<IdentityUser> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            var user = await _userMenager.FindByEmailAsync(resetPasswordViewModel.Email);
            if (user != null)
            {
                resetPasswordViewModel.Token = await _userMenager.GeneratePasswordResetTokenAsync(user);
                var result = await _userMenager.ResetPasswordAsync(user, resetPasswordViewModel.Token, resetPasswordViewModel.Password);
                if (result.Succeeded)
                {
                    return user;
                }
               else
                {
                    return null;
                }
            }

            return user;
        }








    }
}
