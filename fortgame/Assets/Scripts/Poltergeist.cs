using UnityEngine;
using System.Collections;

public class Poltergeist : Unit {
    public float moveSpeed = 10;
    public int floorContribution = 1;
    public int healthBoost = 3;

    public float roofTargetOffset = -40;

	// Use this for initialization
	public override void UnityStart () {
	    
	}
	
	// Update is called once per frame
	public override void UnityFixedUpdate () {
        //figure out move direction
        Vector3 current = GetComponent<Transform>().position;
        Vector3 target = team.fort.roof.GetComponent<Transform>().position;
        target = new Vector3(target.x, target.y + roofTargetOffset, target.z);
        Vector3 moveVector = target - current;
        //die if we reached the target
        if (moveVector.magnitude < moveSpeed)
        {
            team.fort.floorProgress += floorContribution;
            team.fort.hitpoints += healthBoost;
            hitPoints = 0;
        }
        //move
        moveVector.Normalize();
        GetComponent<Transform>().position = current + (moveVector * moveSpeed);
	}
}
