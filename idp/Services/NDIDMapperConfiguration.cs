using AutoMapper;
using idp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace idp.Services
{
    public class NDIDMapperConfiguration: Profile
    {
        public NDIDMapperConfiguration()
        {
            CreateMap<NDIDUserModel, NDIDUserDBModel>();
        }
    }
}
