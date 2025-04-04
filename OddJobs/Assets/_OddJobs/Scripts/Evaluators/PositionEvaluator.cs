using System.Collections.Generic;
using UnityEngine;

public static class PositionEvaluator 
{

    private static float EvaluatePosition(Vector3 position, Vector3 enemyPos, EnemyPersonality enemyPersonality)
    {
        float score = 0f;

        //What is my distance to enemy?
        float distance = Vector3.Distance(position, enemyPos);
        score += Mathf.Exp(-Mathf.Pow((distance - enemyPersonality.OptimalDistance) / enemyPersonality.DistanceTolerance , 2)) * 30f; // Gaussian falloff : I got this code from ChatGPT

        Debug.DrawRay(position + Vector3.up, (enemyPos - position).normalized * distance, Color.green);

        bool lineCast = Physics.Linecast(position + Vector3.up, enemyPos + Vector3.up, out RaycastHit hitInfo, enemyPersonality.CoverMask);

        //Cover Bonus
        if (lineCast)
        {
            //Debug.Log($"Position {position} has great cover!! Hit: {hitInfo.collider.name} at {hitInfo.collider.transform.position}");
            score += enemyPersonality.CoverScore; // High score for cover
            
        }

        //Line of Sight to Player
        if (!lineCast)
        {
            score += enemyPersonality.LineOfSightScore; 
        }

        return score;
    }

    public static Vector3 ChooseBestPosition(List<Vector3> positions, Vector3 enemyPos, EnemyPersonality enemyPersonality)
    {
        Vector3 bestPosition = positions[0];
        float bestScore = float.MinValue;

        foreach (var pos in positions)
        {
            float score = EvaluatePosition(pos, enemyPos, enemyPersonality);

            if (score > bestScore)
            {
                bestScore = score;
                bestPosition = pos;
            }
        }

        return bestPosition;
    }

}
