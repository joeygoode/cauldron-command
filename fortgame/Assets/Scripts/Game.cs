﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
                Left.mobs = new List<Mob>();
                Right.mobs = new List<Mob>();
                Menu.GetComponent<SpriteRenderer>().enabled = true;
            }
            Left.SpawnMobs();
            Right.SpawnMobs();
        } else {
	        //Input
            float s = Input.GetAxis("Start");
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
            Left.RemoveDead();
            Right.RemoveDead();
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
