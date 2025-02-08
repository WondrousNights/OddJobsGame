using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "WeaponProperties", menuName = "Weapons/WeaponProperties", order = 0)]
public abstract class Network_WeaponProperties : ScriptableObject
{
    [Header("Weapon Settings")]
    public string Name;
    public Sprite Sprite;
    public WeaponType type;
    public GameObject ModelPrefab;
    public Vector3 PlayerSpawnPoint;
    public Vector3 PlayerSpawnRotation;
    public Vector3 VisuaSpawnPos;
    public Vector3 VisualRotation;
    public GameObject DroppedPrefab;
    

    public Weapon Spawn()
    {
       GameObject go = Instantiate(ModelPrefab);
       return go.GetComponent<Weapon>();
    }
    
    
}