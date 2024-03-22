using GameSystems.Singleton;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies.ShootingEnemyLogic
{

    //TODO: refactor code turn Lists of projectiles into queues
    public class EnemyProjectilePool : SingletonScene<EnemyProjectilePool>, ISerializationCallbackReceiver
    {
        //The amount of projectiles of each type that are going to be spawned intaly
        private const int _initialSpawnAmount = 5;

        public List<ProjectileType> projectileTypesInLevel;
        public List<GameObject> projectilePrefabs;

        private Dictionary<ProjectileType, GameObject> _projectilesToSpawn = new();
        private Dictionary<ProjectileType, List<GameObject>> _projectiles = new();

        protected override void OnAwake()
        {
            InstantiateProjectiles();
        }

        public GameObject GetProjectile(ProjectileType type)
        {
            List<GameObject> projectiles = _projectiles[type];
            for (int i = 0; i < projectiles.Count; i++)
            {
                if (!projectiles[i].activeInHierarchy)
                {
                    projectiles[i].SetActive(true);
                    return projectiles[i];
                }
            }
            var tmp = AddProjectile(type);
            tmp.SetActive(true);
            return tmp;
        }

        public void DeleteProjectile(GameObject projectileDel)
        {
            projectileDel.SetActive(false);
        }

        private void InstantiateProjectiles()
        {
            foreach (var projectile in _projectilesToSpawn)
            {
                List<GameObject> tmpList = new List<GameObject>();
                for (int i = 0; i < _initialSpawnAmount; i++)
                {
                    GameObject tmp = Instantiate(projectile.Value, gameObject.transform);
                    tmp.SetActive(false);
                    tmpList.Add(tmp);
                }

                _projectiles.Add(projectile.Key, tmpList);
            }
        }

        private GameObject AddProjectile(ProjectileType type)
        {
            GameObject tmp = Instantiate(_projectilesToSpawn[type], gameObject.transform);
            tmp.SetActive(false);
            _projectiles[type].Add(tmp);
            return tmp;
        }


        public void OnBeforeSerialize()
        {
            //Don't need to do anything with this method
        }

        public void OnAfterDeserialize()
        {
            _projectilesToSpawn = new Dictionary<ProjectileType, GameObject>();

            for (int i = 0; i != Math.Min(projectileTypesInLevel.Count, projectilePrefabs.Count); i++)
                _projectilesToSpawn.Add(projectileTypesInLevel[i], projectilePrefabs[i]);
        }
    }
}