
using System.Collections.Generic;

namespace Extensions
{
    public static class StringExtensions
    {
        public static int[] FindIndexOfChar(this string s, char c)
        {
            List<int> occurrences = new List<int>();
            for(int i = 0; i<s.Length; i++)
            {
                if (s[i] == c)
                    occurrences.Add(i);
            }

            return occurrences.ToArray();
        }

        public static string Replace(this string s, int pos, char c)
        {
            if (pos < 0 || pos >= s.Length)
                return s;

            var preChar = s.Substring(0, pos);
            var postChar = s.Substring(pos + 1);

            return preChar + c + postChar;
        }
    }
}