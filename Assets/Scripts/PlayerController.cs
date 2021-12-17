using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 1;
    private Rigidbody2D playerRB;
    private float maxRotate = 0;
    public bool alive;

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
        audioPlayer.Stop();
        audioPlayer.volume = playerVolume;
        audioPlayer.clip = audioPlane;
        audioPlayer.Play();

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
                audioPlayer.Stop();
                audioPlayer.Play();
                //playerRB.velocity = Vector2.zero;
                //playerRB.AddForce(new Vector2(0, 200f));
                playerRB.velocity = Vector2.up * speed;
                maxRotate = 40 - transform.localEulerAngles.z;
            }

            if(maxRotate > 1)
            {
                transform.rotation = Quaternion.Euler(0, 0, transform.localEulerAngles.z + 0.1f);
                maxRotate -= 0.1f;
            }
            else
            {
                maxRotate = 0;
                transform.rotation = Quaternion.Euler(0, 0, playerRB.velocity.y > 0 ? playerRB.velocity.y * 40 : playerRB.velocity.y * 10);
                //print("bajando: " + transform.localEulerAngles.z);
            }

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //audio
        audioPlayer.Stop();
        audioPlayer.clip = crashPlane;
        audioPlayer.Play();

        alive = false;
        sceneController.Lose();
    }
}
