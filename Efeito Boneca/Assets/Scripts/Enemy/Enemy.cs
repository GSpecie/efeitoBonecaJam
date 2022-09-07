using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float actualHealth, maxHealth = 3f;

    void Start()
    {
        actualHealth = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        actualHealth -= damageAmount;

        if(actualHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
