﻿using ISUCorp.Core.Kernel;
using System;

namespace ISUCorp.Core.Domain
{
    public class Reservation : BaseEntity
    {
        /// <summary>
        /// Date for reservation.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Information to take into account for the reservation.
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// The punctuation for the quality of the reservation.
        /// </summary>
        public int? Rating { get; set; }

        /// <summary>
        /// Whether the reservation is favorite or no.
        /// </summary>
        public bool IsFavorite { get; set; }

        /// <summary>
        /// Contact identifier.
        /// </summary>
        public int ContactId { get; set; }

        /// <summary>
        /// Who made the reservation.
        /// </summary>
        public Contact Contact { get; set; }

        /// <summary>
        /// Place identifier.
        /// </summary>
        public int PlaceId { get; set; }

        /// <summary>
        /// Where is the reservation.
        /// </summary>
        public Place Place { get; set; }
    }
}
