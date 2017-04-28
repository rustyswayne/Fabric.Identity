﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fabric.Authorization.Domain.Permissions
{
    public class InMemoryPermissionStore : IPermissionStore
    {
        private static readonly ConcurrentDictionary<Guid, Permission> Permissions = new ConcurrentDictionary<Guid, Permission>();
        public IEnumerable<Permission> GetPermissions(string grain = null, string resource = null, string permissionName = null)
        {
            var permissions = Permissions.Select(kvp => kvp.Value);
            if (!string.IsNullOrEmpty(grain))
            {
                permissions = permissions.Where(p => p.Grain == grain);
            }
            if (!string.IsNullOrEmpty(resource))
            {
                permissions = permissions.Where(p => p.Resource == resource);
            }
            if (!string.IsNullOrEmpty(permissionName))
            {
                permissions = permissions.Where(p => p.Name == permissionName);
            }
            return permissions.Where(p => !p.IsDeleted);
        }

        public Permission GetPermission(Guid permissionId)
        {
            return Permissions.ContainsKey(permissionId) ? Permissions[permissionId] : null;
        }

        public void AddPermission(Permission permission)
        {
            permission.Id = Guid.NewGuid();
            Permissions.TryAdd(permission.Id, permission);
        }

        public void DeletePermission(Permission permission)
        {
            permission.IsDeleted = true;
            UpdatePermission(permission);
        }

        public void UpdatePermission(Permission permission)
        {
            //do nothing since this is an in memory store
        }
    }
}