using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Network_MagicalIK : MonoBehaviour
{
    [SerializeField] TwoBoneIKConstraint rightHandIk;
    [SerializeField] TwoBoneIKConstraint leftHandIk;

    [SerializeField] RigBuilder rigBuilder;

    public void DoMagicalIK(GameObject gun)
    {
        Network_GunIkHandler ikHandler = gun.GetComponent<Network_GunIkHandler>();

        if(ikHandler.oneHanded)
        {
            rightHandIk.data.target = ikHandler.rightHandIKTarget;
        }
        else
        {
            leftHandIk.data.target = ikHandler.leftHandIKTarget;
            rightHandIk.data.target = ikHandler.rightHandIKTarget;
        }

        

        //Gun.getcomponent IK position, then set the target to that, will fix it super easy.
        rigBuilder.Build();
    }

    public void UndoMagicalIk()
    {
        rightHandIk.data.target = null;
        leftHandIk.data.target = null;

        rigBuilder.Build();
    }
}
