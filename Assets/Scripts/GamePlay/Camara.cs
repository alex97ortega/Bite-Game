using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camara : MonoBehaviour
{
    Vector3 initialPos;
    Vector3 initialRot;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;
        initialRot = transform.eulerAngles;
    }

    public void RestauraCamara()
    {
        transform.position = initialPos;
        transform.eulerAngles = initialRot;
    }

    public void EnfocaCamaraAC(Vector3 playerPos, bool isAliado)
    {
        transform.position = playerPos;
        if (isAliado)
        {
            transform.position += new Vector3(-2, 4, -4.5f);
        }
        else
        {
            transform.position += new Vector3(3, 4, -4.5f);
        }
    }
    public void EnfocaCamaraAD(Vector3 playerPos, bool isAliado)
    {
        transform.position = playerPos;
        if (isAliado)
        {
            transform.position += new Vector3(-4, 4.5f, -3f);
            transform.eulerAngles = new Vector3(25, 50, 0);
        }
        else
        {
            transform.position += new Vector3(3, 4.5f, -2.5f);
            transform.eulerAngles = new Vector3(25, -50, 0);
        }
    }
    public void EnfocaCamaraAE(Vector3 playerPos, bool isAliado)
    {
        transform.position = playerPos;
        if (isAliado)
        {
            transform.position += new Vector3(-5, 3, 0);
            transform.eulerAngles = new Vector3(15, 90, 0);
        }
        else
        {
            transform.position += new Vector3(5, 3, 0);
            transform.eulerAngles = new Vector3(15, -90, 0);
        }
    }
}
