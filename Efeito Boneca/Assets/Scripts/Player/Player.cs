using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public float moveSpeed = 15f;

    //variável rigibody
    public Rigidbody rb;

    //variáveis botões e analógicos

    public PlayerActionMapControls actionMap;

    [HideInInspector]
    public Vector2 leftJoystick;
    [HideInInspector]
    public float rightJoystickHorizontal;
    [HideInInspector]
    public float rightJoystickVertical;
    [HideInInspector]
    public bool shieldButton;
    [HideInInspector]
    public bool shareLifeButton;
    [HideInInspector]
    public bool dashButton;
    [HideInInspector]
    public float lastRightJoystickHorizontal;
    [HideInInspector]
    public float lastRightJoystickVertical;

    public Vector3 playerFacingDirection;

    [HideInInspector]
    public RaycastHit lookHit;

    //variáveis vetores de movimentação
    [HideInInspector]
    public Vector3 vertical, horizontal, direction;

    [HideInInspector]
    public Vector3 rightHorizontalMovement;
    [HideInInspector]
    public Vector3 rightVerticalMovement;

    //variáveis vida
    [HideInInspector]
    public float healthPercentage;

    public float currentHealth = 100;
    [HideInInspector]
    public float maximumHealth = 100;
    //public Image currentHealthBar;

    [HideInInspector]
    public Ray cameraRay;

    //cooldowns

    public float cooldownToDie = 10f;
    public float cooldownToDieOriginal = 10f;

    public float cooldownToDash = 1f;
    public float cooldownToDashOriginal = 1f;

    [HideInInspector]
    public Vector3 externalForce = Vector3.zero;
    [HideInInspector]
    public Vector3 dashPower = Vector3.zero;
    //[HideInInspector]
    public float dashMultiplier = 50f;

    float lastDamageTime = Mathf.NegativeInfinity;

    //public Animator CharacterAnim;

    public Camera playerCam;

    [SerializeField]
    AudioClip[] runAudioClip;

    int randomRunSound;

    private void Awake()
    {
        actionMap = new PlayerActionMapControls();

        defaultState = new DefaultState(this);
        fallenState = new FallenState(this);
        deadState = new DeadState(this);

        cooldownToDie = cooldownToDieOriginal;
        //CharacterAnim = GetComponent<Animator>();
    }


    void Start()
    {
        vertical = Camera.main.transform.forward;
        vertical.y = 0;
        vertical = Vector3.ProjectOnPlane(vertical, Vector3.up).normalized;

        horizontal = Camera.main.transform.right;
        horizontal = Vector3.ProjectOnPlane(horizontal, Vector3.up).normalized;

        //rightVertical = Camera.main.transform.forward;
        //rightVertical.y = 0;
        //rightVertical = Vector3.ProjectOnPlane(rightVertical, Vector3.up).normalized;

        //rightHorizontal = Camera.main.transform.right;
        //rightHorizontal = Vector3.ProjectOnPlane(rightHorizontal, Vector3.up).normalized;

        currentState = defaultState;

        Life();
        ChangesInStates();
    }

    void Update()
    {
        CheckInput();
        CheckCasts();
        Life();

        externalForce = Vector3.Lerp(externalForce, Vector3.zero, Time.deltaTime * 8f);

        dashPower = Vector3.Lerp(dashPower, Vector3.zero, Time.deltaTime * 8f);
    }

    private void LateUpdate()
    {
    }

    private void FixedUpdate()
    {
        Move();
        DropCoooldown();
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


    public void TakeDamage(float damage)
    {
        if (Time.time - lastDamageTime > 0.25f)
        {
            currentState.TakeDamage(damage);
            lastDamageTime = Time.time;
        }
    }

    void Life()
    {
        currentState.Life();
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

    //public void RunSound()
    //{
    //    randomRunSound = Random.Range(0, runAudioClip.Length);
    //    audioManagerScript.ChangeClip(runAudioClip[randomRunSound], "run");
    //}

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
}

//public struct X
//{
//    //struct test
//}