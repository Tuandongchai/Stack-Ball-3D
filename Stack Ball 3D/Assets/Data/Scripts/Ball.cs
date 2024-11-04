using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    private static Ball instance;
    public static Ball Instance { get =>instance; }

    private Rigidbody rb;
    private float currentTime;

    private bool smash, invincible;

    private int currentBrokenStacks, totalStacks;

    [SerializeField] protected GameObject invicnbleObj;
    [SerializeField] protected Image invincibleFill;
    [SerializeField] protected GameObject fireEffect, winEffect, splashEffect;

    public enum BallState
    {
        Prepare,
        Playing,
        Died,
        Finish
    }
   
    [HideInInspector] protected BallState ballStateCurrent = BallState.Prepare;
    public BallState BallStateCurrent { get => ballStateCurrent; }


    public AudioClip bounceOffClip, deadClip, winClip, destoryClip, iDestroyClip;
    private void Awake()
    {
        Ball.instance = this;
        rb = GetComponent<Rigidbody>();
        currentBrokenStacks = 0;
    }
    private void Start()
    {
        /*totalStacks = FindObjectOfType<StackController>().Length;*/
        totalStacks = FindObjectOfType<Rotator>().transform.childCount;
    }
    private void Update()
    {
        if(ballStateCurrent == BallState.Playing)
        {
            if (InputManager.Instance.MouseButtonDown) smash = true;
            if (InputManager.Instance.MouseButtonUp) smash = false;

            if (invincible)
            {
                currentTime -= Time.deltaTime * 0.35f;
                if(!fireEffect.activeInHierarchy)
                {
                    fireEffect.SetActive(true);
                }
            }
            else
            {
                if(fireEffect.activeInHierarchy)
                {
                    fireEffect.SetActive(false);
                }
                if (smash) currentTime += Time.deltaTime * 0.8f;
                else currentTime -= Time.deltaTime * 0.5f;
            }
            if(currentTime>=0.3f || invincibleFill.color == Color.red)
            {
                invicnbleObj.SetActive(true);
            }
            else invicnbleObj.SetActive(false) ;

            if (currentTime >= 1)
            {
                currentTime = 1;
                invincible = true;
                invincibleFill.color = Color.red;
            }
            else if (currentTime <= 0)
            {
                currentTime = 0;
                invincible = false;
                invincibleFill.color = Color.white;
            }
            if (invicnbleObj.activeInHierarchy)
                invincibleFill.fillAmount = currentTime / 1;
        }
        /*if (ballState==BallState.Prepare)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ballState = BallState.Playing;
            }
        }*/
        if (ballStateCurrent==BallState.Finish)
        {
            if (InputManager.Instance.MouseButtonDown)
                FindObjectOfType<LevelSpawner>().NextLevel();
        }
        if(GameUI.Instance.InGame)  this.ballStateCurrent=BallState.Playing;
    }
    private void FixedUpdate()
    {
        if (ballStateCurrent == BallState.Playing)
        {
            if (InputManager.Instance.MouseButton)
            {
                smash = true;
                rb.velocity = new Vector3(0, -100 * Time.fixedDeltaTime * 7, 0);
            }
        }
        
        if (rb.velocity.y > 5)
            rb.velocity = new Vector3(rb.velocity.x, 5, rb.velocity.z);
    }
    public void IncreaseBrokenStacks()
    {
        currentBrokenStacks++;
        if(!invincible)
        {
            ScoreManager.Instance.AddScore(1);
            SoundManager.Instance.PlaySoundFX(destoryClip, 0.5f);
        }
        else
        {
            ScoreManager.Instance.AddScore(2);
            SoundManager.Instance.PlaySoundFX(bounceOffClip, 0.5f);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!smash)
        {
            rb.velocity = new Vector3(0, 50 * Time.deltaTime * 5, 0);
            if(collision.gameObject.tag != "Finish")
            {
                GameObject splash = Instantiate(splashEffect);
                splash.transform.SetParent(collision.transform);
                splash.transform.localEulerAngles = new Vector3(90, Random.Range(0, 359), 0);
                float randomScale = Random.Range(0.18f, 0.25f);
                splash.transform.localScale = new Vector3(randomScale, randomScale, 1);
                splash.transform.position = new Vector3(transform.position.x, transform.position.y - 0.22f, transform.position.z);
                splash.GetComponent<SpriteRenderer>().color = transform.GetChild(0).GetComponent<MeshRenderer>().material.color;
            }

            SoundManager.Instance.PlaySoundFX(bounceOffClip, 0.5f);
        }
        else
        {
            if(invincible)
            {
                if(collision.gameObject.tag =="enemy"|| collision.gameObject.tag == "plane")
                {
                    collision.transform.parent.GetComponent<StackController>().ShatterAllParts();
                }
            }
            else
            {
                if (collision.gameObject.tag == "enemy")
                {
                    collision.transform.parent.GetComponent<StackController>().ShatterAllParts();
                }
                if (collision.gameObject.tag == "plane")
                {
                    rb.isKinematic = true;
                    transform.GetChild(0).gameObject.SetActive(false);
                    ballStateCurrent = BallState.Died;
                    SoundManager.Instance.PlaySoundFX(deadClip, 0.5f);
                }
            }
            
        }

        FindObjectOfType<GameUI>().LevelSliderFill(currentBrokenStacks/ (float)totalStacks);

        if(collision.gameObject.tag=="Finish" && ballStateCurrent == BallState.Playing)
        {
            ballStateCurrent = BallState.Finish;
            SoundManager.Instance.PlaySoundFX(winClip, 0.7f);
            GameObject win = Instantiate(winEffect);
            win.transform.SetParent(Camera.main.transform);
            win.transform.localPosition = Vector3.up *1.5f;
            win.transform.eulerAngles = Vector3.zero;
        }

    }
    private void OnCollisionStay(Collision collision)
    {
        if (!smash || collision.gameObject.tag =="Finish")
        {
            rb.velocity = new Vector3(0, 50* Time.deltaTime * 5, 0);
        }
    }
}
