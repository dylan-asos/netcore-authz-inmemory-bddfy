using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace Asos.DotNetCore.Auth.Api.ComponentTests
{
    public class ComponentTestContext
    {
        public IConfigurationRoot ComponentConfigSettings { get; set; }

        public HttpClient Client { get; set; }

        public BearerTokenBuilder TokenBuilder { get; set; }
    }
}