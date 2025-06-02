using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioListenerClear : MonoBehaviour
{
    void Start()
    {
        AudioListener[] listeners = FindObjectsOfType<AudioListener>();
        if (listeners.Length > 1)
        {
            //Debug.LogWarning("여러 개의 Audio Listener가 존재합니다. 하나만 활성화됩니다.");
            for (int i = 1; i < listeners.Length; i++)
            {
                listeners[i].enabled = false;
            }
        }
    }
}
