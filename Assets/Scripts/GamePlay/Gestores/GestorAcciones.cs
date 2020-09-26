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
    bool lanzaAnimacionAM = false;
    bool lanzaAnimacionAD = false;
    bool lanzaAnimacionAE = false;
    Personaje objetivo;

    public void PreparaTurno()
    {
        tablero.RestauraTablero();
        movimientosEsteTurno = 0;
        objetivo = null;

        // enemigo
        if (!gestorPartida.GetPersonajeTurno().IsAliado())
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
        if(gestorPartida.GetPersonajeTurno().IsAliado())
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
            tablero.GetCasilla(newPosX, newPosZ).Ocupar(gestorPartida.GetPersonajeTurno());
            tablero.RestauraTablero();
            tablero.PintaCasillasAmarillas(newPosX, newPosZ, movs- movimientosEsteTurno);
        }
    }

    public void AtaqueCuerpo()
    {
        // primero ver que hay un objetivo delante
        int casillObjX;
        int casillaObjZ = gestorPartida.GetPersonajeTurno().GetCasillaZ();

        if (gestorPartida.GetPersonajeTurno().IsAliado())
        {
            casillObjX = gestorPartida.GetPersonajeTurno().GetCasillaX() - 1;
        }
        else
        {
            casillObjX = gestorPartida.GetPersonajeTurno().GetCasillaX() + 1;
        }
        if (!tablero.GetCasilla(casillObjX, casillaObjZ).EstaOcupada())
            return;

        objetivo = tablero.GetCasilla(casillObjX, casillaObjZ).GetPersonajeCasilla();
        menuAcciones.SetActive(false);

        if(gestorPartida.GetPersonajeTurno().nombre == "Dani")
            camara.EnfocaCamaraAD(gestorPartida.GetPersonajeTurno().transform.position, gestorPartida.GetPersonajeTurno().IsAliado());
        else
            camara.EnfocaCamaraAC(gestorPartida.GetPersonajeTurno().transform.position, gestorPartida.GetPersonajeTurno().IsAliado());

        lanzaAnimacionAM = true;
    }
    public void AtaqueDistancia()
    {
        menuAcciones.SetActive(false);
        camara.EnfocaCamaraAD(gestorPartida.GetPersonajeTurno().transform.position, gestorPartida.GetPersonajeTurno().IsAliado());

        lanzaAnimacionAD = true;
    }
    public void AtaqueEspecial()
    {
        menuAcciones.SetActive(false);
        bool isAliado = gestorPartida.GetTurno() < gestorPartida.GetNumPersonajesPorEquipo();
        camara.EnfocaCamaraAE(gestorPartida.GetPersonajeTurno().transform.position, isAliado);

        lanzaAnimacionAE = true;
    }

    private void Update()
    {
        if(lanzaAnimacionAM)
        {
            if(gestorPartida.GetPersonajeTurno().AnimacionAM(objetivo))
            {
                lanzaAnimacionAM = false;
                PasarTurno();
            }
        }
        else if (lanzaAnimacionAD)
        {
            if (gestorPartida.GetPersonajeTurno().AnimacionAD(objetivo))
            {
                lanzaAnimacionAD = false;
                PasarTurno();
            }
        }
        else if (lanzaAnimacionAE)
        {
            if (gestorPartida.GetPersonajeTurno().AnimacionAE(objetivo))
            {
                lanzaAnimacionAE = false;
                PasarTurno();
            }
        }
    }
}
