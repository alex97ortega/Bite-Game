using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ropa : MonoBehaviour
{
    public Material sudadera, pantalon, vaquero, calzones;
    float objetiveY = 0;
    // Start is called before the first frame update
    void Start()
    {
        int rnd = Random.Range(0, 5);
        switch(rnd)
        {
            case 0:
                GetComponent<MeshRenderer>().material = sudadera;
                break;
            case 1:
                GetComponent<MeshRenderer>().material = pantalon;
                break;
            case 2:
                GetComponent<MeshRenderer>().material = vaquero;
                transform.localScale = new Vector3(0.05f, 2, 1.5f);
                break;
            case 3:
                GetComponent<MeshRenderer>().material = calzones;
                transform.localScale = new Vector3(0.05f, 1, 1.3f);
                break;
            default:
                break;
        }
        transform.eulerAngles = new Vector3(0, Random.Range(0, 100), Random.Range(25, 60));
    }
    public void AddY(float addy)
    {
        objetiveY = 0.7f + addy;
    }
    // Update is called once per frame
    void Update()
    {
        if (objetiveY > 0 && transform.position.y > objetiveY)
            transform.position -= new Vector3(0, 3*Time.deltaTime, 0);
    }
}
