using UnityEngine;
using System.Collections;

public class Poltergeist : Unit {
    public float moveDistance;
    public int floorContribution;

	// Use this for initialization
	public override void UnityStart () {
	    
	}
	
	// Update is called once per frame
	public override void UnityFixedUpdate () {
        Vector3 current = GetComponent<Transform>().position;
        Vector3 target = team.fort.roof.GetComponent<Transform>().position;
        Vector3 moveVector = target - current;
        if (moveVector.magnitude < moveDistance) {
            team.fort.floorProgress += floorContribution;
            hitPoints = 0;
        }
        moveVector.Normalize();
        GetComponent<Transform>().position = current + (moveVector * moveDistance);
	}
}
