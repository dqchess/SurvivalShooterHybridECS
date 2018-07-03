using Unity.Entities;
using UnityEngine;

public class EnemyHealthSystem : ComponentSystem
{
    public struct Data
    {
        public readonly int Length;
        public GameObjectArray GameObject;
        public ComponentDataArray<Enemy> Enemy;
        public ComponentDataArray<Damaged> Damaged;
        public ComponentDataArray<Health> Health;
        public ComponentArray<Animator> Animator;
        public ComponentArray<AudioSource> AudioSource;
        public ComponentArray<CapsuleCollider> CapsuleCollider;
        public SubtractiveComponent<Dead> Dead;
    }

    [Inject] private Data data;

    protected override void OnUpdate()
    {
        var entityManager = World.GetExistingManager<EntityManager>();
        var puc = PostUpdateCommands;

        for (var i = 0; i < data.Length; i++)
        {
            var damaged = data.Damaged[i];

            var entity = data.GameObject[i].GetComponent<GameObjectEntity>().Entity;

            puc.RemoveComponent<Damaged>(entity);

            var newDamage = data.Health[i].Value;
            newDamage -= damaged.Damage;
            data.Health[i] = new Health { Value = newDamage };
            if (data.Health[i].Value <= 0 && !entityManager.HasComponent<Dead>(entity))
            {
                puc.AddComponent(entity, new Dead());
            }

            data.AudioSource[i].Play();

            var particles = data.GameObject[i].gameObject.GetComponentInChildren<ParticleSystem>();
            particles.transform.position = damaged.HitPoint;
            particles.Play();
        }
    }
}
