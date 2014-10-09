﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {
    // Game instances
    public Team left;
    public Team right;
    public GameObject resourcePrefab;
    public float resourceSpawnRate = 2;
    public float resourceSpawnVariance = 1;
    public int maxFreeResources = 30;

    private bool gameRunning = true;
    private float resourceSpawnCountdown;
    private List<Resource> resources = new List<Resource>();
	// Use this for initialization
	void Start () {
        ResetResourceCountdown();
        left.player.game = this;
        right.player.game = this;
	}
	
	// Update is called once per frame
	void Update () {
        float startButton = Input.GetAxisRaw("Start-Pause"); //has to be raw to work while paused
        if (startButton > 0.0)
        {
            Time.timeScale = 0;
            gameRunning = false;
        } 
        else if (left.fort.IsDead() || right.fort.IsDead()) {
            Time.timeScale = 0;
            gameRunning = false;
        }
        else
        {
            Time.timeScale = 1;
            gameRunning = true;
        }

        float resetButton = Input.GetAxisRaw("Reset");
        if (resetButton > 0.0) {
            Time.timeScale = 1;
            Application.LoadLevel("mainmenu");
        }

	}

    void ResetResourceCountdown () {
        resourceSpawnCountdown = Random.Range(resourceSpawnRate - resourceSpawnVariance,
                                              resourceSpawnRate + resourceSpawnVariance);
    }

    void SpawnResource () {
        float x = Random.Range(left.fort.box.x, right.fort.box.x);
        GameObject g = (GameObject)Instantiate(resourcePrefab, new Vector3(x, 0, 0), new Quaternion(0, 0, 0, 0));
        Resource r = g.GetComponent<Resource>();
        r.box.x = x;
        resources.Add(r);
    }

    public void RemoveResource (PlayerController p) {
        Resource removedResource = null;
        foreach (Resource r in resources) {
            if (r.box.overlap(p.box)) {
                p.ReceiveResource(r);
                removedResource = r;
                break;
            }
        }
        if (removedResource != null) {
            resources.Remove(removedResource);
        }
    }

    public void AddResource (Resource r) {
        resources.Add(r);
    }

    void FixedUpdate () {
        if (gameRunning) {

            left.CollideWithFort(right.fort);
            right.CollideWithFort(left.fort);

            //collide mobs with each other
            foreach (Mob m1 in left.mobs)
            {
                foreach (Mob m2 in right.mobs)
                {
                    if (m1.box.overlap(m2.box))
                    {
                        m1.targetMob = m2;
                        m2.targetMob = m1;
                        if (m1.box.velocity == 0)
                        {
                            m2.box.x = m1.box.x + m1.box.width;
                        }
                        if (m2.box.velocity == 0)
                        {
                            m1.box.x = m2.box.x - m1.box.width;
                        }
                    }
                }
            }
            //spawn resources
            if (resources.Count > maxFreeResources) {
            } else if (resourceSpawnCountdown < 0) {
                SpawnResource();
                ResetResourceCountdown();
            } else {
                resourceSpawnCountdown -= Time.fixedDeltaTime;
            }
        }
    }
}
