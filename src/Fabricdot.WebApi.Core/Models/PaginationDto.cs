using System;
using System.ComponentModel.DataAnnotations;

namespace Fabricdot.WebApi.Core.Models
{
    [Obsolete("use PagedResultRequestDto")]//todo:remove this class
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