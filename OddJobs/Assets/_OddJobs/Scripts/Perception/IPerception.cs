using System;
using UnityEngine;

public interface IPerception
{
    void UpdatePerception();
    public event Action<GameObject> OnTargetSpotted;
    public event Action<GameObject> OnTargetLost;
    public event Action<GameObject> OnClosestTargetChanged;
}
