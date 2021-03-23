using System;

namespace Audio
{
    public class MinMaxRangeAttribute : Attribute
    {
        public MinMaxRangeAttribute( float min, float max)
        {
            Min = min;
            Max = max;
        }

        public float Max { get; private set;}
        public float Min { get; private set;}
  
    }
}