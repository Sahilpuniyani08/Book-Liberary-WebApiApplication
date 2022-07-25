

using LiberaryApp.Data;
using LiberaryApp.Dtos;
using LiberaryApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace LiberaryApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController :ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;

        public AuthController( IConfiguration configuration, DataContext context )
        {
          _configuration = configuration;
           _context = context;
        }

        /* [HttpPost("Register")]
         public async Task<ActionResult<ServiceResponse<int>>> Resgister(UserRegisterDto request)
         {
             var response = await _authRepo.Register(
                 new User { UserName = request.UserName }, request.Password
                 );

             if(!response.Success)
             {
                 return BadRequest(response); ;
             }
             return Ok (response);   
         }*/


        /* [HttpPost("Login")]
         public async Task<ActionResult<ServiceResponse<string>>> login(UserLoginDto request)
         {
             var response = await _authRepo.Login(request.UserName, request.Password);
             if(!response.Success)
             {
                 return BadRequest(response); ;
             }
             return Ok (response);   
         }*/





        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserRegisterDto request)
        {
            User user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == request.UserName);
            if (user == null)
            { 

                CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
                User u = new User
                {
                    UserName = request.UserName,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Role =request.Role
                };
                await _context.Users.AddAsync(u);
                await _context.SaveChangesAsync();
                return Ok(u);
            }
            else
            {
                user.UserName = "USER ALREADY EXISTS";
                return BadRequest(user);
            }
        }






        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto request)
        {


            User user = await _context.Users.FirstOrDefaultAsync(c => c.UserName == request.UserName);
            if (user == null)
            {
                return BadRequest("User Do Not Exists");
            }

            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong password");

            }
            
            var jwttoken = CreateToken(user);
            var refreshtoken = CreateRefreshToken();

            await InsertRefreshToken(user.Id, refreshtoken);
            _context.SaveChangesAsync();

            TokenDto t = new TokenDto
            {
                JwtToken= jwttoken,
                RefreshToken= refreshtoken,
            };

            return Ok(t);
        }





        [HttpPost("Renew_Refresh_Token")]

        public async Task<ActionResult<TokenDto>> RenewToken(RenewTokenDto requestToken)
        {
            var userRefreshToken = await _context.RefreshTokens.Where(c => c.token == requestToken.Token && c.ExpirationDate >= DateTime.Now).FirstOrDefaultAsync();

            if (userRefreshToken == null)
            {
                return new TokenDto
                {
                    JwtToken = "Your refresh token is Expired please Go to login!",
                    RefreshToken = null
                };
            }
            var user = await _context.Users.Where(c => c.Id == userRefreshToken.UsreId).FirstOrDefaultAsync();
            var newJwtToken = CreateToken(user);
            var newRefreshToken = CreateRefreshToken();

            userRefreshToken.token = newRefreshToken;
            userRefreshToken.ExpirationDate = DateTime.Now.AddDays(7);
            await _context.SaveChangesAsync();

            return new TokenDto
            {
                JwtToken = newJwtToken,
                RefreshToken = newRefreshToken
            };


        }



        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role)
            
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var Token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(Token);
            return jwt;
        }


        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

        }




            private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
            {
                using (var hmac = new HMACSHA512(passwordSalt))
                {
                    var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                    return computedHash.SequenceEqual(passwordHash);
                }
            }





       private string CreateRefreshToken()
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var RefreshToken = Convert.ToBase64String(tokenBytes);
            var tokenInUse = _context.RefreshTokens.Any(c => c.token == RefreshToken);

            if (tokenInUse)
            {
                return CreateRefreshToken();

            }
            return RefreshToken;
        }




        private async Task InsertRefreshToken(int userId, string refreshToken)
        {
            var newRefreshToken = new RefreshToken
            {
                UsreId = userId,
                token = refreshToken,
                ExpirationDate = DateTime.Now.AddDays(7)
            };
            _context.RefreshTokens.Add(newRefreshToken);
            await _context.SaveChangesAsync();
        }




    }
}
