
using System;

namespace Extensions
{
    public static class EnumExtension
    {
        public static string EnumToPascalCase<T>(this T enumValue) 
        { 
            var s = enumValue.ToString();
            var underScores = s.FindIndexOfChar('_');
            int removedUnderScores = 0;

            s = GetFirstLetterUpperCase(s);

            for(int i = 0; i<underScores.Length; i++)
            {
                int underScorePos = underScores[i] - removedUnderScores;
                s = s.Remove(underScorePos , underScorePos+1);
                s.Replace(underScorePos, char.ToUpper(s[underScorePos]));
                removedUnderScores++;
            }

            return s;
        }

        private static string GetFirstLetterUpperCase(string word)
        {
            char aux = word[0];

            word = word.ToLower();
            word = aux + word.Substring(1);

            return word;
        }
    }
}