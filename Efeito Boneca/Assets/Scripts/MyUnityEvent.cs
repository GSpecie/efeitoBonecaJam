using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class MyUnityEvent : MonoBehaviour
{
    [SerializeField] UnityEvent OnCompleteEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            OnCompleteEvent.Invoke();
        }
    }
}
