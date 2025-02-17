using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using FinanceControl.Application.Notifications;
using FinanceControl.Application.Interfaces;

namespace FinanceControl.API.Controllers
{
    [ApiController]
    public abstract class MainController : ControllerBase
    {
        private readonly INotificator _notificator;
        protected string UserId => User?.FindFirst("userId")?.Value ?? string.Empty;

        protected MainController(INotificator notificator)
        {
            _notificator = notificator;
        }

        protected bool IsValid()
        {
            return !_notificator.HasNotification();
        }

        protected ActionResult CustomResponse(object? result = null)
        {
            if (IsValid())
            {
                return Ok(new
                {
                    success = true,
                    data = result
                });
            }

            return BadRequest(new
            {
                success = false,
                errors = _notificator.GetNotifications().Select(n => n.Message)
            });
        }

        protected void CustomResponse(ModelStateDictionary modelState)
        {
            var errors = modelState.Values.SelectMany(e => e.Errors);
            foreach (var error in errors)
            {
                var errorMsg = error.Exception == null ? error.ErrorMessage : error.Exception.Message;
                NotifyError(errorMsg);
            }

            CustomResponse();
        }

        protected ActionResult CustomResponse(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                NotifyError(error.ErrorMessage);
            }

            return CustomResponse();
        }

        protected void NotifyError(string message)
        {
            _notificator.AddNotification(new Notification(message));
        }
    }
}
