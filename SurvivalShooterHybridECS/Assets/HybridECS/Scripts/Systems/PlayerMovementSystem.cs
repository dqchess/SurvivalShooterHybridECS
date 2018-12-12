using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class PlayerMovementSystem : ComponentSystem
{
#pragma warning disable 649
    private struct Data
    {
        public readonly int Length;
        public GameObjectArray GameObject;
        public ComponentArray<Rigidbody> Rigidbody;
        [ReadOnly] public ComponentDataArray<PlayerInput> PlayerInput;
        public SubtractiveComponent<Dead> Dead;
    }

    [Inject] private Data data;
#pragma warning restore 649

    protected override void OnUpdate()
    {
        var speed = SurvivalShooterBootstrap.Settings.PlayerMoveSpeed;
        var dt = Time.deltaTime;

        for (var i = 0; i < data.Length; i++)
        {
            var move = data.PlayerInput[i].Move;

            var movement = new Vector3(move.x, 0, move.y);
            movement = movement.normalized * speed * dt;

            var position = data.GameObject[i].transform.position;
            var rigidbody = data.Rigidbody[i];

            var newPos = new Vector3(position.x, position.y, position.z) + movement;
            rigidbody.MovePosition(newPos);
        }
    }
}
