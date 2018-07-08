using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace idp.Services
{
    public interface IConfigurationService
    {
        string GetConfig(string key);
        string GetKeyPath();
        string GetAPIServerAddress();
        string GetPersistancePath();
        string GetCallbackPath();
    }
}
