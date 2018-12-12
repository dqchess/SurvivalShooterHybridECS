using Unity.Collections;
using Unity.Entities;
using UnityEngine.AI;

public class EnemyMovementSystem : ComponentSystem
{ 
#pragma warning disable 649
    private struct Data
    {
        public readonly int Length;
        public ComponentArray<NavMeshAgent> NavMeshAgent;
        public ComponentDataArray<Health> Health;
        public SubtractiveComponent<Dead> Dead;
    }

    private struct PlayerData
    {
        public GameObjectArray GameObject;
        [ReadOnly] public ComponentDataArray<Player> Player;
        [ReadOnly] public ComponentDataArray<Health> Health;
    }

    [Inject] private Data data;
    [Inject] private PlayerData playerData;
#pragma warning restore 649

    protected override void OnUpdate()
    {
        for (var i = 0; i < data.Length; i++)
        {
            var agent = data.NavMeshAgent[i];
            if (data.Health[i].Value > 0 && playerData.Health[0].Value > 0)
            {
                agent.SetDestination(playerData.GameObject[0].gameObject.transform.position);
            }
            else
            {
                agent.enabled = false;
            }
        }
    }
}
