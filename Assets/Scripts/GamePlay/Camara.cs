using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camara : MonoBehaviour
{
    public bool camaraLibre;
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
            transform.position += new Vector3(-3, 3.0f, -3f);
            transform.eulerAngles = new Vector3(20, 50, 0);
        }
        else
        {
            transform.position += new Vector3(3, 3.0f, -3f);
            transform.eulerAngles = new Vector3(20, -50, 0);
        }
    }
    public void EnfocaCamaraAE(Vector3 playerPos, bool isAliado)
    {
        transform.position = playerPos;
        if (isAliado)
        {
            transform.position += new Vector3(-3, 3, 0);
            transform.eulerAngles = new Vector3(15, 90, 0);
        }
        else
        {
            transform.position += new Vector3(3, 3, 0);
            transform.eulerAngles = new Vector3(15, -90, 0);
        }
    }
    public void EnfocaCamaraAE2(Vector3 playerPos, bool isAliado)
    {
        transform.position = playerPos;
        if (isAliado)
        {
            transform.position += new Vector3(-3.5f, 3, -1);
            transform.eulerAngles = new Vector3(15, 75, 0);
        }
        else
        {
            transform.position += new Vector3(3.5f, 3, -1);
            transform.eulerAngles = new Vector3(15, -75, 0);
        }
    }
    public void EnfocaCamaraAMDani(Vector3 playerPos, bool isAliado)
    {
        transform.position = playerPos;
        if (isAliado)
        {
            transform.position += new Vector3(-3, 4, -3f);
            transform.eulerAngles = new Vector3(20, 50, 0);
        }
        else
        {
            transform.position += new Vector3(3, 4, -3f);
            transform.eulerAngles = new Vector3(20, -50, 0);
        }
    }
    public void EnfocaCamaraAMAsier(Vector3 playerPos, bool isAliado)
    {
        transform.position = playerPos;
        if (isAliado)
        {
            transform.position += new Vector3(1, 5, -3f);
            transform.eulerAngles = new Vector3(10, 300, 0);
        }
        else
        {
            transform.position += new Vector3(-1, 5, -3f);
            transform.eulerAngles = new Vector3(10, 60, 0);
        }
    }

    private void Update()
    {
        if(camaraLibre)
        {
            //translation
            var x = Input.GetAxis("Horizontal") * Time.deltaTime * 5;
            var y = Input.GetAxis("Vertical") * Time.deltaTime * 5;
            transform.position += new Vector3(x, 0, y);
            if (Input.GetKey(KeyCode.M))
                transform.position += new Vector3(0, 5 * Time.deltaTime, 0);
            else if (Input.GetKey(KeyCode.N))
                transform.position -= new Vector3(0, 5 * Time.deltaTime, 0);
            //rotation
            else if (Input.GetKey(KeyCode.Alpha1))
                transform.eulerAngles += new Vector3(10 * Time.deltaTime, 0, 0);
            else if (Input.GetKey(KeyCode.Alpha2))
                transform.eulerAngles -= new Vector3(10 * Time.deltaTime, 0, 0);
            else if (Input.GetKey(KeyCode.Alpha3))
                transform.eulerAngles += new Vector3(0, 10 * Time.deltaTime, 0);
            else if (Input.GetKey(KeyCode.Alpha4))
                transform.eulerAngles -= new Vector3(0, 10 * Time.deltaTime, 0);
            else if (Input.GetKey(KeyCode.Alpha5))
                transform.eulerAngles += new Vector3(0, 0, 10 * Time.deltaTime);
            else if (Input.GetKey(KeyCode.Alpha6))
                transform.eulerAngles -= new Vector3(0, 0, 10 * Time.deltaTime);
        }
    }
}
