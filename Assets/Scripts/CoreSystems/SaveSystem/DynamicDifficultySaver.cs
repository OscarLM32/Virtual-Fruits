using EditorSystems.Logger;
using Enemies;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CoreSystems.SaveSystem
{
    public class DynamicDifficultySaver
    {
        private const string saveFileName = "dynamicDifficultyData.dat";
        private readonly string path;
        private DataSave save = null;

        public DynamicDifficultySaver()
        {
            path = $"{Application.persistentDataPath}/{saveFileName}";
        }


        public float GetPlayerSkillParameter()
        {
            if (save == null) Load();
            return save.playerSkillParameter;
        }

        public Dictionary<EnemyType, float> GetEnemyDifficultyParameters()
        {
            if (save == null) Load();
            return save.enemyDifficultyParameters;
        }

        public void Save(float skillParameter, Dictionary<EnemyType, float> enemyParameters)
        {
            save = new DataSave(skillParameter, enemyParameters);
            string stringData = JsonUtility.ToJson(save);

            FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
            var byteData = System.Text.Encoding.UTF8.GetBytes(stringData);
            stream.Write(byteData);
            stream.Close();
        }

        private void Load()
        {
            if (!File.Exists(path))
            {
                save = new();
                return;
            }

            FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
            byte[] buffer = new byte[1024];
            while(stream.Read(buffer, 0, buffer.Length) > 0){}

            string stringedData = System.Text.Encoding.UTF8.GetString(buffer);
            EditorLogger.Log(LoggingSystem.SAVE_MANAGER, $"{{DynamicDifficultySaver}}: Teh retrieved info is {stringedData}");
            save = JsonUtility.FromJson<DataSave>(stringedData);

            if (save.enemyDifficultyParameters == null)
            {
                save = new DataSave(save.playerSkillParameter);
            }
            stream.Close();
        }

        private class DataSave
        {
            public float playerSkillParameter;
            public Dictionary<EnemyType, float> enemyDifficultyParameters;

            public DataSave(float skillParameter = 0, Dictionary<EnemyType, float> enemyParameters = null)
            {
                playerSkillParameter = skillParameter;

                if(enemyParameters != null)
                {
                    enemyDifficultyParameters = enemyParameters;
                }
                else
                {
                    InitializeEnemyDifficultyParameters();
                }
            }

            private void InitializeEnemyDifficultyParameters()
            {
                enemyDifficultyParameters = new();
                foreach (EnemyType enemyType in Enum.GetValues(typeof(EnemyType)))
                {
                    enemyDifficultyParameters.Add(enemyType, 0);
                }
            }
        }
    }
}