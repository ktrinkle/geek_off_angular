// public async Task<ActionResult<ApiResponse>> MoveTeamNumberAsync(string yEvent, int oldTeamNum, int newTeamNum)
//        => Ok(await _teamService.MoveTeamNumberAsync(yEvent, oldTeamNum, newTeamNum));

using MediatR.Pipeline;

namespace GeekOff.Handlers;

public class MoveTeamHandler
{
    public class Request : IRequest<ApiResponse<StringReturn>>
    {
        public string YEvent { get; set; } = string.Empty;
        public int OldTeamNum { get; set; }
        public int NewTeamNum { get; set;}
    }

    public class Handler(ContextGo contextGo) : IRequestHandler<Request, ApiResponse<StringReturn>>
    {
        private readonly ContextGo _contextGo = contextGo;

        public async Task<ApiResponse<StringReturn>> Handle(Request request, CancellationToken token)
        {
            var returnString = new StringReturn();
            var existTeam = await _contextGo.Teamreference
                .FirstOrDefaultAsync(tr => tr.TeamNum == request.OldTeamNum
                    && tr.Yevent == request.YEvent);

            if (existTeam is null)
            {
                returnString.Message = "The original team number does not exist.";
                return ApiResponse<StringReturn>.NotFound(returnString);
            }

            var newTeamFlag = await _contextGo.Teamreference
                .AnyAsync(tr => tr.TeamNum == request.NewTeamNum && tr.Yevent == request.YEvent
                    , cancellationToken: token);

            if (newTeamFlag)
            {
                returnString.Message = "The new team number already exists.";
                return ApiResponse<StringReturn>.Conflict(returnString);
            }

            existTeam.TeamNum = request.NewTeamNum;

            var teamUsers = await _contextGo.TeamUser
                .Where(tu => tu.TeamNum == request.OldTeamNum && tu.Yevent == request.YEvent)
                .ToListAsync(cancellationToken: token);

            foreach(var teamUser in teamUsers)
            {
                teamUser.TeamNum = request.NewTeamNum;
            }

            var scores = await _contextGo.Scoring
                .Where(s => s.TeamNum == request.OldTeamNum && s.Yevent == request.YEvent)
                .ToListAsync(cancellationToken: token);

            foreach(var score in scores)
            {
                score.TeamNum = request.NewTeamNum;
            }

            var userAnswers = await _contextGo.UserAnswer
                .Where(u => u.TeamNum == request.OldTeamNum && u.Yevent == request.YEvent)
                .ToListAsync(cancellationToken: token);

            foreach(var userAnswer in userAnswers)
            {
                userAnswer.TeamNum = request.NewTeamNum;
            }

            var roundResults = await _contextGo.Roundresult
                .Where(u => u.TeamNum == request.OldTeamNum && u.Yevent == request.YEvent)
                .ToListAsync(cancellationToken: token);

            foreach(var roundResult in roundResults)
            {
                roundResult.TeamNum = request.NewTeamNum;
            }

            _contextGo.Update(existTeam);
            _contextGo.UpdateRange(teamUsers);
            _contextGo.UpdateRange(scores);
            _contextGo.UpdateRange(userAnswers);
            _contextGo.UpdateRange(roundResults);
            await _contextGo.SaveChangesAsync(token);

            returnString.Message = "The team was successfully moved to the new number.";
            return ApiResponse<StringReturn>.Success(returnString);
        }
    }
}
