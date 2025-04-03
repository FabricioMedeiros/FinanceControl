using FinanceControl.API.Extensions;
using FinanceControl.Application.Interfaces;
using FinanceControl.Application.Notifications;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FinanceControl.API.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        private readonly INotificator _notificator;
        private readonly IServiceProvider _serviceProvider;
        public ValidationFilter(IServiceProvider serviceProvider, INotificator notificator)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _notificator = notificator ?? throw new ArgumentNullException(nameof(notificator));
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Console.WriteLine("ValidationFilter está executando...");

            foreach (var argument in context.ActionArguments.Values)
            {
                if (argument is null) continue; 

                var validator = GetValidator(argument, context);

                if (validator is null)
                {
                    Console.WriteLine($"Nenhum validador encontrado para o tipo {argument.GetType().Name}. Ignorando.");
                    continue; 
                }

                try
                {
                    var validationResult = await validator.ValidateAsync(new ValidationContext<object>(argument));
                    if (!validationResult.IsValid)
                    {
                        AddNotifications(validationResult);
                        CreateResponse(context);
                        return; 
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro durante a validação do tipo {argument.GetType().Name}: {ex.Message}");
                    throw; 
                }
            }

            await next();
        }

        private IValidator? GetValidator(object argument, ActionExecutingContext context)
        {
            var argumentType = argument.GetType();

            var controllerActionDescriptor = context.ActionDescriptor as Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor;
            if (controllerActionDescriptor != null)
            {
                var validationContextAttribute = controllerActionDescriptor.MethodInfo
                    .GetCustomAttributes(typeof(ValidationContextAttribute), false)
                    .FirstOrDefault() as ValidationContextAttribute;

                if (validationContextAttribute != null)
                {
                    var validatorType = validationContextAttribute.ValidatorType;

                    if (validatorType.GetInterface($"IValidator`1")?.GetGenericArguments().FirstOrDefault() == argumentType)
                    {
                        var validator = _serviceProvider.GetService(validatorType) as IValidator;
                        Console.WriteLine($"Validador encontrado: {validator?.GetType().Name} para {argumentType.Name}");
                        return validator;
                    }
                }
            }

            Console.WriteLine($"Nenhum validador compatível encontrado para o tipo: {argumentType.Name}");
            return null; 
        }
        private void AddNotifications(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                _notificator.AddNotification(new Notification(error.ErrorMessage));
            }
        }

        private void CreateResponse(ActionExecutingContext context)
        {
            var errors = _notificator.GetNotifications().Select(n => n.Message);
            context.Result = new BadRequestObjectResult(new { success = false, errors });
        }
    }
}
