using UnityEngine;
using System.Collections;

public class Mob : MonoBehaviour {

    public float width = 10; //width for box collider
    public float height = 10; //height used for stacking
    public float squishiness = 0.5f; //how much it squishes when stacked
    public float walkSpeed = 30.0f;
    public string teamName;
    [HideInInspector] public int faction = 0;
    public int hitPoints = 10;
    public int attackPower = 1; //damage per attack
    public float attackSpeed = 0.5f; //attacks per second
    public int fortDamage = 5;
    private float attackTimer = 0;
    
    [HideInInspector] public OneDBox box = new OneDBox(9999999999, 0, 0); //make sure this doesn't collide before start() is called
    [HideInInspector] public Mob targetMob;
    [HideInInspector] public float y = 0;
    [HideInInspector] public float squish = 0;
    private Animator animator;
    private Team team;
    //for fun
    private float xScale = 1;

	// Use this for initialization
	void Start () {
        //Link names
        team = GameObject.Find(teamName);
        GetComponent<SpriteRenderer>().color = team.TeamColor;

        //for fun
        walkSpeed = walkSpeed * Random.value;
        xScale += Random.value * 3;
        width = width * xScale;
        height = height * xScale;

        //set collision box
        box = new OneDBox(transform.position.x - (width / 2), width, 0); 

	}

	// Update is called once per frame
	void Update () {

	}
    
    void FixedUpdate () {
        //update collision box
        box.FixedUpdate();
        //adjust sprite transform
        transform.position = new Vector3(box.x + (width / 2), y, -y / 1000);
        transform.localScale = new Vector3(xScale * team.Direction, (1 - squish) * xScale, 1);
    }
}
