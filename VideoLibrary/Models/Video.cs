using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace VideoLibrary.Models
{
    [Table("Videos")]
    public class Video : ModelBase
    {
        public int? Id { get; set; }
        public string Prefix { get; set; }
        [Required]
        public string Title { get; set; }
        public string PublisherId { get; set; }
        public string RatingId { get; set; }
        public string Length { get; set; }
        public string SetCollectionId { get; set; }
        public string MediaTypeId { get; set; }
        public string Description { get; set; }

    }
}
