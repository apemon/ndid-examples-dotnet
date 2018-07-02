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

        private IConfigurationService _config;
        private string _persistancePath;

        public LiteDBStorageService(IConfigurationService config)
        {
            _config = config;
            _persistancePath = Path.Combine(_config.GetPersistancePath(), "idp.db");
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
    }

    public class AccessorSignDBModel
    {
        public long Id { get; set; }
        public string Sid { get; set; }
        public string Key { get; set; }
    }
}
