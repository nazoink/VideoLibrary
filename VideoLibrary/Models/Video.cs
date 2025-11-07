using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace VideoLibrary.Models
{
    [Table("Videos")]
    public class Video : ModelBase
    {
        /// <summary>
        /// Primary key for the Video record. Nullable to allow Dapper.Contrib to set it on insert.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Optional prefix for the title (e.g., "Episode", "Vol").
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// The title of the video. Required for saving.
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Identifier for the publisher (foreign key or external id).
        /// </summary>
        public string PublisherId { get; set; }

        /// <summary>
        /// Rating identifier (e.g., PG-13, R).
        /// </summary>
        public string RatingId { get; set; }

        /// <summary>
        /// Length/duration of the video, stored as a string (e.g., "01:30:00").
        /// </summary>
        public string Length { get; set; }

        /// <summary>
        /// Collection or set identifier this video belongs to.
        /// </summary>
        public string SetCollectionId { get; set; }

        /// <summary>
        /// Media type identifier (e.g., DVD, Blu-ray, Digital).
        /// </summary>
        public string MediaTypeId { get; set; }

        /// <summary>
        /// A brief description of the video content.
        /// </summary>
        public string Description { get; set; }

    }
}
