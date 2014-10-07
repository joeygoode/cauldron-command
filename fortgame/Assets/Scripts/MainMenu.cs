using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float s = Input.GetAxis("Start-Pause");
        if (s > 0.0)
        {
            Application.LoadLevel("game");
        }
	}
}
