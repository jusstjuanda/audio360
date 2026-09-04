using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float mouseSensitivity = 0.15f;
    [SerializeField] private Camera playerCamera;

    private CharacterController controller;
    private float verticalRotation;
    private float verticalVelocity;
    private const float Gravity = -9.81f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (playerCamera == null)
        {
            playerCamera = GetComponentInChildren<Camera>();
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        EnsureAudioListener();
    }

    private void EnsureAudioListener()
    {
        Camera targetCamera = playerCamera != null ? playerCamera : Camera.main;

        bool hasListenerOnCamera = targetCamera != null && targetCamera.GetComponent<AudioListener>() != null;
        bool hasListenerOnPlayer = GetComponent<AudioListener>() != null;

        if (!hasListenerOnCamera && !hasListenerOnPlayer)
        {
            GameObject listenerHost = targetCamera != null ? targetCamera.gameObject : gameObject;
            listenerHost.AddComponent<AudioListener>();
        }
    }

    private void Update()
    {
        HandleLook();
        HandleMove();
    }

    private void HandleLook()
    {
        Mouse mouse = Mouse.current;
        if (mouse == null || playerCamera == null) return;

        Vector2 delta = mouse.delta.ReadValue() * mouseSensitivity;

        transform.Rotate(Vector3.up * delta.x);

        verticalRotation = Mathf.Clamp(verticalRotation - delta.y, -85f, 85f);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    private void HandleMove()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        Vector2 input = Vector2.zero;
        if (keyboard.wKey.isPressed) input.y += 1f;
        if (keyboard.sKey.isPressed) input.y -= 1f;
        if (keyboard.dKey.isPressed) input.x += 1f;
        if (keyboard.aKey.isPressed) input.x -= 1f;
        input = Vector2.ClampMagnitude(input, 1f);

        Vector3 move = (transform.right * input.x + transform.forward * input.y) * moveSpeed;

        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }
        verticalVelocity += Gravity * Time.deltaTime;
        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);
    }
}
