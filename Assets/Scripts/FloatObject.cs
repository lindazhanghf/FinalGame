using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatObject : MonoBehaviour
{
    //GameObject mainCamera;
    public Camera mainCamera;
    bool carrying;
    GameObject carriedObject;
    public float distance;
    public float smooth;

    // Use this for initialization
    void Start()
    {
        //mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        //mainCamera = Camera.main;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (carrying)
        {
            carry(carriedObject);
            checkDrop();
        }
        else
        {
            pickup();
        }
    }

    void carry(GameObject o)
    {
        o.transform.position = Vector3.Lerp(o.transform.position, mainCamera.transform.position + mainCamera.transform.forward * distance, Time.deltaTime * smooth);
        //o.transform.rotation = Quaternion.identity;

    }

    void pickup()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            int x = Screen.width / 2;
            int y = Screen.height / 2;

            Ray ray = mainCamera.ScreenPointToRay(new Vector3(x, y));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //Pickupable p = hit.collider.GetComponent<Pickupable>();
                //if (p != null)
                Debug.Log(hit.collider.tag);
                if (hit.collider.tag.Equals("Object"))
                {
                    carrying = true;
                    carriedObject = hit.collider.gameObject;
                    hit.collider.gameObject.GetComponent<Rigidbody>().useGravity = false;
                }
            }

        }
    }

    void checkDrop()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            dropObject();
        }
    }

    void dropObject()
    {
        carrying = false;
        carriedObject.gameObject.GetComponent<Rigidbody>().useGravity = true;
        carriedObject = null;
    }
}