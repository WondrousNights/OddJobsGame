//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.11.2
//     from Assets/_OddJobs/Scriptable Objects/PlayerInputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInputActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""OnFoot"",
            ""id"": ""8a1f1293-1b1c-4b26-bb71-1e8b849b84f3"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""3932cbf9-89a1-4886-9a35-aac4cfe6072c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""12ee524b-17d7-4579-8649-84dbcd55e695"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""5b2c4786-3cee-468f-97de-1b08c82c7878"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""9ec0ad15-c627-4a27-bfe7-f06aeb71f0ab"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Reload"",
                    ""type"": ""Button"",
                    ""id"": ""0efa3191-f55b-44b0-92a7-a1f656e16b5d"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Ragdoll"",
                    ""type"": ""Button"",
                    ""id"": ""bddafb1f-b0e5-44d1-bb84-e0892f888822"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""d6d8b154-39c3-401c-89a3-69d36f6e81a4"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""UI_Next"",
                    ""type"": ""Button"",
                    ""id"": ""ec792995-944c-4f6e-88da-d61a6089b2b5"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""UI_Previous"",
                    ""type"": ""Button"",
                    ""id"": ""5952a2fa-2351-424d-80a9-f46c826be652"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""UI_Select"",
                    ""type"": ""Button"",
                    ""id"": ""81d19976-75d6-4ba9-9260-a92252918456"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""6306204b-ded2-4ed4-ac34-bbf1fb5e118f"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""39ad3be0-94da-40b9-97c6-a3fece2f1495"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard & Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""2be9c38f-cfa9-4656-9c2e-0f6dc0f0089e"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard & Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""cda77416-fc9b-4d09-b207-71a838944b30"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard & Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""d962260a-13c0-4c6d-b8d3-980ea13a0601"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard & Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""367a57c5-fbe5-4f4f-8bed-c0f4f729195b"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Controller"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""30dacfa8-7e94-4feb-889b-7ab7f385ec44"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard & Mouse"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a3ea2ccb-0787-4184-99ec-8e49273d8b33"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Controller"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6db64ee7-6179-4cec-86f5-192e04b7bc77"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard & Mouse"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d43c6b24-3b4b-4a61-b522-de070388397f"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Controller"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""69f13eaf-ba54-4d3b-9f62-1a597457151c"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard & Mouse"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f0b5fbf1-efc7-4cbc-9796-fecc02e1db30"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Controller"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""34a83730-c41a-411a-82be-a350e12f8912"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard & Mouse"",
                    ""action"": ""Reload"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""382f3ebd-b6c2-44bd-858c-cfa534904b56"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": "";Controller"",
                    ""action"": ""Reload"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9c6265d0-1b16-4bf9-83d4-b2a3f99178bd"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Controller"",
                    ""action"": ""Ragdoll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ed5a1f6b-e014-4a2f-b819-c1321fc6f6b4"",
                    ""path"": ""<Keyboard>/l"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard & Mouse"",
                    ""action"": ""Ragdoll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""603e8355-937b-413c-884e-5e6db2546247"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard & Mouse"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0ea4b0cf-9f19-4446-b9f6-b173135aa898"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": "";Controller"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0fe1c301-004b-423b-b25c-98794a7e691f"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Controller"",
                    ""action"": ""UI_Next"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""59189c0f-a2b8-44e8-a56e-bc6f7d1ba61a"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard & Mouse"",
                    ""action"": ""UI_Next"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""20bdd468-cc52-4ba6-8d8a-26156e94bfa4"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Controller"",
                    ""action"": ""UI_Previous"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""af57f3c9-4dd7-49bd-9d80-88500cbe864e"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard & Mouse"",
                    ""action"": ""UI_Previous"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a9ff5a3f-5da7-4372-afcb-074b94fb60d1"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Controller"",
                    ""action"": ""UI_Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7498c3cb-97db-446a-b4ab-6627c0b0ed71"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard & Mouse"",
                    ""action"": ""UI_Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard & Mouse"",
            ""bindingGroup"": ""Keyboard & Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Controller"",
            ""bindingGroup"": ""Controller"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // OnFoot
        m_OnFoot = asset.FindActionMap("OnFoot", throwIfNotFound: true);
        m_OnFoot_Move = m_OnFoot.FindAction("Move", throwIfNotFound: true);
        m_OnFoot_Jump = m_OnFoot.FindAction("Jump", throwIfNotFound: true);
        m_OnFoot_Look = m_OnFoot.FindAction("Look", throwIfNotFound: true);
        m_OnFoot_Shoot = m_OnFoot.FindAction("Shoot", throwIfNotFound: true);
        m_OnFoot_Reload = m_OnFoot.FindAction("Reload", throwIfNotFound: true);
        m_OnFoot_Ragdoll = m_OnFoot.FindAction("Ragdoll", throwIfNotFound: true);
        m_OnFoot_Interact = m_OnFoot.FindAction("Interact", throwIfNotFound: true);
        m_OnFoot_UI_Next = m_OnFoot.FindAction("UI_Next", throwIfNotFound: true);
        m_OnFoot_UI_Previous = m_OnFoot.FindAction("UI_Previous", throwIfNotFound: true);
        m_OnFoot_UI_Select = m_OnFoot.FindAction("UI_Select", throwIfNotFound: true);
    }

    ~@PlayerInputActions()
    {
        UnityEngine.Debug.Assert(!m_OnFoot.enabled, "This will cause a leak and performance issues, PlayerInputActions.OnFoot.Disable() has not been called.");
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // OnFoot
    private readonly InputActionMap m_OnFoot;
    private List<IOnFootActions> m_OnFootActionsCallbackInterfaces = new List<IOnFootActions>();
    private readonly InputAction m_OnFoot_Move;
    private readonly InputAction m_OnFoot_Jump;
    private readonly InputAction m_OnFoot_Look;
    private readonly InputAction m_OnFoot_Shoot;
    private readonly InputAction m_OnFoot_Reload;
    private readonly InputAction m_OnFoot_Ragdoll;
    private readonly InputAction m_OnFoot_Interact;
    private readonly InputAction m_OnFoot_UI_Next;
    private readonly InputAction m_OnFoot_UI_Previous;
    private readonly InputAction m_OnFoot_UI_Select;
    public struct OnFootActions
    {
        private @PlayerInputActions m_Wrapper;
        public OnFootActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_OnFoot_Move;
        public InputAction @Jump => m_Wrapper.m_OnFoot_Jump;
        public InputAction @Look => m_Wrapper.m_OnFoot_Look;
        public InputAction @Shoot => m_Wrapper.m_OnFoot_Shoot;
        public InputAction @Reload => m_Wrapper.m_OnFoot_Reload;
        public InputAction @Ragdoll => m_Wrapper.m_OnFoot_Ragdoll;
        public InputAction @Interact => m_Wrapper.m_OnFoot_Interact;
        public InputAction @UI_Next => m_Wrapper.m_OnFoot_UI_Next;
        public InputAction @UI_Previous => m_Wrapper.m_OnFoot_UI_Previous;
        public InputAction @UI_Select => m_Wrapper.m_OnFoot_UI_Select;
        public InputActionMap Get() { return m_Wrapper.m_OnFoot; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(OnFootActions set) { return set.Get(); }
        public void AddCallbacks(IOnFootActions instance)
        {
            if (instance == null || m_Wrapper.m_OnFootActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_OnFootActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @Look.started += instance.OnLook;
            @Look.performed += instance.OnLook;
            @Look.canceled += instance.OnLook;
            @Shoot.started += instance.OnShoot;
            @Shoot.performed += instance.OnShoot;
            @Shoot.canceled += instance.OnShoot;
            @Reload.started += instance.OnReload;
            @Reload.performed += instance.OnReload;
            @Reload.canceled += instance.OnReload;
            @Ragdoll.started += instance.OnRagdoll;
            @Ragdoll.performed += instance.OnRagdoll;
            @Ragdoll.canceled += instance.OnRagdoll;
            @Interact.started += instance.OnInteract;
            @Interact.performed += instance.OnInteract;
            @Interact.canceled += instance.OnInteract;
            @UI_Next.started += instance.OnUI_Next;
            @UI_Next.performed += instance.OnUI_Next;
            @UI_Next.canceled += instance.OnUI_Next;
            @UI_Previous.started += instance.OnUI_Previous;
            @UI_Previous.performed += instance.OnUI_Previous;
            @UI_Previous.canceled += instance.OnUI_Previous;
            @UI_Select.started += instance.OnUI_Select;
            @UI_Select.performed += instance.OnUI_Select;
            @UI_Select.canceled += instance.OnUI_Select;
        }

        private void UnregisterCallbacks(IOnFootActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @Look.started -= instance.OnLook;
            @Look.performed -= instance.OnLook;
            @Look.canceled -= instance.OnLook;
            @Shoot.started -= instance.OnShoot;
            @Shoot.performed -= instance.OnShoot;
            @Shoot.canceled -= instance.OnShoot;
            @Reload.started -= instance.OnReload;
            @Reload.performed -= instance.OnReload;
            @Reload.canceled -= instance.OnReload;
            @Ragdoll.started -= instance.OnRagdoll;
            @Ragdoll.performed -= instance.OnRagdoll;
            @Ragdoll.canceled -= instance.OnRagdoll;
            @Interact.started -= instance.OnInteract;
            @Interact.performed -= instance.OnInteract;
            @Interact.canceled -= instance.OnInteract;
            @UI_Next.started -= instance.OnUI_Next;
            @UI_Next.performed -= instance.OnUI_Next;
            @UI_Next.canceled -= instance.OnUI_Next;
            @UI_Previous.started -= instance.OnUI_Previous;
            @UI_Previous.performed -= instance.OnUI_Previous;
            @UI_Previous.canceled -= instance.OnUI_Previous;
            @UI_Select.started -= instance.OnUI_Select;
            @UI_Select.performed -= instance.OnUI_Select;
            @UI_Select.canceled -= instance.OnUI_Select;
        }

        public void RemoveCallbacks(IOnFootActions instance)
        {
            if (m_Wrapper.m_OnFootActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IOnFootActions instance)
        {
            foreach (var item in m_Wrapper.m_OnFootActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_OnFootActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public OnFootActions @OnFoot => new OnFootActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard & Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    private int m_ControllerSchemeIndex = -1;
    public InputControlScheme ControllerScheme
    {
        get
        {
            if (m_ControllerSchemeIndex == -1) m_ControllerSchemeIndex = asset.FindControlSchemeIndex("Controller");
            return asset.controlSchemes[m_ControllerSchemeIndex];
        }
    }
    public interface IOnFootActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
        void OnReload(InputAction.CallbackContext context);
        void OnRagdoll(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnUI_Next(InputAction.CallbackContext context);
        void OnUI_Previous(InputAction.CallbackContext context);
        void OnUI_Select(InputAction.CallbackContext context);
    }
}