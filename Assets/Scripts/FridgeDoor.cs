using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FridgeDoor : MonoBehaviour {


    public Animation anim;
    private bool open;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animation>();
        open = false;
	}
	
    public void PlayAnimation()
    {
        anim.Play(open ? "DoorClose" : "DoorOpen");
        open = !open;
    }

	// Update is called once per frame
	void Update () {
		
	}
}
