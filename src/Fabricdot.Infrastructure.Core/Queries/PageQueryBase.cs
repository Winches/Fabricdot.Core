namespace Fabricdot.Infrastructure.Core.Queries
{
    public abstract class PageQueryBase<TResult> : IQuery<TResult>
    {
        /// <summary>
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// </summary>
        public int Size { get; set; }

        protected PageQueryBase(int index, int size)
        {
            Index = index;
            Size = size;
            //todo:querying may construct manually,how to validate it
        }

        protected PageQueryBase()
        {
        }
    }
}