using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;

public sealed class SurvivalShooterBootstrap
{
    public static SurvivalShooterSettings Settings;

    public static void NewGame()
    {
        var player = Object.Instantiate(Settings.PlayerPrefab);
        var entity = player.GetComponent<GameObjectEntity>().Entity;
        var entityManager = World.Active.GetExistingManager<EntityManager>();
        entityManager.AddComponentData(entity, new Player());
        entityManager.AddComponentData(entity, new Health { Value = Settings.StartingPlayerHealth });
        entityManager.AddComponentData(entity, new PlayerInput { Move = new float2(0, 0) });

        Settings.GameUi = GameObject.Find("GameUi").GetComponent<GameUi>();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void InitializeWithScene()
    {
        var settingsGo = GameObject.Find("Settings");
        Settings = settingsGo?.GetComponent<SurvivalShooterSettings>();
        Assert.IsNotNull(Settings);
    }
}
