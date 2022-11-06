
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Entities;

namespace WebApi.Controllers
{
    public class BuggyController: BaseApiController
    {

        private WebContext _webcontext;
        public BuggyController(WebContext webContext)
        {
            _webcontext = webContext;
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret(){
            return "Secret Text";
        } 

        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound(){

            var thing = _webcontext.Users.Find(-1);
            if(thing == null ){
                return  NotFound();
            }

            return Ok(thing);

        } 

        [HttpGet("server-error")]
        public ActionResult<string> GetServerError(){

            var thing = _webcontext.Users.Find(-1);  // this will return null because -1 not exists
            var thingToReturn = thing.ToString();   // this will activate string of error 
            return thingToReturn;

        } 

        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest(){

            return BadRequest("Bad request...!");
            
        } 
           
        
    }
}