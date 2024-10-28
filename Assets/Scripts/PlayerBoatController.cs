using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBoatController : MonoBehaviour
{
    [SerializeField] private GameDataSO _gameData;
    public float acceleration = 10f; // Acceleration factor
    public float maxSpeed = 30f; // Maximum speed of the ship
    public float lerpSpeed = 1f; // Speed of the lerp
    public float friction = 0.999f; // Friction factor
    public float collisionBuffer = 0.5f; // New variable for collision buffer

    public ParticleSystem wakeParticles;
    public ParticleSystem bowRightParticles;
    public ParticleSystem bowLeftParticles;

    private Vector3 velocity = Vector3.zero;

    public bool _isEngineRunning = false;
    private Vector3 lastValidPosition;
    private Rigidbody rb;

    private bool isColliding = false;

    private bool areBowParticlesPlaying = false;

    void Start()
    {
        Debug.Log("PlayerBoatController Start");
        // Make sure we have required components
        if (GetComponent<Collider>() == null)
        {
            Debug.LogWarning("No collider found on ship - adding BoxCollider");
            gameObject.AddComponent<BoxCollider>();
        }

        rb = gameObject.AddComponent<Rigidbody>();
        // Configure Rigidbody to not be affected by physics
        rb.useGravity = false;
        rb.isKinematic = true; // This prevents physics forces
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

        // Store initial position
        lastValidPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {


        // Store position before movement
        lastValidPosition = transform.position;

        Vector3 inputDirection = Vector3.zero;
        bool hasInput = false;

        // Check input for movement
        if (Gamepad.current != null)
        {
            Vector2 joystickInput = Gamepad.current.leftStick.ReadValue();
            inputDirection = new Vector3(joystickInput.x, 0, joystickInput.y);
            hasInput = joystickInput != Vector2.zero;
        }
        else if (Input.GetMouseButton(0)) // Right mouse button is held down
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 shipScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 directionInScreenSpace = mousePosition - shipScreenPosition;
            inputDirection = new Vector3(directionInScreenSpace.x, 0, directionInScreenSpace.y);
            hasInput = true;
        }
        else
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
            {
                inputDirection += Vector3.forward;
                hasInput = true;
            }
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            {
                inputDirection += Vector3.left;
                hasInput = true;
            }
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
            {
                inputDirection += Vector3.back;
                hasInput = true;
            }
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            {
                inputDirection += Vector3.right;
                hasInput = true;
            }
        }

        _isEngineRunning = hasInput;

        if (_isEngineRunning)
        {
            wakeParticles.Play();
        }
        else
        {
            wakeParticles.Stop();
        }

        // Normalize input direction to ensure consistent movement speed regardless of direction
        inputDirection.Normalize();

        // Calculate acceleration based on input direction
        Vector3 accelerationVector = inputDirection * acceleration;

        // Apply acceleration to velocity
        velocity += accelerationVector * Time.deltaTime;

        // Apply friction if there is no input
        if (inputDirection == Vector3.zero)
        {
            velocity *= friction;
        }

        // Clamp velocity magnitude to ensure it doesn't exceed maxSpeed
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        if (!isColliding)
        {
            // Update position based on velocity
            transform.position += velocity * Time.deltaTime;
        }
        else
        {
            // If colliding, set velocity to zero
            velocity = Vector3.zero;
        }

        // Rotate the ship based on the input direction, animate the rotation
        if (inputDirection != Vector3.zero)
        {
            Vector3 normalizedInputDirection = Vector3.Normalize(inputDirection);
            Quaternion targetRotation = Quaternion.LookRotation(normalizedInputDirection);
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * lerpSpeed
            );
        }

        bool shouldPlayBowParticles = Mathf.Abs(velocity.x) > 10f || Mathf.Abs(velocity.z) > 10f;

        if (shouldPlayBowParticles && !areBowParticlesPlaying)
        {
            bowRightParticles.Play();
            bowLeftParticles.Play();
            areBowParticlesPlaying = true;
        }
        else if (!shouldPlayBowParticles && areBowParticlesPlaying)
        {
            bowRightParticles.Stop();
            bowLeftParticles.Stop();
            areBowParticlesPlaying = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("Still colliding with obstacle");
            Vector3 pushDirection = (transform.position - other.transform.position).normalized;
            transform.position = lastValidPosition + pushDirection * collisionBuffer;
            isColliding = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("No longer colliding with obstacle");
            isColliding = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("Obstacle collision detected");
            Vector3 pushDirection = (transform.position - other.transform.position).normalized;
            transform.position = lastValidPosition + pushDirection * collisionBuffer;
            velocity = Vector3.zero;
            isColliding = true;
            Debug.Log("object name: " + other.gameObject.name);
        }

        if (other.CompareTag("DropZone"))
        {
            Debug.Log("Drop zone collision detected");
            EventManager.Game.OnDropZoneEntered?.Invoke();  
        }

        if (other.CompareTag("Trash"))
        {
            TrashController trash = other.GetComponent<TrashController>();
            int trashWeight = trash.TrashData.InventoryWeight;
            if (_gameData.GetBoatInventoryWeight() + trashWeight <= _gameData.BoatInventoryCapacity) 
            {
                EventManager.Game.OnTrashCollected?.Invoke(trash);
                Destroy(other.gameObject);
            }
        }
    }
}
