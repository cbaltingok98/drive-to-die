using _project.Scripts.Input;
using UnityEngine;

namespace _project.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private CharacterController characterController;
    
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundDistance = 0.4f;
        [SerializeField] private LayerMask groundMask;

        [SerializeField] private float speed = 6f;
        [SerializeField] private float turnSmoothTime = 0.1f;
        [SerializeField] private float gravity = -9.81f;

        private JoystickController _joystickController;
        
        private Animator _animator;
        private Vector3 _velocity;
        
        private float _turnSmoothVelocity;
        private float _horizontal;
        private float _vertical;
        private float _targetAngle;
        private float _currentSpeed;

        private bool _isGrounded;
        private bool _isCarrying;

        public void Init(int setSpeed)
        {
            speed = setSpeed;
        }

        private void Start()
        {
            //_animator = player.GetComponent<Animator>();
            _joystickController = JoystickController.Instance;
        }

        private void FixedUpdate()
        {
            MoveCharacter();
        }

        private void MoveCharacter()
        {
            _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (_isGrounded && _velocity.y < 0)
            {
                _velocity.y = -1f;
            }
        
            var direction = _joystickController.GetDirection();
            _targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        
            //HandlePlayerAnimation(direction);

            _velocity.y += gravity * Time.deltaTime;
        
            var angle = Mathf.SmoothDampAngle(player.transform.eulerAngles.y, _targetAngle, ref _turnSmoothVelocity, turnSmoothTime);
            player.transform.rotation = Quaternion.Euler(0f, angle, 0f);
            characterController.Move(direction.normalized * (speed * _joystickController.GetVerticalDirection() * Time.deltaTime));
            characterController.Move(_velocity * Time.deltaTime);
        }

        private void HandlePlayerAnimation(Vector3 direction)
        {
            _animator.SetBool("Carrying", _isCarrying);
            if (_isCarrying)
            {
                _animator.SetBool("StackRun", direction != Vector3.zero);
                _animator.SetBool("StackIdle", direction == Vector3.zero);
            }
            else
            {
                _animator.SetBool("Run", direction != Vector3.zero);
                _animator.SetBool("Idle", direction == Vector3.zero);   
            }
        }

        public void SetIsCarrying(bool set) => _isCarrying = set;
    }
}
