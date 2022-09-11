using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventManager : MonoBehaviour
{
    [SerializeField]
    AudioClip[] runAudioClip;
    public AudioSource SFX_run;
    int randomRunSound;


    public Enemy myEnemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RunSound()
    {
        randomRunSound = Random.Range(0, runAudioClip.Length);
        SFX_run.PlayOneShot(runAudioClip[randomRunSound]);
    }

    public void myEnemyDesactivateVisual()
    {
        myEnemy.TrueDeath();
    }
}
