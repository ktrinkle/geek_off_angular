namespace GeekOff.Models;

public enum QueryStatus
{
    Success,    // 200 (Ok)
    NotFound,   // 404 (Not Found)
    Conflict,   // 409 (Conflict)
    Forbidden,  // 403 (Forbidden)
    NoContent,  // 204 (No Content)
    BadRequest  // 400 (Bad Request)
}
