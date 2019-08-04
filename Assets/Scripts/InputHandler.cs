using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    #region Data
    [BoxGroup("Input Data")]
    public CameraInputData cameraInputData;
    [BoxGroup("Input Data")]
    public InputManager movementInputData;
    
    #endregion
    
    
    void Start()
    {
        cameraInputData.ResetInput();
        movementInputData.ResetInput();
    }

    void Update()
    {
        GetCameraInput();
        GetMovementInputData();
    }
    
    
    #region Custom Methods
   
    void GetCameraInput()
    {
        cameraInputData.InputVectorX = Input.GetAxis("Mouse X");
        cameraInputData.InputVectorY = Input.GetAxis("Mouse Y");

        //cameraInputData.ZoomClicked = Input.GetMouseButtonDown(1);
        //cameraInputData.ZoomReleased = Input.GetMouseButtonUp(1);
    }

    void GetMovementInputData()
    {
        movementInputData.InputVectorX = Input.GetAxisRaw("Horizontal");
        movementInputData.InputVectorY = 1;//Input.GetAxisRaw("Vertical");

        //movementInputData.IsRunning = Input.GetKey(KeyCode.LeftShift);
        movementInputData.RunClicked = Input.GetKeyDown(KeyCode.LeftShift);
        movementInputData.RunReleased = Input.GetKeyUp(KeyCode.LeftShift);

        //if(movementInputData.RunClicked)
            //movementInputData.IsRunning = true;

        //if(movementInputData.RunReleased)
            //movementInputData.IsRunning = false;
            movementInputData.IsRunning = true;

        movementInputData.JumpClicked = Input.GetKeyDown(KeyCode.Space);
        movementInputData.CrouchClicked = Input.GetKeyDown(KeyCode.LeftControl);
    }
    #endregion
}
