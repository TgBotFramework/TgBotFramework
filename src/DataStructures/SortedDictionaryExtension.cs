using System;
using System.Collections.Generic;

namespace TgBotFramework.DataStructures
{
    public static class SortedDictionaryExtension
    {
        public static Type PrefixSearch(this SortedDictionary<string, Type> dictionary, string searchValue)
        {
            if (searchValue is null or "")
                return null;
            
            foreach (KeyValuePair<string, Type> pair in dictionary)
            {
                // going thou sorted array to required letter
                if (pair.Key[0] < searchValue[0] )
                {
                    continue;
                }
                else if(pair.Key[0] == searchValue[0])
                {
                    if(pair.Key.Length > searchValue.Length) // handler name length should be less or equal to search value
                        continue;
                    
                    if (CompareStrings(searchValue, pair.Key)) 
                    {
                        return pair.Value;
                    }
                }
                else // when we passed first letter and there is no chance that our handler there.
                {
                    return null;
                }
            }

            return null;
        }
        
        private static bool CompareStrings(string searchValue, string other)
        {
            //var min = Math.Min(searchValue.Length, other.Length);
            for (var i = 1; i < other.Length; i++)
            {
                if (searchValue[i] != other[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}