﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace idp.Services
{
    public interface IDPKIService
    {
        Task GenNewKey(string keyName);
        Task<string> GetPubKey(string keyName);
        Task<string> Sign(string key, string text);
        Task UpdateKey(string oldKeyName, string newKeyName);
    }
}
