using System;
using System.Collections.Generic;
using System.Linq;
using CacheEnumerable;

namespace ConsoleApplication1
{
    class BetterCode
    {
        private IEnumerable<object> FilterEntitiesByLastModified(IEnumerable<EntityIdAndLastModified> syncRegistryObjects, IEnumerable<EntityIdAndLastModified> forigenEntities, DateTime lastModifiedMap)
        {
            forigenEntities = forigenEntities.AsCacheEnumerable();

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
}
