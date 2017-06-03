using System;

namespace CommonBotLibrary.Services.Models
{
    /// <summary>
    ///   Wrapper class for a real die's side count.
    ///   Implicitly typed as an <see langword="int"/>.
    /// </summary>
    public class Die : IEquatable<Die>
    {
        private Die(int sides) 
            => Sides = sides;

        public int Sides { get; }

        /// <summary>
        ///   Creates a new instance of the <see cref="Die"/> class.
        /// </summary>
        /// <param name="sides">
        ///   The number of sides to use. May have a 'd' before the number.
        ///   Number must be between zero and <see cref="int.MaxValue"/>.
        /// </param>
        /// <returns>A new die.</returns>
        /// <exception cref="OverflowException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static Die Factory(string sides)
        {
            try
            {
                var sideCount = sides != null ? int.Parse(sides) : 6;
                return Factory(sideCount);
            }
            catch (FormatException)
            {
                if (StartsWithD(sides)) return Factory(sides?.Substring(1));
                throw new ArgumentException("Failed to parse a valid side count.");
            }
        }

        /// <summary>
        ///   Creates a new instance of the <see cref="Die"/> class.
        /// </summary>
        /// <param name="sides">
        ///   The number of sides to use.
        ///   Number must be between zero and <see cref="int.MaxValue"/>.
        /// </param>
        /// <returns>A new die.</returns>
        /// <exception cref="OverflowException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static Die Factory(int sides = 6)
        {
            if (sides == int.MaxValue)
                throw new OverflowException($"Side count cannot be >= {nameof(int.MaxValue)}.");

            if (sides <= 0)
                throw new ArgumentOutOfRangeException(nameof(sides), "Side count must be greater than zero.");

            return new Die(sides);
        }

        public static bool StartsWithD(string sides)
            => sides.Length > 1 && char.IsNumber(sides[1]) && char.ToUpperInvariant(sides[0]) == 'D';
        
        public bool Equals(Die other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Sides == other.Sides;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Die) obj);
        }

        public override int GetHashCode() => Sides;
        public override string ToString() => Sides.ToString();

        public static bool operator ==(Die left, Die right) => Equals(left, right);
        public static bool operator !=(Die left, Die right) => !Equals(left, right);

        public static implicit operator int(Die die) => die.Sides;
        public static implicit operator Die(int sides) => Factory(sides);
    }
}
