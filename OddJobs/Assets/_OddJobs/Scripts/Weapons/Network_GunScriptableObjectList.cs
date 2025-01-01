using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GunList", menuName = "Guns/Network Gun Scriptable Object List", order = 0)]
public class Network_GunScriptableObjectList : ScriptableObject
{
    public List<Network_GunScriptableObject> GunScriptableObjectList;
}
