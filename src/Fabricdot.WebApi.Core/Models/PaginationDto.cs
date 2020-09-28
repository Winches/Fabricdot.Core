using System.ComponentModel.DataAnnotations;

namespace Fabricdot.WebApi.Core.Models
{
    public class PaginationDto
    {
        /// <summary>
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int Index { get; set; }

        /// <summary>
        /// </summary>
        [Required]
        [Range(1, 100)]
        public int Size { get; set; }
    }
}