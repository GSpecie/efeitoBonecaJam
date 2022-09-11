using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float actualHealth, maxHealth = 3f;
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform target;
    public float moveSpeed = 4f;

    [SerializeField] float impactIntensity;

    [SerializeField] private Collider myCollider;
    [SerializeField] private GameObject myVisual;

    [SerializeField] private Animator animator;

    [SerializeField] private BossOne myBoss;

    void Start()
    {
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
            Death();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Player")
        {
            Debug.Log("ATINGIU AAAA");
            collision.gameObject.GetComponent<Player>().TakeDamage((collision.gameObject.transform.position - transform.position).normalized * impactIntensity);
        }
    }

    public void Revive()
    {
        rb.isKinematic = false;
        myCollider.enabled = true;
        myVisual.SetActive(true);
    }

    public void Death()
    {
        //rb.velocity = Vector3.zero;
        animator.SetTrigger("Death");
        rb.isKinematic = true;
        myCollider.enabled = false;

        

    }

    public void TrueDeath()
    {
        myVisual.SetActive(false);
        myBoss.ReduceDolls();
    }


}
