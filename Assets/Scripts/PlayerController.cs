using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	// Use this for initialization
	//void Start () {
		
	//}
	
	// Update is called once per frame
	//void Update () {
		
	//}

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "Object")
        {
            Debug.Log("Collides with object: " + c.impulse.ToString());
        }
        
    }
}
