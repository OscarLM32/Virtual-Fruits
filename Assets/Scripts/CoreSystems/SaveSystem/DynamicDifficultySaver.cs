using Enemies;
using System;
using System.Collections.Generic;
using System.IO;
using Testing;
using UnityEngine;

namespace CoreSystems.SaveSystem
{
    public class DynamicDifficultySaver
    {
        private string path = Application.persistentDataPath + "/dynamicDifficultyData.dat";
        private DataSave save = null;


        public float GetPlayerSkillParameter()
        {
            if (save == null) Load();

            return save.playerSkillParameter;
        }

        public Dictionary<EnemyType, float> GetEnemyDifficultyParameters()
        {
            if (save == null) Load();

            var aux = save.enemyDifficultyParameters;
            //The saved data is not too much data but I would not like to have repeated data all over the program
            save.enemyDifficultyParameters = null;
            return aux;
        }

        public void Save(float skillParameter, Dictionary<EnemyType, float> enemyParameters)
        {
            save = new DataSave(skillParameter, enemyParameters);
            string stringData = JsonUtility.ToJson(save);

            FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
            var byteData = System.Text.Encoding.UTF8.GetBytes(stringData);
            stream.Write(byteData);
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
            save = JsonUtility.FromJson<DataSave>(stringedData);
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