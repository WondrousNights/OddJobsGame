using UnityEngine;

public class ThingSpawner : MonoBehaviour
{
   [SerializeField] private GameObject[] thing;
   [SerializeField] private float spawnRate;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnThing), 0, spawnRate);
    }

    private void SpawnThing()
    {
        int thingdex = 0;

        Instantiate(thing[thingdex], transform.position, Quaternion.identity);

        thingdex++;

        if (thingdex >= thing.Length)
        {
            thingdex = 0;
            // send message it's over
            CancelInvoke(nameof(SpawnThing));
        }
    }
}

