using System;

namespace ISUCorp.Core.Kernel
{
    public abstract class BaseEntity
    {
        /// <summary>
        /// Entity identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Create time
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// Update time
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// Delete time
        /// </summary>
        public DateTime? DeleteTime { get; set; }
    }
}
