using Microsoft.Extensions.Configuration;
using System.Linq;

namespace My.ServiceWatchdog.Extensions
{
    public static class ExtentionHelpers
    {
        public static string UnderMye(this string value)
            => string.Concat(value.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString()));

        public static TModel GetOptions<TModel>(this IConfiguration configuration, string section) where TModel : new()
        {
            var model = new TModel();
            configuration.GetSection(section).Bind(model);

            return model;
        }
    }
}