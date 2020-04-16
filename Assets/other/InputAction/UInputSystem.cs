// GENERATED AUTOMATICALLY FROM 'Assets/other/InputAction/UInputSystem.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @UInputSystem : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @UInputSystem()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""UInputSystem"",
    ""maps"": [
        {
            ""name"": ""System"",
            ""id"": ""cb555e62-8c29-4492-9df1-8d8b1b4ad879"",
            ""actions"": [
                {
                    ""name"": ""any"",
                    ""type"": ""Button"",
                    ""id"": ""c12319f1-5bfd-4746-bdc1-297ab1e05054"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""18cf6051-6c54-4bb4-ab31-a765f9352c90"",
                    ""path"": ""<Keyboard>/anyKey"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""any"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // System
        m_System = asset.FindActionMap("System", throwIfNotFound: true);
        m_System_any = m_System.FindAction("any", throwIfNotFound: true);
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

    // System
    private readonly InputActionMap m_System;
    private ISystemActions m_SystemActionsCallbackInterface;
    private readonly InputAction m_System_any;
    public struct SystemActions
    {
        private @UInputSystem m_Wrapper;
        public SystemActions(@UInputSystem wrapper) { m_Wrapper = wrapper; }
        public InputAction @any => m_Wrapper.m_System_any;
        public InputActionMap Get() { return m_Wrapper.m_System; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(SystemActions set) { return set.Get(); }
        public void SetCallbacks(ISystemActions instance)
        {
            if (m_Wrapper.m_SystemActionsCallbackInterface != null)
            {
                @any.started -= m_Wrapper.m_SystemActionsCallbackInterface.OnAny;
                @any.performed -= m_Wrapper.m_SystemActionsCallbackInterface.OnAny;
                @any.canceled -= m_Wrapper.m_SystemActionsCallbackInterface.OnAny;
            }
            m_Wrapper.m_SystemActionsCallbackInterface = instance;
            if (instance != null)
            {
                @any.started += instance.OnAny;
                @any.performed += instance.OnAny;
                @any.canceled += instance.OnAny;
            }
        }
    }
    public SystemActions @System => new SystemActions(this);
    public interface ISystemActions
    {
        void OnAny(InputAction.CallbackContext context);
    }
}
