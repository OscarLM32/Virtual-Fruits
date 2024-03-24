using EditorSystems.Logger;
using Extensions;
using GameSystems.Singleton;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Enemies.ShootingEnemyLogic
{

    //TODO: refactor code turn Lists of projectiles into queues
    public class EnemyProjectilePool : SingletonScene<EnemyProjectilePool>
    {
        //The amount of projectiles of each type that are going to be spawned intaly
        private const int _initialSpawnAmount = 5;

        public List<ProjectileType> projectileTypesInLevel;

        private Dictionary<ProjectileType, GameObject> _projectilesCache = new();
        private Dictionary<ProjectileType, Queue<GameObject>> _projectilePools = new();

        protected override void OnAwake()
        {
            foreach (var projectileType in projectileTypesInLevel)
            {
                InstantiateProjectilesAsync(projectileType);
            }
        }

        public GameObject GetProjectile(ProjectileType type)
        {
            if (!_projectilePools.ContainsKey(type))
            {
                InstantiateProjectilesSync(type);
            }

            var pool = _projectilePools[type];
            if(pool.Count == 0)
                AddProjectile(type);

            return pool.Dequeue();
        }

        public void DeleteProjectile(ProjectileType type, GameObject projectileDel)
        {
            projectileDel.SetActive(false);
            _projectilePools[type].Enqueue(projectileDel);
        }

        private void InstantiateProjectilesAsync(ProjectileType type)
        {
            string addressableAddress = type.EnumToPascalCase();
            EditorLogger.Log(LoggingSystem.ENEMY_PROJECTILE_POOL, $"{type}");
            var op = Addressables.LoadAssetAsync<GameObject>(addressableAddress);
            op.Completed += (opHandler) => { OnProjectileGameObjectLoaded(type, opHandler); };
        }

        private void InstantiateProjectilesSync(ProjectileType type)
        {
            string addressableAddress = type.EnumToPascalCase();
            EditorLogger.Log(LoggingSystem.ENEMY_PROJECTILE_POOL, $"{type}");
            var op = Addressables.LoadAssetAsync<GameObject>(addressableAddress);
            op.WaitForCompletion();
            OnProjectileGameObjectLoaded(type, op);
        }

        private void OnProjectileGameObjectLoaded(ProjectileType type, AsyncOperationHandle<GameObject> handler)
        {
            if (handler.Status == AsyncOperationStatus.Failed)
            {
                EditorLogger.LogError(LoggingSystem.ENEMY_PROJECTILE_POOL, $"There was an error trying to load the asset of a projectile: {type}");
                return;
            }

            Queue<GameObject> queue = new Queue<GameObject>();

            _projectilesCache.Add(type, handler.Result);
            _projectilePools.Add(type, queue);

            for (int i = 0; i < _initialSpawnAmount; i++)
            {
                AddProjectile(type);
            }

            EditorLogger.Log(LoggingSystem.ENEMY_PROJECTILE_POOL, $"The queue has been properly created");
        }

        private GameObject AddProjectile(ProjectileType type)
        {
            GameObject tmp = Instantiate(_projectilesCache[type], gameObject.transform);
            tmp.SetActive(false);
            _projectilePools[type].Enqueue(tmp);
            return tmp;
        }
    }
}