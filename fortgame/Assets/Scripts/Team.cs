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
        Score.text = "HP: " + fort.actualHP;
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
                f.actualHP -= m.fortDamage;
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
