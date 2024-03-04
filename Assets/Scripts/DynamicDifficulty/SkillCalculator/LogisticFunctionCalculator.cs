using System;
using UnityEngine;

namespace DynamicDifficulty.Skillcalculator
{
    /*
     * This player skill calculator uses a logistic function. An 'S' shaped function
     * https://en.wikipedia.org/wiki/Logistic_function
     * The formula is as follows --> L / (1 + pow(e, -k + (x - x0)))
     * Being L, k and x0 constant values in the function and only X (the players points) should vary
     */
    public class LogisticFunctionCalculator : ISkillCalculator
    {
        //TODO: need to assign proper values
        private const float L = 1;
        private const float x0 = 0;
        private const float k = 1;

        private readonly int _numDifficulties = Enum.GetValues(typeof(Difficulty)).Length;


        public Difficulty GetPlayerSkillLevel(float skillParameter)
        {
            var playerSkillScore = L / (1 + Mathf.Pow((float)Math.E, -k * (skillParameter - x0)));
            return CalculateDifficulty(playerSkillScore);
        }

        public Difficulty CalculateEnemyDifficulty(float difficultyParameter)
        {
            //Invert the result so that, if an enemy has an score of difficulty 5 it means that we need to make it easier
            var score = -1 * (L / (1 + Mathf.Pow((float)Math.E, -k * (difficultyParameter - x0))));
            return CalculateDifficulty(score);
        }

        private Difficulty CalculateDifficulty(float score)
        {
            float range = L / _numDifficulties;
            int difficultyIndex = (int)(score / range);

            return (Difficulty)difficultyIndex;
        }

    }
}