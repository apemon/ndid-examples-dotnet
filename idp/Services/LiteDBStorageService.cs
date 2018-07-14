using AutoMapper;
using idp.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace idp.Services
{
    public class LiteDBStorageService : IPersistanceStorageService
    {
        private const string COLLECTION_ACCESSORSIGN = "AccessorSign";
        private const string COLLECTION_REFERENCE = "References";
        private const string COLLECTION_USER = "Users";

        private readonly IConfigurationService _config;
        private readonly IMapper _mapper;
        private string _persistancePath;

        public LiteDBStorageService(IConfigurationService config, IMapper mapper)
        {
            _config = config;
            _mapper = mapper;
            _persistancePath = Path.Combine(_config.GetPersistancePath(), "idp.db");
        }

        public NDIDUserModel FindUser(string namespaces, string identifier)
        {
            using (LiteDatabase db = new LiteDatabase(_persistancePath))
            {
                LiteCollection<NDIDUserModel> users = db.GetCollection<NDIDUserModel>(COLLECTION_USER);
                NDIDUserModel model = users.FindOne(x => x.NameSpace == namespaces && x.Identifier == identifier);
                return model;
            }
        }

        public string GetAccessorSign(string id)
        {
            using(LiteDatabase db = new LiteDatabase(_persistancePath))
            {
                LiteCollection<AccessorSignDBModel> accessorSigns = db.GetCollection<AccessorSignDBModel>(COLLECTION_ACCESSORSIGN);
                AccessorSignDBModel model = accessorSigns.Find(x => x.Key == id).FirstOrDefault();
                return model.Sid;
            }
        }

        public string GetReferecne(string referenceId, string type)
        {
            using (LiteDatabase db = new LiteDatabase(_persistancePath))
            {
                LiteCollection<ReferenceDBModel> references = db.GetCollection<ReferenceDBModel>(COLLECTION_REFERENCE);
                ReferenceDBModel model = references.FindOne(x => x.ReferenceId
                == referenceId && x.Type == type);
                return model.Value;
            }
        }

        public void SaveAccessorSign(string key, string sid)
        {
            using (LiteDatabase db = new LiteDatabase(_persistancePath))
            {
                LiteCollection<AccessorSignDBModel> accessorSigns = db.GetCollection<AccessorSignDBModel>(COLLECTION_ACCESSORSIGN);
                AccessorSignDBModel model = new AccessorSignDBModel();
                model.Key = key;
                model.Sid = sid;
                accessorSigns.Insert(model);
            }
        }

        public void SaveReference(string referenceId, string type, string value)
        {
            using (LiteDatabase db = new LiteDatabase(_persistancePath))
            {
                LiteCollection<ReferenceDBModel> references = db.GetCollection<ReferenceDBModel>(COLLECTION_REFERENCE);
                ReferenceDBModel model = new ReferenceDBModel();
                model.ReferenceId = referenceId;
                model.Type = type;
                model.Value = value;
                references.Insert(model);
            }
        }

        public long CreateNewUser(NDIDUserModel user)
        {
            using (LiteDatabase db = new LiteDatabase(_persistancePath))
            {
                LiteCollection<NDIDUserDBModel> users = db.GetCollection<NDIDUserDBModel>(COLLECTION_USER);
                NDIDUserDBModel model = _mapper.Map<NDIDUserDBModel>(user);
                long id = users.Insert(model);
                return id;
            }
        }

        public void RemoveReference(string referenceId)
        {
            using (LiteDatabase db = new LiteDatabase(_persistancePath))
            {
                LiteCollection<ReferenceDBModel> references = db.GetCollection<ReferenceDBModel>(COLLECTION_REFERENCE);
                references.Delete(x => x.ReferenceId == referenceId);
            }
        }
    }

    public class AccessorSignDBModel
    {
        public long Id { get; set; }
        public string Sid { get; set; }
        public string Key { get; set; }
    }

    public class ReferenceDBModel
    {
        public long Id { get; set; }
        public string ReferenceId { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }

    public class NDIDUserDBModel
    {
        public long Id { get; set; }
        public string NameSpace { get; set; }
        public string Identifier { get; set; }
        public List<NDIDAccessorModel> Accessors { get; set; }
    }
}
