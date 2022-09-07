using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    [SerializeField] private int m_damage = 1;
    [SerializeField] private float impactIntensity;
    [SerializeField] private float speed;

    public int M_damage { get { return m_damage; } }
    public float ImpactIntensity { get { return impactIntensity; } }

    private float cooldownToDisappear;
    [SerializeField] private float cooldownToDisappearOriginal;

    private Player player;


    [SerializeField] private ParticleSystem hitImpactVFX;

    [SerializeField] private ParticleSystem shootVFX;
    [SerializeField] private SphereCollider myCollider;
    private bool alreadyHitImpact;

    // Start is called before the first frame update
    void Start()
    {
        ResetTiming();
    }

    // Update is called once per frame
    void Update()
    {
        cooldownToDisappear -= Time.deltaTime;

        if (cooldownToDisappear <= 0)
        {
            //this.gameObject.SetActive(false);
            shootVFX.Stop();
            myCollider.enabled = false;

            cooldownToDisappear = cooldownToDisappearOriginal;
        }
    }

    private void FixedUpdate()
    {
        if (alreadyHitImpact == false) rb.velocity = transform.forward * speed;
        else rb.velocity = Vector3.zero;
    }

    public void WhoIsThePlayer(Player player)
    {
        this.player = player;
    }

    public void ResetTiming()
    {
        cooldownToDisappear = cooldownToDisappearOriginal;
        shootVFX.Play();
        myCollider.enabled = true;
        alreadyHitImpact = false;
    }

    //private void OnEnable()
    //{
    //    shootVFX.Play();
    //    myCollider.enabled = true;
    //    alreadyHitImpact = true;
    //    //cooldownToDisappear = cooldownToDisappearOriginal;
    //}

    private void OnCollisionEnter(Collision collision)
    {


        if (collision.gameObject.name.Contains("wall"))
        {
            shootVFX.Stop();
            hitImpactVFX.Play();
            myCollider.enabled = false;
            alreadyHitImpact = true;
        }

        if (collision.gameObject.tag.Contains("Enemy"))
        {
            Enemy otherEnemy = collision.gameObject.GetComponent<Enemy>();
            otherEnemy.TakeDamage(m_damage);
            shootVFX.Stop();
            hitImpactVFX.Play();
            myCollider.enabled = false;
            alreadyHitImpact = true;
        }
    }
}
