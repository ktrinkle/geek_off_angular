using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GeekOff.Entities;
using GeekOff.Helpers;
using GeekOff.Models;
using GeekOff.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using Swashbuckle.AspNetCore.Annotations;
using GeekOff.Data;

namespace GeekOff.Controllers
{
    [ApiController]
    [Route("api/eventmanage")]
    public class EventManageController : ControllerBase
    {
        private readonly ILogger<EventManageController> _logger;
        private readonly IManageEventService _manageEventService;
        private readonly ITeamService _teamService;

        public EventManageController(ILogger<EventManageController> logger, IManageEventService manageEventService,
                ITeamService teamService)
        {
            _logger = logger;
            _manageEventService = manageEventService;
            _teamService = teamService;
        }

        [Authorize(Roles = "admin")]
        [HttpGet("eventList")]
        [SwaggerOperation(Summary = "Get a list of all events.")]
        public async Task<ActionResult<List<EventMaster>>> GetEventListAsync()
            => Ok(await _manageEventService.GetAllEventsAsync());

        [Authorize(Roles = "admin")]
        [HttpPost("addEvent")]
        [SwaggerOperation(Summary = "Add a new event.")]
        public async Task<ActionResult<ApiResponse>> AddEventAsync(EventMaster newEvent)
        {
            var response = await _manageEventService.AddEventAsync(newEvent);

            return response.SuccessInd ? Ok(response.Response) : BadRequest(response.Response);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("setEvent/{yEvent}")]
        [SwaggerOperation(Summary = "Set the current event. Requires yEvent to be populated.")]
        public async Task<ActionResult<ApiResponse>> GetCurrentQuestionAsync(string yEvent)
        {
            var response = await _manageEventService.SetCurrentEventAsync(yEvent);

            return response.SuccessInd ? Ok(response.Response) : NotFound(response.Response);
        }


        [Authorize(Roles = "admin")]
        [HttpPut("dollarAmount/{yEvent}/{teamNum}")]
        [SwaggerOperation(Summary = "Get current user and team info from database based on logged in user.")]
        public async Task<ActionResult<string>> UpdateFundAmountAsync(string yEvent, int teamNum, decimal? dollarAmount)
            => Ok(await _manageEventService.UpdateFundAmountAsync(yEvent, teamNum, dollarAmount));

        [Authorize(Roles = "admin")]
        [HttpPut("cleanEvent/{yEvent}")]
        [SwaggerOperation(Summary = "Clean all results out of system for this event.")]
        public async Task<ActionResult<string>> ResetEventAsync(string yEvent)
            => Ok(await _manageEventService.ResetEvent(yEvent));

        [Authorize(Roles = "admin")]
        [HttpPut("createTeam/{yEvent}/{teamName}")]
        [SwaggerOperation(Summary = "Create a new team for the event")]
        public async Task<ActionResult<NewTeamEntry>> AddNewEventTeamAsync(string yEvent, string? teamName)
            => Ok(await _teamService.AddNewEventTeamAsync(yEvent, teamName));

    }
}
