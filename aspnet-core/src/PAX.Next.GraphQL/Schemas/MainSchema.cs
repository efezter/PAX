using Abp.Dependency;
using GraphQL.Types;
using GraphQL.Utilities;
using PAX.Next.Queries.Container;
using System;

namespace PAX.Next.Schemas
{
    public class MainSchema : Schema, ITransientDependency
    {
        public MainSchema(IServiceProvider provider) :
            base(provider)
        {
            Query = provider.GetRequiredService<QueryContainer>();
        }
    }
}