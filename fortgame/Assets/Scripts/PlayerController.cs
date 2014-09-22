using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private float walkSpeed = 50.0f;
    public string walkAxis = "Horizontal";
    
    private Vector3 velocity = new Vector3(0.0f,0.0f,0.0f);
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () { 
        //input
        float walkDir = Input.GetAxis(walkAxis);
        //physics
        velocity = new Vector3(walkDir * walkSpeed, 0, 0);
        //animation
		if (walkDir < 0) {
            transform.localScale = new Vector3 (-1, 1, 1);
		} 
        else if (walkDir > 0) {
			transform.localScale = new Vector3(1, 1, 1);
		}
	}

	//Fixed Update is called at a fixed timestep
	void FixedUpdate () {
		transform.position += velocity * Time.fixedDeltaTime;
	}
}
