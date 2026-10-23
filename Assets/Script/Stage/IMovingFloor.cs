using UnityEngine;

namespace InGame.Stage
{
    /// <summary>うごく床用インターフェース。Power = 床の現在の移動力(ワールド座標, m/s)</summary>
    public interface IMovingFloor
    {
        Vector3 Power { get; }
    }
}