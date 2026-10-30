using UnityEngine;

namespace Wildhaven
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class PlayerMotor : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 4.5f;
        [SerializeField] private float turnSpeed = 12f;
        [SerializeField] private float gravity = -20f;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private MobileJoystick joystick;
        private CharacterController controller;
        private float verticalVelocity;

        private void Awake() => controller = GetComponent<CharacterController>();

        public void SetJoystick(MobileJoystick value) => joystick = value;

        private void Update()
        {
            Vector2 input = joystick != null ? joystick.Value :
                new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            Vector3 forward = cameraTransform != null ? cameraTransform.forward : Vector3.forward;
            Vector3 right = cameraTransform != null ? cameraTransform.right : Vector3.right;
            forward.y = 0; right.y = 0;
            forward.Normalize(); right.Normalize();

            Vector3 move = Vector3.ClampMagnitude(forward * input.y + right * input.x, 1f);
            if (move.sqrMagnitude > 0.01f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(move), turnSpeed * Time.deltaTime);
            }

            if (controller.isGrounded && verticalVelocity < 0) verticalVelocity = -2f;
            verticalVelocity += gravity * Time.deltaTime;
            Vector3 velocity = move * moveSpeed;
            velocity.y = verticalVelocity;
            controller.Move(velocity * Time.deltaTime);
        }
    }
}