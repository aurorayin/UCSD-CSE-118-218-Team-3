using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.WitAi.Dictation;
using UnityEngine.Serialization;
using UnityEngine.InputSystem;
using UnityEngine.Networking;

namespace Meta.Voice.Samples.Dictation
{
    public class SpeechRecognitionActivation : MonoBehaviour
    {
        [FormerlySerializedAs("dictation")]
        [SerializeField] private DictationService _dictation;



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
