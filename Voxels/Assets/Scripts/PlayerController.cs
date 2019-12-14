using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float mouseSen = 1;
    public Transform cameraRot;
    public float minRot = -90;
    public float maxRot = 90;
    public float moveSpeed = 5;
    public float jumpStrength = 5;
    public bool climbSlobes = false;
    public float climbingSpeed = 8f;

    Rigidbody rigid;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rigid = GetComponent<Rigidbody>();
    }

    float _rotX;
    float _rotY;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            Cursor.lockState = CursorLockMode.Locked;
        if (Input.GetKeyDown(KeyCode.Escape))
            Cursor.lockState = CursorLockMode.None;

        if (Cursor.lockState == CursorLockMode.None)
            return;

        _rotX -= Input.GetAxis("Mouse Y") * mouseSen;
        _rotX = Mathf.Clamp(_rotX, minRot, maxRot);
        _rotY -= Input.GetAxis("Mouse X") * mouseSen * -1;
        cameraRot.localEulerAngles = new Vector3(_rotX, _rotY);
    }

    public void ClimbSlobe()
    {
        if (climbSlobes)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                transform.Translate(0, climbingSpeed * Time.deltaTime, 0);
                rigid.velocity = new Vector3(0, 0, 0);
            }
        }
    }

    private void FixedUpdate()
    {
        if (Cursor.lockState != CursorLockMode.None)
        {
            Vector3 currentRot = transform.localEulerAngles;
            transform.localEulerAngles = new Vector3(currentRot.x, cameraRot.localEulerAngles.y, currentRot.z);
            Vector3 Trans = Vector3.zero;
            if (Input.GetKey(KeyCode.W))
                Trans = new Vector3(Trans.x, 0, moveSpeed);
            if (Input.GetKey(KeyCode.A))
                Trans = new Vector3(-moveSpeed, 0, Trans.z);
            if (Input.GetKey(KeyCode.S))
                Trans = new Vector3(Trans.x, 0, -moveSpeed);
            if (Input.GetKey(KeyCode.D))
                Trans = new Vector3(moveSpeed, 0, Trans.z);
            transform.Translate(Trans * Time.fixedDeltaTime);
            transform.localEulerAngles = currentRot;
        }

        //if ()
        //    transform.Translate(0, 1, 0);

        //if (Input.GetKey(KeyCode.Space))
        //    if (rigid.velocity.y == 0)
        //        rigid.velocity = new Vector3(rigid.velocity.x, jumpStrength, rigid.velocity.z);

        if (transform.position.y < 1)
        {
            transform.position = new Vector3(transform.position.x, 1, transform.position.z);
            rigid.velocity = new Vector3(0, 0, 0);
        }

        rigid.velocity = new Vector3(0, rigid.velocity.y, 0);
    }
}
