using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Team : MonoBehaviour {
    //strings for getting the game objects placed in the scene
    public Color Color;
    public int Direction;
    // Ugly, need a better way to do this
    public GameObject enemyPrefab;

    public List<Mob> mobs = new List<Mob>();
    //GameObjects in the scene
    public GameObject FortObj;
    public GameObject PlayerObj;
    public GameObject ScoreObj;
    //Fort script objects
    [HideInInspector] public Fort fort;
    //Character script objects
    public PlayerController Player;
    //Text display objects
    private GUIText Score;
    private int LastMob = 0;
	// Use this for initialization
	void Start () {
        fort = FortObj.GetComponent<Fort>();
        Player = PlayerObj.GetComponent<PlayerController>();
        Score = ScoreObj.GetComponent<GUIText>();
	}

    // Update is called once per frame
    void Update () {
        Score.text = "HP: " + fort.hitpoints;
    }

    // Fixed Update is called 
    void FixedUpdate()
    {
        StackMobs();

    }

    private void StackMobs()
    {
        //collide mobs with themselves, and
        // stack them in an animated way based on their position relative to their width/height

        //reset stacking vars
        foreach (Mob m in mobs)
        {
            m.squish = 0;
            m.y = 0;
        }

        //convert to array to iterate over
        Mob[] mobArr = mobs.ToArray();
        //collide every mob with every mob (except itself)
        //and only do it if there actually is more than 1 mob
        if (mobArr.Length > 1)
        {
            for (int i = 0; i < mobArr.Length - 1; i++)
            {
                for (int j = i + 1; j < mobArr.Length; j++)
                {
                    if (mobArr[i].box.overlap(mobArr[j].box))
                    {
                        //calculate ratio for squishiness/height boost
                        bool iThin = true;
                        float thinner = mobArr[i].width;
                        if (mobArr[j].width < thinner)
                        {
                            thinner = mobArr[j].width;
                            iThin = false;
                        }
                        float overlapRatio = mobArr[i].box.overlapAmount(mobArr[j].box) / thinner;
                        //thinner mob goes on top
                        Mob thinMob;
                        Mob thickMob;
                        if (iThin)
                        {
                            thinMob = mobArr[i];
                            thickMob = mobArr[j];
                        }
                        else
                        {
                            thinMob = mobArr[j];
                            thickMob = mobArr[i];
                        }
                        //set squishiness only if we are the "heaviest" thing squishing the mob.
                        //"heaviest" meaning "thing that's squishing it the most"
                        if (overlapRatio > mobArr[i].squish)
                        {
                            thickMob.squish = overlapRatio * thickMob.squishiness;
                        }
                        thinMob.y += overlapRatio * thickMob.height * (1 - thickMob.squishiness);
                    }
                }
            }
        }
    }

    public void SpawnMobs () {
        if (LastMob == 0)
        {
            Transform t = fort.GetComponentInParent<Transform>();
            GameObject g = (GameObject)Instantiate(enemyPrefab, t.position, new Quaternion(0, 0, 0, 0));
            Mob m = g.GetComponent<Mob>();
            m.team = this;
            mobs.Add(m);
            LastMob = (int)(Random.value) % 60 + 100;
        } else {
            LastMob--;
        }
    }

    public void CollideWithFort (Fort f) {
        foreach (Mob m in mobs)
        {
            if (m.box.overlap(f.box))
            {
                f.hitpoints -= m.fortDamage;
                m.hitPoints = 0;
            }
        }
    }

    public void RemoveDead () {
        //destroy any mobs with 0 hitpoints
        List<Mob> deadMobBuffer = new List<Mob>();
        foreach (Mob m in mobs)
        {
            if(m.hitPoints <= 0)
            {
                deadMobBuffer.Add(m);
            }
        }
        foreach (Mob m in deadMobBuffer)
        {
            mobs.Remove(m);
            Destroy(m.gameObject);
        }
    }
}
