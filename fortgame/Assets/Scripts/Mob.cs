using UnityEngine;
using System.Collections;

public class Mob : Unit {
    public float width = 10; //width for box collider
    public float height = 10; //height used for stacking
    public float squishiness = 0.5f; //how much it squishes when stacked
    public float walkSpeed = 30.0f;
    public float walkSpeedRandomness = 0.1f; //how random walkspeed is.
    public float xScaleRandomness = 1f;
    [HideInInspector] public int faction = 0;
    public int attackPower = 1; //damage per attack
    public float attackSpeed = 0.5f; //attacks per second
    public int fortDamage = 5;
    private float attackTimer = 0;
    
    [HideInInspector] public Mob targetMob;
    [HideInInspector] public float y = 0;
    [HideInInspector] public float squish = 0;
    private Animator animator;
    //for fun
    private float xScale = 1;

	// Use this for initialization
	public override void UnityStart () {
        //for fun
        walkSpeed = walkSpeed * (1 + walkSpeedRandomness * (Random.value - 0.5f));
        xScale += Random.value * xScaleRandomness;
        width = width * xScale;
        height = height * xScale;

        //set collision box
        box = new OneDBox(transform.position.x - (width / 2), width, 0); 

        animator = GetComponent<Animator>();
	}
    
    public override void UnityFixedUpdate () {
        //motion
        if (targetMob == null)
        {
            box.velocity = walkSpeed * team.direction;
            animator.SetBool("isAttacking", false);
        }
        else
        {
            box.velocity = 0;
            animator.SetBool("isAttacking", true);
        }
        //update collision box
        box.FixedUpdate();
        //adjust sprite transform
        transform.position = new Vector3(box.x + (box.width / 2), y, -y / 1000);
        transform.localScale = new Vector3(xScale * team.direction, (1 - squish) * xScale, 1);
        //attack
        if (attackTimer > 0) {
            attackTimer -= Time.fixedDeltaTime;
        } else {
            attackTimer = 0;
        }

        if (targetMob != null) {
            if (attackTimer == 0) {
                attackTimer = attackSpeed;
                targetMob.hitPoints -= attackPower;
                if (targetMob.hitPoints <= 0) {
                    targetMob = null;
                }
            }
        }
    }
}
