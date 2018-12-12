using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class CameraFollowSystem : ComponentSystem
{
#pragma warning disable 649
    private struct Data
    {
        public readonly int Length;
        public GameObjectArray GameObject;
        [ReadOnly] public ComponentDataArray<PlayerInput> PlayerInput;
    }

    [Inject] private Data data;
#pragma warning restore 649

    private bool firstFrame = true;
    private Vector3 offset;

    protected override void OnUpdate()
    {
        var mainCamera = Camera.main;
        var smoothing = SurvivalShooterBootstrap.Settings.CamSmoothing;
        var dt = Time.deltaTime;

        var playerPos = data.GameObject[0].transform.position;

        if (firstFrame)
        {
            offset = mainCamera.transform.position - playerPos;
        }

        var targetCamPos = playerPos + offset;
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetCamPos, smoothing * dt);

        firstFrame = false;
    }
}
