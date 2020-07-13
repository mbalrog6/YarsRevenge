using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Test_CardinalDirections
{
    public class the_cardinal_directions
    {
        // A Test behaves as an ordinary method
        [Test]
        public void returns_north_from_positive_y_vector3()
        {
            Assert.AreEqual(CardinalDirection.NORTH, CardinalDirections.GetDirectionFromVector(Vector3.up));
        }
        
        [Test]
        public void returns_south_from_negative_y_vector3()
        {
            Assert.AreEqual(CardinalDirection.SOUTH, CardinalDirections.GetDirectionFromVector(-Vector3.up));
        }
        
        [Test]
        public void returns_east_from_positive_x_vector3()
        {
            Assert.AreEqual(CardinalDirection.EAST, CardinalDirections.GetDirectionFromVector(Vector3.right));
        }
        
        [Test]
        public void returns_west_from_negative_x_vector3()
        {
            Assert.AreEqual(CardinalDirection.WEST, CardinalDirections.GetDirectionFromVector(-Vector3.right));
        }
        
        [Test]
        public void returns_northeast_from_positive_xy_vector3()
        {
            Assert.AreEqual(CardinalDirection.NORTH_EAST, CardinalDirections.GetDirectionFromVector(new Vector3(1, 1, 0)));
        }
        
        [Test]
        public void returns_southeast_from_negative_y_positive_x_vector3()
        {
            Assert.AreEqual(CardinalDirection.SOUTH_EAST, CardinalDirections.GetDirectionFromVector(new Vector3(1, -1, 0)));
        }
        
        [Test]
        public void returns_northwest_from_positive_y_negative_x_vector3()
        {
            Assert.AreEqual(CardinalDirection.NORTH_WEST, CardinalDirections.GetDirectionFromVector(new Vector3(-1, 1, 0)));
        }
        
        [Test]
        public void returns_southwest_from_negative_xy_vector3()
        {
            Assert.AreEqual(CardinalDirection.SOUTH_WEST, CardinalDirections.GetDirectionFromVector(new Vector3(-1, -1, 0)));
        }

       

    }

    public class the_cardinal_directions_unit_vectors
    {
        [Test]
        public void returns_normalized_north_vector_from_north_cardinal_direction()
        {
            var directionVector = CardinalDirections.GetUnitVectorFromCardinalDirection(CardinalDirection.NORTH);
            Assert.AreEqual(Vector3.up, directionVector);
        }
        
        [Test]
        public void returns_normalized_south_vector_from_south_cardinal_direction()
        {
            var directionVector = CardinalDirections.GetUnitVectorFromCardinalDirection(CardinalDirection.SOUTH);
            Assert.AreEqual(-Vector3.up, directionVector);
        }
        
        [Test]
        public void returns_normalized_east_vector_from_east_cardinal_direction()
        {
            var directionVector = CardinalDirections.GetUnitVectorFromCardinalDirection(CardinalDirection.EAST);
            Assert.AreEqual(Vector3.right, directionVector);
        }
        
        [Test]
        public void returns_normalized_west_vector_from_west_cardinal_direction()
        {
            var directionVector = CardinalDirections.GetUnitVectorFromCardinalDirection(CardinalDirection.WEST);
            Assert.AreEqual(-Vector3.right, directionVector);
        }
        
        [Test]
        public void returns_normalized_northeast_vector_from_northeast_cardinal_direction()
        {
            var directionVector = CardinalDirections.GetUnitVectorFromCardinalDirection(CardinalDirection.NORTH_EAST);
            Assert.AreEqual(new Vector3(1, 1, 0).normalized, directionVector);
        }
        
        [Test]
        public void returns_normalized_northwest_vector_from_northwest_cardinal_direction()
        {
            var directionVector = CardinalDirections.GetUnitVectorFromCardinalDirection(CardinalDirection.NORTH_WEST);
            Assert.AreEqual(new Vector3(-1, 1, 0).normalized, directionVector);
        }
        
        [Test]
        public void returns_normalized_southeast_vector_from_southeast_cardinal_direction()
        {
            var directionVector = CardinalDirections.GetUnitVectorFromCardinalDirection(CardinalDirection.SOUTH_EAST);
            Assert.AreEqual(new Vector3(1, -1, 0).normalized, directionVector);
        }
        
        [Test]
        public void returns_normalized_southwest_vector_from_southwest_cardinal_direction()
        {
            var directionVector = CardinalDirections.GetUnitVectorFromCardinalDirection(CardinalDirection.SOUTH_WEST);
            Assert.AreEqual(new Vector3(-1, -1, 0).normalized, directionVector);
        }
    }
}
