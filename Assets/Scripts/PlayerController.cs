using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameObject AI;
    public UnityStandardAssets.Characters.ThirdPerson.AIController AI_controller;
    public Transform AI_target;

    // Use this for initialization
    void Start()
    {
        AI_controller = AI.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AIController>();
    }

    // Update is called once per frame
    //void Update () {

    //}

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag != "Object")
            return;
        //if (c.impulse.x == 0 && c.impulse.y == 0 && c.impulse.z == 0) // Collision too light (relativeVelocity)
        //    return;

        //float scare_factor = c.relativeVelocity.magnitude;
        //AI_controller.change_scare_level(scare_factor);
        //Debug.Log("Collides with " + c.gameObject.name + scare_factor.ToString());
        Debug.Log("Player collides");
        AI_controller.collision_scared(c);
        AI_controller.play_audio();

        AI_target.SetParent(c.gameObject.transform);
        AI_target.transform.localPosition = new Vector3();
    }
}
