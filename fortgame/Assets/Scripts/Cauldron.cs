using UnityEngine;
using System.Collections.Generic;

public class Cauldron : MonoBehaviour {

    public float width = 20;
    public int floor = 1;
    [HideInInspector]
    public Resource resource;
    [HideInInspector]
    public bool hasResource = false;
    [HideInInspector]
    public OneDBox box;

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

    public void recieveResource(Resource resource)
    {
        this.resource = resource;
        hasResource = true;
    }
}
