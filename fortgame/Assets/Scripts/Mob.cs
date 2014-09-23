using UnityEngine;
using System.Collections;

public class Mob : MonoBehaviour {

    public float width = 10;
    public float walkSpeed = 20.0f;
    public int faction = 0;
    public int hitPoints = 10;
    public int attackPower = 1; //damage per attack
    public float attackSpeed = 0.5f; //attacks per second
    private float attackTimer = 0;
    
    [HideInInspector] public OneDBox box;
    [HideInInspector] public Mob targetMob;
    private Animator animator;

	// Use this for initialization
	void Start () {
        box = new OneDBox(transform.position.x - (width / 2), width, 0, false); 
        
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
	    if (faction == 1) {
            walkSpeed = -walkSpeed;
            spriteRenderer.color = new Color(1f, 0.847f, 0f);
        }
        else {
            spriteRenderer.color = new Color(0.35937f, 0.847f, 1f);
        }
	}
	
	// Update is called once per frame
	void Update () {

	}
    
    void FixedUpdate () {
        if (targetMob == null)
        {
            box.velocity = walkSpeed;
        }
        else
        {
            box.velocity = 0;
        }
        box.velocity = walkSpeed;

        box.FixedUpdate();
        transform.position = new Vector3(box.x + (width / 2), transform.position.y, transform.position.z);
        
        if (attackTimer > 0) {
            attackTimer--;
        }
        if (attackTimer < 0) {
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
