using APP.Bus.Repository.DTOs;
using APP.Bus.Repository.DTOs.Customer;
using APP.Bus.Repository.DTOs.Login;
using APP.Bus.Repository.DTOs.User;
using APP.DAL.Repository.Auth;
using APP.DAL.Repository.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace APP.Bus.Repository.BLLs
{
    public class UserBLL
    {
        private AppDBContext DB;
        private AuthDBContext AuthDB;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private IEmailSender _emailSender;
        private string secretKey = "";

        public UserBLL(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            DB = new AppDBContext();
            AuthDB = new AuthDBContext();
            secretKey = Environment.GetEnvironmentVariable("MYAPI_SECRET_KEY");
            _emailSender = emailSender;
        }

        public async Task<DTOResponse> RegisterUserAsync(dynamic requestParam)
        {
            string message = string.Empty;
            DTOResponse respond = new DTOResponse();
            IdentityResult result = new IdentityResult();
            try
            {
                var param = JsonConvert.DeserializeObject<DTORegister>(requestParam.ToString());

                IdentityUser newUser = new IdentityUser
                {
                    UserName = param.PhoneNumber,
                    Email = param.Email,
                    PhoneNumber = param.PhoneNumber,
                };
                result = await _userManager.CreateAsync(newUser, param.Password);
                

                if (!result.Succeeded)
                {
                    respond.ErrorString = "Failed Register !";
                }
                else
                {
                    await _userManager.AddToRoleAsync(newUser, "Customer");
                    User newDBUser = new User
                    {
                        IdUser = newUser.Id,
                        PhoneNumber = newUser.PhoneNumber,
                        Email = newUser.Email,
                        Status = 0,
                        Permission = _userManager.GetRolesAsync(newUser).Result.First()
                        
                    };

                    DB.Users.Add(newDBUser);
                    DB.SaveChanges();

                    Customer newDBCustomer = new Customer
                    {
                        CodeUser = newDBUser.Code,
                        Name = param.Name
                    };
                    DB.Customers.Add(newDBCustomer);
                    DB.SaveChanges();

                    var confirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                    string eventName = $"DeleteUnconfirmedUsers_{newUser.UserName}";
                    string sqlStatement = $@"
                    CREATE EVENT {eventName}
                    ON SCHEDULE AT CURRENT_TIMESTAMP + INTERVAL 10 MINUTE
                    DO
                    CALL DeleteUser('{newUser.Id}');";
                    AuthDB.Database.ExecuteSqlRaw(sqlStatement);
                    DB.Database.ExecuteSqlRaw(sqlStatement);
                    // Send the confirmation email with a link including the token
                    var confirmTokenStr = $"https://hypersapi.onrender.com/api/auth/confirmemail?userId={HttpUtility.UrlEncode(newUser.Id)}&token={HttpUtility.UrlEncode(confirmToken)}";
                    await _emailSender.SendEmailAsync(newUser.Email, "Confirm your email",
                    $"Please confirm your account by clicking this link: <button href='{confirmTokenStr}'>Confirm</button> <br/> Or this: <a href='{confirmTokenStr}'>Confirm</a>");
                }

                respond.ObjectReturn = result;

            }
            catch (Exception ex)
            {
                respond.StatusCode = 500;
                respond.ErrorString = ex.Message;
            }

            return respond;
        }

        public async Task<DTOResponse> LoginUserAsync(dynamic requestParam)
        {
            DTOResponse respond = new DTOResponse();
            try
            {
                var param = JsonConvert.DeserializeObject<DTOLogin>(requestParam.ToString());
                IdentityUser user = await FindUserAsync(param.Username);
                if (user != null)
                {
                    var userInDB = DB.Users.FirstOrDefault(u => u.IdUser == user.Id);
                    if (userInDB != null && userInDB.Status == 0)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user.UserName, param.Password, false, false);
                        if (result.Succeeded)
                        {
                            var redirect = "";
                            int objectRes = -1;
                            var roles = await _userManager.GetRolesAsync(user);
                            if (roles.Contains("Customer"))
                            {
                                redirect = "jkwt";
                                objectRes = DB.Customers.FirstOrDefault(c => c.CodeUser == userInDB.Code).Code;
                            }
                            else
                            {
                                redirect = "uije";
                            }

                            

                            var authClaims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, user.UserName),
                                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                            };

                            authClaims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

                            var authToken = GetToken(authClaims); 

                            var token = new
                            {
                                Token = new JwtSecurityTokenHandler().WriteToken(authToken),
                                Expires = authToken.ValidTo,
                            };
                          

                            respond.ObjectReturn = new { ResultLogin = result, ResultToken = token, ResultRedirect = redirect, ResultCus = objectRes };
                        }
                        else respond.ObjectReturn = new { ResultLogin = result, ResultToken = string.Empty, ResultRedirect = string.Empty, ResultCus = -1 };
                    }
                    else
                    {
                        respond.ErrorString = "Your account has been blocked";
                    }
                }
                else
                {
                    respond.ErrorString = "Undefined User in system";
                    respond.ObjectReturn = SignInResult.Failed;
                }

            }
            catch (Exception ex)
            {
                respond.StatusCode = 500;
                respond.ErrorString = ex.Message;
            }
            

            return respond;
        }

        public async Task<DTOResponse> ConfirmEmailAsync(string userId, string token)
        {
            var respond = new DTOResponse();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                respond.ErrorString = "Tài khoản không tồn tại trong hệ thống";
                return respond;
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                // Email confirmed successfully
                var customer = DB.Users.FirstOrDefault(u => u.IdUser == user.Id);
                if (customer != null)
                {
                    customer.EmailConfirm = 1;
                }
                respond.ErrorString = "Thành công";
                return respond;
            }
            else
            {
                respond.ErrorString = "Xác nhận thất bại";
                return respond;
            }
        }

        public async Task<DTOResponse> LogOut()
        {
            DTOResponse respond = new DTOResponse();
            try
            {
               /* string[] roleNames = { "Admin", "Customer", "Staff"};
                foreach (var roleName in roleNames)
                {
                    var roleExist = await _roleManager.RoleExistsAsync(roleName);
                    if (!roleExist)
                    {
                        await _roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }*/
                await _signInManager.SignOutAsync();
            }
            catch (Exception ex)
            {
                respond.StatusCode = 500;
                respond.ErrorString = ex.Message;
            }
            return respond;
        }

        public async Task<DTOResponse> ForgotPassword(string phoneNumber)
        {
            DTOResponse respond = new DTOResponse();
            try
            {
                IdentityUser user = await _userManager.FindByNameAsync(phoneNumber);
                if(user != null)
                {
                    var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var resetLink = $"https://hypersapi.onrender.com/api/auth/confirmemail?username={HttpUtility.UrlEncode(phoneNumber)}&token={HttpUtility.UrlEncode(resetToken)}";
                    await _emailSender.SendEmailAsync(user.Email, "Reset your password",
                    $"To reset your password please clicking this link: <button href='{resetLink}'>Confirm</button> <br/> Or this: <a href='{resetLink}'>Confirm</a>");
                }
                /* string[] roleNames = { "Admin", "Customer", "Staff"};
                 foreach (var roleName in roleNames)
                 {
                     var roleExist = await _roleManager.RoleExistsAsync(roleName);
                     if (!roleExist)
                     {
                         await _roleManager.CreateAsync(new IdentityRole(roleName));
                     }
                 }*/
                
            }
            catch (Exception ex)
            {
                respond.StatusCode = 500;
                respond.ErrorString = ex.Message;
            }
            return respond;
        }

        private async Task<IdentityUser> FindUserAsync(string username)
        {
            IdentityUser user = new IdentityUser();

            if (username.Contains("@"))
            {
                // Login is an email
                user = await _userManager.FindByEmailAsync(username);
            }
            /*else if (username.All(char.IsDigit))
            {
                // Login is a phone number
                user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == username);
            }*/
            else
            {
                // Login is a username
                user = await _userManager.FindByNameAsync(username);
            }

            return user;
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var token = new JwtSecurityToken(
                issuer: "https://hypersapi.onrender.com",
                audience: "https://hypersshop.online",
                expires: DateTime.UtcNow.AddHours(20),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }


    }
}
