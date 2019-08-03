using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    //Data
    public InputManager inputManager;
    public HeadBobData headBobData;
    
    //Locomotion
    #region Locomotion
    [Space]
    [BoxGroup("Locomotion Settings")] public float crouchSpeed = 1f;
    [BoxGroup("Locomotion Settings")] public float walkSpeed = 2f;
    [BoxGroup("Locomotion Settings")] public float runSpeed = 3f;
    [BoxGroup("Locomotion Settings")] public float jumpSpeed = 5f;
    [BoxGroup("Locomotion Settings")][Slider(0f,1f)] public float moveBackwardsSpeedPercent = 0.5f;
    [BoxGroup("Locomotion Settings")][Slider(0f,1f)]  public float moveSideSpeedPercent = 0.75f;
    #endregion
    
    //Run
    #region Run Settings
    [Space]
    [BoxGroup("Run Settings")][Slider(-1f,1f)] public float canRunThreshold = 0.8f;
    [BoxGroup("Run Settings")] public AnimationCurve runTransitionCurve = AnimationCurve.EaseInOut(0f,0f,1f,1f);
    #endregion
    
    //Crouch 
    #region Crouch Settings
    [Space]
    [BoxGroup("Crouch Settings")] [Slider(0.2f,0.9f)] public float crouchPercent = 0.6f;
    [BoxGroup("Crouch Settings")] public float crouchTransitionDuration = 1f;
    [BoxGroup("Crouch Settings")] public AnimationCurve crouchTransitionCurve = AnimationCurve.EaseInOut(0f,0f,1f,1f);
    #endregion
    
    //Landing
    #region Landing Settings
    [Space]
    [BoxGroup("Landing Settings")] [Slider(0.05f,0.5f)] public float lowLandAmount = 0.1f;
    [BoxGroup("Landing Settings")] [Slider(0.2f,0.9f)] public float highLandAmount = 0.6f;
    [BoxGroup("Landing Settings")] public float landTimer = 0.5f;
    [BoxGroup("Landing Settings")] public float landDuration = 1f;
    [BoxGroup("Landing Settings")] public AnimationCurve landCurve = AnimationCurve.EaseInOut(0f,0f,1f,1f);
    #endregion
    
    #region Gravity
    [Space]
    [BoxGroup("Gravity Settings")] public float gravityMultiplier = 2.5f;
    [BoxGroup("Gravity Settings")] public float stickToGroundForce = 5f;
    [Space]
    [BoxGroup("Gravity Settings")] public LayerMask groundLayer;
    [BoxGroup("Gravity Settings")] [Slider(0f,1f)] public float rayLength = 0.1f;
    [BoxGroup("Gravity Settings")] [Slider(0.01f,1f)] public float raySphereRadius = 0.1f;
    #endregion
    
    //Wall
    #region Wall Settings
    [BoxGroup("Gravity Settings")] public LayerMask obstacleLayers;
    [BoxGroup("Check Wall Settings")] [Slider(0f,1f)] public float rayObstacleLength = 0.1f;
    [BoxGroup("Check Wall Settings")] [Slider(0.01f,1f)] public float rayObstacleSphereRadius = 0.1f;
                    
    #endregion
    #region Smooth Settings
    [Space]                
    [BoxGroup("Smooth Settings")] [Range(1f,100f)] public float smoothRotateSpeed = 5f;
    [BoxGroup("Smooth Settings")] [Range(1f,100f)] public float smoothInputSpeed = 5f;
    [BoxGroup("Smooth Settings")] [Range(1f,100f)] public float smoothVelocitySpeed = 5f;
    [BoxGroup("Smooth Settings")] [Range(1f,100f)] public float smoothFinalDirectionSpeed = 5f;
    [BoxGroup("Smooth Settings")] [Range(1f,100f)] public float smoothHeadBobSpeed = 5f;

    [Space]
    [BoxGroup("Smooth Settings")] public bool experimental;
    [InfoBox("It should smooth our player movement to not start fast and not stop fast but it's somehow jerky", InfoBoxType.Warning)]
    [Tooltip("If set to very high it will stop player immediately after releasing input, otherwise it just another smoothing to our movement to make our player not move fast immediately and not stop immediately")]
    [BoxGroup("Smooth Settings")] [ShowIf("experimental")] [Range(1f,100f)] public float smoothInputMagnitudeSpeed = 5f;
                    
    #endregion
    
    
    //Private-----------------------------
    CharacterController m_characterController;
    Transform m_yawTransform;
    Transform m_camTransform;
    HeadBob m_headBob;
    CameraController m_cameraController;
 
     #region Debug/Cache
    [BoxGroup("DEBUG")][SerializeField][ReadOnly] Vector2 m_inputVector;
    [BoxGroup("DEBUG")][SerializeField][ReadOnly] Vector2 m_smoothInputVector;

    [Space]
    [BoxGroup("DEBUG")][SerializeField][ReadOnly] Vector3 m_finalMoveDir;
    [BoxGroup("DEBUG")][SerializeField][ReadOnly] Vector3 m_smoothFinalMoveDir;
    [Space]
    [BoxGroup("DEBUG")][SerializeField][ReadOnly] Vector3 m_finalMoveVector;

    [Space]
    [BoxGroup("DEBUG")][SerializeField][ReadOnly] float m_currentSpeed;
    [BoxGroup("DEBUG")][SerializeField][ReadOnly] float m_smoothCurrentSpeed;
    [BoxGroup("DEBUG")][SerializeField][ReadOnly] float m_finalSmoothCurrentSpeed;
    [BoxGroup("DEBUG")][SerializeField][ReadOnly] float m_walkRunSpeedDifference;

    [Space]
    [BoxGroup("DEBUG")][SerializeField][ReadOnly] float m_finalRayLength;
    [BoxGroup("DEBUG")][SerializeField][ReadOnly] bool m_hitWall;
    [BoxGroup("DEBUG")][SerializeField][ReadOnly] bool m_isGrounded;
    [BoxGroup("DEBUG")][SerializeField][ReadOnly] bool m_previouslyGrounded;

    [Space]
    [BoxGroup("DEBUG")][SerializeField][ReadOnly] float m_initHeight;
    [BoxGroup("DEBUG")][SerializeField][ReadOnly] float m_crouchHeight;
    [BoxGroup("DEBUG")][SerializeField][ReadOnly] Vector3 m_initCenter;
    [BoxGroup("DEBUG")][SerializeField][ReadOnly] Vector3 m_crouchCenter;
    [Space]
    [BoxGroup("DEBUG")][SerializeField][ReadOnly] float m_initCamHeight;
    [BoxGroup("DEBUG")][SerializeField][ReadOnly] float m_crouchCamHeight;
    [BoxGroup("DEBUG")][SerializeField][ReadOnly] float m_crouchStandHeightDifference;
    [BoxGroup("DEBUG")][SerializeField][ReadOnly] bool m_duringCrouchAnimation;
    [BoxGroup("DEBUG")][SerializeField][ReadOnly] bool m_duringRunAnimation;
    [Space]
    [BoxGroup("DEBUG")][SerializeField][ReadOnly] float m_inAirTimer;

    [Space]
    [BoxGroup("DEBUG")][ShowIf("experimental")][SerializeField][ReadOnly] float m_inputVectorMagnitude;
    [BoxGroup("DEBUG")][ShowIf("experimental")][SerializeField][ReadOnly] float m_smoothInputVectorMagnitude;
    #endregion
            
    #region Cache Non Serialized
    RaycastHit m_hitInfo;
    IEnumerator m_CrouchRoutine;
    IEnumerator m_LandRoutine;
    #endregion

    #region Unity Methods

    private void Start()
    {
        GetReferences();
        InitVariables();
        
    }

    private void Update()
    {
        if (m_yawTransform != null)
            RotateTowardsCamera();

        if (m_characterController)
        {
            // Check if Grounded,Wall etc
            CheckIfGrounded();
            CheckIfWall();

            // Apply Smoothing
            SmoothInput();
            SmoothSpeed();
            SmoothDir();

            if (experimental)
                SmoothInputMagnitude();

            // Calculate Movement
            CalculateMovementDirection();
            CalculateSpeed();
            CalculateFinalMovement();

            // Handle Player Movement, Gravity, Jump, Crouch etc.
            //HandleCrouch();
            HandleHeadBob();
            HandleRunFOV();
            //HandleCameraSway();
            HandleLanding();

            ApplyGravity();
            ApplyMovement();

            m_previouslyGrounded = m_isGrounded;

        }
    }

    #endregion


        #region My Methods

        //Initialization Methods
        void GetReferences()
        {
            m_characterController = GetComponent<CharacterController>();
            m_cameraController = GetComponentInChildren<CameraController>();
            m_yawTransform = m_cameraController.transform;
            m_camTransform = GetComponentInChildren<Camera>().transform;
            m_headBob = new HeadBob(headBobData, moveBackwardsSpeedPercent, moveSideSpeedPercent);
        }

        void InitVariables()
        {
            // Calculate where our character center should be based on height and skin width
            m_characterController.center = new Vector3(0f,m_characterController.height / 2f + m_characterController.skinWidth,0f);

            m_initCenter = m_characterController.center;
            m_initHeight = m_characterController.height;

            m_crouchHeight = m_initHeight * crouchPercent;
            m_crouchCenter = (m_crouchHeight / 2f + m_characterController.skinWidth) * Vector3.up;

            m_crouchStandHeightDifference = m_initHeight - m_crouchHeight;

            m_initCamHeight = m_yawTransform.localPosition.y;
            m_crouchCamHeight = m_initCamHeight - m_crouchStandHeightDifference;

            // Sphere radius not included. If you want it to be included just decrease by sphere radius at the end of this equation
            m_finalRayLength = rayLength + m_characterController.center.y;

            m_isGrounded = true;
            m_previouslyGrounded = true;

            m_inAirTimer = 0f;
            m_headBob.CurrentStateHeight = m_initCamHeight;

            m_walkRunSpeedDifference = runSpeed - walkSpeed;
        }
    
        #endregion


        #region Smoothing

        void SmoothInput()
        {
            m_inputVector = inputManager.InputVector.normalized;
            m_smoothInputVector = Vector2.Lerp(m_smoothInputVector,m_inputVector,Time.deltaTime * smoothInputSpeed);

        }

        void SmoothSpeed()
        {
            m_smoothCurrentSpeed = Mathf.Lerp(m_smoothCurrentSpeed, m_currentSpeed, Time.deltaTime * smoothVelocitySpeed);

            if(inputManager.IsRunning && CanRun())
            {
                float _walkRunPercent = Mathf.InverseLerp(walkSpeed,runSpeed, m_smoothCurrentSpeed);
                m_finalSmoothCurrentSpeed = runTransitionCurve.Evaluate(_walkRunPercent) * m_walkRunSpeedDifference + walkSpeed;
            }
            else
            {
                m_finalSmoothCurrentSpeed = m_smoothCurrentSpeed;
            }
        }

        void SmoothDir()
        {
        
            m_smoothFinalMoveDir = Vector3.Lerp(m_smoothFinalMoveDir, m_finalMoveDir, Time.deltaTime * smoothFinalDirectionSpeed);
            Debug.DrawRay(transform.position, m_smoothFinalMoveDir, Color.yellow);
        }
                
        void SmoothInputMagnitude()
        {
            m_inputVectorMagnitude = m_inputVector.magnitude;
            m_smoothInputVectorMagnitude = Mathf.Lerp(m_smoothInputVectorMagnitude, m_inputVectorMagnitude, Time.deltaTime * smoothInputMagnitudeSpeed);
        }
    
        #endregion
    
        #region Calculation Methods
        void CheckIfGrounded()
        {
            Vector3 _origin = transform.position + m_characterController.center;

            bool _hitGround = Physics.SphereCast(_origin,raySphereRadius,Vector3.down,out m_hitInfo,m_finalRayLength,groundLayer);
            Debug.DrawRay(_origin,Vector3.down * (m_finalRayLength),Color.red);

            m_isGrounded = _hitGround ? true : false;
        }

        void CheckIfWall()
        {
                    
            Vector3 _origin = transform.position + m_characterController.center;
            RaycastHit _wallInfo;

            bool _hitWall = false;

            if(inputManager.HasInput)
                _hitWall = Physics.SphereCast(_origin,rayObstacleSphereRadius,m_smoothFinalMoveDir, out _wallInfo,rayObstacleLength,obstacleLayers);
            Debug.DrawRay(_origin,m_smoothFinalMoveDir * rayObstacleLength,Color.blue);

            m_hitWall = _hitWall ? true : false;
        }

        bool CheckIfRoof() /// TO FIX
        {
            Vector3 _origin = transform.position;
            RaycastHit _roofInfo;

            bool _hitRoof = Physics.SphereCast(_origin,raySphereRadius,Vector3.up,out _roofInfo,m_initHeight);

            return _hitRoof;
        }

        bool CanRun()
        {
            Vector3 _normalizedDir = Vector3.zero;

            if(m_smoothFinalMoveDir != Vector3.zero)
                _normalizedDir = m_smoothFinalMoveDir.normalized;

            float _dot = Vector3.Dot(transform.forward,_normalizedDir);
            return _dot >= canRunThreshold && !inputManager.IsCrouching ? true : false;
        }

        void CalculateMovementDirection()
        {

            Vector3 _vDir = transform.forward * m_smoothInputVector.y;
            Vector3 _hDir = transform.right * m_smoothInputVector.x;

            Vector3 _desiredDir = _vDir + _hDir;
            Vector3 _flattenDir = FlattenVectorOnSlopes(_desiredDir);

            m_finalMoveDir = _flattenDir;
        }

        Vector3 FlattenVectorOnSlopes(Vector3 _vectorToFlat)
        {
            if(m_isGrounded)
                _vectorToFlat = Vector3.ProjectOnPlane(_vectorToFlat,m_hitInfo.normal);
                    
            return _vectorToFlat;
        }

        void CalculateSpeed()
        {
            m_currentSpeed = inputManager.IsRunning && CanRun() ? runSpeed : walkSpeed;
            m_currentSpeed = inputManager.IsCrouching ? crouchSpeed : m_currentSpeed;
            m_currentSpeed = !inputManager.HasInput ? 0f : m_currentSpeed;
            m_currentSpeed = inputManager.InputVector.y == -1 ? m_currentSpeed * moveBackwardsSpeedPercent : m_currentSpeed;
            m_currentSpeed = inputManager.InputVector.x != 0 && inputManager.InputVector.y ==  0 ? m_currentSpeed * moveSideSpeedPercent :  m_currentSpeed;
        }

        void CalculateFinalMovement()
        {
            float _smoothInputVectorMagnitude = experimental ? m_smoothInputVectorMagnitude : 1f;
            Vector3 _finalVector = m_smoothFinalMoveDir * m_finalSmoothCurrentSpeed * _smoothInputVectorMagnitude;

            // We have to assign individually in order to make our character jump properly because before it was overwriting Y value and that's why it was jerky now we are adding to Y value and it's working
            m_finalMoveVector.x = _finalVector.x ;
            m_finalMoveVector.z = _finalVector.z ;

            if(m_characterController.isGrounded) // Thanks to this check we are not applying extra y velocity when in air so jump will be consistent
                m_finalMoveVector.y += _finalVector.y ; //so this makes our player go in forward dir using slope normal but when jumping this is making it go higher so this is weird
        }
                
        #region Landing Methods
        void HandleLanding()
        {
            if(!m_previouslyGrounded && m_isGrounded)
            {
                InvokeLandingRoutine();
            }
        }

        void InvokeLandingRoutine()
        {
            if(m_LandRoutine != null)
                StopCoroutine(m_LandRoutine);

            m_LandRoutine = LandingRoutine();
            StartCoroutine(m_LandRoutine);
        }

        IEnumerator LandingRoutine()
        {
            float _percent = 0f;
            float _landAmount = 0f;

            float _speed = 1f / landDuration;

            Vector3 _localPos = m_yawTransform.localPosition;
            float _initLandHeight = _localPos.y;

            _landAmount = m_inAirTimer > landTimer ? highLandAmount : lowLandAmount;

            while(_percent < 1f)
            {
                _percent += Time.deltaTime * _speed;
                float _desiredY = landCurve.Evaluate(_percent) * _landAmount;

                _localPos.y = _initLandHeight + _desiredY;
                m_yawTransform.localPosition = _localPos;

                yield return null;
            }
        }
        #endregion
                
        #region Locomotion Apply Methods

        void HandleHeadBob()
        {
                    
            if(inputManager.HasInput && m_isGrounded  && !m_hitWall)
            {
                if(!m_duringCrouchAnimation) // we want to make our head bob only if we are moving and not during crouch routine
                {
                    m_headBob.ScrollHeadBob(inputManager.IsRunning && CanRun(),inputManager.IsCrouching, inputManager.InputVector);
                    m_yawTransform.localPosition = Vector3.Lerp(m_yawTransform.localPosition,(Vector3.up * m_headBob.CurrentStateHeight) + m_headBob.FinalOffset,Time.deltaTime * smoothHeadBobSpeed);
                }
            }
            else // if we are not moving or we are not grounded
            {
                if(!m_headBob.Resetted)
                {
                    m_headBob.ResetHeadBob();
                }

                if(!m_duringCrouchAnimation) // we want to reset our head bob only if we are standing still and not during crouch routine
                    m_yawTransform.localPosition = Vector3.Lerp(m_yawTransform.localPosition,new Vector3(0f,m_headBob.CurrentStateHeight,0f),Time.deltaTime * smoothHeadBobSpeed);
            }

            //m_camTransform.localPosition = Vector3.Lerp(m_camTransform.localPosition,m_headBob.FinalOffset,Time.deltaTime * smoothHeadBobSpeed);
        }

        void HandleCameraSway()
        {
            m_cameraController.HandleSway(m_smoothInputVector,inputManager.InputVector.x);
        }

        void HandleRunFOV()
        {
            if(inputManager.HasInput && m_isGrounded  && !m_hitWall)
            {
                if(inputManager.RunClicked && CanRun())
                {
                    m_duringRunAnimation = true;
                    m_cameraController.ChangeRunFOV(false);
                }

                if(inputManager.IsRunning && CanRun() && !m_duringRunAnimation )
                {
                    m_duringRunAnimation = true;
                    m_cameraController.ChangeRunFOV(false);
                }
            }

            if(inputManager.RunReleased || !inputManager.HasInput || m_hitWall)
            {
                if(m_duringRunAnimation)
                {
                    m_duringRunAnimation = false;
                    m_cameraController.ChangeRunFOV(true);
                }
            }
        }
        void HandleJump()
        {
            if(inputManager.JumpClicked && !inputManager.IsCrouching)
            {
                //m_finalMoveVector.y += jumpSpeed /* m_currentSpeed */; // we are adding because ex. when we are going on slope we want to keep Y value not overwriting it
                m_finalMoveVector.y = jumpSpeed /* m_currentSpeed */; // turns out that when adding to Y it is too much and it doesn't feel correct because jumping on slope is much faster and higher;
                    
                m_previouslyGrounded = true;
                m_isGrounded = false;
            }
        }
        void ApplyGravity()
        {
            if(m_characterController.isGrounded) // if we would use our own m_isGrounded it would not work that good, this one is more precise
            {
                m_inAirTimer = 0f;
                m_finalMoveVector.y = -stickToGroundForce;

                HandleJump();
            }
            else
            {
                m_inAirTimer += Time.deltaTime;
                m_finalMoveVector += gravityMultiplier * Time.deltaTime * Physics.gravity;
            }
        }

        void ApplyMovement()
        {
            m_characterController.Move(m_finalMoveVector * Time.deltaTime);
        }

        void RotateTowardsCamera()
        {
            Quaternion _currentRot = transform.rotation;
            Quaternion _desiredRot = m_yawTransform.rotation;

            transform.rotation = Quaternion.Slerp(_currentRot,_desiredRot,Time.deltaTime * smoothRotateSpeed);
        }
        #endregion
            
        #endregion
}
