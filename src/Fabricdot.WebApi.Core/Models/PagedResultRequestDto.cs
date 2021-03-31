using System.ComponentModel.DataAnnotations;

namespace Fabricdot.WebApi.Core.Models
{
    public class PagedResultRequestDto : IPagedResultRequest
    {
        /// <inheritdoc />
        [Required]
        [Range(1, int.MaxValue)]
        public int Index { get; set; }

        /// <inheritdoc />
        [Required]
        [Range(1, 1000)]
        public int Size { get; set; }
    }
}