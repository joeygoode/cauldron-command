using UnityEngine;
using System.Collections;

enum PlayerState {
    Idle,
    PickUp,
    Carry,
    PutDown
}

public class PlayerController : MonoBehaviour {

    public float walkSpeed = 100.0f;
    private float width = 10;

    public string walkAxis = "P1Horizontal";
    public string digButton = "P1Dig";
    public float pickUpDuration = 0.2f;
    public float putDownDuration = 0.1f;

    [HideInInspector]
    public OneDBox box;
    [HideInInspector] 
    public Game game;
    [HideInInspector]
    public Team team;

    private Animator animator;
    private float animationTimer = 0.0f;
    private Resource heldResource;


    // Use this for initialization
    void Start () {
        box = new OneDBox(transform.position.x - (width / 2), width, 0); 
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {
        //update timers
        animationTimer -= Time.deltaTime;
        if (animationTimer < 0)
        {
            animationTimer = 0;
        }

        //check pick-up and put-down
        if (Input.GetAxis(digButton) > 0 && animationTimer == 0)
        {
            if (heldResource == null) {
                if (game.RemoveResource(this))
                {
                    animationTimer = pickUpDuration;
                    animator.SetBool("isLifting", true); //do you even lift bro?
                }
            }
            else
            {
                heldResource.Drop();
                game.AddResource(heldResource);
                heldResource = null;
                animationTimer = putDownDuration;
            }
        }

        //check walk if we're not animating
        if (animationTimer == 0)
        {
            animator.SetBool("isLifting", false);
            //input
            float walkDir = Input.GetAxis(walkAxis);
            //physics
            OneDBox next = new OneDBox(box.x, box.width, walkDir * walkSpeed);
            if ( game.IsValidMove(team,next) )
            {
                box.velocity = next.velocity;
            }
            else
            {
                box.velocity = 0;
            }
            //animation
            if (walkDir != 0)
            {
                animator.SetBool("isWalking", true);
                if (walkDir < 0)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                } else if (walkDir > 0)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
            } else
            {
                animator.SetBool("isWalking", false);
            }
        }
        else
        {
            box.velocity = 0;
            animator.SetBool("isWalking", false);
        }

        //final animator information
        animator.SetFloat("animTimer", animationTimer);
        animator.SetBool("isHolding", heldResource != null);
    }

    public void ReceiveResource(Resource r) {
        heldResource = r;
        heldResource.Pickup();
    }

    //Fixed Update is called at a fixed timestep
    void FixedUpdate () {
        box.FixedUpdate();
        transform.position = new Vector3(box.x + (width / 2), transform.position.y, transform.position.z);
        if (heldResource != null) {
            heldResource.box.x = box.x;
        }
    }
}
