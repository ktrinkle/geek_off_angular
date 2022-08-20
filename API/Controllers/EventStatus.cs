using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GeekOff.Helpers;
using GeekOff.Models;
using GeekOff.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using Swashbuckle.AspNetCore.Annotations;

namespace GeekOff.Controllers
{
    [ApiController]
    [Route("api/eventstatus")]
    public class EventStatusController : ControllerBase
    {
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
        public async Task<ActionResult<string>> GetCurrentEventAsync()
            => Ok(await _manageEventService.GetCurrentEventAsync());

        [Authorize(Roles = "admin,player")]
        [HttpGet("currentQuestion/{yEvent}")]
        [SwaggerOperation(Summary = "Get the current question. Called when round1/contestant loads.")]
        public async Task<ActionResult<CurrentQuestionDto>> GetCurrentQuestionAsync(string yEvent)
            => Ok(await _manageEventService.GetCurrentQuestion(yEvent));

        [Authorize]
        [HttpGet("currentUser")]
        [SwaggerOperation(Summary = "Get current user and team info from database based on logged in user.")]
        public async Task<ActionResult<UserInfoDto>> GetUserInfoAsync()
        {
            var userId = User.UserId();
            return userId == null ? null : (ActionResult<UserInfoDto>)await _loginService.Login(userId);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("dollarAmount/{yEvent}/{teamNum}")]
        [SwaggerOperation(Summary = "Get current user and team info from database based on logged in user.")]
        public async Task<ActionResult<string>> UpdateFundAmountAsync(string yEvent, int teamNum, decimal? dollarAmount)
            => Ok(await _manageEventService.UpdateFundAmountAsync(yEvent, teamNum, dollarAmount));

    }
}
