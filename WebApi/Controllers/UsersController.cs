
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Entities;
using WebApi.Data;
using Microsoft.AspNetCore.Authorization;
using WebApi.interfaces;
using WebApi.DTOs;
using AutoMapper;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [Authorize]
    public class UsersController:BaseApiController
    {
        private IUserRepository _userRepository;
        private IMapper _mapper;
        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers(){
            
            var users = await _userRepository.GetUsersAsync();
             var userMapp = _mapper.Map<IEnumerable<MemberDto>>(users);
            if(users == null){
                return NotFound();
            }

            return Ok(userMapp);
                
        }
    

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUserName(string username){

            var user = await _userRepository.GetUserByUsernameAsync(username);
            var mapUser = _mapper.Map<MemberDto>(user);
            return Ok(mapUser);
        }


        [HttpPut]

        public async Task<ActionResult> UpdateUser(MemberUpdateDto request){

            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userRepository.GetUserByUsernameAsync(username);
            _mapper.Map(request, user);

            _userRepository.Update(user);

            if(await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Fails");
        }





        
    }
}