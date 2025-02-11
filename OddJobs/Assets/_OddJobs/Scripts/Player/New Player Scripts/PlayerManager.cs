using RootMotion.Dynamics;
using Unity.Netcode;
using UnityEngine;


//This class should hold all references and configure the network properties
public class PlayerManager : NetworkBehaviour
{

    //Player State
    public enum PlayerState
    {
        Active,
        Dead,
        InMenu
    }

    public PlayerState currentPlayerState = PlayerState.Active;
    bool hasSpawned = false;


    //Access Scripts
    public PlayerController playerController;
    public NetworkAnimationController networkAnimationController;
    public Player_UIManager playerUIManager;
    public Network_WeaponInventory weaponInventory;
    public Network_PlayerWeaponHandler weaponHandler;
    public PlayerAmmoHandler ammoHandler;
    public Network_HealthManager healthManager;

    //Random Shit
    [Header("Set in Inspector")]
    public PuppetMaster puppetMaster;
    [SerializeField] GameObject myCanvas;
    [SerializeField] AudioListener myListener;
    [SerializeField] GameObject[] bodyRenders;
   
    
    
    void Awake()
    {
        playerController =  GetComponent<PlayerController>();
        networkAnimationController = GetComponent<NetworkAnimationController>();
        healthManager = GetComponent<Network_HealthManager>();
        playerUIManager = GetComponentInChildren<Player_UIManager>();
        weaponInventory = GetComponent<Network_WeaponInventory>();
        weaponHandler = GetComponent<Network_PlayerWeaponHandler>();
        ammoHandler = GetComponent<PlayerAmmoHandler>();

    }



    private void Start()
    {
        
        //Register Events
        NetworkManager.Singleton.SceneManager.OnSceneEvent += SetSpawn;
        
        if(!IsOwner)
        {
            puppetMaster.mode = PuppetMaster.Mode.Disabled;
            Invoke("SetPuppetActive", 3f);
        }

        Cursor.lockState = CursorLockMode.Locked;
        

        if (IsOwner)
        {
            playerController.cam.gameObject.SetActive(true);
            gameObject.layer = 15;

            foreach(GameObject go in bodyRenders)
            {
            go.layer = 15;
            }

        }
        else
        {
            playerController.cam.gameObject.SetActive(false);
            myCanvas.gameObject.SetActive(false);
            playerController.controller.enabled = false;
            myListener.enabled = false;
        }

    }
    



    
    void SetSpawn(SceneEvent sceneEvent)
    {

        if (sceneEvent.SceneEventType == SceneEventType.LoadEventCompleted)
        {
            Debug.Log("Spawn at pos");

            SetSpawnRpc();

        }

        if (sceneEvent.SceneEventType == SceneEventType.Unload)
        {
            SetHasSpawnedFalseRpc();
        }

    }
    
    [Rpc(SendTo.Everyone)]
    void SetSpawnRpc()
    {
        Transform spawnPos = GameObject.FindGameObjectWithTag("PlayerSpawn").transform;
        this.gameObject.transform.position = spawnPos.position;
        Debug.Log(spawnPos.position);
        puppetMaster.mode = PuppetMaster.Mode.Disabled;
        Invoke("SetSpawnTrue", 2f);
    }

    [Rpc(SendTo.Everyone)]
    void SetHasSpawnedFalseRpc()
    {
        hasSpawned = false;
    }

    void SetSpawnTrue()
    {
        hasSpawned = true;
        healthManager.Respawn();

        puppetMaster.mode = PuppetMaster.Mode.Active;

    }



   void Update()
   {
        if(currentPlayerState == PlayerState.InMenu)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
   }


}
