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
    private PlayerController Player;
    //Text display objects
    private GUIText Score;
	// Use this for initialization
	void Start () {
        fort = FortObj.GetComponent<Fort>();
        Player = PlayerObj.GetComponent<PlayerController>();
        Score = ScoreObj.GetComponent<GUIText>();
	}

    // Update is called once per frame
    void Update () {
        Score.text = "HP: " + fort.GetActualHP();
    }

    public void SpawnMobs () {
        Transform t = fort.GetComponentInParent<Transform>();
        GameObject g = (GameObject)Instantiate(enemyPrefab, t.position, new Quaternion(0, 0, 0, 0));
        Mob m = g.GetComponent<Mob>();
        mobs.Add(m);
    }

    public void CollideWithFort (Fort f) {
        foreach (Mob m in fort.mobs)
        {
            if (m.box.overlap(f.box))
            {
                f.actualHP -= m.fortDamage;
                m.hitPoints = 0;
            }
        }
    }
}
