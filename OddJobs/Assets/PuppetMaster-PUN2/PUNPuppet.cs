using UnityEngine;
using System.Collections;
using Unity.Netcode;
using RootMotion.Dynamics;


/// <summary>
/// Synchronizes a puppet over the network. The server instance of the puppet is fully authoritative over switching BehaviourPuppet states. 
/// Other than that, the clients are free to collide with objects and solve their own physics. Ragoll syncing is done only while the puppet is unpinned.
/// </summary>
public class PUNPuppet : NetworkBehaviour
    {

        [Tooltip("The frequency of syncing Rigidbodies.")]
        public float rigidbodySyncInterval = 0.1f;

        [Tooltip("Normally rigidbodies ase synced via velocity and angular velocity only (for better smoothness). However if a rigidbody drifts more than this value from it's synced position, it's position and rotation will be snapped to the synced state.")]
        public float rigidbodyPositionTolerance = Mathf.Infinity;

        private Rigidbody[] syncRigidbodies = new Rigidbody[0];
        private PuppetMaster puppetMaster;
        private BehaviourPuppet puppet;
        private float nextSyncTime;
        private float syncBlend;
        private Vector3[] velocities = new Vector3[0];
        private Vector3[] angularVelocities = new Vector3[0];
        private Vector3[] positions = new Vector3[0];
        private Vector3[] rotations = new Vector3[0];
        private bool syncFlag;

        [SerializeField] bool CanLoseBalance = false;

        void Start()
        {
            // Find all required components
            puppetMaster = GetComponentInChildren<PuppetMaster>();
            puppet = GetComponentInChildren<BehaviourPuppet>();
            syncRigidbodies = puppetMaster.GetComponentsInChildren<Rigidbody>();

            velocities = new Vector3[syncRigidbodies.Length];
            angularVelocities = new Vector3[syncRigidbodies.Length];
            positions = new Vector3[syncRigidbodies.Length];
            rotations = new Vector3[syncRigidbodies.Length];

            // Only the owner instance will get event calls
            if (IsOwner)
            {
                puppet.onLoseBalance.unityEvent.AddListener(OnLoseBalance);
                puppet.onGetUpProne.unityEvent.AddListener(OnGetUp);
                puppet.onGetUpSupine.unityEvent.AddListener(OnGetUp);
                name = "PUN Puppet " + "Mine";

                //My bullets cant hit my body
                
                puppetMaster.gameObject.layer = 16;
                Transform[] ragdollBits = puppetMaster.GetComponentsInChildren<Transform>();
                foreach(Transform go in ragdollBits)
                {
                    go.gameObject.layer = 16;
                }
                
            }
            else
            {
                // Make sure that I can't change other person puppet.
                puppet.knockOutDistance = Mathf.Infinity;
                puppet.canGetUp = false;
                puppet.canMoveTarget = false;
                puppet.CanLoseBalance = false;
                name = "PUN Puppet " + "Not Mine";
            }

            puppetMaster.transform.parent = null;
        }

        // Force instances on the client machines to lose balance
        void OnLoseBalance()
        {
            if(!IsOwner) return;
            LoseBalanceServerRpc();
        }

        // Force instances on the client machines to get up
        void OnGetUp()
        {
            if(!IsOwner) return;
            GetUpServerRpc();
        }

        // Force instances on the client machines to lose balance
        [Rpc(SendTo.NotOwner)]
        void LoseBalanceServerRpc()
        {
            puppet.SetState(BehaviourPuppet.State.Unpinned);
        }

        // Force instances on the client machines to get up
        [Rpc(SendTo.NotOwner)]
        void GetUpServerRpc()
        {
           puppet.SetState(BehaviourPuppet.State.GetUp);
        }

     

        // We have unparented PuppetMaster so make sure it doesn't remain when this character is destroyed.
        void OnDestroy()
        {
            if (puppetMaster != null) Destroy(puppetMaster.gameObject);
        }

        // Returns true if puppet is fully pinned and no rigidbody syncing should be required
        private bool PuppetIsPinned()
        {
            if (puppet.state == BehaviourPuppet.State.Unpinned) return false;
            if (!puppetMaster.isActive) return true;

            foreach (Muscle m in puppetMaster.muscles)
            {
                if (m.state.pinWeightMlp < 1f) return false;
            }
            return true;
        }

        // Rigidbody syncing
        void FixedUpdate()
        {
            if (IsOwner)
            {
                FixedUpdateLocal();
            }
            else
            {
                FixedUpdateRemote();
            }
        }

        // Rigidbody syncing, local
        private void FixedUpdateLocal()
        {
            bool puppetIsPinned = PuppetIsPinned();
            syncBlend = Mathf.MoveTowards(syncBlend, puppetIsPinned ? 0f : 1f, Time.deltaTime * 3f);

            // Time to sync
            if (Time.time >= nextSyncTime && syncBlend > 0)
            {
                bool velocitiesOnly = rigidbodyPositionTolerance == Mathf.Infinity;

                // Read the positions, rotations and velocities of the Rigidbodies
                for (int i = 0; i < syncRigidbodies.Length; i++)
                {
                    if (!velocitiesOnly)
                    {
                        positions[i] = syncRigidbodies[i].position;
                        rotations[i] = syncRigidbodies[i].rotation.eulerAngles;
                    }
                    velocities[i] = syncRigidbodies[i].linearVelocity;
                    angularVelocities[i] = syncRigidbodies[i].angularVelocity;
                }

                // RPC to send the information over to the clients
                if (velocitiesOnly)
                {
                    SyncRigidbodyVelocitiesServerRpc(velocities, angularVelocities, syncBlend);
                }
                else
                {
                    SyncRigidbodiesServerRpc(positions, rotations, velocities, angularVelocities, syncBlend);
                }

                // When to sync next?
                nextSyncTime = Time.time + rigidbodySyncInterval;
            }
        }

        // Rigidbody syncing, remote
        private void FixedUpdateRemote()
        {
            if (syncFlag) // Using syncFlag to make sure rigidbodies are moved/rotated in FixedUpdate, not whenever the RPC arrives
            {
                float toleranceSqr = rigidbodyPositionTolerance * rigidbodyPositionTolerance;
                bool velocitiesOnly = rigidbodyPositionTolerance == Mathf.Infinity;

                // Ragdoll rigidbodies
                for (int i = 0; i < syncRigidbodies.Length; i++)
                {
                    if (!velocitiesOnly)
                    {
                        float posOffsetSqr = Vector3.SqrMagnitude(syncRigidbodies[i].position - positions[i]);
                        if (posOffsetSqr > toleranceSqr)
                        {
                            syncRigidbodies[i].MovePosition(Vector3.Lerp(syncRigidbodies[i].position, positions[i], syncBlend));
                            syncRigidbodies[i].MoveRotation(Quaternion.Slerp(syncRigidbodies[i].rotation, Quaternion.Euler(rotations[i]), syncBlend));
                        }
                    }

                    syncRigidbodies[i].linearVelocity = Vector3.Lerp(syncRigidbodies[i].linearVelocity, velocities[i], syncBlend);
                    syncRigidbodies[i].angularVelocity = Vector3.Lerp(syncRigidbodies[i].angularVelocity, angularVelocities[i], syncBlend);
                }

                syncFlag = false;
            }
        }

        // Syncing the positions, rotations and velocities of the Rigidbodies from the owner to the clients.
        [Rpc(SendTo.NotMe)]
        void SyncRigidbodiesServerRpc(Vector3[] positions, Vector3[] rotations, Vector3[] velocities, Vector3[] angularVelocities, float syncBlend)
        {

            if (this.positions.Length == 0) return; // Not initiated yet

            for (int i = 0; i < positions.Length; i++)
            {
                this.positions[i] = positions[i];
                this.rotations[i] = rotations[i];
                this.velocities[i] = velocities[i];
                this.angularVelocities[i] = angularVelocities[i];
            }

            this.syncBlend = syncBlend;
            syncFlag = true; // Using this to make sure rigidbodies are moved/rotated in FixedUpdate, not whenever this RPC arrives

           
           
        }


        // Syncing only the velocities and angularVelocities of the Rigidbodies from the owner to the clients.
        [Rpc(SendTo.NotMe)]
        void SyncRigidbodyVelocitiesServerRpc(Vector3[] velocities, Vector3[] angularVelocities, float syncBlend)
        {
            if (this.positions.Length == 0) return; // Not initiated yet

            for (int i = 0; i < positions.Length; i++)
            {
                this.velocities[i] = velocities[i];
                this.angularVelocities[i] = angularVelocities[i];
            }

            this.syncBlend = syncBlend;
            syncFlag = true; // Using this to make sure rigidbodies are moved/rotated in FixedUpdate, not whenever this RPC arrives

           
           
        }

        
        
    }

