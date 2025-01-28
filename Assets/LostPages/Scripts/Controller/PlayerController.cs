using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public static PlayerController instance;

    [Header("Options")]
    [SerializeField] private Animator animator;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private CharacterController character;
    [SerializeField] private float jumpForce = 0.5f;   // Force initiale du saut
    [SerializeField] private float gravity = -8;     // Gravité appliquée
    [SerializeField] private AudioClip jumpSound;

    private AudioSource audioSource;

    private Vector3 lerpedDirection = Vector3.zero;
    private const float lerpDirectionScaler = 10.0f;

    private Coroutine currentFireCoroutine;

    private bool sprinting = false;
    public Vector2 MoveInputDirection => moveInputDirection;
    private Vector2 moveInputDirection;
    public Vector2 LookInputDirection => lookInputDirection;
    private Vector2 lookInputDirection;

    private float verticalVelocity = -2.0f;             // Vélocité verticale

    private bool isJumping = false;                          // Indique si le joueur est déjà en train de sauter
    private bool ontheGround = true;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        moveInputDirection = Vector2.zero;
        lookInputDirection = Vector2.zero;
        audioSource = GetComponent<AudioSource>();
    }

    // appelé par PlayerInput
    public void OnMove(InputValue move)
    {
        moveInputDirection = move.Get<Vector2>();
    }

    // appelé par PlayerInput
    public void OnLook(InputValue look)
    {
        lookInputDirection = look.Get<Vector2>();
    }
    public void OnJump(InputValue jump)
    {
        if (!ontheGround) return;
        audioSource.PlayOneShot(jumpSound);
        isJumping = true;
        ontheGround = false;
        verticalVelocity = jumpForce; // Applique une impulsion vers le haut
    }

    // appelé par PlayerInput
    public void OnSprint(InputValue sprint)
    {
        float sprintValue = sprint.Get<float>();
        sprinting = sprintValue > 0.1f;
    }

    void Update()
    {
        if (!isJumping)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
        else
        {
            isJumping = false;
        }
        if (verticalVelocity < -2)
            ontheGround = true;

        Vector3 verticalMovement = new Vector3(0, verticalVelocity, 0);

        // Combine le mouvement vertical et horizontal
        Vector3 horizontalMovement = lerpedDirection * speed * Time.deltaTime;

        character.Move((verticalMovement + horizontalMovement) * Time.deltaTime);

        // Appelle les fonctions de mouvement pour l'animation
        MovePlayerThirdPerson();
    }

    void MovePlayerThirdPerson()
    {
        // HorizontalController et VerticalController vous donneront la valeur du joystick gauche
        // Horizontal et Vertical vous donneront les valeures de ZQSD
        float x = moveInputDirection.x;
        float y = moveInputDirection.y;

        Vector3 rawDirection = transform.forward * Mathf.Clamp01(y);

        rawDirection = (sprinting) ? rawDirection : rawDirection * 0.5f;

        lerpedDirection = Vector3.Lerp(lerpedDirection, rawDirection, Time.deltaTime * lerpDirectionScaler);

        character.Move(rawDirection  * speed * Time.deltaTime);

        float moveSpeed = Mathf.Clamp01(rawDirection.magnitude); // Between 0 & 1

        animator.SetFloat("Speed", moveSpeed);
        
        // plus le joueur va vite, moins il tourne
        float speedRotationScaler = (1.5f - speed);
        
        transform.localEulerAngles += new Vector3(0, -x * Time.deltaTime * 90 * speedRotationScaler, 0);
    }

    void MovePlayerTopDown()
    {
        // HorizontalController et VerticalController vous donneront la valeur du joystick gauche
        // Horizontal et Vertical vous donneront les valeures de ZQSD
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 rawDirection = new Vector3(x, 0, y);

        rawDirection = (sprinting) ? rawDirection : rawDirection * 0.5f;

        //TODO_Exercice_1: adoucir les chagements de vitesse

        lerpedDirection = Vector3.Lerp(lerpedDirection, rawDirection, Time.deltaTime * lerpDirectionScaler); 

        character.Move(rawDirection * speed * Time.deltaTime);

        float moveSpeed = Mathf.Clamp01(rawDirection.magnitude); // Between 0 & 1

        animator.SetFloat("Speed", moveSpeed);

        //Look forward while walking
        Vector3 lookAtOffset = rawDirection;
        lookAtOffset.y = 0;
        transform.LookAt(transform.position + lookAtOffset);
    }

}
