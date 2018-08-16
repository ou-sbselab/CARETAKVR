using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveButton : MonoBehaviour {

    public GameObject fire;
    
    private Animation animation;

	// Use this for initialization
	void Start () {
        animation = GetComponent<Animation>();
    }

	// Update is called once per frame
	void Update () {
	}

    public void ToggleFire()
    {
        animation.Play("StovePress");
        fire.SetActive(!fire.activeSelf);
    }
}
