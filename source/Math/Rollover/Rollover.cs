
namespace MathHelper.Rollover
{
    using System;
    public static class Rollover 
    {
            public static UInt16 GetDifference (UInt16 oldValue, UInt16 actualValue) 
            {
                if (actualValue >= oldValue)
                {
                   return (UInt16) (actualValue - oldValue);
                }

                // actualValue wrapped around unsinged maximum
                // 0                                        Unsigned max
                //   D1                           D2
                // |<--->|                |<----------------->|
                // |-----|----------------|-------------------|
                //      actualValue     oldValue

                return (UInt16) ((UInt16.MaxValue - oldValue) + actualValue);
            }

            public static UInt32 GetDifference (UInt32 oldValue, UInt32 actualValue) 
            {
                if (actualValue >= oldValue)
                {
                   return (UInt32) (actualValue - oldValue);
                }

                // actualValue wrapped around unsinged maximum
                // 0                                        Unsigned max
                //   D1                           D2
                // |<--->|                |<----------------->|
                // |-----|----------------|-------------------|
                //      actualValue     oldValue

                return (UInt32) ((UInt32.MaxValue - oldValue) + actualValue);
            }

            public static UInt64 GetDifference (UInt64 oldValue, UInt64 actualValue) 
            {
                if (actualValue >= oldValue)
                {
                   return (UInt64) (actualValue - oldValue);
                }

                // actualValue wrapped around unsinged maximum
                // 0                                        Unsigned max
                //   D1                           D2
                // |<--->|                |<----------------->|
                // |-----|----------------|-------------------|
                //      actualValue     oldValue

                return (UInt64) ((UInt64.MaxValue - oldValue) + actualValue);
            }

            public static Byte GetDifference (Byte oldValue, Byte actualValue) 
            {
                if (actualValue >= oldValue)
                {
                   return (Byte) (actualValue - oldValue);
                }

                // actualValue wrapped around unsinged maximum
                // 0                                        Unsigned max
                //   D1                           D2
                // |<--->|                |<----------------->|
                // |-----|----------------|-------------------|
                //      actualValue     oldValue

                return (Byte) ((Byte.MaxValue - oldValue) + actualValue);
            }

            }
}