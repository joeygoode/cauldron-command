﻿using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {
    // String names for modification in the editor
    public string MenuName;
    public string LeftTeamName;
    public string RightTeamName;

    // Game instances
    private GameObject Menu;
    private Team Left;
    private Team Right;
    private bool GameRunning;

	// Use this for initialization
	void Start () {
        Menu = GameObject.Find(MenuName);
        Left = GameObject.Find(LeftTeamName).GetComponent<Team>();
        Right = GameObject.Find(RightTeamName).GetComponent<Team>();
	}
	
	// Update is called once per frame
	void Update () {
        if (GameRunning) {
            if (Left.GetFort().IsDead() || Right.GetFort().IsDead()) {
                GameRunning = false;
                Menu.GetComponent<SpriteRenderer>().enabled = true;
            }
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
