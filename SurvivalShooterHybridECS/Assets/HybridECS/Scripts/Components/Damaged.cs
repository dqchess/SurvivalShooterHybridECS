using Unity.Entities;
using Unity.Mathematics;

public struct Damaged : IComponentData
{
    public int Damage;
    public float3 HitPoint;
}
