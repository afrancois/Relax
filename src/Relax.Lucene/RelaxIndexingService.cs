﻿using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using Symbiote.Core.Extensions;
using Symbiote.Daemon;
using Symbiote.Lucene;
using Symbiote.Restfully.Impl;

namespace Relax.Lucene
{
    public class RelaxIndexingService
        : IDaemon
    {
        protected IRelaxLuceneConfiguration configuration { get; set; }
        protected ILuceneServiceFactory luceneServiceFactory { get; set; }
        protected IDocumentRepository repository { get; set; }
        protected IHttpServer server { get; set; }
        
        public void Start()
        {
            server.Start();
            configuration
                .Databases
                .ForEach(x => repository.HandleUpdates(x, 0, OnChange, null));
        }

        public void Stop()
        {
            server.Stop();
            configuration
                .Databases
                .ForEach(x => repository.StopChangeStreaming(x));
        }

        protected void OnChange(string database, ChangeRecord record)
        {
            "Indexing document id '{0}', sequence {1}"
                .ToInfo<RelaxIndexingService>(record.Id, record.Sequence);

            var indexer = luceneServiceFactory.GetIndexingObserverForIndex(database);
            var visitor = new JsonVisitor();
            visitor.Subscribe(indexer);
            visitor.Accept(record.Document);
        }

        public RelaxIndexingService(
            IRelaxLuceneConfiguration configuration,
            IDocumentRepository respository,
            ILuceneServiceFactory serviceFactory,
            IHttpServer server)
        {
            this.configuration = configuration;
            this.repository = respository;
            this.luceneServiceFactory = serviceFactory;
            this.server = server;
        }
    }
}