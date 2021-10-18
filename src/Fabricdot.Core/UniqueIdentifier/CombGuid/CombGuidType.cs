using System;

namespace Fabricdot.Core.UniqueIdentifier.CombGuid
{
    public enum CombGuidType
    {
        /// <summary>
        ///     The GUID should be sequential when formatted using the <see cref="Guid.ToString()"
        ///     /> method.
        /// </summary>
        SequentialAsString,

        /// <summary>
        ///     The GUID should be sequential when formatted using the <see cref="Guid.ToByteArray"
        ///     /> method.
        /// </summary>
        SequentialAsBinary,

        /// <summary>
        ///     The sequential portion of the GUID should be located at the end of the Data4 block.
        /// </summary>
        SequentialAtEnd
    }
}