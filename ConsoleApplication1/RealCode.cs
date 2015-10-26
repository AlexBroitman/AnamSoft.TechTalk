using System;
using System.Collections.Generic;
using System.Linq;
using CacheEnumerable;

namespace ConsoleApplication1
{
    class RealCode
    {
        // the below method is real code from SalesForceIntegration.cs
        private IEnumerable<object> FilterEntitiesByLastModified(IEnumerable<EntityIdAndLastModified> syncRegistryObjects,
            IEnumerable<EntityIdAndLastModified> forigenEntities, DateTime lastModifiedMap)
        {
            
            foreach (var syncRegistryObject in syncRegistryObjects)
            {
                var foriegnEntity = forigenEntities.FirstOrDefault(o => o.Id.Equals(syncRegistryObject.Id));
                if (foriegnEntity != null)
                {
                    if (foriegnEntity.IsDeleted || foriegnEntity.LastModified >= syncRegistryObject.LastModified || syncRegistryObject.LastModified <= lastModifiedMap)
                        yield return syncRegistryObject.Id;
                }
            }
        }
    }

    public class EntityIdAndLastModified
    {
        public string Id { get; set; }
        public DateTime? LastModified { get; set; }
        public bool IsDeleted { get; set; }
    }
}
