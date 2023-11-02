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
        public float PlayerSkillScore { get; private set; }

        //TODO: need to assign proper values
        private const float L = 1;
        private const float x0 = 1;
        private const float k = 1;

        private const float minimumSkillParameter = -3;
        private const float maximumSkillParameter = 3;

        private float currentSkillParameter = 0;
        private bool wasLastObstacleSurpassed = false;
        //Want to make player skill scoring exponential in case the player score very well or poorly
        private float consecutive;


        #region UNITY_FUNCTIONS
        private void Start()
        {
            //Need to retrieve a the saved data and propagate the this information
            //to the components needing it (sectors)
        }

        private void OnEnable()
        {
            //Need to suscribe to the related events
        }

        private void OnDisable()
        {
            //Need to unsuscribe from the suscribed event
        }
        #endregion

        private void PlayerSurpassedObstacle()
        {
            //if obstacle was not surpassed last time
            //consecutiveness == 0
            //currentSkillParameter += f(value,consecutiveness)
            //consecutiveness += 1
            //if currentSkillParameter > 3 --> currentSkillParameter = 3
            //CalculatePlayerSkillScore
        }

        private void PlayerFailedObstacle()
        {

        }

        private void CalculatePlayerSkillScore()
        {
            //
            //f(currentSkillParameter) =  L / (1 + pow(e, -k + (x - x0)))
        }
    }
}