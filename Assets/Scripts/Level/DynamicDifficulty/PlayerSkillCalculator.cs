using System;
using UnityEngine;

namespace Level.DynamicDifficulty
{
    /*
     * This player skill calculator uses a logistic function. An 'S' shaped function
     * https://en.wikipedia.org/wiki/Logistic_function
     * The formula is as follows --> L / (1 + pow(e, -k + (x - x0)))
     * Being L, k and x0 constant values in the function and only X (the players points) should vary
     */
    public class PlayerSkillCalculator : MonoBehaviour
    {
        public float PlayerSkillScore = 0;

        //TODO: need to assign proper values
        private const float L = 1;
        private const float x0 = 1;
        private const float k = 1;

        //Want to make player skill scoring exponential

        
    }
}