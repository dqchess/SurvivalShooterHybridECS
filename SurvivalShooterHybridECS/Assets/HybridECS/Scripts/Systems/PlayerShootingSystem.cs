using Unity.Entities;
using UnityEngine;

public class PlayerShootingSystem : ComponentSystem
{
#pragma warning disable 649
    private struct Data
    {
        public readonly int Length;
        public GameObjectArray GameObject;
        public ComponentArray<PlayerGun> PlayerGun;
        public ComponentArray<ParticleSystem> ParticleSystem;
        public ComponentArray<LineRenderer> LineRenderer;
        public ComponentArray<AudioSource> AudioSource;
        public ComponentArray<Light> Light;
    }

    private struct PlayerData
    {
        public ComponentDataArray<Player> Player;
        public ComponentDataArray<Health> Health;
    }

    [Inject] private Data data;
    [Inject] private PlayerData playerData;
#pragma warning restore 649

    private float timer;

    protected override void OnUpdate()
    {
        if (playerData.Health[0].Value <= 0)
        {
            return;
        }

        timer += Time.deltaTime;

        var timeBetweenBullets = SurvivalShooterBootstrap.Settings.TimeBetweenBullets;
        var effectsDisplayTime = SurvivalShooterBootstrap.Settings.GunEffectsDisplayTime;

        for (var i = 0; i < data.Length; i++)
        {
            if (Input.GetButton("Fire1") && timer > timeBetweenBullets)
            {
                Shoot(i);
            }

            if (timer >= timeBetweenBullets * effectsDisplayTime)
            {
                DisableEffects(i);
            }
        }
    }

    private void Shoot(int i)
    {
        timer = 0f;

        data.AudioSource[i].Play();

        data.Light[i].enabled = true;

        data.ParticleSystem[i].Stop();
        data.ParticleSystem[i].Play();

        data.LineRenderer[i].enabled = true;
        data.LineRenderer[i].SetPosition(0, data.GameObject[i].transform.position);

        var shootRay = new Ray
        {
            origin = data.GameObject[i].transform.position,
            direction = data.GameObject[i].transform.forward
        };

        RaycastHit shootHit;
        if (Physics.Raycast(shootRay, out shootHit, SurvivalShooterBootstrap.Settings.GunRange,
            LayerMask.GetMask("Shootable")))
        {
            var goEntity = shootHit.collider.gameObject.GetComponent<GameObjectEntity>();
            if (goEntity != null)
            {
                var hitEntity = shootHit.collider.gameObject.GetComponent<GameObjectEntity>().Entity;
                var entityManager = World.GetExistingManager<EntityManager>();
                entityManager.AddComponentData(hitEntity, new Damaged { Damage = SurvivalShooterBootstrap.Settings.DamagePerShot, HitPoint = shootHit.point });
            }

            data.LineRenderer[i].SetPosition(1, shootHit.point);
        }
        else
        {
            data.LineRenderer[i].SetPosition(1, shootRay.origin + shootRay.direction * SurvivalShooterBootstrap.Settings.GunRange);
        }
    }

    private void DisableEffects(int i)
    {
        data.LineRenderer[i].enabled = false;
        data.Light[i].enabled = false;
    }
}
