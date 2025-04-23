using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Avocado.Utilities
{
    public class AngleUtilities 
    {
        /// <summary>
        /// Calculates the angle of a line created by two transforms from the positive or negative x-axis
        /// </summary>
        public static float AngleFromFacingDirection(Transform receiver, Transform source, int direction)
        {
            return Vector2.SignedAngle(Vector2.right * direction,
                source.position - receiver.position) * direction;
        }
    }
}
