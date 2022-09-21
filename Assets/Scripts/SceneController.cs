using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;



public class SceneController : MonoBehaviour
{
    public GameObject loseGO;
    public GameObject settingsGO;
    public GameObject scoreGO;
    public GameObject TutorialGO;
    public GameObject CreditsGO;

    public GameObject generatorController;

    public AudioClip audioGame;
    public AudioClip audioLose;
    public AudioSource music;
    public float musicVolume;
    public GameObject player;
    private GameObject playerInstace;
    private bool musicOn;
    private bool soundOn;

    private Color transp = new Color(255, 255, 255, 0.5f);
    private Color solid = new Color(255, 255, 255, 1);

    public GameObject resetButton;
    public GameObject pauseButton;
    public GameObject clickImage;
    public GameObject shieldButton;
    public GameObject playButton;
    public GameObject tutorialButton;
    public GameObject creditsButton;

    public GameObject recordText;

    public GameObject gameOver;



    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;

        // music and sound settings
        musicOn = !PlayerPrefs.HasKey("music") || PlayerPrefs.GetInt("music") == 1;
        soundOn = !PlayerPrefs.HasKey("sound") || PlayerPrefs.GetInt("sound") == 1;
        SetButtonAlpha("MusicButton", musicOn);
        SetButtonAlpha("SoundButton", soundOn);

        music.clip = audioLose;
        if (musicOn)
        {
            music.Play();
        }

        // score setting
        recordText.GetComponent<TMPro.TextMeshProUGUI>().text = "RECORD: " + PlayerPrefs.GetInt("record", 0).ToString();

        // buttons settings
        //resetButton = loseGO.transform.Find("ResetButton").gameObject;
        //pauseButton = settingsGO.transform.Find("PauseButton").gameObject;
        //clickImage = scoreGO.transform.Find("ClickImage").gameObject;
        //shieldButton = settingsGO.transform.Find("ShieldButton").gameObject;
        //playButton = loseGO.transform.Find("PlayButton").gameObject;
        //recordLabel = GameObject.Find("RecordText").GetComponent<TMPro.TextMeshProUGUI>().text;

    }

    public void RunGame()
    {
        // instant score reset
        ScoreController.score = 0;

        // buttons reset
        pauseButton.SetActive(true);
        resetButton.SetActive(false);
        tutorialButton.SetActive(false);
        creditsButton.SetActive(false);

        playerInstace = Instantiate(player, new Vector3(-0.40f, 0, 0), Quaternion.identity);
        loseGO.SetActive(false);
        clickImage.SetActive(true);

        // set the music play
        music.Stop();
        music.clip = audioGame;
        if (musicOn)
        {
            music.Play();
        }

        // start game
        Time.timeScale = 1;
    }

    public void ToggleShield(bool value)
    {
        shieldButton.SetActive(value);
    }

    public void Lose()
    {
        pauseButton.SetActive(false);
        tutorialButton.SetActive(true);
        creditsButton.SetActive(true);

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
        playButton.SetActive(false);
        // Get child nested parent, parsing to GO
        loseGO.transform.Find("GameOver").gameObject.SetActive(true);
        recordText.GetComponent<TMPro.TextMeshProUGUI>().text = "RECORD: " + PlayerPrefs.GetInt("record", 0).ToString();
        StartCoroutine(stopTimeScale());

    }

    private IEnumerator stopTimeScale()
    {
        yield return new WaitForSeconds(1.5f);
        Time.timeScale = 0;
        resetButton.SetActive(true);
    }

    public void Reset()
    {
        generatorController.GetComponent<GeneratorController>().initGenerator();
        removeEnemies();
        removePowers();
        Destroy(playerInstace);
        RunGame();
    }

    private void removeEnemies()
    {
        GameObject[] normalEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] flyEnemies = GameObject.FindGameObjectsWithTag("FlyEnemy");

        var enemies = normalEnemies.Concat(flyEnemies).ToArray();
        foreach (GameObject enemy in enemies)
        {
            GameObject.Destroy(enemy);
        }
    }

    private void removePowers()
    {
        GameObject[] powers = GameObject.FindGameObjectsWithTag("Power");
        foreach (GameObject power in powers)
        {
            GameObject.Destroy(power);
        }
    }


    public void DeleteRecord()
    {
        PlayerPrefs.DeleteKey("record");
        recordText.GetComponent<TMPro.TextMeshProUGUI>().text = "RECORD: 0";
    }


    public void ToggleMusic()
    {
        if (musicOn)
        {
            musicOn = false;
            music.Stop();
            SetButtonAlpha("MusicButton", false);
            PlayerPrefs.SetInt("music", 0);
        }
        else
        {
            musicOn = true;
            music.Play();
            SetButtonAlpha("MusicButton", true);
            PlayerPrefs.SetInt("music", 1);
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
        PlayerPrefs.SetInt("sound", soundOn ? 1 : 0);
    }

    public void PauseGame()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }

    public void ShowTutorial()
    {
        settingsGO.SetActive(false);
        loseGO.SetActive(false);
        scoreGO.SetActive(false);
        TutorialGO.SetActive(true);
    }

    public void ShowCredits()
    {
        settingsGO.SetActive(false);
        loseGO.SetActive(false);
        scoreGO.SetActive(false);
        CreditsGO.SetActive(true);
    }

    public void ExitCanvas()
    {
        TutorialGO.SetActive(false);
        CreditsGO.SetActive(false);

        settingsGO.SetActive(true);
        loseGO.SetActive(true);
        scoreGO.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
        //if (UnityEditor.EditorApplication.isPlaying)
        //{
        //    UnityEditor.EditorApplication.isPlaying = false;
        //}
        //else
        //{
        //    Application.Quit();
        //}
    }
}
