using UnityEngine;
using System.Collections;

public class Team : MonoBehaviour {
    //strings for getting the game objects placed in the scene
    public string FortName;
    public string PlayerName;
    public string ScoreName;
    //GameObjects in the scene
    private GameObject FortObj;
    private GameObject PlayerObj;
    private GameObject ScoreObj;
    //Fort script objects
    private Fort Fort;
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
        Score.text = "HP: " + Fort.hitpoints;
    }

    public Fort GetFort () {
        return Fort;
    }
}
