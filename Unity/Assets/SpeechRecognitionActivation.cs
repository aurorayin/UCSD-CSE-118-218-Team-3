using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.WitAi.Dictation;
using UnityEngine.Serialization;
using UnityEngine.InputSystem;

namespace Meta.Voice.Samples.Dictation
{
    public class SpeechRecognitionActivation : MonoBehaviour
    {
        [FormerlySerializedAs("dictation")]
        [SerializeField] private DictationService _dictation;
        // public InputActionReference triggerPressedAction;

        // void OnEnable()
        // {
        //     // Subscribe to the trigger action
        //     if (triggerPressedAction != null)
        //     {
        //         triggerPressedAction.action.performed += OnTriggerPressed;
        //         triggerPressedAction.action.Enable();
        //     }
        // }

        // void OnDisable()
        // {
        //     if (triggerPressedAction != null)
        //     {
        //         triggerPressedAction.action.performed -= OnTriggerPressed;
        //         triggerPressedAction.action.Disable();
        //     }
        // }

        // void OnTriggerPressed(InputAction.CallbackContext context)
        // {
        //     ToggleActivation();
        //     Debug.Log("Trigger pressed");
        // }

        // Update is called once per frame
        public void ToggleActivation()
        {   

            if (_dictation.MicActive)
            {
                _dictation.Deactivate();
            }
            else
            {
                _dictation.Activate();
            }
        }
    }
}
