using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class PlayerController : MonoBehaviour
{
    public float speed = 1;
    private Rigidbody2D playerRB;
    private float maxRotate = 0;
    public bool alive;
    public bool firstClick = false;

    //audio
    public AudioClip crashPlane;
    public AudioClip audioPlane;
    public AudioSource audioPlayer;
    public float playerVolume;

    public SceneController sceneController;

    // Start is called before the first frame update
    void Start()
    {
        //scene set
        sceneController = GameObject.Find("GameController").GetComponent<SceneController>();

        //audio
        if (sceneController.GetSoundValue())
        {
            audioPlayer.Stop();
            audioPlayer.volume = playerVolume;
            audioPlayer.clip = audioPlane;
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
                    maxRotate = 40 - transform.localEulerAngles.z;
                }


            }

            if (firstClick && maxRotate > 1)
            {
                transform.rotation = Quaternion.Euler(0, 0, transform.localEulerAngles.z + 0.1f);
                maxRotate -= 0.1f;
            }
            else if (firstClick && maxRotate <= 1)
            {
                maxRotate = 0;
                transform.rotation = Quaternion.Euler(0, 0, playerRB.velocity.y > 0 ? playerRB.velocity.y * 40 : playerRB.velocity.y * 10);
                //print("bajando: " + transform.localEulerAngles.z);
            }
        }
    }

    IEnumerator HideClickButton()
    {
        yield return new WaitForSeconds(4);
        GameObject.Find("ClickImage").SetActive(false);
    }


    private void OnCollisionEnter2D(Collision2D collision)
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


}
