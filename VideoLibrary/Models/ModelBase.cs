using System;
using System.Collections.Generic;
using System.Text;

namespace VideoLibrary.Models
{
    public class ModelBase
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedDate {
            get {
                return DateTime.UtcNow;
            }
        }
        //ModifiedBy
        //ModifiedDate
    }
}
