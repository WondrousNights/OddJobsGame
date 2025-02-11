using UnityEngine;

[CreateAssetMenu(fileName = "EnemyPersonality", menuName = "Enemies/EnemyPersonality", order = 1)]
public class EnemyPersonality : ScriptableObject
{
    public float OptimalDistance;
    public float DistanceTolerance;

    public LayerMask CoverMask;
    public float CoverScore;
    public float LineOfSightScore;
    
}
