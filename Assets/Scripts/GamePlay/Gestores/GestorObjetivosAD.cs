using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestorObjetivosAD : MonoBehaviour
{
    public ObjetivoAD [] botonesObjetivos;
    public GestorPartida gestorPartida;

    private bool firstTime = true;
    private List<ObjetivoAD> objetivosEnemigos;
    private List<ObjetivoAD> objetivosAliados;

    private void Awake()
    {
        objetivosAliados = new List<ObjetivoAD>();
        objetivosEnemigos = new List<ObjetivoAD>();
    }

    public void ActualizaEnemigos(bool objetivosEnemis)
    {
        if (firstTime)
            CreaBotonesEnemigos();
        if(objetivosEnemis)
        {
            foreach (var x in objetivosEnemigos)
            {
                x.gameObject.SetActive(true);
                x.ActualizaEnemigo();
            }
            foreach (var x in objetivosAliados)
                x.gameObject.SetActive(false);
        }
        else
        {
            foreach (var x in objetivosAliados)
            {
                x.gameObject.SetActive(true);
                x.ActualizaEnemigo();
            }
            foreach (var x in objetivosEnemigos)
                x.gameObject.SetActive(false);
        }
    }

    private void CreaBotonesEnemigos()
    {
        int numPlayers = gestorPartida.GetAllEnemigos().Length;

        // enemigos
        for(int i = 0; i < numPlayers; i++)
        {
            ObjetivoAD objAux = botonesObjetivos[i];
            objAux.SetProperties(gestorPartida.GetAllEnemigos()[i], gestorPartida.GetAllEnemigos()[i].foto);
            float relation = 1920.0f / (float)Screen.width;
            objAux.transform.position = transform.position + new Vector3(-150 / relation + 150 * i/relation, 0, 0);
            objetivosEnemigos.Add(objAux);
        }
        //aliados
        for (int i = 0; i < numPlayers; i++)
        {
            ObjetivoAD objAux = botonesObjetivos[i+3];
            objAux.SetProperties(gestorPartida.GetAllAliados()[i], gestorPartida.GetAllAliados()[i].foto);
            float relation = 1920.0f / (float)Screen.width;
            objAux.transform.position = transform.position + new Vector3(-150 / relation + 150 * i / relation, 0, 0);
            objetivosAliados.Add(objAux);
        }

        firstTime = false;
    }
}
