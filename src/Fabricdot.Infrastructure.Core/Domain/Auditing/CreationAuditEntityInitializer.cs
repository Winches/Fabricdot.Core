using System;
using System.Reflection;
using Fabricdot.Domain.Core.Auditing;
using Fabricdot.Domain.Core.SharedKernel;

namespace Fabricdot.Infrastructure.Core.Domain.Auditing
{
    public class CreationAuditEntityInitializer
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
        ///     初始化创建操作审计初始化器
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="userId">用户标识</param>
        private CreationAuditEntityInitializer(object entity, string userId)
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
            new CreationAuditEntityInitializer(entity, userId).Init();
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
                case ICreationAuditEntity _:
                    {
                        var type = _entity.GetType();
                        type.GetProperty(nameof(ICreationAuditEntity.CreationId))
                            ?.SetValue(_entity, _userId, BindingFlags.Public | BindingFlags.Instance, null, null, null!);
                        type.GetProperty(nameof(ICreationAuditEntity.CreationTime))
                            ?.SetValue(_entity, SystemClock.Now, BindingFlags.Public | BindingFlags.Instance, null, null,
                                null!);
                        break;
                    }
            }
        }
    }
}