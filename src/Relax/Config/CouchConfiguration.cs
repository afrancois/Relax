using System;
using System.Collections.Generic;
using Relax.Impl.Serialization;

namespace Relax.Config
{
    public class CouchConfiguration : ICouchConfiguration
    {
        protected Dictionary<Type, string> _databaseForType = new Dictionary<Type, string>();

        public string GetDatabaseNameForType<T>()
        {
            string dbname = null;
            if(DatabaseResolver != null)
            {
                dbname = DatabaseResolver.GetDatabaseNameFor<T>();
            }
            if(dbname == null)
            {
                var type = typeof(T);
                _databaseForType.TryGetValue(type, out dbname);
            }
            return (string.IsNullOrEmpty(dbname) ? DefaultDatabaseName : dbname).ToLower();
        }
        
        public void SetDatabaseNameForType<T>(string databaseName)
        {
            _databaseForType[typeof (T)] = databaseName.ToLower();
        }
        public IResolveDatabaseNames DatabaseResolver { get; set; }
        public string DefaultDatabaseName { get; set; }
        public bool BreakDownDocumentGraphs { get; set; }
        public string Protocol { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
        public bool Preauthorize { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public int TimeOut { get; set; }
        public DocumentConventions Conventions { get; set; }
        public bool Cache { get; set; }
        public DateTime CacheExpiration { get; set; }
        public TimeSpan CacheLimit { get; set; }
        public bool Throw404Exceptions { get; set; }
        public bool IncludeTypeSpecification { get; set; }
        public string RelaxQueryServiceUrl { get; set; }

        public CouchConfiguration()
        {
            Protocol = "http";
            Server = "localhost";
            Port = 5984;
            Preauthorize = false;
            TimeOut = 6000;
            IncludeTypeSpecification = true;
            RelaxQueryServiceUrl = @"http://localhost:8420/";
            Conventions = new DocumentConventions();
            DefaultDatabaseName = 
                (System.Reflection.Assembly.GetEntryAssembly() ??
                System.Reflection.Assembly.GetExecutingAssembly()).GetName().Name.Replace(".", "");
        }
    }
}