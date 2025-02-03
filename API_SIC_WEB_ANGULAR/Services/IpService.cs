using API_SIC_WEB_ANGULAR.Interfaces;

namespace API_SIC_WEB_ANGULAR.Services
{
    public class IpService : IpInterface
    {
        public string GetIp(HttpContext httpContextRequest)
        {
            var forwardedHeaderFF = httpContextRequest.Request.Headers["X-Forwarded-For"].FirstOrDefault();

            return forwardedHeaderFF?.Split(',')[0] ?? httpContextRequest.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
        }
    }
}
