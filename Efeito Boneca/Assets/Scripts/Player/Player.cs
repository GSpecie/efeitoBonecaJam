using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // public GameObject cameraGameObject;


    //referências dos estados
    [HideInInspector] public DefaultState defaultState;
    [HideInInspector] public FallenState fallenState;
    [HideInInspector] public DeadState deadState;

    [HideInInspector]
    public IPlayerStates CurrentState
    {
        set
        {
            if (value != CurrentState)
            {
                currentState = value;
                ChangesInStates();
                //Debug.Log("jogador número: " + playerNumber + "está no estado :" + currentState);
            }
        }
        get { return currentState; }
    }
    IPlayerStates currentState;

    //variável velocidade de movimentação
    public float moveSpeed;
    public float minimumSpeed, mediumSpeed, highSpeed;

    //variável rigibody
    public Rigidbody rb;

    public GameObject playerChar;

    //variáveis botões e analógicos

    public bool isGamepad;
    public PlayerActionMapControls actionMap;

    [HideInInspector]
    public Vector2 leftJoystick;
    [HideInInspector]
    public Vector2 rightJoystick;
    [HideInInspector]
    public bool shootButton;
    [HideInInspector]
    public bool dashButton;
    [HideInInspector]
    public float lastRightJoystickHorizontal;
    [HideInInspector]
    public float lastRightJoystickVertical;

    [HideInInspector] public Vector3 playerFacingDirection;
        
    [HideInInspector]
    public RaycastHit lookHit;

    //variáveis vetores de movimentação
    [HideInInspector]
    public Vector3 vertical, horizontal, rightVertical, rightHorizontal, direction;

    [HideInInspector]
    public Vector3 rightHorizontalMovement;
    [HideInInspector]
    public Vector3 rightVerticalMovement;

    //variáveis energia movimentação
    [HideInInspector]
    public float dashEnergyPercentage;

    public float currentDashEnergy = 100;
    [HideInInspector]
    public float maximumDashEnergy = 100;
    public Image currentDashBar;

    //variáveis energia disparo
    [HideInInspector]
    public float shootEnergyPercentage;

    public float currentShootEnergy = 100;
    [HideInInspector]
    public float maximumShootEnergy = 100;
    public Image currentShootBar;

    [HideInInspector]
    public Ray cameraRay;


    //variáveis projéil
    public PlayerProjectile[] bullets;
    [HideInInspector] public int bulletIndex = 0;
    public Transform bulletPoint;

    public PlayerProjectile bulletPrefab;
    [HideInInspector] public Queue<PlayerProjectile> bulletShots = new Queue<PlayerProjectile>();

    public ParticleSystem muzzleVFX;

    //cooldowns

    [HideInInspector] public float cooldownToDie = 10f;
    public float cooldownToDieOriginal = 10f;

    [HideInInspector] public float cooldownToDash = 1f;
    public float cooldownToDashOriginal = 1f;

    [HideInInspector] public float cooldownToShoot = 1f;
    public float cooldownToShootOriginal = 1f;

    [HideInInspector]
    public Vector3 externalForce = Vector3.zero;
    [HideInInspector]
    public Vector3 dashForce = Vector3.zero;
    //[HideInInspector]
    public float dashPower;
    public float mininumDash, mediumDash, highDash, superDash;

    public ParticleSystem dashVFX;

    //recoil
    [HideInInspector]
    public Vector3 recoilForce = Vector3.zero;
    //[HideInInspector]
    public float recoilPower;
    public float mininumRecoil, mediumRecoil, highRecoil, superRecoil;

    float lastDamageTime = Mathf.NegativeInfinity;

    public Animator animatorIdentity;
    public Animator animatorChar;

    public Camera playerCam;

    public AudioSource sFX_shoot;
    public AudioSource sFX_dash;
    public AudioSource sFX_identityFallen;

    //váriaveis identidade

    [HideInInspector] public bool isFallen = false;
    [HideInInspector] public float cooldownToRaise;
    public float cooldownToRaiseOriginal;
    public PlayerHealth myLife;
    public Texture charTexture, dollTexture;
    public SkinnedMeshRenderer myMeshRenderer;

    private void Awake()
    {
        actionMap = new PlayerActionMapControls();

        defaultState = new DefaultState(this);
        fallenState = new FallenState(this);
        deadState = new DeadState(this);

        cooldownToDie = cooldownToDieOriginal;
        cooldownToDash = cooldownToDashOriginal;
        cooldownToShoot = cooldownToShootOriginal;
        cooldownToRaise = cooldownToRaiseOriginal;
        //CharacterAnim = GetComponent<Animator>();
    }


    void Start()
    {
        AttMoveReference();

        currentState = defaultState;

        EnergyBars();
        ChangesInStates();
    }

    void Update()
    {
        CheckInput();
        CheckCasts();
        EnergyBars();

        ResetForces();
    }

    void ResetForces()
    {
        externalForce = Vector3.Lerp(externalForce, Vector3.zero, Time.deltaTime * 8f);

        dashForce = Vector3.Lerp(dashForce, Vector3.zero, Time.deltaTime * 8f);

        recoilForce = Vector3.Lerp(recoilForce, Vector3.zero, Time.deltaTime * 8f);
    }

    private void LateUpdate()
    {
    }

    public void OnDeviceChange(PlayerInput pI)
    {
        isGamepad = pI.currentControlScheme.Equals("Gamepad") ? true : false;
    }

    private void FixedUpdate()
    {
        Move();
        DropCoooldown();

        if (isFallen)
        {
            cooldownToRaise -= Time.fixedDeltaTime;

            if (cooldownToRaise <= 0)
            {
                isFallen = false;
                animatorIdentity.SetBool("IsFallen", false);
                cooldownToRaise = cooldownToRaiseOriginal;

            }

        }
    }

    void CheckCasts()
    {
        currentState.CheckCasts();
    }

    void Move()
    {
        currentState.Move();
    }

    void ChangesInStates()
    {
        CurrentState.ChangesInStates();
    }

    void CheckInput()
    {
        currentState.CheckInput();
    }

    public void DropCoooldown()
    {
        CurrentState.DropCooldown();
    }


    public void TakeDamage(Vector3 impactValue)
    {
        if (Time.time - lastDamageTime > 1f)
        {
            currentState.TakeDamage(impactValue);
            lastDamageTime = Time.time;

            Fallen();
        }
    }

    void EnergyBars()
    {
        currentState.EnergyBars();
    }

    void Fallen()
    {
        currentState.Fallen();
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, 2.5f);
        Gizmos.DrawWireSphere(transform.position, 1f);
    }

    public void CreatePlayerImpact(Vector3 ImpactValue)
    {
        CurrentState.CreatePlayerImpact(ImpactValue);
    }

    public void RunVFX(int booleanInt)
    {
        ////forward run
        //if (booleanInt == 0) runSmokeVFX.transform.position = positionsRunVFX[0].transform.position;
        ////right run
        //else if (booleanInt == 1) runSmokeVFX.transform.position = positionsRunVFX[1].transform.position;
        ////left run
        //else if (booleanInt == 2) runSmokeVFX.transform.position = positionsRunVFX[2].transform.position;
        ////backward run
        //else if (booleanInt == 3) runSmokeVFX.transform.position = positionsRunVFX[3].transform.position;

        //runSmokeVFX.Emit(1);
    }



    //public void DashVFX(int booleanInt)
    //{
    //    ////forward dash
    //    //if (booleanInt == 0) dashSmokeVFX.transform.position = positionsDashVFX[0].transform.position;
    //    ////right dash
    //    //else if (booleanInt == 1) dashSmokeVFX.transform.position = positionsDashVFX[1].transform.position;
    //    ////left dash
    //    //else if (booleanInt == 2) dashSmokeVFX.transform.position = positionsDashVFX[2].transform.position;
    //    ////backward dash
    //    //else if (booleanInt == 3) dashSmokeVFX.transform.position = positionsDashVFX[3].transform.position;

    //    audioManagerScript.PlayOneShot("dash", 1f);

    //    dashSmokeVFX.Emit(5);
    //}

    private void OnEnable()
    {
        actionMap.Enable();
    }

    private void OnDisable()
    {
        actionMap.Disable();
    }

    public void DesactivateHealthVisuals()
    {
        myLife.StopAllVisuals();
        sFX_identityFallen.Stop();
        myMeshRenderer.material.mainTexture = charTexture;
    }

    public void AttMoveReference()
    {
        vertical = Camera.main.transform.forward;
        vertical.y = 0;
        vertical = Vector3.ProjectOnPlane(vertical, Vector3.up).normalized;

        horizontal = Camera.main.transform.right;
        horizontal = Vector3.ProjectOnPlane(horizontal, Vector3.up).normalized;

        rightVertical = Camera.main.transform.forward;
        rightVertical.y = 0;
        rightVertical = Vector3.ProjectOnPlane(rightVertical, Vector3.up).normalized;

        rightHorizontal = Camera.main.transform.right;
        rightHorizontal = Vector3.ProjectOnPlane(rightHorizontal, Vector3.up).normalized;
    }
}

//public struct X
//{
//    //struct test
//}