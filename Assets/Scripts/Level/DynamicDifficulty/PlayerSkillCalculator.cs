using GameSystems.Singleton;
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
    public class PlayerSkillCalculator : SingletonScene<PlayerSkillCalculator>
    {
        #if UNITY_EDITOR
        public float PlayerSkillParameter 
        {
            get
            {
                return _playerSkillParameter;
            }
            set
            {
                _playerSkillParameter = value;
                if (_playerSkillParameter > _maximumSkillParameter) _playerSkillParameter = _maximumSkillParameter;
                else if (_playerSkillParameter < _minimumSkillParameter) _playerSkillParameter = _minimumSkillParameter;

                CalculatePlayerSkillScore();
            }
        }
        #endif

        //TODO: need to assign proper values
        private const float L = 1;
        private const float x0 = 0;
        private const float k = 1;

        //I don't know if this will be necessary
        private const float _maximumSkillParameter = 5;
        private const float _minimumSkillParameter = -_maximumSkillParameter;

        private float _playerSkillScore = L / 2;
        private float _playerSkillParameter = 0;

        //TODO: implement consecutiveness bonus
        //private bool _wasLastObstacleSurpassed = false;
        //private float _consecutive;


        #region UNITY_FUNCTIONS
        private void Start()
        {
            //Need to retrieve a the saved data and propagate this information
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

        public Difficulty GetPlayerLevelDifficulty()
        {
            int numDifficulties = Enum.GetNames(typeof(Difficulty)).Length;
            float range = L / numDifficulties;

            int difficultyIndex = (int)(_playerSkillScore / range);
            Debug.Log("Difficulty index: " + difficultyIndex);

            return (Difficulty)difficultyIndex;
        }

        private void PlayerSurpassedObstacle()
        {
            //if obstacle was not surpassed last time
            //consecutiveness == 0
            //currentSkillParameter += f(value,consecutiveness)
            //consecutiveness += 1
            //CalculatePlayerSkillScore
        }

        private void PlayerFailedObstacle()
        {

        }

        private void CalculatePlayerSkillScore()
        {
            _playerSkillScore = L / (1 + Mathf.Pow((float)Math.E, -k*(PlayerSkillParameter - x0)));
        }

        protected override void OnAwake()
        {
            //Access the save manager and collect the data neccessary for this class
        }
    }
}