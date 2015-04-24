using UnityEngine;
using System.Collections;

public abstract class Unit : MonoBehaviour {
    [HideInInspector] public Team team;
    public int hitPoints;
    [HideInInspector] public OneDBox box;

    public abstract void UnityStart();
    public abstract void UnityFixedUpdate();
    public abstract void UnityUpdate();

    public AudioClip sound;
    private AudioSource aSource;

    static float maxDelay = .2f;
    static float pitchMax = 2;
    static float pitchMin = .5f;

	// Use this for initialization
	void Start () {
        GetComponent<SpriteRenderer>().color = team.color;
        aSource = gameObject.AddComponent<AudioSource>();
        aSource.clip = sound;
        aSource.loop = false;
        aSource.playOnAwake = false;
        PlaySound();
        UnityStart();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        UnityFixedUpdate();
	}

    void Update()
    {
        UnityUpdate();
    }

    public void PlaySound()
    {
        aSource.PlayDelayed(Random.value * maxDelay);
        aSource.pitch = Random.Range(pitchMin, pitchMax);
    }
}
