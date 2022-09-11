using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int currenthealth;
    [SerializeField] ParticleSystem identityV3VFX, identityV2VFX, identityV1VFX, identityV0VFX;
    [SerializeField] Player myPlayer;

    float lastDamageTime = Mathf.NegativeInfinity;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Enemy"))
        {
            TakeDamage();
        }
        //else if (collision.gameObject.name.Contains("Bullet"))
        //{
        //    TakeDamage();
        //    Debug.Log("");
        //}
    }

    public void TakeDamage()
    {
        if (Time.time - lastDamageTime > 1f)
        {
            currenthealth--;

            AttVisualFeedback();

            lastDamageTime = Time.time;
        }
    }

    public void AttVisualFeedback()
    {
        if (currenthealth >= 3)
        {
            if (!identityV3VFX.isPlaying) { identityV3VFX.Play(); }
            if (identityV2VFX.isPlaying) { identityV2VFX.Clear(); identityV2VFX.Stop(); }
            if (identityV1VFX.isPlaying) { identityV1VFX.Clear(); identityV1VFX.Stop(); }
        }
        else if (currenthealth == 2)
        {
            if (identityV3VFX.isPlaying) { identityV3VFX.Clear(); identityV3VFX.Stop(); }
            if (!identityV2VFX.isPlaying) { identityV2VFX.Play(); }
            if (identityV1VFX.isPlaying) { identityV1VFX.Clear(); identityV1VFX.Stop(); }
        }
        else if (currenthealth == 1)
        {
            if (identityV3VFX.isPlaying) { identityV3VFX.Clear(); identityV3VFX.Stop(); }
            if (identityV2VFX.isPlaying) { identityV2VFX.Clear(); identityV2VFX.Stop(); }
            if (!identityV1VFX.isPlaying) identityV1VFX.Play();
        }
        else if (currenthealth == 0)
        {
            currenthealth = 0;
            myPlayer.CurrentState = myPlayer.deadState;
            myPlayer.animatorChar.SetTrigger("Death");
            identityV0VFX.Play();
        }
    }

    public void StopAllVisuals()
    {

        identityV3VFX.Clear();
        identityV3VFX.Stop();

        identityV2VFX.Clear();
        identityV2VFX.Stop();

        identityV1VFX.Clear();
        identityV1VFX.Stop();
    }
}
