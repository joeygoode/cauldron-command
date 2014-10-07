﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {
    // Game instances
    public GameObject Menu;
    public Team Left;
    public Team Right;
    public GameObject ResourcePrefab;
    public float ResourceSpawnRate;
    public float ResourceSpawnVariance;
    public int MaxFreeResources;

    private bool GameRunning = true;
    private float ResourceSpawnCountdown;
    private List<Resource> resources = new List<Resource>();
	// Use this for initialization
	void Start () {
        ResetResourceCountdown();
	}
	
	// Update is called once per frame
	void Update () {
        float startButton = Input.GetAxis("Start");
        if (startButton > 0.0)
        {
            GameRunning = false;
        } 
        else if (Left.fort.IsDead() || Right.fort.IsDead()) {
            GameRunning = false;
        }
        else
        {
            GameRunning = true;
        }

        float resetButton = Input.GetAxis("Reset");
        if (resetButton > 0.0) {
            Application.LoadLevel("mainmenu");
        }

        if (GameRunning) {
            Left.SpawnMobs();
            Right.SpawnMobs();
        }

	}

    void ResetResourceCountdown () {
        ResourceSpawnCountdown = Random.Range(ResourceSpawnRate - ResourceSpawnVariance,
                                              ResourceSpawnRate + ResourceSpawnVariance);
    }

    void SpawnResource () {
        float x = Random.Range(Left.fort.box.x, Right.fort.box.x);
        GameObject g = (GameObject)Instantiate(ResourcePrefab, new Vector3(x, 0, 0), new Quaternion(0, 0, 0, 0));
        Resource r = g.GetComponent<Resource>();
        r.box.x = x;
        resources.Add(r);
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
            if (resources.Count > MaxFreeResources) {
            } else if (ResourceSpawnCountdown < 0) {
                SpawnResource();
                ResetResourceCountdown();
            } else {
                ResourceSpawnCountdown -= Time.fixedDeltaTime;
            }
        }
    }
}
