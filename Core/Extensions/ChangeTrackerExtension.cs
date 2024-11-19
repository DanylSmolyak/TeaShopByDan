using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Core.Extensions;

public static class ChangeTrackerExtension
{
    public static void SetAuditProperties(this ChangeTracker changeTracker)
    {
        changeTracker.DetectChanges();
        IEnumerable<EntityEntry> entities =
            changeTracker
                .Entries()
                .Where(t => t.Entity is BaseEntitie && 
                            (t.State == EntityState.Deleted || t.State == EntityState.Added));

        foreach (EntityEntry entry in entities)
        {
            var entityType = entry.Entity.GetType();

            if (entry.State == EntityState.Deleted)
            {
                if (entityType == typeof(CartItem)) 
                {
                    entry.State = EntityState.Deleted;
                }
                else
                {
                    entry.State = EntityState.Modified;
                    entry.CurrentValues["IsDeleted"] = true;
                }
            }
            else if (entry.State == EntityState.Added)
            {
                entry.CurrentValues["IsDeleted"] = false;
            }
        }
    }
}
