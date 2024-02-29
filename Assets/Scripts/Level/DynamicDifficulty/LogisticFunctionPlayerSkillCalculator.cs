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
    public class LogisticFunctionPlayerSkillCalculator
    {
        #if UNITY_EDITOR
        //For testing purposes
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

        public Difficulty currentDifficulty {
            get
            {
                if (!dataLoaded) { } LoadData();
                return currentDifficulty;
            }

            private set { }
        }

        //To determine whether the data stored by the savesystem has been loaded.
        private bool dataLoaded = false;

        //TODO: need to assign proper values
        private const float L = 1;
        private const float x0 = 0;
        private const float k = 1;

        //I don't know if this will be necessary
        private const float _maximumSkillParameter = 3;
        private const float _minimumSkillParameter = -_maximumSkillParameter;

        private float _playerSkillScore = L / 2;
        private float _playerSkillParameter = 0;



        //TODO: implement consecutiveness bonus
        //private bool _lastObstacleSurpassed = false;
        //private float _consecutive;

        public void CalculatePlayerLevelDifficulty()
        {
            int numDifficulties = Enum.GetNames(typeof(Difficulty)).Length;
            float range = L / numDifficulties;

            int difficultyIndex = (int)(_playerSkillScore / range);
            //Debug.Log("Difficulty index: " + difficultyIndex);

            currentDifficulty = (Difficulty)difficultyIndex;
        }

        private void CalculatePlayerSkillScore()
        {
            _playerSkillScore = L / (1 + Mathf.Pow((float)Math.E, -k * (PlayerSkillParameter - x0)));
        }

        private void LoadData()
        {
            //Load the data and calculate the difficulty
            CalculatePlayerSkillScore();
            CalculatePlayerLevelDifficulty();
        }
    }
}