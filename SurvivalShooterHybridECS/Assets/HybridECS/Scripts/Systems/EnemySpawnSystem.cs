using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class EnemySpawnSystem : ComponentSystem
{
    public struct Data
    {
        public int Length;
        public ComponentArray<EnemySpawner> Spawner;
    }

    public struct PlayerData
    {
        public int Length;
        public ComponentDataArray<Health> Health;
    }

    [Inject] private Data data;
    [Inject] private PlayerData playerData;

    private List<float> time = new List<float>();

    protected override void OnUpdate()
    {
        if (playerData.Health[0].Value <= 0)
        {
            return;
        }

        var entityManager = World.GetExistingManager<EntityManager>();
        var dt = Time.deltaTime;
        var startingHealth = SurvivalShooterBootstrap.Settings.StartingEnemyHealth;

        for (var i = 0; i < data.Length; i++)
        {
            if (time.Count < i + 1)
            {
                time.Add(0f);
            }

            time[i] += dt;

            var spawner = data.Spawner[i];

            if (time[i] >= spawner.SpawnTime)
            {
                var enemy = Object.Instantiate(spawner.Enemy, spawner.transform.position, quaternion.identity);
                var entity = enemy.GetComponent<GameObjectEntity>().Entity;
                entityManager.AddComponentData(entity, new Enemy());
                entityManager.AddComponentData(entity, new Health { Value = startingHealth });
                time[i] = 0f;
            }
        }
    }
}
