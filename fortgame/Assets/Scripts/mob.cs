using UnityEngine;
using System.Collections;

public class mob : MonoBehaviour {

    public float walkSpeed = 20.0f;
    public int faction = 0;
    public int hitPoints = 10;
    public int attackPower = 1; //damage per "tick"
    private int tickRate = 5; //FixedUpdates per "tick"
    private int tickIncrement = 0;
    
    private Vector3 velocity = new Vector3(0.0f,0.0f,0.0f);
    private mob targetMob = null;

	// Use this for initialization
	void Start () {
	    if (faction == 1) {
            walkSpeed = -walkSpeed;
        }
	}
	
	// Update is called once per frame
	void Update () {
	    velocity = new Vector3(walkSpeed, 0, 0);
	}
    
    void FixedUpdate () {
        transform.position += velocity * Time.fixedDeltaTime;
        if (tickIncrement > 0) {
            tickIncrement--;
        }
    }
}
