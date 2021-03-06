﻿using idp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace idp.Services
{
    public class EnvironmentConfigurationService : IConfigurationService
    {
        private Dictionary<string, string> _configs;

        public EnvironmentConfigurationService()
        {
            _configs = new Dictionary<string, string>();
        }

        public string GetConfig(string key)
        {
            return _GetConfig(key);
        }

        public string GetKeyPath()
        {
            return _GetConfig("KEYPATH");
        }

        public string GetAPIServerAddress()
        {
            return _GetConfig("API_SERVER_ADDRESS");
        }

        public string GetPersistancePath()
        {
            return _GetConfig("PERSISTANCEPATH");
        }

        public string GetCallbackPath()
        {
            return _GetConfig("CALLBACK_URL");
        }

        public string GetEnvironment()
        {
            return _GetConfig("ASPNETCORE_ENVIRONMENT");
        }

        private string _GetConfig(string key)
        {
            if (!_configs.ContainsKey(key))
                _configs[key] = Environment.GetEnvironmentVariable(key);
            return _configs[key];
        }
    }
}
