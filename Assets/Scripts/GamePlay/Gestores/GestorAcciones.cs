using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GestorAcciones : MonoBehaviour
{
    public GestorPartida gestorPartida;
    public Terreno tablero;
    public Camara camara;
    public GameObject menuAcciones;
    public Material aliadoAzul,enemigoRojo, aliadoAmarillo;
    public Button botonAD, botonAE;
    public GestorObjetivosAD objetivosAD;
    public Log log;

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
        log.gameObject.SetActive(true);

        gestorPartida.GetPersonajeTurno().ComienzoTurno();
        tablero.GestionaEnvenenamientosCasillas(gestorPartida.GetPersonajeTurno().nombre);

        if (gestorPartida.GetPersonajeTurno().EstaMuerto() || gestorPartida.GetPersonajeTurno().EstaParalizado())
        {
            PasarTurno();
            return;
        }

        // enemigo
        //provisional hasta multiplayer
        if (false)//!gestorPartida.GetPersonajeTurno().IsAliado())
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

            botonAD.interactable = !gestorPartida.GetPersonajeTurno().UltimaJugoAtaqueDistancia();
            botonAE.interactable = !gestorPartida.GetPersonajeTurno().HaJugadoUlti();
        }
    }

    public void PasarTurno()
    {
        if(gestorPartida.GetPersonajeTurno().IsAliado())
            gestorPartida.GetPersonajeTurno().SetColor(aliadoAzul);
        else
            gestorPartida.GetPersonajeTurno().SetColor(enemigoRojo);
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

    /////////////////////////////////////////
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
        if (objetivo.IsAliado() == gestorPartida.GetPersonajeTurno().IsAliado())
            return;
        if (objetivo.EstaMuerto() || objetivo.EsInmune())
            return;

        string nombre = gestorPartida.GetPersonajeTurno().nombre;

        if (nombre == "Dani")
            camara.EnfocaCamaraAMDani(gestorPartida.GetPersonajeTurno().transform.position, gestorPartida.GetPersonajeTurno().IsAliado());
        else
            camara.EnfocaCamaraAC(gestorPartida.GetPersonajeTurno().transform.position, gestorPartida.GetPersonajeTurno().IsAliado());

        menuAcciones.SetActive(false);
        log.gameObject.SetActive(false);
        objetivosAD.gameObject.SetActive(false);
        tablero.RestauraTablero();
        lanzaAnimacionAM = true;
    }

    /////////////////////////////////////////
    public void AtaqueDistancia()
    {
        string nombre = gestorPartida.GetPersonajeTurno().nombre;

        // para ataque de reygon tiene que haber un aliado al lado
        if (nombre == "Reygon")
        {
            Personaje aliadoAdyacente = tablero.EncuentraAliadoAdyacente(gestorPartida.GetPersonajeTurno().GetCasillaX(),
                                                                         gestorPartida.GetPersonajeTurno().GetCasillaZ(),
                                                                         gestorPartida.GetPersonajeTurno().IsAliado());
            if (aliadoAdyacente == null)
            {
                log.LanzaLog("Reygon intentó usar a un compañero, pero está más solo que la una.");
                return;
            }
            objetivo = aliadoAdyacente;
            camara.EnfocaCamaraAD(objetivo.transform.position, gestorPartida.GetPersonajeTurno().IsAliado());
            menuAcciones.SetActive(false);
            log.gameObject.SetActive(false);
            tablero.RestauraTablero();

            lanzaAnimacionAD = true;
        }
        else if (gestorPartida.GetPersonajeTurno().necesitaObjetivoAD)
        {
            objetivosAD.gameObject.SetActive(!objetivosAD.gameObject.activeSelf);
            if(objetivosAD.gameObject.activeSelf)
            {
                objetivosAD.ActualizaEnemigos(gestorPartida.GetPersonajeTurno().IsAliado());
            }
        }
        else
        {
            ConfirmadoObjetivoAD(null);
        }
    }

    public void ConfirmadoObjetivoAD(Personaje objetivoAD)
    {
        objetivo = objetivoAD;
        string nombre = gestorPartida.GetPersonajeTurno().nombre;
        if (nombre == "Dani" || nombre == "Laura" || nombre == "Sergio")
            camara.EnfocaCamaraAE2(gestorPartida.GetPersonajeTurno().transform.position, gestorPartida.GetPersonajeTurno().IsAliado());
        else
            camara.EnfocaCamaraAD(gestorPartida.GetPersonajeTurno().transform.position, gestorPartida.GetPersonajeTurno().IsAliado());
        menuAcciones.SetActive(false);
        log.gameObject.SetActive(false);
        objetivosAD.gameObject.SetActive(false);
        tablero.RestauraTablero();

        lanzaAnimacionAD = true;
    }

    /////////////////////////////////////////
    public void AtaqueEspecial()
    {
        if (gestorPartida.GetPersonajeTurno().HaJugadoUlti())
            return;

        string nombre = gestorPartida.GetPersonajeTurno().nombre;
        if (nombre == "Alex")
            camara.EnfocaCamaraAD(gestorPartida.GetPersonajeTurno().transform.position, gestorPartida.GetPersonajeTurno().IsAliado());
        else if (nombre == "Sergio")
            camara.EnfocaCamaraAE2(gestorPartida.GetPersonajeTurno().transform.position, gestorPartida.GetPersonajeTurno().IsAliado());
        else
            camara.EnfocaCamaraAE(gestorPartida.GetPersonajeTurno().transform.position, gestorPartida.GetPersonajeTurno().IsAliado());

        menuAcciones.SetActive(false);
        log.gameObject.SetActive(false);
        objetivosAD.gameObject.SetActive(false);
        tablero.RestauraTablero();
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
