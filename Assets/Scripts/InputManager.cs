using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "InputManager", menuName = "Player/InputManager", order = 1)]

public class InputManager : ScriptableObject
{
    //Data
    private Vector2 _inputVector;

    private bool _isRunning;
    bool _isCrouching;

    bool _crouchClicked;
    bool _jumpClicked;

    bool _runPressed;
    bool _runReleased;
    
    //Properties
    
    public Vector2 InputVector => _inputVector;
    public bool HasInput => _inputVector != Vector2.zero;
    public float InputVectorX
    {
        set => _inputVector.x = value;
    }

    public float InputVectorY
    {
        set => _inputVector.y = value;
    }

    public bool IsRunning
    {
        get => _isRunning;
        set => _isRunning = value;
    }

    public bool IsCrouching
    {
        get => _isCrouching;
        set => _isCrouching = value;
    }

    public bool CrouchClicked
    {
        get => _crouchClicked;
        set => _crouchClicked = value;
    }

    public bool JumpClicked
    {
        get => _jumpClicked;
        set => _jumpClicked = value;
    }

    public bool RunClicked
    {
        get => _runPressed;
        set => _runPressed = value;
    }

    public bool RunReleased
    {
        get => _runReleased;
        set => _runReleased = value;
    }
    
    //Methods
    public void ResetInput()
    {
        _inputVector = Vector2.zero;
        _isRunning = false;
        _isCrouching = false;
        _crouchClicked = false;
        _jumpClicked = false;
        _runPressed = false;
        _runReleased =false;
    }
}
