using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float actualHealth, maxHealth = 3f;
    Rigidbody rb;
    [SerializeField] Transform target;
    public float moveSpeed = 4f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        actualHealth = maxHealth;
    }

    private void FixedUpdate()
    {
        Vector3 pos = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(pos);
        transform.LookAt(target);
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
