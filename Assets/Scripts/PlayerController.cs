using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class PlayerController : MonoBehaviour
{
    public float speed = 1;
    private Rigidbody2D playerRB;
    private float maxRotate;
    public bool alive;
    public bool firstClick = false;
    public static bool shield = false;
    private static bool vulnerable = true;


    //audio
    public AudioClip crashPlane;
    public AudioClip audioPlane;
    public AudioClip powerPlane;
    public AudioSource audioPlayer;

    public AudioClip coinSound;
    public AudioSource coinPlayer;
    private float playerVolume = 0.05f;
    private float coinVolume = 0.1f;
    private float powerVolume = 0.3f;

    public SceneController sceneController;


    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Animator>().SetBool("crashed", false);
        shield = false;
        maxRotate = 0f;
        //scene set
        sceneController = GameObject.Find("GameController").GetComponent<SceneController>();

        //audio
        if (sceneController.GetSoundValue())
        {
            audioPlayer.Stop();
            audioPlayer.volume = playerVolume;
            audioPlayer.clip = audioPlane;

            coinPlayer.Stop();
            coinPlayer.volume = coinVolume;
            coinPlayer.clip = coinSound;

        }

        playerRB = GetComponent<Rigidbody2D>();
        alive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Check if the mouse was clicked over a UI element
                if (!EventSystem.current.IsPointerOverGameObject() || EventSystem.current.currentSelectedGameObject?.gameObject.tag != "Settings")
                {
                    if (Time.timeScale > 0)
                    {
                        if (!firstClick)
                        {
                            firstClick = true;
                            GetComponent<Rigidbody2D>().gravityScale = 0.3f;
                            StartCoroutine(HideClickButton());
                        }
                        if (sceneController.GetSoundValue() && vulnerable)
                        {
                            audioPlayer.Stop();
                            audioPlayer.Play();
                        }
                        playerRB.velocity = Vector2.up * speed;
                    }
                }


            }
            //Rotations
            if (firstClick && Time.timeScale > 0)
            {
                if (maxRotate < 0 && playerRB.velocity.y > 0)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0f);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 0, playerRB.velocity.y * 50f < -85f ? -85f : playerRB.velocity.y * 50f);
                }
                maxRotate = playerRB.velocity.y;
            }
        }
    }

    IEnumerator HideClickButton()
    {
        //a thread that hide click image at 4 sec after first player click
        yield return new WaitForSeconds(4);
        GameObject.Find("ClickImage").SetActive(false);
    }

    //power controller
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Power" && alive)
        {
            //power objects collision
            switch (collision.gameObject.name)
            {
                case "Diamond":
                    coinPlayer.pitch = 0.8f;
                    ScoreController.score = ScoreController.score + 20;
                    break;
                case "Gold":
                    coinPlayer.pitch = 0.9f;
                    ScoreController.score = ScoreController.score + 15;
                    break;
                case "Silver":
                    coinPlayer.pitch = 1.0f;
                    ScoreController.score = ScoreController.score + 10;
                    break;
                case "Bronze":
                    coinPlayer.pitch = 1.1f;
                    ScoreController.score = ScoreController.score + 5;
                    break;
                case "Shield":
                    coinPlayer.pitch = 0.6f;
                    shield = true;
                    sceneController.ToggleShield(true);
                    break;
            }
            collision.gameObject.SetActive(false);
            if (sceneController.GetSoundValue())
            {
                coinPlayer.Play();
            }
        }
        else if (collision.gameObject.tag == "FlyEnemy" || collision.gameObject.tag == "Enemy")
        {
            //enemy collision check
            checkIsVulnerable();
        }
        else if (collision.gameObject.tag == "Floor" && alive)
        {
            //enemy collision with the floor always lose
            loseGame();
        }
        else if (collision.gameObject.tag == "Score" && alive)
        {
            //prevent double score disabling score line
            collision.gameObject.SetActive(false);
            ScoreController.score++;
        }
    }


    private void checkIsVulnerable()
    {
        //if have shield
        if (!shield)
        {
            if (vulnerable && alive)
            {
                loseGame();
            }
        }
        else
        {
            //disable damages
            vulnerable = false;
            //if don't have
            shield = false;
            //hide shiel icon
            sceneController.ToggleShield(false);
            //start count down to reactive damage
            StartCoroutine(invulnerableTime());
            //set animator variable to true for star power animation
            gameObject.GetComponent<Animator>().SetBool("playerPower", true);
        }
    }

    private IEnumerator invulnerableTime()
    {
        // audio
        audioPlayer.Stop();
        audioPlayer.volume = powerVolume;
        audioPlayer.clip = powerPlane;
        if (sceneController.GetSoundValue())
        {
            audioPlayer.Play();
        }

        // count down to reactiave damages
        yield return new WaitForSeconds(5);
        if (!vulnerable)
        {
            //audio
            audioPlayer.Stop();
            audioPlayer.volume = playerVolume;
            audioPlayer.clip = audioPlane;

            vulnerable = true;
            gameObject.GetComponent<Animator>().SetBool("playerPower", false);
        }

    }


    private void loseGame()
    {
        alive = false;
        gameObject.GetComponent<Animator>().SetBool("crashed", true);
        //audio
        audioPlayer.Stop();
        audioPlayer.volume = powerVolume;
        audioPlayer.clip = crashPlane;
        if (sceneController.GetSoundValue())
        {
            audioPlayer.Play();
        }
        alive = false;
        sceneController.Lose();
    }

    public bool GetFirstClick()
    {
        return firstClick;
    }
}
