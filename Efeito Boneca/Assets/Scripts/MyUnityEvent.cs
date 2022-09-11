using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class MyUnityEvent : MonoBehaviour
{
    [SerializeField] UnityEvent OnCompleteEvent;

    private bool alreadyHappen = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player" && alreadyHappen == false)
        {
            alreadyHappen = true;
            OnCompleteEvent.Invoke();
        }
    }
}
