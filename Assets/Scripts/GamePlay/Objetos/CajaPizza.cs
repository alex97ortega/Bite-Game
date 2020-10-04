using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CajaPizza : MonoBehaviour
{
    public Transform solapa, borde1, borde2, borde3;

    Queue<Transform> bordes;
    bool finish = false;
    bool aliado = true;

    private void Start()
    {
        bordes = new Queue<Transform>();

        bordes.Enqueue(borde1);
        bordes.Enqueue(borde2);
        bordes.Enqueue(borde3);
    }

    // Update is called once per frame
    void Update()
    {
        if (solapa.eulerAngles.z < 120)
            solapa.eulerAngles += new Vector3(0, 0, 100 * Time.deltaTime);
        else
        {
            if (bordes.Count == 0)
                finish = true;
            else
            {
                if(aliado)
                    bordes.Peek().position += new Vector3(2 * Time.deltaTime, 2 * Time.deltaTime, 0);
                else
                    bordes.Peek().position += new Vector3(-2 * Time.deltaTime, 2 * Time.deltaTime, 0);

                if (bordes.Peek().position.y > 2.6f)
                {
                    Destroy(bordes.Dequeue().gameObject);
                }
            }
        }
    }
    public bool Finished() { return finish; }
    public void SetEnemigo() { aliado = false; }
}
