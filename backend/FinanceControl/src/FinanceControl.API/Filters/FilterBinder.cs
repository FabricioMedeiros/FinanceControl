using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FinanceControl.API.Filters
{
    public class FilterBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null) throw new ArgumentNullException(nameof(bindingContext));

            var filters = bindingContext.HttpContext.Request.Query
                .Where(q => !string.IsNullOrWhiteSpace(q.Value))
                .ToDictionary(
                    q => q.Key.Replace("[", ".").Replace("]", ""),
                    q => q.Value.ToString()
                );

            bindingContext.Result = ModelBindingResult.Success(filters);
            return Task.CompletedTask;
        }
    }
}
