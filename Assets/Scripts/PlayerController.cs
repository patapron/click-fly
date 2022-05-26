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


    //audio
    public AudioClip crashPlane;
    public AudioClip audioPlane;
    public AudioSource audioPlayer;

    public AudioClip coinSound;
    public AudioSource coinPlayer;
    public float playerVolume;

    public SceneController sceneController;

    // Start is called before the first frame update
    void Start()
    {
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
                if (EventSystem.current.IsPointerOverGameObject() && EventSystem.current.currentSelectedGameObject?.gameObject.tag == "Settings")
                {

                }
                else
                {
                    if (Time.timeScale > 0)
                    {
                        if (!firstClick)
                        {
                            firstClick = true;
                            GetComponent<Rigidbody2D>().gravityScale = 0.3f;
                            StartCoroutine(HideClickButton());
                        }
                        if (sceneController.GetSoundValue())
                        {
                            audioPlayer.Stop();
                            audioPlayer.Play();
                        }
                        //playerRB.velocity = Vector2.zero;
                        //playerRB.AddForce(new Vector2(0, 200f));
                        playerRB.velocity = Vector2.up * speed;
                        //maxRotate = 40 - transform.localEulerAngles.z;
                    }
                }


            }
            //Rotations
            if (firstClick && Time.timeScale > 0)
            {
                //Debug.Log("primero rot: " + transform.rotation);
                //Debug.Log("primero localEulerAngles: " + transform.localEulerAngles.z);
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
        if (collision.gameObject.tag == "Power")
        {
            //score point collision
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
            coinPlayer.Play();
        }
        else if (collision.gameObject.tag == "FlyEnemy")
        {
            //enemy collision
            loseGame();
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        loseGame();
    }

    private void loseGame()
    {
        //audio
        if (sceneController.GetSoundValue())
        {
            audioPlayer.Stop();
            audioPlayer.clip = crashPlane;
            audioPlayer.Play();
        }
        alive = false;
        sceneController.Lose();
    }

    public bool GetFirstClick()
    {
        return firstClick;
    }

    private void SetPlayerAlpha(string name, bool value)
    {
        //gameObject.GetComponent<Image>().color = value ? solid : transp;
    }
}
