using UnityEngine;
using System.Collections.Generic;

public class Cauldron : MonoBehaviour {

    public float width = 20;
    int floor = 1;
    Resource mob;
    [HideInInspector]
    OneDBox box;

	// Use this for initialization
	void Start () {
        box = new OneDBox(transform.position.x - (width / 2), width, 0);
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void Destroy()
    {
        Object.Destroy(this.gameObject);
    }
}
