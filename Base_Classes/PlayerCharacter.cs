using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : ICombatable
{
    
    private Animator animator;

    public KeyCode forward = KeyCode.W;
    public KeyCode left = KeyCode.A;
    public KeyCode backward = KeyCode.S;
    public KeyCode right = KeyCode.D;
    
    public KeyCode toggleWalk = KeyCode.C;
    private bool isRunning = true;
    private bool canMove = true;

    /*
    protected override float CalculateIncomingDamage(ISkill skill)
    {
        throw new System.NotImplementedException();
    }
    */
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        //DataTable.Instance.player = this;
        this.CritResist = new IEntityAttribute(0.2f, 1, 0, 0, 0, 0);
        this.MaxHealth = new IEntityAttribute(100, 100, 0, 0, 0, 0);
        this.Health = 100;
    }

    private void ToggleWalk()
    {
        if (animator != null)
        {
            isRunning = !isRunning;
            animator.SetBool("IsWalking", !isRunning);
        }
    }

    /*
    private void Update()
    {
        bool isMoving = Input.GetKey(forward) || Input.GetKey(left) || Input.GetKey(backward) || Input.GetKey(right);

        animator.SetBool("IsMoving", isMoving);
        if (isMoving)
        {
            animator.SetBool("W", Input.GetKey(forward));
            animator.SetBool("A", Input.GetKey(left));
            animator.SetBool("S", Input.GetKey(backward));
            animator.SetBool("D", Input.GetKey(right));
        }
        else
        {
            animator.SetBool("W", false);
            animator.SetBool("A", false);
            animator.SetBool("S", false);
            animator.SetBool("D", false);
        }

        if (Input.GetKeyDown(toggleWalk))
        {
            ToggleWalk();
        }
    }
    */

    [SerializeField] private float moveSpeed = 5f;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        if(canMove)
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");

            Vector3 forward = mainCamera.transform.forward;
            forward.y = 0f;
            forward.Normalize();

            Vector3 right = mainCamera.transform.right;
            right.y = 0f;
            right.Normalize();

            Vector3 moveDirection = (forward * verticalInput) + (right * horizontalInput);

            if (moveDirection.magnitude > 0f)
            {
                transform.rotation = Quaternion.LookRotation(moveDirection);
                transform.Translate(moveDirection.normalized * moveSpeed * Time.fixedDeltaTime, Space.World);
            }
        }
        if(Input.GetKey(KeyCode.O))
        {
            string temp = "";
            foreach (StatusEffect se in currentStatusEffects)
            {
                temp += "\r\n" + se.EffectData.EffectName + "   " + se.Duration;
            }
            Debug.LogWarning("Current Status Effects:" + temp);
        }
    }

    public void ToggleCanMove()
    {
        canMove= !canMove;
    }
}
