using System;
using System.Collections.Generic;
using System.Text;

namespace VideoLibrary.Models
{
    /// <summary>
    /// Base class for models providing common auditing properties.
    /// </summary>
    public class ModelBase
    {
        /// <summary>
        /// User that created the record. Caller should populate when creating entities.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Created date in UTC. Implemented as read-only to provide the current UTC time when accessed.
        /// Consider changing this to be set at creation time if consistent timestamps are required.
        /// </summary>
        public DateTime CreatedDate {
            get {
                return DateTime.UtcNow;
            }
        }
        // ModifiedBy
        // ModifiedDate
    }
}
