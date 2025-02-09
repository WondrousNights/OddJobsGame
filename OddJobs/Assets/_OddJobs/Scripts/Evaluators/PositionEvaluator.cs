using System.Collections.Generic;
using UnityEngine;

public static class PositionEvaluator 
{
    private static float EvaluatePosition(Vector3 position, Vector3 enemyPos)
    {
        float score = 0f;

        // 🔹 1. Distance to Enemy (Prefer Mid-Range)
        float distance = Vector3.Distance(position, enemyPos);
        score += Mathf.Clamp(30f - distance, 0, 30f); // Favor 10-30m range

        // 🔹 2. Cover Bonus
        if (Physics.Linecast(position, enemyPos))
        {
            score += 100f; // High score for cover
        }

        // 🔹 3. Line of Sight to Enemy (Higher if visible)
        if (!Physics.Linecast(position, enemyPos))
        {
            score += 20f; // Encourage good attack spots
        }

        return score;
    }

    public static Vector3 ChooseBestPosition(List<Vector3> positions, Vector3 enemyPos)
    {
        Vector3 bestPosition = positions[0];
        float bestScore = float.MinValue;

        foreach (var pos in positions)
        {
            float score = EvaluatePosition(pos, enemyPos);

            if (score > bestScore)
            {
                bestScore = score;
                bestPosition = pos;
            }
        }

        return bestPosition;
    }

}
