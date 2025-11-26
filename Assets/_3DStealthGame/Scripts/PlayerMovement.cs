using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Animator m_Animator;
    public InputAction MoveAction;

    private float walkSpeed;
    public float turnSpeed = 20f;

    //sprint numbers
    public float sprintSpeed = 8.0f;
    public float walkS = 5f;
    public float cooldownTime = 3f;
    public float cooldownDuration = 0.5f;
    private float cooldownTimer = 0f;

    Rigidbody m_Rigidbody;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;


    IEnumerator cooldownlogic(){
        if (cooldownTime <= cooldownTimer){
            walkSpeed = sprintSpeed;
            cooldownTimer = 0f;
            yield return new WaitForSeconds(cooldownDuration);
            walkSpeed = walkS;
            Debug.Log("Sprint Successful");
        }
        else{
            Debug.Log("Sprint Not Ready");
        }

    }

    void Start ()
    {
        walkSpeed = walkS;
        m_Rigidbody = GetComponent<Rigidbody> ();
        MoveAction.Enable();
        m_Animator = GetComponent<Animator> ();
    }

    void FixedUpdate ()
    {
        var pos = MoveAction.ReadValue<Vector2>();
        
        float horizontal = pos.x;
        float vertical = pos.y;
        
        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize ();

        bool hasHorizontalInput = !Mathf.Approximately (horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately (vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool ("IsWalking", isWalking);

        Vector3 desiredForward = Vector3.RotateTowards (transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation (desiredForward);
        
        m_Rigidbody.MoveRotation (m_Rotation);
        m_Rigidbody.MovePosition (m_Rigidbody.position + m_Movement * walkSpeed * Time.deltaTime);

        cooldownTimer += 0.1f;

        //Spriint Logic
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(cooldownlogic());
        }
    }
}