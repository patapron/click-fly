using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorController : MonoBehaviour
{
    public float timeToGenerate;
    private float timeInit = 0;
    public GameObject[] powerUps;
    public GameObject tube;
    public float hight;
    private GameObject newPowerUp;
    private bool firstClick = false;

    // Start is called before the first frame update
    void Start()
    {
        //GameObject newTube = Instantiate(tube);
        //newTube.transform.position = transform.position + new Vector3(0, 0, 0);
        //Destroy(newTube, 4);
    }

    // Update is called once per frame
    void Update()
    {
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
                //instantiate power
                //Debug.Log("instaciar: " + prefabSelected.name);
                newPowerUp = Instantiate(prefabSelected);
                //position power
                newPowerUp.transform.position = transform.position + new Vector3(0, Random.Range(-hight, hight), 0);
                //destroy power at 4.5 seconds if exists
                Destroy(newPowerUp, 4.5f);
            }
            //instaciate new tube prefab
            GameObject newTube = Instantiate(tube);
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
