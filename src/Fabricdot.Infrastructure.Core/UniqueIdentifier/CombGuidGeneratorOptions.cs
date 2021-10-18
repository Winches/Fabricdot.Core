using Fabricdot.Core.UniqueIdentifier.CombGuid;

namespace Fabricdot.Infrastructure.Core.UniqueIdentifier
{
    public class CombGuidGeneratorOptions
    {
        public CombGuidType CombGuidType { get; set; }

        public CombGuidGeneratorOptions()
        {
            CombGuidType = CombGuidType.SequentialAtEnd;
        }
    }
}