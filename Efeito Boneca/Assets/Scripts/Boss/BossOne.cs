using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossOne : MonoBehaviour
{

    public float currentHealth;
    public float damage;
    public float timeBtwDamage;

    public Animator animatorBoss;
    public Slider sliderHealthBar;


    // variáveis bonecas
    public Enemy[] dolls;
    [HideInInspector] public int dollIndex = 0;
    public Transform[] dollPoints;
    private int dollPointIndex;

    public Enemy dollPrefab;
    [HideInInspector] public Queue<Enemy> dollsInvoke = new Queue<Enemy>();


    //variáveis projéteis
    public BossProjectile[] bullets;
    [HideInInspector] public int bulletIndex = 0;
    public Transform[] bulletPoints;
    [SerializeField] private List<Transform> bulletSpawnPoints = new List<Transform>();

    private int bulletPointIndex;

    public BossProjectile bulletPrefab;
    [HideInInspector] public Queue<BossProjectile> bulletShots = new Queue<BossProjectile>();

    [SerializeField] public int amountOfDollsOne;
    [SerializeField] public int amountOfDollsOneOriginal;

    [SerializeField] public int amountOfShootsOne;
    [SerializeField] public int amountOfShootsOneOriginal;

    [SerializeField] private Animator animatorScnTransition;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Life();
    }


    public void DesactivateEnemies()
    {
        for(int i = 0; i < dolls.Length; i++)
        {
            dolls[i].gameObject.SetActive(false);
        }

        StartCoroutine(CreditsScn());
    }
    IEnumerator CreditsScn()
    {
        yield return new WaitForSeconds(2f);
        animatorScnTransition.SetTrigger("ScreenOn");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Scn_Credits");
    }

    public void ResetAmountOfShoots()
    {
        amountOfShootsOne = amountOfShootsOneOriginal;
    }

    public void ResetAmountOfDolls()
    {
        amountOfDollsOne = amountOfDollsOneOriginal;
    }

    public void ReduceShoots()
    {
        amountOfShootsOne--;
    }

    public void ReduceDolls()
    {
        amountOfDollsOne--;
    }

    public void InvokeDoll()
    {
        dollPointIndex = Random.Range(0, dollPoints.Length);

        dolls[dollIndex].transform.position = dollPoints[dollPointIndex].position;
        dolls[dollIndex].transform.rotation = dollPoints[dollPointIndex].rotation;
        dolls[dollIndex].Revive();
        dolls[dollIndex].gameObject.SetActive(true);
        dollIndex++;
        if (dollIndex == dolls.Length - 1) dollIndex = 0;
    }

    public void Shoot()
    {
            bulletPointIndex = Random.Range(0, bulletSpawnPoints.Count);

            bullets[bulletIndex].transform.position = bulletSpawnPoints[bulletPointIndex].position;
            bullets[bulletIndex].transform.rotation = bulletSpawnPoints[bulletPointIndex].rotation;

            bulletSpawnPoints.Remove(bulletSpawnPoints[bulletPointIndex]);


            bullets[bulletIndex].ResetTiming();
            bullets[bulletIndex].gameObject.SetActive(true);
            bulletIndex++;
            if (bulletIndex == bullets.Length - 1) bulletIndex = 0;

        if (bulletSpawnPoints.Count == 0)
        {
            bulletSpawnPoints.Insert(0, bulletPoints[0]);
            bulletSpawnPoints.Insert(1, bulletPoints[1]);
            bulletSpawnPoints.Insert(2, bulletPoints[2]);
            bulletSpawnPoints.Insert(3, bulletPoints[3]);
            bulletSpawnPoints.Insert(4, bulletPoints[4]);
            bulletSpawnPoints.Insert(5, bulletPoints[5]);
        }
    }

    public void Life()
    {
        sliderHealthBar.value = currentHealth;

        if(currentHealth <= 0)
        {
            animatorBoss.SetTrigger("Death");
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        animatorBoss.SetTrigger("TakeDamage");
    }

    public void iniciateBoss()
    {
        animatorBoss.SetTrigger("Start");
    }
}
