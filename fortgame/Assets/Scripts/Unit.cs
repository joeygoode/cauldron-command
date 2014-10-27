using UnityEngine;
using System.Collections;

public abstract class Unit : MonoBehaviour {
    [HideInInspector] public Team team;
    public int hitPoints;
    [HideInInspector] public OneDBox box;

    public abstract void UnityStart();
    public abstract void UnityFixedUpdate();

	// Use this for initialization
	void Start () {
        GetComponent<SpriteRenderer>().color = team.color;
        UnityStart();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        UnityFixedUpdate();
	}
}
