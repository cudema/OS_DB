using UnityEngine;

public class CameraContoller : MonoBehaviour  //화면 회전
{
    [SerializeField]
    float xMoveSpeed;
    [SerializeField]
    float yMoveSpeed;

    float x, y;

    [SerializeField]
    GameManager manager;

    private void Awake()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.x;
        y = angles.y;

    }

    void Update()
    {
        if (manager.IsGamePlsying)
        {
            UpdateCameraRotation();
        }
    }

    void UpdateCameraRotation()
    {


        x -= xMoveSpeed * Input.GetAxisRaw("Mouse Y") * Time.deltaTime;
        y += yMoveSpeed * Input.GetAxisRaw("Mouse X") * Time.deltaTime;

        x = ClampAngle(x);

        transform.rotation = Quaternion.Euler(new Vector3(x, y, 0));
    }

    float ClampAngle(float angle)
    {
        if (angle < -360)
        {
            angle += 360;
        }
        if (angle > 360)
        {
            angle -= 360;
        }

        return Mathf.Clamp(angle, -80.0f, 80.0f);
    }
}
