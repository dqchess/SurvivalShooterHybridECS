using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class PlayerInputSystem : ComponentSystem
{
    public struct Data
    {
        public int Length;
        public ComponentDataArray<PlayerInput> PlayerInput;
        public SubtractiveComponent<Dead> Dead;
    }

    [Inject] private Data data;

    protected override void OnUpdate()
    {
        for (var i = 0; i < data.Length; i++)
        {
            var newInput = new PlayerInput
            {
                Move = new float2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))
            };
            data.PlayerInput[i] = newInput;
        }
    }
}
