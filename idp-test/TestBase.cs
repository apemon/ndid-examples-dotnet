using AutoMapper;
using idp.Models;
using idp.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace idp_test
{
    public class TestBase:IDisposable
    {
        protected readonly IConfigurationService _config;
        protected AutoMapper.MapperConfiguration _mapperConfiguration { get; set; }

        public TestBase()
        {
            // initalize environment variable
            using (var file = File.OpenText("Properties\\launchSettings.json"))
            {
                var reader = new JsonTextReader(file);
                var jObject = JObject.Load(reader);

                var variables = jObject
                    .GetValue("profiles")
                    //select a proper profile here
                    .SelectMany(profiles => profiles.Children())
                    .SelectMany(profile => profile.Children<JProperty>())
                    .Where(prop => prop.Name == "environmentVariables")
                    .SelectMany(prop => prop.Value.Children<JProperty>())
                    .ToList();

                foreach (var variable in variables)
                {
                    Environment.SetEnvironmentVariable(variable.Name, variable.Value.ToString());
                }
            }
            _config = new EnvironmentConfigurationService();
            // initailize mapper
            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new NDIDMapperConfiguration());
            });
        }

        public virtual void Dispose()
        {
            // some global tear down here
        }
    }
}
