using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class SceneController : MonoBehaviour
{
    public GameObject loseGO;
    public GameObject settingsGO;
    public GameObject scoreGO;
    //private GameObject scoreGO;
    //private GameObject recordTextGO;
    public AudioClip audioGame;
    public AudioClip audioLose;
    public AudioSource music;
    public float musicVolume;
    public GameObject player;
    private GameObject playerInstace;
    private bool musicOn = true;
    private bool soundOn = true;

    private Color transp = new Color(255, 255, 255, 0.5f);
    private Color solid = new Color(255, 255, 255, 1);




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

        settingsGO.transform.Find("PauseButton").gameObject.SetActive(true);

        //SetButtonAlpha("MusicButton", true);

        ScoreController.score = 0;
        playerInstace = Instantiate(player, new Vector3(-0.40f, 0, 0), Quaternion.identity);
        loseGO.SetActive(false);
        scoreGO.transform.Find("ClickImage").gameObject.SetActive(true);
        Time.timeScale = 1;
        music.Stop();
        music.clip = audioGame;
        if (musicOn)
        {
            music.Play();
        }
    }

    public void Lose()
    {
        settingsGO.transform.Find("PauseButton").gameObject.SetActive(false);
        music.Stop();
        music.clip = audioLose;
        if (musicOn)
        {
            music.Play();
        }
        if (ScoreController.score > PlayerPrefs.GetInt("record", 0))
        {
            PlayerPrefs.SetInt("record", ScoreController.score);
        }
        loseGO.SetActive(true);
        loseGO.transform.Find("PlayButton").gameObject.SetActive(false);
        loseGO.transform.Find("ResetButton").gameObject.SetActive(true);
        // Get child nested parent, parsing to GO
        loseGO.transform.Find("GameOver").gameObject.SetActive(true);
        GameObject.Find("RecordText").GetComponent<TMPro.TextMeshProUGUI>().text = "RECORD: " + PlayerPrefs.GetInt("record", 0).ToString();
        Time.timeScale = 0;

    }

    public void Reset()
    {
        removeEnemies();
        removePowers();
        Destroy(playerInstace);
        RunGame();
    }

    private void removeEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
            GameObject.Destroy(enemy);
    }

    private void removePowers()
    {
        GameObject[] powers = GameObject.FindGameObjectsWithTag("Power");
        foreach (GameObject power in powers)
            GameObject.Destroy(power);
    }

    public void DeleteRecord()
    {
        PlayerPrefs.DeleteKey("record");
        string recordLabel = GameObject.Find("RecordText").GetComponent<TMPro.TextMeshProUGUI>().text;
        recordLabel = "RECORD: 0";
    }


    public void ToggleMusic()
    {
        if (musicOn)
        {
            musicOn = false;
            music.Stop();
            SetButtonAlpha("MusicButton", false);
        }
        else
        {
            musicOn = true;
            music.Play();
            SetButtonAlpha("MusicButton", true);
        }
    }

    private void SetButtonAlpha(string name, bool value)
    {
        settingsGO.transform.Find(name).gameObject.GetComponent<Image>().color = value ? solid : transp;
    } 

    public bool GetSoundValue()
    {
        return soundOn;
    }

    public void ToggleSound()
    {
        soundOn = !soundOn;
        SetButtonAlpha("SoundButton", soundOn);
    }

    public void PauseGame()
    {
        if(Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }
}
