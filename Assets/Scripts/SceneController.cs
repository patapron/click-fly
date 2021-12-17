using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour
{
    public GameObject loseGO;
    //private GameObject scoreGO;
    //private GameObject recordTextGO;
    public AudioClip audioGame;
    public AudioClip audioLose;
    public AudioSource music;
    public float musicVolume;
    public GameObject player;
    private GameObject playerInstace;



    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        music.clip = audioLose;
        music.Play();
        GameObject.Find("RecordText").GetComponent<TMPro.TextMeshProUGUI>().text = "RECORD: " + PlayerPrefs.GetInt("record", 0).ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void RunGame()
    {
        ScoreController.score = 0;
        playerInstace = Instantiate(player, new Vector3(-0.40f, 0, 0), Quaternion.identity);
        loseGO.SetActive(false);
        Time.timeScale = 1;
        music.Stop();
        music.clip = audioGame;
        music.Play();
    }

    public void Lose()
    {
        music.Stop();
        music.clip = audioLose;
        music.Play();
        if (ScoreController.score > PlayerPrefs.GetInt("record", 0))
        {
            PlayerPrefs.SetInt("record", ScoreController.score);
        }
        loseGO.SetActive(true);
        // Get child nested parent, parsing to GO
        loseGO.transform.Find("GameOver").gameObject.SetActive(true);
        GameObject.Find("RecordText").GetComponent<TMPro.TextMeshProUGUI>().text = "RECORD: " + PlayerPrefs.GetInt("record", 0).ToString();
        Time.timeScale = 0;

    }

    public void Reset()
    {
        removeEnemies();
        Destroy(playerInstace);
        RunGame();
    }

    private void removeEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
            GameObject.Destroy(enemy);
    }

    public void DeleteRecord()
    {
        PlayerPrefs.DeleteKey("record");
        string recordLabel = GameObject.Find("RecordText").GetComponent<TMPro.TextMeshProUGUI>().text;
        recordLabel = "RECORD: 0";
    }
}
