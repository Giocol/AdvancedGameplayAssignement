using UnityEngine;

namespace Game
{
    public class Utils
    {
        public static float CalculateGravitySpeed(float currentGravitySpeed, bool isGrounded, float deltaTime)
        {
            return isGrounded ? 0.0f : currentGravitySpeed + deltaTime * 9.82f;
        }
    }
}