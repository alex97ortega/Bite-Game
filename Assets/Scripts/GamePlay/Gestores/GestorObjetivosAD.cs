using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestorObjetivosAD : MonoBehaviour
{
    public ObjetivoAD botonEnemyPrefab;
    public GestorPartida gestorPartida;

    private bool firstTime = true;

    public void ActualizaEnemigos()
    {
        if (firstTime)
            CreaBotonesEnemigos();
        foreach (var x in FindObjectsOfType<ObjetivoAD>())
            x.ActualizaEnemigo();
    }

    private void CreaBotonesEnemigos()
    {
        int numEnemigos = gestorPartida.GetAllEnemigos().Length;

        for(int i = 0; i < numEnemigos; i++)
        {
            ObjetivoAD objAux = Instantiate(botonEnemyPrefab);
            objAux.SetProperties(gestorPartida.GetAllEnemigos()[i], gestorPartida.GetAllEnemigos()[i].foto);
            objAux.transform.SetParent(transform);
            objAux.transform.position = transform.position + new Vector3(-150 +150 * i, 0, 0);
        }

        firstTime = false;
    }
}
