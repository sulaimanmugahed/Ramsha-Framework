
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Ramsha;
using Ramsha.AspNetCore;


namespace Ramsha.AspNetCore
{

}

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AspNetCoreServiceCollectionExtension
    {

        public static IWebHostEnvironment GetHostingEnvironment(this IServiceCollection services)
        {
            var hostingEnvironment = services.GetSingletonInstanceOrNull<IWebHostEnvironment>();

            if (hostingEnvironment == null)
            {
                return new EmptyHostingEnvironment()
                {
                    EnvironmentName = Environments.Development
                };
            }

            return hostingEnvironment;
        }
    }
}
