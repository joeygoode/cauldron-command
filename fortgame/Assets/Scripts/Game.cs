using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {
    // Game instances
    public GameObject Menu;
    public Team Left;
    public Team Right;
    private bool GameRunning = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (GameRunning) {
            if (Left.fort.IsDead() || Right.fort.IsDead()) {
                GameRunning = false;
                Menu.GetComponent<SpriteRenderer>().enabled = true;
            }
            Left.SpawnMobs();
            Right.SpawnMobs();
        } else {
	        //Input
            float s = Input.GetAxis("start");
            if (s > 0.0) {
                Left.fort.ResetHP();
                Right.fort.ResetHP();
                Menu.GetComponent<SpriteRenderer>().enabled = false;
                GameRunning = true;
            }
        }
	}

    void FixedUpdate () {
        if (GameRunning) {
            Left.CollideWithFort(Right.fort);
            Right.CollideWithFort(Left.fort);
            //collide mobs with each other
            foreach (Mob m1 in Left.mobs)
            {
                foreach (Mob m2 in Right.mobs)
                {
                    if (m1.box.overlap(m2.box))
                    {
                        m1.targetMob = m2;
                        m2.targetMob = m1;
                    }
                }
            }
        }
    }
}
