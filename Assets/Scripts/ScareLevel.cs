using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScareLevel : MonoBehaviour {

    public float scare_level = 0f;
    public float speed_rise = 0.02f;
    public float speed_drop = 0.01f;
    private Slider self;

    // Use this for initialization
    void Start()
    {
        self = GetComponent<Slider>();
    }
    // Update is called once per frame
    void Update () {
        if (self.value + 0.001 < scare_level)
        {
            self.value += speed_rise;
        }
        else if (self.value - 0.001 > scare_level)
        {
            self.value -= speed_drop;
        }
	}

    public void update_scare_level(float new_scare_level)
    {
        Debug.Log(new_scare_level);
        scare_level = new_scare_level;
    }
}
