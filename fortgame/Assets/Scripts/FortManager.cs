using UnityEngine;
using System.Collections;

public class FortManager : MonoBehaviour {

    public GameObject LFortPrefab;
    public GameObject RFortPrefab;
    public GameObject LCharacterPrefab;
    public GameObject RCharacterPrefab;
    public GameObject LScorePrefab;
    public GameObject RScorePrefab;
    


	// Use this for initialization
	void Start () {
        Instantiate(LCharacterPrefab);
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
    
    void FixedUpdate() {
        
    }
}
