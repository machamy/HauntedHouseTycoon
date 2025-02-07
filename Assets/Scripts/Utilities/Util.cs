using UnityEngine;

namespace Utilities
{
    public static class Util
    {
        
        public static float LerfAngleToAbsAngle(float startAngle, float endAngle, float value)
        {
            float deltaAngle = endAngle - startAngle;
            return Mathf.Lerp(0, deltaAngle, value);
        }
        
        public static float NormalizeAngle(float angle)
        {
            return (angle + 360) % 360;
        }
    }
}