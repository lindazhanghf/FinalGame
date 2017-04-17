using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Transform AI_target;

	// Use this for initialization
	//void Start () {
		
	//}
	
	// Update is called once per frame
	//void Update () {
		
	//}

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag != "Object")
            return;

        //Debug.Log("Collides with object: " + c.impulse.ToString());
        //AI_target.position = new Vector3(c.gameObject.transform.position.x, AI_target.position.y, c.gameObject.transform.position.y);
        //Debug.Log(c.gameObject.transform.position);
        AI_target.SetParent(c.gameObject.transform);
        AI_target.transform.localPosition = new Vector3();

    }
}
