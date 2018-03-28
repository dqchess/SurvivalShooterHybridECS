using Unity.Entities;
using UnityEngine;

public class EnemyAttackSystem : ComponentSystem
{
    public struct Data
    {
        public int Length;
        public ComponentArray<EnemyAttacker> EnemyAttacker;
        public ComponentDataArray<Health> Health;
        public ComponentArray<Animator> Animator;
    }

    public struct PlayerData
    {
        public int Length;
        public EntityArray Entity;
        public ComponentDataArray<Player> Player;
        public ComponentDataArray<Health> Health;
    }

    [Inject] private Data data;
    [Inject] private PlayerData playerData;

    protected override void OnUpdate()
    {
        var entityManager = World.GetExistingManager<EntityManager>();
        var puc = PostUpdateCommands;
        var dt = Time.deltaTime;
        var timeBetweenAttacks = SurvivalShooterBootstrap.Settings.TimeBetweenEnemyAttacks;
        var enemyDamage = SurvivalShooterBootstrap.Settings.EnemyAttackDamage;

        for (var i = 0; i < data.Length; i++)
        {
            var attacker = data.EnemyAttacker[i];
            var enemyHealth = data.Health[i].Value;
            var playerHealth = playerData.Health[0].Value;

            attacker.Timer += Time.deltaTime;

            if (attacker.Timer >= timeBetweenAttacks && attacker.PlayerInRange && enemyHealth > 0)
            {
                attacker.Timer = 0f;
                var playerEntity = playerData.Entity[0];
                if (playerHealth > 0 && !entityManager.HasComponent<Damaged>(playerEntity))
                {
                    puc.AddComponent(playerEntity, new Damaged { Damage = enemyDamage });
                }
            }

            if (playerHealth <= 0)
            {
                var animator = data.Animator[i];
                animator.SetTrigger("PlayerDead");
            }
        }
    }
}
