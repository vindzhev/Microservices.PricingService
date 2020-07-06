namespace PricingService.API.Extensions
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.Infrastructure;

    using Microsoft.Extensions.DependencyInjection;

    public static class ApiBehaviourOptionsExtensions
    {
        public static void UseCustomInvalidModelUnprocessableEntityResponse(this ApiBehaviorOptions options)
        {
            options.InvalidModelStateResponseFactory = (actionContext) =>
            {
                ProblemDetailsFactory problemFactory = actionContext.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
                ProblemDetails problemDetails = problemFactory.CreateValidationProblemDetails(actionContext.HttpContext, actionContext.ModelState);

                problemDetails.Instance = actionContext.HttpContext.Request.Path;
                problemDetails.Detail = "See errors property for additional details.";

                var actionExecutingContext = actionContext as ActionExecutingContext;
                if (actionContext.ModelState.ErrorCount != 0 &&
                    (actionContext is ControllerContext || actionContext.ActionDescriptor.Parameters.Count == actionExecutingContext.ActionArguments.Count))
                {
                    problemDetails.Title = "One or more validation errors occured.";
                    problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                    problemDetails.Instance = $"{actionContext.HttpContext.Request.PathBase}/modelvalidationerror";

                    return new UnprocessableEntityObjectResult(problemDetails) { ContentTypes = { "application/problem+json" } };
                }

                problemDetails.Title = "One or more errors occured on the input";
                problemDetails.Status = StatusCodes.Status400BadRequest;

                return new BadRequestObjectResult(problemDetails) { ContentTypes = { "application/problem+json" } };
            };
        }
    }
}
