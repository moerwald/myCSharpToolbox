﻿using System.Collections.Generic;
using System.Reflection;

namespace Reflection
{
    public static  class AttributesFromType
    {
        public static IEnumerable<FieldInfo> ConstantAttributesOf<TClass>(TClass type)  where TClass : class
        {
            List<FieldInfo> constants = new List<FieldInfo>();

            FieldInfo[] fieldInfos = type.GetType().GetFields(
                // Gets all public and static fields

                BindingFlags.Public | BindingFlags.Static |
                // This tells it to get the fields from all base types as well

                BindingFlags.FlattenHierarchy);

            // Go through the list and only pick out the constants
            foreach (FieldInfo fi in fieldInfos)
                // IsLiteral determines if its value is written at 
                //   compile time and not changeable
                // IsInitOnly determine if the field can be set 
                //   in the body of the constructor
                // for C# a field which is readonly keyword would have both true 
                //   but a const field would have only IsLiteral equal to true
                if (fi.IsLiteral && !fi.IsInitOnly)
                {
                    constants.Add(fi);
                }

            // Return an array of FieldInfos
            return constants;
        }
    }
}
