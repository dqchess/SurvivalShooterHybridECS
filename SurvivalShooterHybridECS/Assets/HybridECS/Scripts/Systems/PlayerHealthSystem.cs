using Unity.Entities;
using UnityEngine;

public class PlayerHealthSystem : ComponentSystem
{
    public struct Data
    {
        public int Length;
        public EntityArray Entity;
        public ComponentDataArray<Player> Player;
        public ComponentDataArray<Damaged> Damaged;
        public ComponentDataArray<Health> Health;
        public ComponentArray<Animator> Animator;
        public ComponentArray<AudioSource> AudioSource;
    }

    [Inject] private Data data;

    protected override void OnUpdate()
    {
        var puc = PostUpdateCommands;
        var gameUi = SurvivalShooterBootstrap.Settings.GameUi;
        var playerDeathClip = SurvivalShooterBootstrap.Settings.PlayerDeathClip;

        for (var i = 0; i < data.Length; i++)
        {
            var entity = data.Entity[i];

            var damaged = data.Damaged[i];
            var newHealth = data.Health[i].Value - damaged.Damage;
            data.Health[i] = new Health { Value = newHealth };

            gameUi.OnPlayerTookDamage(newHealth);


            if (newHealth <= 0)
            {
                data.AudioSource[i].clip = playerDeathClip;
                data.Animator[i].SetTrigger("Die");
                puc.AddComponent(entity, new Dead());
            }

            data.AudioSource[i].Play();

            puc.RemoveComponent<Damaged>(entity);
        }
    }
}
