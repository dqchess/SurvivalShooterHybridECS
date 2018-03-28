using Unity.Entities;
using UnityEngine;

public class EnemyDeathSystem : ComponentSystem
{
    public struct Data
    {
        public int Length;
        public GameObjectArray GameObject;
        public ComponentDataArray<Enemy> Enemy;
        public ComponentDataArray<Dead> Dead;
        public ComponentArray<CapsuleCollider> CapsuleCollider;
        public ComponentArray<Animator> Animator;
        public ComponentArray<AudioSource> AudioSource;
    }

    [Inject] private Data data;

    private int score;

    protected override void OnUpdate()
    {
        var puc = PostUpdateCommands;
        var gameUi = SurvivalShooterBootstrap.Settings.GameUi;
        var scorePerDeath = SurvivalShooterBootstrap.Settings.ScorePerDeath;

        for (var i = 0; i < data.Length; i++)
        {
            data.CapsuleCollider[i].isTrigger = true;

            data.Animator[i].SetTrigger("Dead");

            data.AudioSource[i].clip = SurvivalShooterBootstrap.Settings.EnemyDeathClip;
            data.AudioSource[i].Play();

            var entity = data.GameObject[i].GetComponent<GameObjectEntity>().Entity;
            puc.RemoveComponent<Dead>(entity);

            score += scorePerDeath;
            gameUi.OnEnemyKilled(score);
        }
    }
}
