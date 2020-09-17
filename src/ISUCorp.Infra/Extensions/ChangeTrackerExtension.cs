using ISUCorp.Core.Kernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;

namespace ISUCorp.Infra.Extensions
{
    public static class ChangeTrackerExtension
    {
        public static void ApplyAuditInformation(this ChangeTracker changeTracker)
        {
            foreach (var entry in changeTracker.Entries())
            {
                if (!(entry.Entity is BaseEntity baseEntity)) continue;

                var now = DateTime.UtcNow;

                switch (entry.State)
                {
                    case EntityState.Modified:
                        baseEntity.ModifiedAt = now;
                        break;

                    case EntityState.Added:
                        baseEntity.AddedAt = now;
                        baseEntity.ModifiedAt = null;
                        break;
                }
            }
        }
    }
}
