using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Auth;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
namespace Auth
{
    [Route("api/[action]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private IConfiguration _Configuration;
        private readonly Context _dbContext;
        private readonly IRepositoryWrapper _user;
        private readonly IMapper _mapper;
        public ValuesController(IConfiguration configuration, IRepositoryWrapper user, IMapper mapper, Context context)
        {
            _Configuration = configuration;
            _dbContext = context;
            _user = user;
            _mapper = mapper;
        }
        [Authorize]
        [HttpGet("{id}", Name = "GetUserById")]

        public ActionResult<UserReadDto> GetUserById(Guid Id)
        {
            var result = _dbContext.User.FirstOrDefault(x => x.Id == Id);
            if (result != null)
            {
                return Ok(result);

            }
            return NotFound();
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("{PageNumber}/{count}")]
        public async Task<ActionResult<UserReadDto>> GetAllUsers(int PageNumber, int count = 10)
        {
            var Result = await _user.User.FindAll(PageNumber, count);
            var UserModel = _mapper.Map<IList<UserReadDto>>(Result);
            return Ok(UserModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<UserReadDto>> Create(UserCreateDto userCreateDto)
        {
            var UserModel = _mapper.Map<User>(userCreateDto);
            var User = _user.User.FindById(userCreateDto.Id);
            if (User !=null)
            {
                return BadRequest("User is Exist");
            }
            await _user.User.Create(UserModel);
            var UserReadDto = _mapper.Map<UserReadDto>(UserModel);
            return CreatedAtRoute(nameof(GetUserById), new { Id = UserReadDto.Id }, UserReadDto);
        }
        [Authorize]
        [HttpPut("{id}")]
        public ActionResult Update(Guid id,[FromBody] UserUpdateDto userUpdateDto)
        {
            var UserModelFromRepo = _user.User.FindById(id);
            if (UserModelFromRepo == null)
            {
                return NotFound();
            }
            UserModelFromRepo.Result.UserName = userUpdateDto.UserName;
            UserModelFromRepo.Result.Password = userUpdateDto.Password;
            UserModelFromRepo.Result.Role = userUpdateDto.Role;
            UserModelFromRepo.Result.Usertype = userUpdateDto.Usertype;
            UserModelFromRepo.Result.Province = userUpdateDto.Province;
            _user.User.SaveChanges();
            return NoContent();
        }
        [HttpPatch("{id}")]
        public ActionResult PartialCommandUpdate(Guid id, JsonPatchDocument<UserUpdateDto> patchDoc)
        {
           // var commandModelFromRepo = _user.read(id);
           // if (commandModelFromRepo == null)
            {
                return NotFound();
            }

          //  var commandToPatch = _mapper.Map<UserUpdateDto>(commandModelFromRepo);
          /*  patchDoc.ApplyTo(commandToPatch, (Microsoft.AspNetCore.JsonPatch.Adapters.IObjectAdapter)ModelState);

            if (!TryValidateModel(commandToPatch))
            {
                return ValidationProblem(ModelState);
            }*/

          // _mapper.Map(commandToPatch, commandModelFromRepo);

           // _user.update(commandModelFromRepo);

           // _user.SaveChanges();

            return NoContent();
        }
        [HttpPost]
        public async Task<ActionResult<User>> Login([FromBody] User form)
        {
            var user =await _dbContext.User.FirstOrDefaultAsync(x=>x.UserName==form.UserName&& x.Password==form.Password);
            if (user != null)
            {
                var claims = new[]
                {
                   new Claim("Username", user.UserName),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim("Role", user.Role),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                var token = new JwtSecurityToken(claims: claims, expires: DateTime.UtcNow.AddDays(30),
                  notBefore: DateTime.UtcNow, audience: "Audience", issuer: "Issuer",
                  signingCredentials: new SigningCredentials(
                      new SymmetricSecurityKey(
                          Encoding.UTF8.GetBytes("Hlkjds0-324mf34pojf-14r34fwlknef0943")),
                      SecurityAlgorithms.HmacSha256));
                var Token = new JwtSecurityTokenHandler().WriteToken(token);
                var expire = DateTime.UtcNow.AddDays(30);
                return Ok(new {Token=Token,Expire= expire});

            }
            else return BadRequest();

        }
    }
}
