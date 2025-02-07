using System.Collections;
using System.Numerics;
//using AlmenaraGames;
using UnityEngine;
using UnityEngine.Pool;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;
using Unity.Netcode;
using Vector2 = UnityEngine.Vector2;

[CreateAssetMenu(fileName = "WeaponProperties", menuName = "Weapons/WeaponProperties", order = 0)]
public abstract class Network_WeaponProperties : ScriptableObject
{
    [Header("Weapon Settings")]
    public string Name;
    public WeaponType type;
    public GameObject ModelPrefab;
    public Vector3 PlayerSpawnPoint;
    public Vector3 PlayerSpawnRotation;
    public Vector3 VisuaSpawnPos;
    public Vector3 VisualRotation;
    public GameObject DroppedPrefab;
    public Transform FirePoint;
    

    public Weapon Spawn()
    {
       GameObject go = Instantiate(ModelPrefab);
       return go.GetComponent<Weapon>();
    }
    
    
}