using System;
using System.Reflection;
using Fabricdot.Domain.Core.Auditing;

namespace Fabricdot.Infrastructure.Core.Domain.Auditing
{
    /// <summary>
    ///     修改操作审计初始化器
    /// </summary>
    public class ModificationAuditEntityInitializer
    {
        /// <summary>
        ///     实体
        /// </summary>
        private readonly object _entity;

        /// <summary>
        ///     用户标识
        /// </summary>
        private readonly string _userId;

        /// <summary>
        ///     初始化修改操作审计初始化器
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="userId">用户标识</param>
        private ModificationAuditEntityInitializer(object entity, string userId)
        {
            _entity = entity;
            _userId = userId;
        }

        /// <summary>
        ///     初始化
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="userId">用户标识</param>
        public static void Init(object entity, string userId)
        {
            new ModificationAuditEntityInitializer(entity, userId).Init();
        }

        /// <summary>
        ///     初始化
        /// </summary>
        public void Init()
        {
            switch (_entity)
            {
                case null:
                    return;
                case IAuditEntity _:
                {
                    var type = _entity.GetType();
                    type.GetProperty(nameof(IAuditEntity.LastModifierId))
                        ?.SetValue(_entity, _userId, BindingFlags.Public | BindingFlags.Instance, null, null, null!);
                    type.GetProperty(nameof(IAuditEntity.LastModificationTime))
                        ?.SetValue(_entity, DateTime.Now, BindingFlags.Public | BindingFlags.Instance, null, null,
                            null!);
                    break;
                }
            }
        }
    }
}