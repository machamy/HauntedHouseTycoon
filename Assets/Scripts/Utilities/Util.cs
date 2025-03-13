using System.Linq;
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
        
        public static int WeightedRandom(params int[] weights)
        {
            int totalWeight = weights.Sum();

            int randomValue = Random.Range(0, totalWeight);
            for (int i = 0; i < weights.Length; i++)
            {
                if (randomValue < weights[i])
                {
                    return i;
                }

                randomValue -= weights[i];
            }

            return weights.Length - 1;
        }
        
        public static int WeightedRandom(params float[] weights)
        {
            float totalWeight = weights.Sum();

            float randomValue = Random.Range(0, totalWeight);
            for (int i = 0; i < weights.Length; i++)
            {
                if (randomValue < weights[i])
                {
                    return i;
                }

                randomValue -= weights[i];
            }

            return weights.Length - 1;
        }
    }
}