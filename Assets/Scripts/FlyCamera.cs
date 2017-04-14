using UnityEngine;
using System.Collections;
//using UnityStandardAssets.CrossPlatformInput;

public class FlyCamera : MonoBehaviour
{

    /*
    Writen by Windexglow 11-13-10.  Use it, edit it, steal it I don't care.  
    Converted to C# 27-02-13 - no credit wanted.
    Simple flycam I made, since I couldn't find any others made public.  
    Made simple to use (drag and drop, done) for regular keyboard layout  
    wasd : basic movement
    shift : Makes camera accelerate
    space : Moves camera on X and Z axis only.  So camera doesn't gain any height*/

    public float mainSpeed = 15f; //100.0f; //regular speed
    float shiftAdd = 0f; //250.0f; //multiplied by how long shift is held.  Basically running
    float maxShift = 0f; //1000.0f; //Maximum speed when holdin gshift
    public float camSens = 0.2f; //0.25f; //How sensitive it with mouse
    private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
    private float totalRun = 1.0f;

    public Camera cam;
    //public UnityStandardAssets.Characters.FirstPerson.MouseLook mouseLook = new UnityStandardAssets.Characters.FirstPerson.MouseLook();

    void Start()
    {
        // Hide mouse cursor
        //mouseLook.Init(transform, cam.transform);
        Cursor.visible = false;
    }

    void Update()
    {
        //RotateView();
        //Debug.Log(Input.mousePosition);

        lastMouse = Input.mousePosition - lastMouse;
        lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0);
        lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x, transform.eulerAngles.y + lastMouse.y, 0);
        transform.eulerAngles = lastMouse;
        lastMouse = Input.mousePosition;
        //Mouse  camera angle done.  

        //Keyboard commands (NOTE: disabled running)
        Vector3 p = GetBaseInput();
        //if (Input.GetKey(KeyCode.LeftShift))
        //{
        //    totalRun += Time.deltaTime;
        //    p = p * totalRun * shiftAdd;
        //    p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
        //    p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
        //    p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
        //}
        //else
        //{
            totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
            p = p * mainSpeed;
        //}

        p = p * Time.deltaTime;
        Vector3 newPosition = transform.position;
        if (Input.GetKey(KeyCode.Space))
        { //If player wants to move on X and Z axis only
            Debug.Log("Lock Y");
            transform.Translate(p);
            newPosition.x = transform.position.x;
            newPosition.z = transform.position.z;
            transform.position = newPosition;
        }
        else
        {
            transform.Translate(p);
        }

    }

    private void RotateView()
    {
        //avoids the mouse looking if the game is effectively paused
        if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

        // get the rotation before it's changed
        float oldYRotation = transform.eulerAngles.y;

        //mouseLook.LookRotation(transform, cam.transform);

        //if (m_IsGrounded || advancedSettings.airControl)
        //{
        //    // Rotate the rigidbody velocity to match the new direction that the character is looking
        //    Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, Vector3.up);
        //    m_RigidBody.velocity = velRotation * m_RigidBody.velocity;
        //}
    }


    private Vector3 GetBaseInput()
    { //returns the basic values, if it's 0 than it's not active.
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            p_Velocity += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            p_Velocity += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            p_Velocity += new Vector3(1, 0, 0);
        }
        return p_Velocity;
    }
}
