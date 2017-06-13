using System;
using CommonBotLibrary.Services.Models;

namespace CommonBotLibrary.Services
{
    public class RandomService
    {
        public static Random Generator { get; } = new Random();

        /// <summary>
        ///   Rolls a pseudo-random die.
        /// </summary>
        /// <param name="die">The die to roll. Six-sided if null.</param>
        /// <returns>The result of the roll.</returns>
        /// <remarks>
        ///   If explicitly using an <see langword="int"/> as an argument, check the
        ///   <see langword="Factory()"/> method in <see cref="Die"/> for exception docs.
        /// </remarks>
        public int Roll(Die die = null)
            => Generator.Next(1, (die ?? Die.Factory()).Sides + 1);

        /// <summary>
        ///   Flips a pseudo-random coin.
        /// </summary>
        /// <returns>True if heads, false if tails.</returns>
        public bool FlipCoin()
            => Convert.ToBoolean(Generator.Next(2));
    }
}
