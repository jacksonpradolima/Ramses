using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data;
using System.Data.Objects.DataClasses;
using System.Data.Metadata.Edm;

namespace Ramses.Data
{
    /// <summary>
    /// Extension methods for ObjectContext type
    /// </summary>
    public static class ObjectContextExtensions
    {
        /// <summary>
        /// Checks whether there are items in the ObjectStateManager.GetObjectStateEntries set as Added, Deleted or Modified
        /// </summary>
        /// <param name="context">The context which provides facilities for querying and working with entity data as object</param>
        /// <returns>True if there is at least one entry in the ObjectStateEntries collection</returns>
        public static Boolean HasPendingChanges(this ObjectContext context)
        {
            return context.ObjectStateManager
                        .GetObjectStateEntries(EntityState.Added 
                                            |  EntityState.Deleted 
                                            |  EntityState.Modified)
                        .Any();
        }

        /// <summary>
        /// Creates the entity key for a specific object, or returns then entity key if it already exists.
        /// </summary>
        /// <param name="context">The context which provides facilities for querying and working with entity data as object</param>
        /// <param name="entity">The object for which the entity key is being retrieved. The object must implement System.Data.Objects.DataClasses.IEntityWithKey</param>
        /// <returns></returns>
        public static EntityKey CreateEntityKey(this ObjectContext context, EntityObject entity)
        {
            return context.CreateEntityKey(GetEntitySetName(context, entity.GetType().Name), entity);
        }

        /// <summary>
        /// Obtains entity set name from the given EntityObject type
        /// </summary>
        /// <param name="context">The context which provides facilities for querying and working with entity data as object</param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static string GetEntitySetName(this ObjectContext context, Type entityType)
        {
            return GetEntitySetName(context, entityType.Name);
        }

        /// <summary>
        /// Obtains entity set name from the given type name.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        private static string GetEntitySetName(this ObjectContext context, string className)
        {
            var container = context.MetadataWorkspace.GetEntityContainer(context.DefaultContainerName, DataSpace.CSpace);
            return (from meta in container.BaseEntitySets
                    where meta.ElementType.Name == className
                    select meta.Name).First();
        }
    }
}
