using System.Reflection;
using Fabricdot.Domain.Core.Auditing;

namespace Fabricdot.Infrastructure.Core.Domain.Auditing
{
    public class SoftDeleteEntityInitializer
    {
        private readonly object _entity;

        /// <summary>
        ///     初始化软删除审计初始化器
        /// </summary>
        /// <param name="entity">实体</param>
        private SoftDeleteEntityInitializer(object entity)
        {
            _entity = entity;
        }

        /// <summary>
        ///     初始化
        /// </summary>
        /// <param name="entity">实体</param>
        public static void Init(object entity)
        {
            new SoftDeleteEntityInitializer(entity).Init();
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
                case ISoftDelete _:
                {
                    var type = _entity.GetType();
                    type.GetProperty(nameof(ISoftDelete.IsDeleted))
                        ?.SetValue(_entity, true, BindingFlags.Public | BindingFlags.Instance, null, null, null!);
                    break;
                }
            }
        }
    }
}