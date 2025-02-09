using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationPerception : MonoBehaviour, IPerception
{
    Enemy_PerceptionManager PerceptionManager;

    [SerializeField] private float gridSize = 10f;
    [SerializeField] private int gridExtent = 10;
    [SerializeField] float spacingX = 0.5f; 
    [SerializeField] float spacingZ = 0.5f; 
    [SerializeField] float updateThreshold = 1f;
    [SerializeField] List<Vector3> validPositions = new List<Vector3>();
    private Vector3 lastSamplePosition;



    public void UpdatePerception()
    {
        if (Vector3.Distance(transform.position, lastSamplePosition) > updateThreshold)
        {
            lastSamplePosition = transform.position;
            UpdateNavMeshSamples();
            
        }
    }

    private void UpdateNavMeshSamples()
    {
        validPositions.Clear();
        Vector3 basePosition = transform.position;

        for (int x = -gridExtent; x <= gridExtent; x++)
        {
            for (int z = -gridExtent; z <= gridExtent; z++)
            {
                Vector3 offset = new Vector3(x * spacingX, 0, z * spacingZ);
                Vector3 samplePos = basePosition + offset;

                if (NavMesh.SamplePosition(samplePos, out NavMeshHit hit, gridSize, NavMesh.AllAreas))
                {
                    validPositions.Add(hit.position);
                }
            }
        }
        PerceptionManager.InvokePerceptionEvent("OnNavigationPerception", validPositions);
    }

    private void OnDrawGizmos()
    {
        // Draw all sampled positions as small blue spheres
        Gizmos.color = Color.blue;
        foreach (Vector3 pos in validPositions)
        {
            Gizmos.DrawSphere(pos, 0.3f);
        }
        
    }

    public void StartPerception()
    {
        lastSamplePosition = transform.position;
        UpdateNavMeshSamples();
    }

    public void SetManager(Enemy_PerceptionManager perceptionManager)
    {
        this.PerceptionManager = perceptionManager;
    }
}
