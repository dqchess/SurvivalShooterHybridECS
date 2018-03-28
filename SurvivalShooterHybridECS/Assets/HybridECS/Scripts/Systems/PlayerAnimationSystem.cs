using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class PlayerAnimationSystem : ComponentSystem
{
    public struct Data
    {
        public int Length;
        public ComponentArray<Animator> Animator;
        [ReadOnly] public ComponentDataArray<PlayerInput> PlayerInput;
    }

    [Inject] private Data data;

    protected override void OnUpdate()
    {
        for (var i = 0; i < data.Length; i++)
        {
            var move = data.PlayerInput[i].Move;
            data.Animator[i].SetBool("IsWalking", move.x != 0f || move.y != 0f);
        }
    }
}
