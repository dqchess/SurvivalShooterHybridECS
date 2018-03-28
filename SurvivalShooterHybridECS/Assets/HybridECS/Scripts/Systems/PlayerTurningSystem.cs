using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class PlayerTurningSystem : ComponentSystem
{
    public struct Data
    {
        public int Length;
        public ComponentDataArray<Player> Player;
        public GameObjectArray GameObject;
        public ComponentArray<Rigidbody> Rigidbody;
        public SubtractiveComponent<Dead> Dead;
    }

    [Inject] private Data data;

    protected override void OnUpdate()
    {
        var camera = Camera.main;
        var mousePos = Input.mousePosition;

        var camRayLen = SurvivalShooterBootstrap.Settings.CamRayLen;
        var floor = LayerMask.GetMask("Floor");

        for (var i = 0; i < data.Length; i++)
        {
            var camRay = camera.ScreenPointToRay(mousePos);
            RaycastHit floorHit;
            if (Physics.Raycast(camRay, out floorHit, camRayLen, floor))
            {
                var position = data.GameObject[i].transform.position;
                var rigidbody = data.Rigidbody[i];
                var playerToMouse = floorHit.point - new Vector3(position.x, position.y, position.z);
                playerToMouse.y = 0f;
                var newRot = Quaternion.LookRotation(playerToMouse);
                rigidbody.MoveRotation(newRot);
            }
        }
    }
}
