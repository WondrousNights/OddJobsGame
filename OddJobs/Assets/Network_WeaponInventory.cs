using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Network_WeaponInventory : NetworkBehaviour
{
    [SerializeField] Transform gunHolder;
    [SerializeField] Weapon[] Inventory;
    [SerializeField] Network_GunScriptableObjectList PossibleWeaponsList;
    private int currentWeaponIndex = 0;
    [SerializeField] Weapon activeWeapon;
    [SerializeField]  Weapon activeWeaponVisual;

    [SerializeField] bool debug;

    Network_MagicalIK magicalIK;
    Network_PlayerInputController playerInputController;

    float itemDropForce = 2f;

    void Start()
    {
        magicalIK = GetComponent<Network_MagicalIK>();
        playerInputController = GetComponent<Network_PlayerInputController>();

        playerInputController.onFoot.DropItem.performed += ctx => DropCurrentGun();
        playerInputController.onFoot.SwitchItemNext.performed += ctx => SwitchWeaponNext();
        playerInputController.onFoot.SwitchItemPrevious.performed += ctx => SwitchWeaponPrevious();

    }

    public Weapon GetCurrentWeapon()
    {
        return activeWeapon;
    }

    public Weapon GetCurrentVisualWeapon()
    {
        return activeWeaponVisual;
    }

   
    public void PickupWeapon(Network_WeaponProperties weaponProperties)
    {
        if(!IsOwner) return;
        // find the next empty slot in the inventory
        int nextFreeIndex = -1;
        for (int i = 0; i < Inventory.Length; i++)
        {
            if (Inventory[i] == null)
            {
                if (debug) Debug.Log(Inventory[i]);
                nextFreeIndex = i;
                break;
            }
        }

        Weapon newWeapon = weaponProperties.Spawn();
        newWeapon.transform.SetParent(gunHolder);
        newWeapon.transform.localPosition = weaponProperties.PlayerSpawnPoint;
        newWeapon.transform.localRotation = Quaternion.Euler(weaponProperties.PlayerSpawnRotation);

        PickupWeaponRpc(newWeapon.transform.position, newWeapon.transform.rotation.eulerAngles, newWeapon.weaponProperties.type);
  
        // if there's an empty slot, add the gun to the inventory there
        if (nextFreeIndex != -1)
        {
            AddWeaponToInventory(newWeapon, nextFreeIndex);
  
        } 
        else
        {
            DropCurrentGun(); // throw current weapon
            AddWeaponToInventory(newWeapon, currentWeaponIndex); // add the gun to the inventory
        }
        if (debug) Debug.Log("Picked up gun: " + weaponProperties.name);
    }

    [Rpc(SendTo.Everyone)]
    public void PickupWeaponRpc(Vector3 position, Vector3 rotation, WeaponType type)
    {


        for(var i = 0; i < PossibleWeaponsList.WeaponPropertiesList.Count; i++)
        {
            if(PossibleWeaponsList.WeaponPropertiesList[i].type == type)
            {
                //Spawn shit here
                Network_WeaponProperties weaponProperties = PossibleWeaponsList.WeaponPropertiesList[i];
                

                GameObject Model = Instantiate(weaponProperties.ModelPrefab);
                Model.transform.SetParent(gameObject.transform, false);
                Model.transform.localPosition = weaponProperties.VisuaSpawnPos;
                Model.transform.localRotation = Quaternion.Euler(weaponProperties.VisualRotation);

                activeWeaponVisual = Model.GetComponent<Weapon>();

                magicalIK.DoMagicalIK(activeWeaponVisual);
            }
        }

        if(IsOwner)
        {
            activeWeaponVisual.gameObject.SetActive(false);
        }
    }

    public void AddWeaponToInventory(Weapon weapon, int weaponIndex)
    {
        if(!IsOwner) return;
        Inventory[weaponIndex] = weapon;
       
        activeWeapon = weapon;
    }

    public void EquipGunFromInventory(int index = -1)
    {
        if(!IsOwner) return;
        DeEquipCurrentGun();

        if (index != -1) {
            currentWeaponIndex = index;
        }
        
        activeWeapon = Inventory[currentWeaponIndex];

        if(activeWeapon != null) activeWeapon.gameObject.SetActive(true);
    }

    public void DropCurrentGun()
    {
        if(!IsOwner) return;
        if(activeWeapon == null) return;
        DropGunRpc(0);
        activeWeapon.DestroyWeapon();
        activeWeaponVisual.DestroyWeapon();
    }

    [Rpc(SendTo.Everyone)]
    public void DropGunRpc(int ammoInClip)
    {
        var droppedModel = Instantiate(activeWeapon.weaponProperties.DroppedPrefab);
        //droppedModel.GetComponent<Network_ItemPickup>().ammoInClip = ammoInClip;

        var instanceNetworkObject = droppedModel.GetComponent<NetworkObject>();
        instanceNetworkObject.Spawn();

        droppedModel.transform.position = gunHolder.position + (Vector3.forward * 2);
        droppedModel.transform.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), 0);
       
        // add throwing force to the weapon
        Rigidbody rb = droppedModel.GetComponent<Rigidbody>();
        rb.AddForce(gunHolder.forward * itemDropForce, ForceMode.VelocityChange);
            
    }

     public void DeEquipCurrentGun()
    {
        if(!IsOwner) return;
        if (activeWeapon) 
        {
            activeWeapon.gameObject.SetActive(false);
            DeEquipVisualGunRpc();
        }

        /*
        if (gunEffects)
        {
            gunEffects.gameObject.SetActive(false);
            Destroy(gunEffects.gameObject);
        }
        
        inventoryUI.UpdateInventoryUI(Inventory);
        */
    }

    [Rpc(SendTo.Everyone)]
    private void DeEquipVisualGunRpc()
    {
        activeWeaponVisual.gameObject.SetActive(false);
    }

    public void SwitchWeaponNext()
    {
        if (debug) Debug.Log("Switching to next weapon");

        currentWeaponIndex++;
        if (currentWeaponIndex >= Inventory.Length) currentWeaponIndex = 0;

        // if the index has no gun in it, find the next gun in the inventory
        if (Inventory[currentWeaponIndex] == null)
        {
            for (int i = 0; i < Inventory.Length; i++)
            {
                if (Inventory[i] != null) { currentWeaponIndex = i; break; }
            }
        }

        EquipGunFromInventory(currentWeaponIndex);
        //inventoryUI.UpdateInventoryUI(Inventory);
    }
    public void SwitchWeaponPrevious()
    {
        if (debug) Debug.Log("Switching to previous weapon");
        
        currentWeaponIndex--;
        if (currentWeaponIndex < 0) currentWeaponIndex = Inventory.Length - 1;

        // if the index has no gun in it, find the previous gun in the inventory
        if (Inventory[currentWeaponIndex] == null)
        {
            for (int i = Inventory.Length - 1; i >= 0; i--)
            {
                if (Inventory[i] != null) { currentWeaponIndex = i; break; }
            }
        }

        EquipGunFromInventory(currentWeaponIndex);
        //inventoryUI.UpdateInventoryUI(Inventory);
    }

    

}
