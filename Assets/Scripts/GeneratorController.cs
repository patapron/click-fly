using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorController : MonoBehaviour
{
    private float timeToGenerate;
    private float timeInit;
    public GameObject[] powerUps;
    public GameObject tube;
    public float hight;
    private GameObject newPowerUp;
    private bool firstClick = false;

    private float counter;
    public float speed;
    private float incrementSpeed;
    private float timeWhenChange;

    // Start is called before the first frame update
    void Start()
    {
        initGenerator();
    }

    public void initGenerator()
    {
        // speed of the elements
        speed = 0.68f;

        // counter to generate other power, sec
        timeInit = 0;

        //to generate another element, sec
        timeToGenerate = 1.6f;

        // counter speed dificult, sec
        counter = 0;

        //increment speed dificult interval, sec
        timeWhenChange = 2;

        //increment dificult
        incrementSpeed = 0.007f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 1)
        {

            counter += Time.deltaTime;
            if (!firstClick && GameObject.Find("Player(Clone)") != null)
            {
                firstClick = GameObject.Find("Player(Clone)").GetComponent<PlayerController>().GetFirstClick();
            }

            if (firstClick && timeInit > timeToGenerate)
            {

                if (!newPowerUp || newPowerUp == null)
                {
                    //get random power prefab
                    GameObject prefabSelected = powerUps[Random.Range(0, powerUps.Length)];

                    while (prefabSelected.name == "Shield" && PlayerController.shield)
                    {
                        prefabSelected = powerUps[Random.Range(0, powerUps.Length)];
                    }
                    //instantiate power
                    newPowerUp = Instantiate(prefabSelected);
                    newPowerUp.GetComponent<TubeController>().speed = counter > timeWhenChange ? speed + incrementSpeed : speed;

                    //position power
                    newPowerUp.transform.position = transform.position + new Vector3(0, Random.Range(-hight, hight), 0);
                    //destroy power at 4.5 seconds if exists
                    Destroy(newPowerUp, 4.5f);
                }
                //instaciate new tube prefab
                GameObject newTube = Instantiate(tube);
                newTube.GetComponent<TubeController>().speed = counter > timeWhenChange ? speed + incrementSpeed : speed;
                if (counter > timeWhenChange)
                {
                    timeToGenerate -= incrementSpeed;
                    counter = 0;
                    speed += incrementSpeed / 2;
                }




                //positionate tube
                newTube.transform.position = transform.position + new Vector3(0, Random.Range(-hight, hight), 0);
                //destro tuve at 4 if exists
                Destroy(newTube, 4);
                //reset time
                timeInit = 0;
            }
            else
            {
                timeInit += Time.deltaTime;
            }
        }
    }
}
