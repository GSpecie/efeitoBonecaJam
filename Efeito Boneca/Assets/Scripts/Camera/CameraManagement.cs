using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManagement : MonoBehaviour
{
    public Animator camAnimator;
    public int roomNumber;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            camAnimator.SetInteger("roomNumber", roomNumber);

            //myCamera.SetActive(true);
            //for (int i = 0; i < othersCamera.Length; i++)
            //{
            //    othersCamera[i].SetActive(false);
            //}
        }

    }
}
