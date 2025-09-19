using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.3f;
    public LayerMask groundMask;

    [Header("Camera Reference")]
    public Transform cameraTransform;

    [Header("Footstep Audio")]
    public float footstepDelay = 0.4f; // Delay between footsteps

    [Header("Audio Clips")]
    public AudioClip jumpSound;

    private CharacterController controller;
    private Animator animator;
    private AudioSource footstepAudio;
    private AudioSource jumpAudio;

    private Vector3 velocity;
    private bool isGrounded;

    private Coroutine footstepCoroutine;
    private bool isMoving;

    private int comboStep = 0;
    private float lastClickTime = 0f;
    public float comboResetTime = 1f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        footstepAudio = GetComponent<AudioSource>();
        footstepAudio.loop = false;
        jumpAudio = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        GroundCheck();
        MovePlayer();
        ApplyGravity();
        HandleAttackInput();
    }

    void GroundCheck()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }
    }

  void MovePlayer()
{
    float horizontal = Input.GetAxis("Horizontal");
    float vertical = Input.GetAxis("Vertical");

    Vector3 forward = cameraTransform.forward;
    Vector3 right = cameraTransform.right;

    forward.y = 0f;
    right.y = 0f;

    forward.Normalize();
    right.Normalize();

    Vector3 moveDirection = forward * vertical + right * horizontal;

    if (moveDirection.magnitude > 0.1f)
    {
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
    }

    animator.SetFloat("Speed", moveDirection.magnitude * moveSpeed);
    controller.Move(moveDirection * moveSpeed * Time.deltaTime);

    // ✅ Jump logic — moved above footstep logic to ensure jump sound plays first
    if (Input.GetButtonDown("Jump") && isGrounded)
    {
    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

    if (jumpSound != null && jumpAudio != null)
        {
        jumpAudio.PlayOneShot(jumpSound);
        }
    }

    // ✅ Footstep sound logic
    if (isGrounded && moveDirection.magnitude > 0.1f)
    {
        if (!footstepAudio.isPlaying)
        {
            footstepAudio.Play();
        }
    }
    else
    {
        if (footstepAudio.isPlaying)
        {
            footstepAudio.Stop();
        }
    }
}

    IEnumerator PlayFootstepsLoop()
    {
        while (true)
        {
            footstepAudio.Play();
            yield return new WaitForSeconds(footstepAudio.clip.length + footstepDelay);
        }
    }

    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    void HandleAttackInput()
    {
        if (Input.GetMouseButtonDown(0)) // Left click
        {
            float timeSinceLastClick = Time.time - lastClickTime;

            if (timeSinceLastClick > comboResetTime)
            {
                comboStep = 0;
            }

            lastClickTime = Time.time;

            if (comboStep == 0)
            {
                animator.SetTrigger("Attack1");
                comboStep = 1;
            }
            else if (comboStep == 1)
            {
                animator.SetTrigger("Attack2");
                comboStep = 2;
            }
            else
            {
                animator.SetTrigger("Attack1");
                comboStep = 1;
            }
        }
    }
}
