using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartasMagic : MonoBehaviour
{
    public List<Transform> cartas;
    public AudioSource golpe;
    Stack<Transform> pilaCartas;
    Queue<Transform> listaCartasLanzadas;

    float avanzado= 1, velX = 0, velZ = 0;
    float objetivoX = 0, objetivoZ = 0;
    bool empiezaLanzamiento = false;
    
    public void SetObjetivo(float objX, float objZ)
    {
        pilaCartas = new Stack<Transform>();
        listaCartasLanzadas = new Queue<Transform>();
        foreach (var c in cartas)
        {
            pilaCartas.Push(c);
        }
        objetivoX = objX;
        objetivoZ = objZ;
        velX = objX - pilaCartas.Peek().position.x;
        velZ = objZ - pilaCartas.Peek().position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if(empiezaLanzamiento)
        {
            avanzado += Time.deltaTime;
            // lanzar 1 carta mas
            if(avanzado > 0.5f)
            {
                if(pilaCartas.Count>0 )
                {
                    avanzado = 0;
                    Transform aux = pilaCartas.Pop();
                    aux.eulerAngles -= new Vector3(0, 0, 90);
                    listaCartasLanzadas.Enqueue(aux);
                }
            }
            // mover las cartas que ya hayan sido lanzadas y destruirlas si llegan al objetivo
            if(listaCartasLanzadas.Count>0)
            {
                foreach(var c in listaCartasLanzadas)
                    c.position += new Vector3(velX * Time.deltaTime, 0, velZ * Time.deltaTime);

                bool llegadoX, llegadoZ;
                if (velX > 0)
                    llegadoX = listaCartasLanzadas.Peek().position.x >= objetivoX;
                else
                    llegadoX = listaCartasLanzadas.Peek().position.x <= objetivoX;

                if (velZ > 0)
                    llegadoZ = listaCartasLanzadas.Peek().position.z >= objetivoZ;
                else
                    llegadoZ = listaCartasLanzadas.Peek().position.z <= objetivoZ;

                if (llegadoX && llegadoZ)
                {
                    Transform aux = listaCartasLanzadas.Dequeue();
                    Destroy(aux.gameObject);
                    golpe.Play();
                }
            }
        }
    }

    public bool LanzadasTodasLasCartas()
    {
        empiezaLanzamiento = true;
        return pilaCartas.Count == 0 && listaCartasLanzadas.Count == 0;
    }
}
