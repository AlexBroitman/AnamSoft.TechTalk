using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApplication1
{
    class EvenBetterCode
    {
        private IEnumerable<object> FilterEntitiesByLastModified(IEnumerable<EntityIdAndLastModified> syncRegistryObjects
            , IEnumerable<EntityIdAndLastModified> forigenEntities, DateTime lastModifiedMap)
        {
            var foreignDic = forigenEntities.ToDictionary(e => e.Id);

            foreach (var syncRegistryObject in syncRegistryObjects)
            {
                if (foreignDic.ContainsKey(syncRegistryObject.Id))
                {
                    var foreignEntity = foreignDic[syncRegistryObject.Id];
                    if (foreignEntity.IsDeleted || foreignEntity.LastModified >= syncRegistryObject.LastModified || syncRegistryObject.LastModified <= lastModifiedMap)
                        yield return syncRegistryObject.Id;
                }
            }
        }
    }
}
