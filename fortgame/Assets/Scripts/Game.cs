using UnityEngine;
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

    private bool GameRunning = false;
    private float ResourceSpawnCountdown;
    private List<Resource> resources = new List<Resource>();
	// Use this for initialization
	void Start () {
        ResetResourceCountdown();
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
