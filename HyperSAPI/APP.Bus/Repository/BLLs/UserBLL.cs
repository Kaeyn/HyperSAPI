using APP.Bus.Repository.DTOs;
using APP.Bus.Repository.DTOs.Customer;
using APP.Bus.Repository.DTOs.Login;
using APP.Bus.Repository.DTOs.User;
using APP.DAL.Repository.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace APP.Bus.Repository.BLLs
{
    public class UserBLL
    {
        private AppDBContext DB;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private string secretKey = "";

        public UserBLL(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            DB = new AppDBContext();
            secretKey = Environment.GetEnvironmentVariable("MYAPI_SECRET_KEY");
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
                            var roles = await _userManager.GetRolesAsync(user);
                            var redirect = "";
                            int objectRes = -1;
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
                        else respond.ObjectReturn = new { ResultLogin = result};
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

/*        public async Task<DTOResponse> CheckUser(ClaimsPrincipal claims)
        {
            DTOResponse respond = new DTOResponse();
            try
            {
                var result = _signInManager.IsSignedIn(claims);
                if (result)
                {
                    var user = await _signInManager.UserManager.GetUserAsync(claims);
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Contains("Customer"))
                    {
                        respond.ObjectReturn = new { Res = "jkwt" };
                    }
                    else
                    {
                        respond.ObjectReturn = new { Res = "uije" };
                    }                                       
                }
                else
                {
                    respond.ErrorString = "UnSingedin";
                }
            }
            catch (Exception ex)
            {
                respond.StatusCode = 500;
                respond.ErrorString = ex.Message;
            }
            return respond;
        }*/

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
