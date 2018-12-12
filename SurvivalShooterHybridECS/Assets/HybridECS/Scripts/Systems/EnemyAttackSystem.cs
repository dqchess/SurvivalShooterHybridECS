using Unity.Entities;
using UnityEngine;

public class EnemyAttackSystem : ComponentSystem
{
#pragma warning disable 649
    private struct Data
    {
        public readonly int Length;
        public ComponentArray<EnemyAttacker> EnemyAttacker;
        public ComponentDataArray<Health> Health;
        public ComponentArray<Animator> Animator;
    }

    private struct PlayerData
    {
        public readonly int Length;
        public EntityArray Entity;
        public ComponentDataArray<Player> Player;
        public ComponentDataArray<Health> Health;
    }

    [Inject] private Data data;
    [Inject] private PlayerData playerData;
#pragma warning restore 649

    private EntityManager entityManager;

    protected override void OnCreateManager()
    {
        entityManager = World.GetExistingManager<EntityManager>();
    }

    protected override void OnUpdate()
    {
        var puc = PostUpdateCommands;
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
