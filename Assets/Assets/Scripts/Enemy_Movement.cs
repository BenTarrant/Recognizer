using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    public enum NPCState
    {
        Idle,
        Chasing,
        ReturnToOrigin
    }

    public Transform Player;
    public float MoveSpeed = 4;
    [SerializeField] private float MaxChaseDist = 10;
    [SerializeField] private float MinChaseDist = 5;
    private Animator Enemy_Animate;
    public CharacterController CC_NPC;
    public Transform EnemyHome;

    private float fl_delay;
    public bool bl_line_of_sight;
    public float fl_cool_down = 1;
    public float fl_attack_range = 20;
    public GameObject BulletPrefab;
    public Transform BulletStart;
    public float BulletSpeed = 5.0f;

    private NPCState mCurrentState = NPCState.Idle;
    private Vector3 mOrigin = Vector3.zero;

    private void Awake()
    {
        Enemy_Animate = GetComponent<Animator>();
        CC_NPC = GetComponent<CharacterController>();
        MaxChaseDist = GetComponentInChildren<ParticleSystem>().shape.radius -7.5f;
        print("MaxChaseDistance: " + MaxChaseDist);
    }

    void Start()
    {
        mOrigin = transform.position;
    }

    void Update()
    {
        SwitchStates();

        if (Vector3.Distance(transform.position, Player.position) < MaxChaseDist) // If the NPC is within 10 meters.
        {
            SetState(NPCState.Chasing); // Chase();
        }

        else if (Vector3.Distance(transform.position, Player.position) >= MaxChaseDist) 
        {
            if (Vector3.Distance(transform.position, mOrigin) < 0.5f)
            {
                SetState(NPCState.Idle);
            }

            else
            {
                SetState(NPCState.ReturnToOrigin);
            }
        }

        if (Vector3.Distance(transform.position, Player.position) < MinChaseDist)
        {
            SetState(NPCState.Idle);
            AttackTarget();
        }

        else if (Vector3.Distance(transform.position, Player.position) >= MinChaseDist && Vector3.Distance(transform.position, Player.position) < MaxChaseDist)
        {
            SetState(NPCState.Chasing);
        }

        //print("Current State: " + GetState() + "Distance From Player: " + Vector3.Distance(transform.position, Player.transform.position));
    }

    void Chase()
    {
        AttackTarget();
        transform.LookAt(Player);
        transform.position += transform.forward * MoveSpeed * Time.deltaTime;
        NPC_Animate();

        if (Vector3.Distance(transform.position, Player.position) >= MaxChaseDist)
        {
            SetState(NPCState.Idle);
            print("SetState() to: Idle");
        }

        if (Vector3.Distance(transform.position, mOrigin) < 0.5f)
        {
            SetState(NPCState.Idle);
            print("SetState() to: Idle");
        }
    }

    void ReturnToOrigin()
    {
        transform.LookAt(EnemyHome.transform.position);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        CC_NPC.SimpleMove(MoveSpeed * transform.TransformDirection(Vector3.forward));
        NPC_Animate();
    }

    void NPC_Animate()
    {
        if (CC_NPC.velocity.x != 0 || CC_NPC.velocity.z != 0)
        {
            Enemy_Animate.SetBool("bl_walking", false);
        }

        else
        {
            Enemy_Animate.SetBool("bl_walking", true);
        }
    }

    void AttackTarget()
    {
        if (Time.time > fl_delay && Vector3.Distance(transform.position, Player.transform.position) < fl_attack_range)
        {
            // Face the Target
            transform.LookAt(Player.transform.position);

            // Is the line of sight flag checked? 
            if (bl_line_of_sight)
            {    // Cast a Ray to check if Target in is view of NPC
                RaycastHit _RC_hit;
                Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out _RC_hit, fl_attack_range);

                // if the Target is in sight fire a projectile
                if (_RC_hit.collider != null && _RC_hit.collider.gameObject == Player) FireProjectile();
            }

            else
            {
                FireProjectile();
            }
        }
    }

    void FireProjectile()
    {
        // Spawn a bullet    
        var bullet = (GameObject)Instantiate(BulletPrefab, BulletStart.position, Quaternion.identity); // instatiate the bulletprefab set in IDE
        bullet.GetComponent<Rigidbody>().velocity = transform.forward * BulletSpeed; // give it the velocity Bullet Speed defined in IDE
        Destroy(bullet, 2.0f);

        //Reset Cooldown
        fl_delay = Time.time + fl_cool_down;
    }

    void SwitchStates()
    {
        switch (mCurrentState)
        {
            case NPCState.Idle: /*DoNothing*/ break;
            case NPCState.Chasing: Chase(); break;
            case NPCState.ReturnToOrigin: ReturnToOrigin(); break;
        }
    }

    public void SetState(NPCState vState)
    {
        if (vState != mCurrentState)
        {
            mCurrentState = vState;
        }
    }

    public NPCState GetState()
    {
        return mCurrentState;
    }
}
