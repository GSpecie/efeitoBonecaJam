using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossOne : MonoBehaviour
{

    public float currentHealth;
    public float damage;
    public float timeBtwDamage;

    public Animator animatorBoss;
    public Slider sliderHealthBar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        Life();
    }

    public void Life()
    {
        sliderHealthBar.value = currentHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }
}
