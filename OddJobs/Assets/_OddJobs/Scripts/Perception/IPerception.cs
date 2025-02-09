using System;
using UnityEngine;

public interface IPerception
{
    void SetManager(Enemy_PerceptionManager perceptionManager);
    void StartPerception();
    void UpdatePerception();
}
