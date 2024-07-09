namespace GeekOff.Handlers;

public class FinalizeRoundHandler
{
    public class Request : IRequest<ApiResponse<StringReturn>>
    {
        public string YEvent { get; set; } = string.Empty;
        public int RoundNum { get; set; }
    }

    public class Handler(ContextGo contextGo) : IRequestHandler<Request, ApiResponse<StringReturn>>
    {
        private readonly ContextGo _contextGo = contextGo;

        public async Task<ApiResponse<StringReturn>> Handle(Request request, CancellationToken token)
        {
            var returnString = new StringReturn();

            // this condition should not be possible with MediaTr.
            if (request.YEvent is null)
            {
                returnString.Message = "No event was specified.";
                return ApiResponse<StringReturn>.BadRequest(returnString);
            }

            if (request.RoundNum is < 1 or > 3)
            {
                returnString.Message = "Incorrect round number.";
                return ApiResponse<StringReturn>.BadRequest(returnString);
            }

            var totalPoints = await _contextGo.Scoring.Where(s => s.RoundNum == request.RoundNum && s.Yevent == request.YEvent)
                                .GroupBy(s => s.TeamNum)
                                .Select(s => new
                                {
                                    TeamNum = s.Key,
                                    FinalScore = s.Sum(s => s.PointAmt)
                                }).ToListAsync(cancellationToken: token);

            if (totalPoints.Count == 0)
            {
                returnString.Message = "We found no scores for this event and round.";
                return ApiResponse<StringReturn>.NotFound(returnString);
            }

            // now we rank and store into the DB. First we remove anything that already exists.

            var scorestoRemove = await _contextGo.Roundresult.Where
                                    (r => r.Yevent == request.YEvent &&
                                    r.RoundNum == request.RoundNum)
                                    .ToListAsync(cancellationToken: token);

            if (scorestoRemove.Count > 0)
            {
                _contextGo.RemoveRange(scorestoRemove);
                await _contextGo.SaveChangesAsync(cancellationToken: token);
            }

            // add the new records
            var scorestoAdd = (from s in totalPoints
                               orderby s.FinalScore descending
                               select new Roundresult()
                               {
                                   Yevent = request.YEvent,
                                   TeamNum = s.TeamNum,
                                   RoundNum = request.RoundNum,
                                   Ptswithbonus = s.FinalScore,
                                   Rnk = (from r in totalPoints
                                          where r.FinalScore > s.FinalScore
                                          select r).Count() + 1
                               }).ToList();

            await _contextGo.Roundresult.AddRangeAsync(scorestoAdd, cancellationToken: token);
            await _contextGo.SaveChangesAsync(cancellationToken: token);

            returnString.Message = "The selected round was finalized and saved to the system.";
            return ApiResponse<StringReturn>.Success(returnString);
        }
    }
}
