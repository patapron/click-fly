using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorController : MonoBehaviour
{
    public float timeToGenerate;
    private float timeInit = 0;
    public GameObject tube;
    public float hight;
    // Start is called before the first frame update
    void Start()
    {
        GameObject newTube = Instantiate(tube);
        newTube.transform.position = transform.position + new Vector3(0, 0, 0);
        Destroy(newTube, 4);
    }

    // Update is called once per frame
    void Update()
    {
        if(timeInit > timeToGenerate)
        {
            GameObject newTube = Instantiate(tube);
            newTube.transform.position = transform.position + new Vector3(0, Random.Range(-hight, hight), 0);
            Destroy(newTube, 4);
            timeInit = 0;
        }
        else
        {
            timeInit += Time.deltaTime;
        }
    }
}
