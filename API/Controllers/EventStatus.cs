using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using Microsoft.Identity.Web.Resource;
using GeekOff.Helpers;
using GeekOff.Services;
using GeekOff.Models;

namespace GeekOff.Controllers
{
    [ApiController]
    [Route("api/eventstatus")]
    public class EventStatusController : ControllerBase
    {
        static readonly string[] scopeRequiredByApi = new string[] { "access_as_user" };
        private readonly ILogger<EventStatusController> _logger;
        private readonly IManageEventService _manageEventService;
        private readonly ILoginService _loginService;

        public EventStatusController(ILogger<EventStatusController> logger, ILoginService loginService, IManageEventService manageEventService)
        {
            _logger = logger;
            _manageEventService = manageEventService;
            _loginService = loginService;
        }

        [HttpGet("currentEvent")]
        [SwaggerOperation(Summary = "Get the current event. Called as part of app.component.ts.")]
        public async Task<ActionResult<string>> GetCurrentEvent(string yEvent, int questionId)
            => Ok(await _manageEventService.GetCurrentEvent());

        [Authorize(Roles = "admin,player")]
        [HttpGet("currentQuestion/{yEvent}")]
        [SwaggerOperation(Summary = "Get the current question. Called when round1/contestant loads.")]
        public async Task<ActionResult<CurrentQuestionDto>> GetCurrentQuestion(string yEvent)
            => Ok(await _manageEventService.GetCurrentQuestion(yEvent));

        [Authorize]
        [HttpGet("currentUser")]
        [SwaggerOperation(Summary = "Get current user and team info from database based on logged in user.")]
        public async Task<ActionResult<UserInfoDto>> GetUserInfo()
        {
            var userId = User.UserId();
            if (userId == "000000")
            {
                return null;
            }

            return await _loginService.Login(userId);
        }
        
    }
}