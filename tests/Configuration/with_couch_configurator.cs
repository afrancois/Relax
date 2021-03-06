﻿using Machine.Specifications;
using Relax.Config;
using Symbiote.Core;

namespace Relax.Tests.Configuration
{
    public abstract class with_couch_configurator
    {
        protected static CouchConfigurator configurator;
        private Establish context = () =>
                                        {
                                            configurator = new CouchConfigurator();
                                            Assimilate.Initialize();
                                        };
    }
}