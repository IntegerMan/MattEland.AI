using System;
using System.Collections.Generic;
using System.Text;

namespace MattEland.Shared.Numerics
{
    /// <summary>
    /// Extension methods related to integer values
    /// </summary>
    public static class IntegerExtensions
    {
        /// <summary>
        /// Converts a decimal <paramref name="value"/> to an integer value by dropping all values after the decimal marker.
        /// </summary>
        /// <remarks>
        /// This is a convenience method designed to prevent needing to cast to int after flooring a decimal.
        /// </remarks>
        /// <param name="value">The value to convert</param>
        /// <returns>An integer representing the portion of <paramref name="value"/> to the left of the decimal place.</returns>
        public static int ToInt(this decimal value) 
            => value < 0 
                ? (int) Math.Ceiling(value) 
                : (int) Math.Floor(value);
    }
}
