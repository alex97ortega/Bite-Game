using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestorAcciones : MonoBehaviour
{
    public GestorPartida gestorPartida;
    public Terreno tablero;
    public Camara camara;
    public GameObject menuAcciones;
    public Material aliadoAzul, aliadoAmarillo;

    int movimientosEsteTurno;

    public void PreparaTurno()
    {
        tablero.RestauraTablero();
        movimientosEsteTurno = 0;

        // enemigo
        if (gestorPartida.GetTurno() >= gestorPartida.GetNumPersonajesPorEquipo())
        {
            menuAcciones.SetActive(false);
            PasarTurno(); // sin ia, quitar
        }
        //aliado
        else
        {
            menuAcciones.SetActive(true);
            gestorPartida.GetPersonajeTurno().SetColor(aliadoAmarillo);
            tablero.PintaCasillasAmarillas(gestorPartida.GetPersonajeTurno().GetCasillaX(), 
                                           gestorPartida.GetPersonajeTurno().GetCasillaZ(),
                                           gestorPartida.GetPersonajeTurno().movimientos);
        }
    }

    public void PasarTurno()
    {
        if(gestorPartida.GetTurno() < gestorPartida.GetNumPersonajesPorEquipo())
            gestorPartida.GetPersonajeTurno().SetColor(aliadoAzul);
        gestorPartida.SiguienteTurno();
        camara.RestauraCamara();
        PreparaTurno();
    }

    public void MueveIzq()
    {
        MueveCasilla(-1, 0);
    }
    public void MueveDch()
    {
        MueveCasilla(1, 0);
    }
    public void MueveArriba()
    {
        MueveCasilla(0, 1);
    }
    public void MueveAbajo()
    {
        MueveCasilla(0, -1);
    }

    private void MueveCasilla(int x, int z)
    {
        int movs = gestorPartida.GetPersonajeTurno().movimientos;
        if (movimientosEsteTurno == movs)
            return;

        int oldPosX = gestorPartida.GetPersonajeTurno().GetCasillaX();
        int oldPosZ = gestorPartida.GetPersonajeTurno().GetCasillaZ();
        int newPosX = oldPosX + x;
        int newPosZ = oldPosZ + z;

        if (tablero.PuedeMoverACasilla(newPosX, newPosZ))
        {
            movimientosEsteTurno++;
            gestorPartida.GetPersonajeTurno().SetPos(newPosX, newPosZ);

            tablero.GetCasilla(oldPosX, oldPosZ).Desocupar();
            tablero.GetCasilla(newPosX, newPosZ).Ocupar();
            tablero.RestauraTablero();
            tablero.PintaCasillasAmarillas(newPosX, newPosZ, movs- movimientosEsteTurno);
        }
    }

    public void AtaqueCuerpo()
    {
        menuAcciones.SetActive(false);
        bool isAliado = gestorPartida.GetTurno() < gestorPartida.GetNumPersonajesPorEquipo();
        camara.EnfocaCamaraAC(gestorPartida.GetPersonajeTurno().transform.position, isAliado);
    }
    public void AtaqueDistancia()
    {
        menuAcciones.SetActive(false);
        bool isAliado = gestorPartida.GetTurno() < gestorPartida.GetNumPersonajesPorEquipo();
        camara.EnfocaCamaraAD(gestorPartida.GetPersonajeTurno().transform.position, isAliado);
    }
    public void AtaqueEspecial()
    {
        menuAcciones.SetActive(false);
        bool isAliado = gestorPartida.GetTurno() < gestorPartida.GetNumPersonajesPorEquipo();
        camara.EnfocaCamaraAE(gestorPartida.GetPersonajeTurno().transform.position, isAliado);
    }
}
