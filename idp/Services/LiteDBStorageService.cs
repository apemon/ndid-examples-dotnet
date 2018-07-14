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
        private const string COLLECTION_ACCESSORSIGN = "AccessorSigns";
        private const string COLLECTION_REFERENCE = "References";
        private const string COLLECTION_USER = "Users";
        private const string COLLECTION_REQUEST = "Requests";

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
                LiteCollection<NDIDUserModel> collection = db.GetCollection<NDIDUserModel>(COLLECTION_USER);
                NDIDUserModel model = collection.FindOne(x => x.NameSpace == namespaces && x.Identifier == identifier);
                return model;
            }
        }

        public string GetAccessorSign(string id)
        {
            using(LiteDatabase db = new LiteDatabase(_persistancePath))
            {
                LiteCollection<AccessorSignDBModel> collection = db.GetCollection<AccessorSignDBModel>(COLLECTION_ACCESSORSIGN);
                AccessorSignDBModel model = collection.Find(x => x.Key == id).FirstOrDefault();
                return model.Sid;
            }
        }

        public void RemoveAccessorSign(string id)
        {
            using (LiteDatabase db = new LiteDatabase(_persistancePath))
            {
                LiteCollection<AccessorSignDBModel> collection = db.GetCollection<AccessorSignDBModel>(COLLECTION_ACCESSORSIGN);
                collection.Delete(x => x.Key == id);
            }
        }

        public string GetReferecne(string referenceId, string type)
        {
            using (LiteDatabase db = new LiteDatabase(_persistancePath))
            {
                LiteCollection<ReferenceDBModel> collection = db.GetCollection<ReferenceDBModel>(COLLECTION_REFERENCE);
                ReferenceDBModel model = collection.FindOne(x => x.ReferenceId
                == referenceId && x.Type == type);
                return model.Value;
            }
        }

        public void SaveAccessorSign(string key, string sid)
        {
            using (LiteDatabase db = new LiteDatabase(_persistancePath))
            {
                LiteCollection<AccessorSignDBModel> collection = db.GetCollection<AccessorSignDBModel>(COLLECTION_ACCESSORSIGN);
                AccessorSignDBModel model = new AccessorSignDBModel();
                model.Key = key;
                model.Sid = sid;
                collection.Insert(model);
            }
        }

        public void SaveReference(string referenceId, string type, string value)
        {
            using (LiteDatabase db = new LiteDatabase(_persistancePath))
            {
                LiteCollection<ReferenceDBModel> collection = db.GetCollection<ReferenceDBModel>(COLLECTION_REFERENCE);
                ReferenceDBModel model = new ReferenceDBModel();
                model.ReferenceId = referenceId;
                model.Type = type;
                model.Value = value;
                collection.Insert(model);
            }
        }

        public long CreateNewUser(NDIDUserModel user)
        {
            using (LiteDatabase db = new LiteDatabase(_persistancePath))
            {
                LiteCollection<NDIDUserDBModel> collection = db.GetCollection<NDIDUserDBModel>(COLLECTION_USER);
                NDIDUserDBModel model = _mapper.Map<NDIDUserDBModel>(user);
                long id = collection.Insert(model);
                return id;
            }
        }

        public void RemoveReference(string referenceId)
        {
            using (LiteDatabase db = new LiteDatabase(_persistancePath))
            {
                LiteCollection<ReferenceDBModel> collection = db.GetCollection<ReferenceDBModel>(COLLECTION_REFERENCE);
                collection.Delete(x => x.ReferenceId == referenceId);
            }
        }

        public long SaveUserRequest(string namespaces, string identifier, string requestId, NDIDCallbackRequestModel request)
        {
            using (LiteDatabase db = new LiteDatabase(_persistancePath))
            {
                LiteCollection<NDIDUserRequestDBModel> collection = db.GetCollection<NDIDUserRequestDBModel>(COLLECTION_REQUEST);
                NDIDUserRequestDBModel model = _mapper.Map<NDIDUserRequestDBModel>(request);
                long id = collection.Insert(model);
                return id;
            }
        }

        public NDIDCallbackRequestModel GetUserRequest(string namespaces, string identifier, string requestId)
        {
            using (LiteDatabase db = new LiteDatabase(_persistancePath))
            {
                LiteCollection<NDIDUserRequestDBModel> collection = db.GetCollection<NDIDUserRequestDBModel>(COLLECTION_REQUEST);
                NDIDUserRequestDBModel request = collection.FindOne(x => x.Namespace == namespaces && x.Identifier == identifier && x.RequestId == requestId);
                NDIDCallbackRequestModel result = _mapper.Map<NDIDCallbackRequestModel>(request);
                return result;
            }
        }

        public List<NDIDCallbackRequestModel> ListUserRequest(string namespaces, string identifier)
        {
            using (LiteDatabase db = new LiteDatabase(_persistancePath))
            {
                LiteCollection<NDIDUserRequestDBModel> collection = db.GetCollection<NDIDUserRequestDBModel>(COLLECTION_REQUEST);
                List<NDIDUserRequestDBModel> requests = collection.Find(x => x.Namespace == namespaces && x.Identifier == identifier).ToList<NDIDUserRequestDBModel>();
                List<NDIDCallbackRequestModel> results = requests.Select(x => _mapper.Map<NDIDCallbackRequestModel>(x)).ToList<NDIDCallbackRequestModel>();
                return results;
            }
        }

        public void RemoveUserRequest(string requestId)
        {
            using (LiteDatabase db = new LiteDatabase(_persistancePath))
            {
                LiteCollection<NDIDUserRequestDBModel> collection = db.GetCollection<NDIDUserRequestDBModel>(COLLECTION_REQUEST);
                collection.Delete(x => x.RequestId == requestId);
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

    public class NDIDUserRequestDBModel
    {
        public long Id { get; set; }
        public string Namespace { get; set; }
        public string Identifier { get; set; }
        public string Type { get; set; }
        public int Mode { get; set; }
        public string RequestId { get; set; }
        public string RequestMsg { get; set; }
        public string RequestMsgHash { get; set; }
        public string RequesterNodeId { get; set; }
        public decimal MinIAL { get; set; }
        public decimal MinAAL { get; set; }
        public List<NDIDDataRequestModel> DataRequests { get; set; }
        public NDIDErrorModel Error { get; set; }
    }
}
