﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using FluentAssertions;
using AutoMapper;
using idp.Services;
using idp.Models;
using Xunit;

namespace idp_test
{
    public class LiteDBStorageServiceTest:TestBase
    {
        private readonly IPersistanceStorageService _db;
        private readonly IMapper _mapper;
        
        public LiteDBStorageServiceTest()
        {
            _mapper = _mapperConfiguration.CreateMapper();
            _db = new LiteDBStorageService(_config, _mapper);
        }

        [Fact]
        public void SaveAndFindNDIDUser()
        {
            NDIDUserModel user = new NDIDUserModel();
            user.NameSpace = "cid";
            user.Identifier = "1234";
            user.Accessors.Add(new NDIDAccessorModel
            {
                AccessorId = "hello",
                Secret = "this should be secret"
            });
            _db.SaveUser(user);
            
            NDIDUserModel retrievedUser = _db.FindUser(user.NameSpace, user.Identifier);
            retrievedUser.Should().BeEquivalentTo<NDIDUserModel>(user);
        }

        [Fact]
        public void SaveAndFindReference()
        {
            string referenceId = "6271DE23-9AAC-4B8E-B30B-56F19707B966";
            string key1 = "hello";
            string value1 = "world";
            string key2 = "alice";
            string value2 = "wonderland";
            _db.SaveReference(referenceId, key1, value1);
            string actual_value1 = _db.GetReferecne(referenceId, key1);
            actual_value1.Should().Equals(value1);
            _db.SaveReference(referenceId, key2, value2);
            actual_value1 = _db.GetReferecne(referenceId, key1);
            actual_value1.Should().Equals(value1);
            string actual_value2 = _db.GetReferecne(referenceId, key2);
            actual_value2.Should().Equals(value2);
        }

        // clean up data
        public override void Dispose()
        {
            string path = Path.Combine(_config.GetPersistancePath(), "idp.db");
            File.Delete(path);
        }
    }
}