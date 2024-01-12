using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.Systems
{
    /*[Title("Photon Character Controller")]
    [Image(typeof(IconCapsuleSolid), ColorTheme.Type.Blue)]
    
    [Category("Photon Character Controller")]
    [Description("Moves the Character using Unity's default Character Controller")]
    
    [Serializable]
    public class UnitDriverPhotonController : TUnitDriver
    {
        private const float MAX_SLOPE_SLIDE_FROM_CHARACTER = 90;
        private const float EPSILON_SLIDE_FROM_CHARACTER = 0.001f;
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] protected float m_SkinWidth = 0.08f;
        [SerializeField] protected float m_PushForce = 1.0f;
        [SerializeField] protected float m_MaxSlope = 45f;
        [SerializeField] protected float m_StepHeight = 0.3f;
        [SerializeField] private Axonometry m_Axonometry = new Axonometry();

        // MEMBERS: -------------------------------------------------------------------------------

        [NonSerialized] protected CharacterController m_Controller;

        [NonSerialized] protected Vector3 m_MoveDirection;
        [NonSerialized] protected float m_VerticalSpeed;
 
        [NonSerialized] protected AnimFloat m_IsGrounded;
        [NonSerialized] protected AnimVector3 m_FloorNormal;
 
        [NonSerialized] protected int m_GroundFrame = -100;
        [NonSerialized] protected float m_GroundTime = -100f;
        [NonSerialized] protected float m_JumpTime = -100f;

        [NonSerialized] private DriverControllerComponent m_Helper;
        
        [NonSerialized] private Vector3 m_SlideFromCharacter;
        [NonSerialized] private int m_FrameSlideFromCharacter;

        // INTERFACE PROPERTIES: ------------------------------------------------------------------

        public override Vector3 WorldMoveDirection => m_Controller.velocity;
        public override Vector3 LocalMoveDirection => Transform.InverseTransformDirection(
            WorldMoveDirection
        );

        public override float SkinWidth => m_Controller.skinWidth;
        public override bool IsGrounded => m_Controller.isGrounded &&
                                           m_FrameSlideFromCharacter < Time.frameCount;
        
        public override Vector3 FloorNormal => m_FloorNormal.Current;

        public override bool Collision
        {
            get => m_Controller.detectCollisions;
            set => m_Controller.detectCollisions = value;
        }
        
        public Vector3 Movement { get; set; }
        public bool IsMine { get; set; }

        // INITIALIZERS: --------------------------------------------------------------------------

        public UnitDriverPhotonController()
        {
            m_MoveDirection = Vector3.zero;
            m_VerticalSpeed = 0f;
            
            m_SlideFromCharacter = Vector3.zero;
            m_FrameSlideFromCharacter = -1;
        }

        public override void OnStartup(Character character)
        {
            base.OnStartup(character);

            m_IsGrounded = new AnimFloat(1f, 0.01f);
            m_FloorNormal = new AnimVector3(Vector3.up, 0.05f);

            m_Controller = Character.GetComponent<CharacterController>();
            if (m_Controller == null)
            {
                var instance = Character.gameObject;
                m_Controller = instance.AddComponent<CharacterController>();
                m_Controller.hideFlags = HideFlags.HideInInspector;
            }
            
            m_Helper = DriverControllerComponent.Register(
                Character,
                OnControllerColliderHit
            );

            character.Ragdoll.EventBeforeStartRagdoll += OnStartRagdoll;
            character.Ragdoll.EventAfterStartRecover += OnEndRagdoll;
        }

        public override void OnDispose(Character character)
        {
            base.OnDispose(character);

            UnityEngine.Object.Destroy(m_Helper);
            UnityEngine.Object.Destroy(m_Controller);
            
            character.Ragdoll.EventBeforeStartRagdoll -= OnStartRagdoll;
            character.Ragdoll.EventAfterStartRecover -= OnEndRagdoll;
        }

        // UPDATE METHODS: ------------------------------------------------------------------------

        public override void OnUpdate()
        {
            if (Character.IsDead) return;
            
            UpdateProperties();

            UpdateGravity(Character.Motion);
            UpdateJump(Character.Motion);

            UpdateTranslation(Character.Motion);
            m_Axonometry?.ProcessPosition(this, Transform.position);
        }

        public override void OnFixedUpdate()
        {
            if (Character.IsDead) return;
            
            base.OnFixedUpdate();
            UpdatePhysicProperties();
        }

        protected virtual void UpdateProperties()
        {
            m_FloorNormal.UpdateWithDelta(Character.Time.DeltaTime);
            m_MoveDirection = Vector3.zero;
            m_IsGrounded.Update(IsGrounded, COYOTE_TIME);
            
            if (Math.Abs(m_Controller.skinWidth - m_SkinWidth) > float.Epsilon)
            {
                m_Controller.skinWidth = m_SkinWidth;
            }
            
            if (Math.Abs(m_Controller.slopeLimit - m_MaxSlope) > float.Epsilon)
            {
                m_Controller.slopeLimit = m_MaxSlope;
            }
            
            if (Math.Abs(m_Controller.stepOffset - m_StepHeight) > float.Epsilon)
            {
                m_Controller.stepOffset = m_StepHeight;
            }
        }
        
        protected virtual void UpdatePhysicProperties()
        {
            var height = Character.Motion.Height;
            var radius = Character.Motion.Radius;

            if (Math.Abs(m_Controller.height - height) > float.Epsilon)
            {
                var offset = (m_Controller.height - height) * 0.5f;
                
                Transform.localPosition += Vector3.down * offset;
                m_Controller.height = height;
                
                Character.Animim.ResetModelPosition();
            }

            if (Math.Abs(m_Controller.radius - radius) > float.Epsilon)
            {
                m_Controller.radius = radius;
            }

            if (m_Controller.center != Vector3.zero)
            {
                m_Controller.center = Vector3.zero;   
            }
        }

        protected virtual void UpdateJump(IUnitMotion motion)
        {
            if (!motion.IsJumping) return;
            if (!motion.CanJump) return;
            
            var jumpCooldown = m_JumpTime + motion.JumpCooldown < Character.Time.Time;
            if (!jumpCooldown) return;
            
            m_VerticalSpeed = motion.IsJumpingForce;
            m_JumpTime = Character.Time.Time;
            Character.OnJump(motion.IsJumpingForce);
        }

        protected virtual void UpdateGravity(IUnitMotion motion)
        {
            var gravity = WorldMoveDirection.y >= 0f 
                ? motion.GravityUpwards 
                : motion.GravityDownwards;

            gravity *= GravityInfluence;
            
            m_VerticalSpeed += gravity * Character.Time.DeltaTime;

            if (m_Controller.isGrounded)
            {
                if (Character.Time.Time - m_GroundTime > COYOTE_TIME &&
                    Character.Time.Frame - m_GroundFrame > COYOTE_FRAMES)
                {
                    Character.OnLand(m_VerticalSpeed);
                }
                
                m_GroundTime = Character.Time.Time;
                m_GroundFrame = Character.Time.Frame;

                m_VerticalSpeed = Mathf.Max(
                    m_VerticalSpeed, gravity
                );
            }

            m_VerticalSpeed = Mathf.Max(
                m_VerticalSpeed,
                motion.TerminalVelocity
            );
        }

        protected virtual void UpdateTranslation(IUnitMotion motion)
        {
            var movement = Movement;
            if(IsMine)
            {
                movement = Vector3.up * (m_VerticalSpeed * Character.Time.DeltaTime);

                var kinetic = motion.MovementType switch
                {
                    Character.MovementType.MoveToDirection => UpdateMoveToDirection(motion),
                    Character.MovementType.MoveToPosition => UpdateMoveToPosition(motion),
                    _ => Vector3.zero
                };

                var rootMotion = Character.Animim.RootMotionDeltaPosition;
                var translation = Vector3.Lerp(kinetic, rootMotion, Character.RootMotionPosition);

                movement += m_Axonometry?.ProcessTranslation(this, translation) ?? translation;

                if (m_FrameSlideFromCharacter >= Time.frameCount - 1)
                {
                    var deltaSpeed = motion.LinearSpeed * Character.Time.DeltaTime;
                    movement += m_SlideFromCharacter * deltaSpeed;
                }

                Movement = movement;
            }

            if (m_Controller.enabled)
            {
                m_Controller.Move(movement);
            }
        }


        // POSITION METHODS: ----------------------------------------------------------------------

        protected virtual Vector3 UpdateMoveToDirection(IUnitMotion motion)
        {
            m_MoveDirection = motion.MoveDirection;
            return m_MoveDirection * Character.Time.DeltaTime;
        }

        protected virtual Vector3 UpdateMoveToPosition(IUnitMotion motion)
        {
            var distance = Vector3.Distance(Character.Feet, motion.MovePosition);
            var brakeRadiusHeuristic = Math.Max(motion.Height, motion.Radius * 2f);
            var velocity = motion.MoveDirection.magnitude;
            
            if (distance < brakeRadiusHeuristic)
            {
                velocity = Mathf.Lerp(
                    motion.LinearSpeed, Mathf.Max(motion.LinearSpeed * 0.25f, 1f),
                    1f - Mathf.Clamp01(distance / brakeRadiusHeuristic)
                );
            }
            
            m_MoveDirection = motion.MoveDirection;
            return m_MoveDirection.normalized * (velocity * Character.Time.DeltaTime);
        }

        // INTERFACE METHODS: ---------------------------------------------------------------------

        public override void SetPosition(Vector3 position)
        {
            position += Vector3.up * (Character.Motion.Height * 0.5f);
            Transform.position = position;
            Physics.SyncTransforms();
        }

        public override void SetRotation(Quaternion rotation)
        {
            Transform.rotation = rotation;
            Physics.SyncTransforms();
        }

        public override void SetScale(Vector3 scale)
        {
            Transform.localScale = scale;
            Physics.SyncTransforms();
        }

        public override void AddPosition(Vector3 amount)
        {
            Transform.position += amount;
            Physics.SyncTransforms();
        }

        public override void AddRotation(Quaternion amount)
        {
            Transform.rotation *= amount;
            Physics.SyncTransforms();
        }

        public override void AddScale(Vector3 scale)
        {
            Transform.localScale += scale;
            Physics.SyncTransforms();
        }
        
        // GRAVITY METHODS: -----------------------------------------------------------------------

        public override void ResetVerticalVelocity()
        {
            m_VerticalSpeed = 0f;
        }

        // CALLBACK METHODS: ----------------------------------------------------------------------

        protected virtual void OnControllerColliderHit(ControllerColliderHit hit)
        {
            m_FloorNormal.Target = hit.normal;
            var angle = Vector3.Angle(hit.normal, Vector3.up);
            
            OnColliderHitPushRigidbodies(hit, angle);
            OnColliderHitSlideFromCharacters(hit, angle);
        }

        private void OnColliderHitSlideFromCharacters(ControllerColliderHit hit, float angle)
        {
            if (WorldMoveDirection.y > 0f || angle >= MAX_SLOPE_SLIDE_FROM_CHARACTER) return;
            
            var other = hit.collider.Get<Character>();
            if (other == null) return;
            
            var slideDirection =
                Vector3.Scale(Character.transform.position, Vector3Plane.NormalUp) -
                Vector3.Scale(other.transform.position, Vector3Plane.NormalUp);

            slideDirection = slideDirection.sqrMagnitude > EPSILON_SLIDE_FROM_CHARACTER
                ? slideDirection.normalized
                : other.transform.forward;
            
            slideDirection.y = -1f;
                    
            m_SlideFromCharacter = slideDirection;
            m_FrameSlideFromCharacter = Time.frameCount;
        }

        private void OnColliderHitPushRigidbodies(ControllerColliderHit hit, float angle)
        {
            if (m_PushForce < float.Epsilon) return;
            
            if (angle > 90f) return;
            if (angle < 5f) return;

            var hitRigidbody = hit.collider.attachedRigidbody;
            if (!hitRigidbody || hitRigidbody.isKinematic) return;
            
            var force = hit.controller.velocity * m_PushForce;
            force /= Character.Time.FixedDeltaTime;

            hitRigidbody.AddForceAtPosition(force, hit.point, ForceMode.Force);
        }
        
        private void OnStartRagdoll()
        {
            m_Controller.enabled = false;
            m_Controller.detectCollisions = false;
        }
        
        private void OnEndRagdoll()
        {
            m_Controller.enabled = true;
            m_Controller.detectCollisions = true;
            
            m_Controller.Move(Vector3.zero);
            m_MoveDirection = Vector3.zero;
        }

        // GIZMOS: --------------------------------------------------------------------------------

        public override void OnDrawGizmos(Character character)
        {
            if (!Application.isPlaying) return;

            var motion = character.Motion;
            if (motion == null) return;

            switch (motion.MovementType)
            {
                case Character.MovementType.MoveToPosition:
                    OnDrawGizmosToTarget(motion);
                    break;
            }
        }

        protected void OnDrawGizmosToTarget(IUnitMotion motion)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(Character.Feet, motion.MovePosition);
        }
        
        // STRING: --------------------------------------------------------------------------------

        public override string ToString() => "Photon Character Controller";
    }*/
}