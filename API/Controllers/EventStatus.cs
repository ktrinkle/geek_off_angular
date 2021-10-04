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
        private readonly IScoreService _scoreService;
        private readonly IManageEventService _manageEventService;
        private readonly IHubContext<EventHub> _eventHub;
        private readonly IQuestionService _questionService;

        public EventStatusController(ILogger<EventStatusController> logger, IScoreService scoreService, IManageEventService manageEventService,
                                IHubContext<EventHub> eventHub, IQuestionService questionService)
        {
            _logger = logger;
            _scoreService = scoreService;
            _manageEventService = manageEventService;
            _eventHub = eventHub;
            _questionService = questionService;
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
        
    }
}