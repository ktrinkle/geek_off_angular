namespace GeekOff.Handlers;

public class ResetEventHandler
{
    public class Request : IRequest<ApiResponse<StringReturn>>
    {
        public string YEvent { get; set; } = string.Empty;
    }

    public class Handler(ContextGo contextGo) : IRequestHandler<Request, ApiResponse<StringReturn>>
    {
        private readonly ContextGo _contextGo = contextGo;

        public async Task<ApiResponse<StringReturn>> Handle(Request request, CancellationToken token)
        {
            var returnString = new StringReturn();

            // YEvent cannot be null, so removed the null check.

            // set up stuff to remove
            var currentQuestion = new CurrentQuestion()
            {
                YEvent = request.YEvent
            };

            var roundResult = new Roundresult()
            {
                Yevent = request.YEvent
            };

            var score = new Scoring()
            {
                Yevent = request.YEvent
            };

            var userAnswer = new UserAnswer()
            {
                Yevent = request.YEvent
            };

            _contextGo.CurrentQuestion.Remove(currentQuestion);
            _contextGo.Roundresult.Remove(roundResult);
            _contextGo.Scoring.Remove(score);
            _contextGo.UserAnswer.Remove(userAnswer);

            await _contextGo.SaveChangesAsync(token);

            returnString.Message = $"Event {request.YEvent} results were removed from the system.";
            return ApiResponse<StringReturn>.Success(returnString);
        }
    }
}
