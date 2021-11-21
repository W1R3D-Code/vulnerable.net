using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.RegularExpressions;
using Vulnerable.Common.Enums;

namespace Vulnerable.MVC.Filters;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
public class ChallengeFilter : Attribute, IAsyncActionFilter
{
    private readonly string _challengeName;
    private readonly ChallengeCategory _challengeCategory;
    private readonly Regex _challengeRegex;

    public ChallengeFilter(string ChallengeName, ChallengeCategory challengeCategory, string challengeRegex)
    {
        if (string.IsNullOrEmpty(challengeRegex))
            throw new ArgumentNullException(nameof(challengeRegex));

        _challengeName = ChallengeName;
        _challengeCategory = challengeCategory;
        _challengeRegex = new Regex(challengeRegex);
    }

    public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (string.Equals(context.HttpContext.Request.Method, "GET", StringComparison.OrdinalIgnoreCase))
        {
            var query = context.HttpContext.Request.Query;

            if (query != null && query.Any(x => _challengeRegex.IsMatch(x.Value)))
            {
                //  TODO:: Handle challenge success
            }
        }

        return next();
    }
}
