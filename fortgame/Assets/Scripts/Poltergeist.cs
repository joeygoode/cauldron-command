using UnityEngine;
using System.Collections;

public class Poltergeist : Unit {
    public float moveSpeed = 10;
    public float launchSpeed = 10;
    public int floorContribution = 1;
    public int healthBoost = 3;
    public float maxRotPerSec = 3;
    public float velocityInfluenceTime = 2;
    private float rotationSpeed = 0;
    private float rotation = 0;
    private float velocityInfluenceTimer = 2;

    public float roofTargetOffset = 100;

    public Vector3 velocity = new Vector3(0, 0, 0);

	// Use this for initialization
	public override void UnityStart () {
        rotationSpeed = (Random.value * 2 - 1) * maxRotPerSec;
        velocityInfluenceTimer = velocityInfluenceTime;
        velocity = new Vector3((Random.value * 2 - 1) * launchSpeed, Random.value * launchSpeed, 0);
	}

    public override void UnityFixedUpdate()
    {

    }

	// Update is called once per frame
    public override void UnityUpdate()
    {
        //timer
        velocityInfluenceTimer -= Time.deltaTime;
        if (velocityInfluenceTimer < 0)
        {
            velocityInfluenceTimer = 0;
        }

        //get positions
        Vector3 current = GetComponent<Transform>().position;
        Vector3 target = team.fort.roof.GetComponent<Transform>().position;
        target = new Vector3(target.x, target.y + roofTargetOffset, target.z);
        Vector3 moveVector = target - current;

        //die if we reached the target
        if (moveVector.magnitude < moveSpeed * 2)
        {
            team.fort.floorProgress += floorContribution;
            team.fort.hitpoints += healthBoost;
            hitPoints = 0;
        }
        //move
        moveVector.Normalize();
        transform.position = current + moveVector * moveSpeed + velocity * (velocityInfluenceTimer / velocityInfluenceTime);
        //rotate
        rotation += rotationSpeed * Time.deltaTime * 360;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation));
	}
}
