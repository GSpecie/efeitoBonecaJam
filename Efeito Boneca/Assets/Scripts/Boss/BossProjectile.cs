using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    [SerializeField] private int m_damage = 1;
    [SerializeField] private float impactIntensity;
    [SerializeField] private float speed;

    public int M_damage { get { return m_damage; } }
    public float ImpactIntensity { get { return impactIntensity; } }

    private float cooldownToDisappear;
    [SerializeField] private float cooldownToDisappearOriginal;


    private float cooldownToAttack;
    [SerializeField] private float cooldownToAttackOriginal;


    [SerializeField] private BossOne boss;


//    [SerializeField] private ParticleSystem hitImpactVFX;

  //  [SerializeField] private ParticleSystem shootVFX;
    [SerializeField] private Collider myCollider;
    private bool alreadyHitImpact;

    [SerializeField] private AudioSource sFX_hit;

    [SerializeField] private GameObject myVisual;

    [SerializeField] private Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        ResetTiming();
    }

    // Update is called once per frame
    void Update()
    {
        cooldownToDisappear -= Time.deltaTime;
        cooldownToAttack -= Time.deltaTime;

        if (cooldownToDisappear <= 0)
        {
            //this.gameObject.SetActive(false);
            //shootVFX.Stop();

            myCollider.enabled = false;

            cooldownToDisappear = cooldownToDisappearOriginal;
            //cooldownToAttack = cooldownToAttackOriginal;
        }
    }

    private void FixedUpdate()
    {
        if(cooldownToAttack <= 0)
        {
            if (alreadyHitImpact == false) rb.velocity = transform.forward * speed;
            else rb.velocity = Vector3.zero;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }

    }

    //public void WhoIsThePlayer(Player player)
    //{
    //    this.player = player;
    //}

    public void ResetTiming()
    {
        cooldownToDisappear = cooldownToDisappearOriginal;
        cooldownToAttack = cooldownToAttackOriginal;
        //shootVFX.Play();
        myVisual.SetActive(true);
        myAnimator.SetBool("IsInvisible", false);
        myAnimator.SetTrigger("Anticipation");
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
        if (collision.gameObject.name.Contains("Wall"))
        {
            //shootVFX.Stop();
            //hitImpactVFX.Play();
            myCollider.enabled = false;
            alreadyHitImpact = true;
            myAnimator.SetTrigger("Hit");

            myAnimator.SetBool("IsInvisible", true);
            //sFX_hit.Play();


            Debug.Log("colidiu com parede");

            boss.ReduceShoots();
        }

        if (collision.gameObject.CompareTag("BossBullet"))
        {
            //shootVFX.Stop();
            //hitImpactVFX.Play();
            myCollider.enabled = false;
            alreadyHitImpact = true;
            myAnimator.SetTrigger("Hit");

            myAnimator.SetBool("IsInvisible", true);
            //sFX_hit.Play();

            boss.ReduceShoots();
            Debug.Log("colidiu com parede");
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Player otherEnemy = collision.gameObject.GetComponent<Player>();
            otherEnemy.TakeDamage((collision.gameObject.transform.position - transform.position).normalized * impactIntensity);
            //shootVFX.Stop();
            //hitImpactVFX.Play();
            myCollider.enabled = false;
            alreadyHitImpact = true;
            myAnimator.SetTrigger("Hit");

            Debug.Log("colidiu com player");

            myAnimator.SetBool("IsInvisible", true);
            //sFX_hit.Play();

            boss.ReduceShoots();
        }

        if (collision.gameObject.name.Contains("Identity"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage();

            //Debug.Log("acertooou");

            //shootVFX.Stop();
            //hitImpactVFX.Play();
            myCollider.enabled = false;
            alreadyHitImpact = true;

            myAnimator.SetTrigger("Hit");

            Debug.Log("colidiu com identidade");

            myAnimator.SetBool("IsInvisible", true);

            boss.ReduceShoots();
            //sFX_hit.Play();

        }
    }
}
