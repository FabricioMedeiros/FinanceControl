using FinanceControl.Application.Interfaces;
using FinanceControl.Application.Notifications;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace FinanceControl.Application.Services
{
    public abstract class BaseService
    {
        private readonly INotificator _notificator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        protected BaseService(
            INotificator notificator,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _notificator = notificator;
            _httpContextAccessor = httpContextAccessor;
        }

        protected void Notify(string message)
        {
            _notificator.AddNotification(new Notification(message));
        }

        protected bool IsValidOperation()
        {
            return !_notificator.HasNotification();
        }

        public Guid GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirstValue("userId");

            if (string.IsNullOrEmpty(userIdClaim))
                throw new UnauthorizedAccessException("Usuário não autenticado ou claim 'userId' não encontrado.");

            if (!Guid.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedAccessException("Claim 'userId' inválido.");

            return userId;
        }
    }
}
