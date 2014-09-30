using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Team : MonoBehaviour {
    //strings for getting the game objects placed in the scene
    public string FortName;
    public string PlayerName;
    public string ScoreName;
    public Color Color;
    public int Direction;

    public List<Mob> mobs = new List<Mob>();
    //GameObjects in the scene
    private GameObject FortObj;
    private GameObject PlayerObj;
    private GameObject ScoreObj;
    //Fort script objects
    [HideInInspector] public Fort fort;
    //Character script objects
    private PlayerController Player;
    //Text display objects
    private GUIText Score;
	// Use this for initialization
	void Start () {
        FortObj = GameObject.Find(FortName);
        PlayerObj = GameObject.Find(PlayerName);
        ScoreObj = GameObject.Find(ScoreName);
        
        Fort = FortObj.GetComponent<Fort>();
        Player = PlayerObj.GetComponent<PlayerController>();
        Score = ScoreObj.GetComponent<GUIText>();
	}
	
    // Update is called once per frame
    void Update () {
        Score.text = "HP: " + Fort.GetActualHP();
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
