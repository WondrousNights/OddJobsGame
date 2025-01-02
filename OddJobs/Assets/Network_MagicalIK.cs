using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Network_MagicalIK : MonoBehaviour
{
    [SerializeField] TwoBoneIKConstraint rightHandIk;
    [SerializeField] TwoBoneIKConstraint leftHandIk;

    [SerializeField] RigBuilder rigBuilder;

    public void DoMagicalIK(GameObject gun)
    {
        rightHandIk.data.target = gun.transform;
        rigBuilder.Build();
    }
}
