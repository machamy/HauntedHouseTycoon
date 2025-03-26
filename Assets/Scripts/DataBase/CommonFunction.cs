using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Interfaces
{
    public interface IBaseData
    {
        long Index { get; }
    }
}

namespace CommonFunction.TypeConversion
{
    public static class TypeConverter
    {
        public static int[] ExtractIntArray(JObject obj, string baseKey)
        {
            List<int> values = new List<int>();

            if (obj.TryGetValue(baseKey, out JToken singleValue) && !string.IsNullOrEmpty(singleValue.ToString()))
            {
                if (int.TryParse(singleValue.ToString(), out int parsedValue))
                {
                    values.Add(parsedValue);
                }
            }

            foreach (var property in obj.Properties())
            {
                if (property.Name.StartsWith(baseKey + "_"))
                {
                    if (int.TryParse(property.Value.ToString(), out int parsedValue))
                    {
                        values.Add(parsedValue);
                    }
                }
            }

            return values.ToArray();
        }

        public static long[] ExtractLongArray(JObject obj, string baseKey)
        {
            List<long> values = new List<long>();

            if (obj.TryGetValue(baseKey, out JToken singleValue) && !string.IsNullOrEmpty(singleValue.ToString()))
            {
                if (long.TryParse(singleValue.ToString(), out long parsedValue))
                {
                    values.Add(parsedValue);
                }
            }

            foreach (var property in obj.Properties())
            {
                if (property.Name.StartsWith(baseKey + "_"))
                {
                    if (long.TryParse(property.Value.ToString(), out long parsedValue))
                    {
                        values.Add(parsedValue);
                    }
                }
            }

            return values.ToArray();
        }

        public static float[] ExtractFloatArray(JObject obj, string baseKey)
        {
            List<float> values = new List<float>();

            if (obj.TryGetValue(baseKey, out JToken singleValue) && !string.IsNullOrEmpty(singleValue.ToString()))
            {
                if (float.TryParse(singleValue.ToString(), out float parsedValue))
                {
                    values.Add(parsedValue);
                }
            }

            foreach (var property in obj.Properties())
            {
                if (property.Name.StartsWith(baseKey + "_"))
                {
                    if (float.TryParse(property.Value.ToString(), out float parsedValue))
                    {
                        values.Add(parsedValue);
                    }
                }
            }

            return values.ToArray();
        }

        public static int TryParseInt(string value, int defaultValue)
        {
            return int.TryParse(value, out int result) ? result : defaultValue;
        }

        public static long TryParseLong(string value, long defaultValue)
        {
            return long.TryParse(value, out long result) ? result : defaultValue;
        }

        public static float TryParseFloat(string value, float defaultValue)
        {
            return float.TryParse(value, out float result) ? result : defaultValue;
        }

        public static bool TryParseBool(string value, bool defaultValue)
        {
            return bool.TryParse(value, out bool result) ? result : defaultValue;
        }
    }
}