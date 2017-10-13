
public interface ICameraController
{
    void MoveCameraUp(float deltaTime);
    void MoveCameraDown(float deltaTime);
    void MoveCameraLeft(float deltaTime);
    void MoveCameraRight(float deltaTime);
    void MoveCameraForward(float deltaTime);
    void MoveCameraBackward(float deltaTime);

    void RotateCameraLeft(float deltaTime);
    void RotateCameraRight(float deltaTime);

    void ZoomCameraIn(float deltaTime);
    void ZoomCameraOut(float deltaTime);

    void ResetCamera();
    void Activate();
    void Deactivate();
}

