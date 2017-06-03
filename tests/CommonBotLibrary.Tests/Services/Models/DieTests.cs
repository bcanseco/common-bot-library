using System;
using CommonBotLibrary.Services.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommonBotLibrary.Tests.Services.Models
{
    [TestClass]
    public class DieTests
    {
        /// <summary>
        ///   <see cref="Random.Next()"/> needs to increment, so the
        ///   acceptable range is capped at <see cref="int.MaxValue"/> - 1.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public void Should_Fail_With_MaxValue()
        {
            var die = Die.Factory(int.MaxValue);
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public void Should_Fail_When_Over_MaxValue()
        {
            var die = Die.Factory("999999999999999");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Should_Fail_With_Zero()
        {
            var die = Die.Factory(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Should_Fail_With_Negatives()
        {
            var die = Die.Factory(-1);
        }

        [TestMethod]
        public void Should_Work_With_1()
        {
            var die = Die.Factory(1);
            Assert.AreEqual(die.Sides, 1);
        }

        [TestMethod]
        public void Should_Be_Six_Sided_By_Default()
        {
            var die = Die.Factory();
            Assert.AreEqual(die.Sides, 6);
            die = Die.Factory(null);
            Assert.AreEqual(die.Sides, 6);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Should_Fail_With_Invalid_String()
        {
            var die = Die.Factory("asdf");
        }

        [TestMethod]
        public void Should_Work_With_D_Before_Number()
        {
            var die = Die.Factory("d20");
            Assert.AreEqual(die.Sides, 20);
        }
    }
}
