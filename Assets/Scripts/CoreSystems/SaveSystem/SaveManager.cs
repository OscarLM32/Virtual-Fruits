using EditorSystems.Logger;
using Enemies;
using Extensions.Serializables;
using GameSystems.Singleton;
using System.Collections.Generic;


namespace CoreSystems.SaveSystem
{
    public class SaveManager : Singleton<SaveManager>
    {
        private DynamicDifficultySaver _dynamicDifficultySaver;
        private LevelsDataSaver _levelsDataSaver;

        protected override void OnAwake()
        {
            //This needs to be called here since "Application.persistentDataPath"
            //cannot be called inside instance initializer
            _dynamicDifficultySaver = new DynamicDifficultySaver();
        }

        public float GetPlayerSkillParameter()
        {
            return _dynamicDifficultySaver.GetPlayerSkillParameter();
        }

        public Dictionary<EnemyType, float> GetEnemyDifficultyParameters()
        {
            return _dynamicDifficultySaver.GetEnemyDifficultyParameters();
        }

        public void SaveDynamicDifficultyData(float skillParameter, Dictionary<EnemyType, float> enemyDifficutlyParameters)
        {
            if(enemyDifficutlyParameters == null)
            {
                EditorLogger.LogError(LoggingSystem.SAVE_MANAGER, "The enemy difficulty parameters you are trying to save are null. SAVE ABORT");
                return;
            }
            //TODO: check the max skillParameter
            _dynamicDifficultySaver.Save(skillParameter, enemyDifficutlyParameters);
        }
    }
}