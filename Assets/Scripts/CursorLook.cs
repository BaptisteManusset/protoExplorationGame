using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorLook : MonoBehaviour
{
    // [Header("Player")] [Tooltip("Move speed of the character in m/s")]
    // public float MoveSpeed = 2.0f;

    // [Tooltip("Sprint speed of the character in m/s")]
    // public float SprintSpeed = 5.335f;

    // [Tooltip("How fast the character turns to face movement direction")] [Range(0.0f, 0.3f)]
    // public float RotationSmoothTime = 0.12f;

    // [Tooltip("Acceleration and deceleration")]
    // public float SpeedChangeRate = 10.0f;

    // public AudioClip LandingAudioClip;
    // public AudioClip[] FootstepAudioClips;
    // [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

    // [Space(10)] [Tooltip("The height the player can jump")]
    // public float JumpHeight = 1.2f;

    // [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    // public float Gravity = -15.0f;

    // [Space(10)]
    // [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    // public float JumpTimeout = 0.50f;
    //
    // [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    // public float FallTimeout = 0.15f;

    // [Header("Player Grounded")]
    // [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    // public bool Grounded = true;

    // [Tooltip("Useful for rough ground")] public float GroundedOffset = -0.14f;
    //
    // [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    // public float GroundedRadius = 0.28f;

    // [Tooltip("What layers the character uses as ground")]
    // public LayerMask GroundLayers;

    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject CinemachineCameraTarget;

    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 70.0f;

    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -30.0f;

    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;

    // [Tooltip("For locking the camera position on all axis")]
    // public bool LockCameraPosition = false;

    // cinemachine
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    // player
    // private float _speed;
    // private float _animationBlend;
    // private float _targetRotation = 0.0f;
    // private float _rotationVelocity;
    // private float _verticalVelocity;
    // private float _terminalVelocity = 53.0f;

    // timeout deltatime
    // private float _jumpTimeoutDelta;
    // private float _fallTimeoutDelta;

    // animation IDs
    private int _animIDSpeed;
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDFreeFall;
    private int _animIDMotionSpeed;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
    private PlayerInput _playerInput;
#endif
    // private Animator _animator;
    // private CharacterController _controller;
    private StarterAssetsInputs _input;
    private GameObject _mainCamera;

    private const float _threshold = 0.01f;

    // private bool _hasAnimator;

    private bool IsCurrentDeviceMouse
    {
        get
        {
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
            if (_playerInput == null)
            {
                _playerInput = FindObjectOfType<PlayerInput>(true);
            }
            
            return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
        }
    }


    private void Awake()
    {
        // get a reference to our main camera
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }

    private void Start()
    {
        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
        // TryGetComponent(out _animator);
        // // _hasAnimator = TryGetComponent(out _animator);
        // _controller = GetComponent<CharacterController>();
        _input = FindObjectOfType<StarterAssetsInputs>(true);
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        _playerInput = GetComponent<PlayerInput>();
#endif

        // AssignAnimationIDs();

        // // reset our timeouts on start
        // _jumpTimeoutDelta = JumpTimeout;
        // _fallTimeoutDelta = FallTimeout;
    }

    private void Update()
    {
        // _hasAnimator = TryGetComponent(out _animator);

        if (!CameraTargetSwitcher.Instance.IsWalking) return;

        // JumpAndGravity();
        // GroundedCheck();
        // Move();
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    // private void AssignAnimationIDs()
    // {
    //     // _animIDSpeed = Animator.StringToHash("Speed");
    //     // _animIDGrounded = Animator.StringToHash("Grounded");
    //     // _animIDJump = Animator.StringToHash("Jump");
    //     // _animIDFreeFall = Animator.StringToHash("FreeFall");
    //     // _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    // }

    // private void GroundedCheck()
    // {
    //     // set sphere position, with offset
    //     Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
    //         transform.position.z);
    //     Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
    //         QueryTriggerInteraction.Ignore);
    //
    //     // update animator if using character
    //     // if (_hasAnimator)
    //     // {
    //     _animator.SetBool(_animIDGrounded, Grounded);
    //     // }
    // }

    private void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (_input.look.sqrMagnitude >= _threshold)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
            _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}