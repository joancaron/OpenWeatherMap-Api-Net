// --------------------------------------------------------------------------------------------------------------------
// <copyright file="City.cs" company="Joan Caron">
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Joan Caron</author>
// <summary>Implements the city class</summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OpenWeatherMap
{
    using System.Xml.Serialization;

    /// <summary>
    ///     Class City.
    /// </summary>
    public sealed class City
    {
        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>
        ///     The identifier.
        /// </value>
        [XmlAttribute("id")]
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the coordinates.
        /// </summary>
        /// <value>
        ///     The coordinates.
        /// </value>
        [XmlElement("coord")]
        public Coordinates Coordinates { get; set; }

        /// <summary>
        ///     Gets or sets the sun.
        /// </summary>
        /// <value>
        ///     The sun.
        /// </value>
        [XmlElement("sun")]
        public Sun Sun { get; set; }

        /// <summary>
        ///     Gets or sets the sun.
        /// </summary>
        /// <value>
        ///     The sun.
        /// </value>
        [XmlElement("country")]
        public string Country { get; set; }
    }
}
