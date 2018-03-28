using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class CameraFollowSystem : ComponentSystem
{
    public struct Data
    {
        public int Length;
        public GameObjectArray GameObject;
        [ReadOnly] public ComponentDataArray<PlayerInput> PlayerInput;
    }

    [Inject] private Data data;

    private bool firstFrame = true;

    private Vector3 offset;

    protected override void OnUpdate()
    {
        var smoothing = SurvivalShooterBootstrap.Settings.CamSmoothing;
        var dt = Time.deltaTime;

        var camera = Camera.main;
        var playerPos = data.GameObject[0].transform.position;

        if (firstFrame)
        {
            offset = camera.transform.position - playerPos;
        }

        var targetCamPos = playerPos + offset;
        camera.transform.position = Vector3.Lerp(camera.transform.position, targetCamPos, smoothing * dt);

        firstFrame = false;
    }
}
