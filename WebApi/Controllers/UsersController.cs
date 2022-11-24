
using Microsoft.AspNetCore.Mvc;
using WebApi.Entities;
using Microsoft.AspNetCore.Authorization;
using WebApi.interfaces;
using WebApi.DTOs;
using AutoMapper;
using WebApi.Extenstions;

namespace WebApi.Controllers
{
    [Authorize]
    public class UsersController:BaseApiController
    {
        private IUserRepository _userRepository;
        private IMapper _mapper;
        private IphotoService _iphotoService;
        public UsersController(IUserRepository userRepository, IMapper mapper, IphotoService photoService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _iphotoService = photoService;
        }


        // we get all users and turn it to Member
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers(){
            
            var users = await _userRepository.GetUsersAsync();
             var userMapp = _mapper.Map<IEnumerable<MemberDto>>(users);
            if(users == null){
                return NotFound();
            }

            return Ok(userMapp);
                
        }
    
        [HttpGet("{username}", Name ="GetUser")]
        public async Task<ActionResult<MemberDto>> GetUserName(string username){

            var user = await _userRepository.GetUserByUsernameAsync(username);
            var mapUser = _mapper.Map<MemberDto>(user);
            return Ok(mapUser);
        }


        [HttpPut]

        public async Task<ActionResult> UpdateUser(MemberUpdateDto request){

            // var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var username = User.GetUsername();
            var user = await _userRepository.GetUserByUsernameAsync(username);
            _mapper.Map(request, user);

            _userRepository.Update(user);

            if(await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Fails");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file){
            //Check user is current LoggedIn user
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            // Add photo to cloudinary and get back result
            var result = await _iphotoService.AddPhotoAsync(file);

            if(result.Error != null) return BadRequest(result.Error.Message);

            // create a new photo so we put its url into our database
            var photo = new Photo{
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };
            if(user.Photos.Count == 0){
                photo.IsMain = true;
            }

            // add photo to Database
            user.Photos.Add(photo);
            if(await _userRepository.SaveAllAsync()) {
                return CreatedAtRoute("GetUser", new {username = user.UserName}, _mapper.Map<PhotoDto>(photo));
            }

            return BadRequest("Problem adding photo");


        }
        
         [HttpPut("set-main-photo/{photoId}")]

            public async Task<ActionResult> SetMainPhoto(int photoId){
                // get current logged-in user
                var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
                // access user relationship with photo base on photo id
                var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

                if(photo.IsMain) return BadRequest("already");

                var currentMain = user.Photos.FirstOrDefault(x =>x.IsMain);
                if(currentMain != null) currentMain.IsMain = false;
                photo.IsMain = true;
                if(await _userRepository.SaveAllAsync()) return NoContent();
                return BadRequest("Failed to set");
            }
        
        [HttpDelete("delete-photo/{photoId}")]
            public async Task<ActionResult> DeletePhoto(int photoId){
                var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
                var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
                if(photo == null) return NotFound();

                if(photo.IsMain) return BadRequest("Cant delete main");
                if(photo.PublicId != null){
                   var result = await _iphotoService.DeletePhotoAsync(photo.PublicId);
                   if(result.Error != null) return BadRequest("Erro");

                }

                user.Photos.Remove(photo);
                if(await _userRepository.SaveAllAsync()) return Ok();

                return BadRequest("Fail to dele");



            }
        
    }
}