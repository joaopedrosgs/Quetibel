using UnityEngine;

public class DragMouseOrbit : MonoBehaviour
{
    public Transform Target;
    public float Distance = 5.0f;
    public float XSpeed = 120.0f;
    public float YSpeed = 120.0f;
    public float YMinLimit = -20f;
    public float YMaxLimit = 80f;
    public float DistanceMin = .5f;
    public float DistanceMax = 15f;
    public float SmoothTime = 2f;
    private float _rotationYAxis;
    private float _rotationXAxis;
    private float _velocityX;

    float _velocityY;

    // Use this for initialization
    void Start()
    {

        Vector3 angles = transform.eulerAngles;
        _rotationYAxis = angles.y;
        _rotationXAxis = angles.x;
        // Make the rigid body not change rotation
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }
        Target = GameController.Player.transform;
    }

    void LateUpdate()
    {
        if (GameController.GetGameState() != GameController.State.Playing)
            return;



        if (Input.GetMouseButton(1))
        {
            _velocityX += XSpeed * Input.GetAxis("Mouse X") * Distance * 0.02f;
            _velocityY += YSpeed * Input.GetAxis("Mouse Y") * 0.02f;
        }
        _rotationYAxis += _velocityX;
        _rotationXAxis -= _velocityY;
        _rotationXAxis = ClampAngle(_rotationXAxis, YMinLimit, YMaxLimit);
        Quaternion toRotation = Quaternion.Euler(_rotationXAxis, _rotationYAxis, 0);
        Quaternion rotation = toRotation;

        Distance = Mathf.Clamp(Distance - Input.GetAxis("Mouse ScrollWheel") * 5, DistanceMin, DistanceMax);
        Vector3 negDistance = new Vector3(0.0f, 0.0f, -Distance);
        Vector3 position = rotation * negDistance + Target.position;

        position.y += 4;
        transform.rotation = rotation;
        transform.position = position;
        _velocityX = Mathf.Lerp(_velocityX, 0, Time.deltaTime * SmoothTime);
        _velocityY = Mathf.Lerp(_velocityY, 0, Time.deltaTime * SmoothTime);
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}