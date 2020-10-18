using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GestorAcciones : MonoBehaviour
{
    public GestorPartida gestorPartida;
    public Terreno tablero;
    public Camara camara;
    public GameObject menuAcciones, turnos, loading;
    public Material aliadoAzul,enemigoRojo, aliadoAmarillo;
    public Button botonAD, botonAE;
    public GestorObjetivosAD objetivosAD;
    public GestorMultiplayer gestorMultiplayer;
    public Log log;
    public Text cronometro;
    public int tiempoTurnos;

    int movimientosEsteTurno;
    bool lanzaAnimacionAM = false;
    bool lanzaAnimacionAD = false;
    bool lanzaAnimacionAE = false;
    Personaje objetivo;
    float startTime, time, cont;
    GameManager gm;

    bool tieneQueAtacarAC    = false;
    bool tieneQueAtacarAD    = false;
    bool tieneQueAtacarAE    = false;
    bool tieneQueMoverIzq    = false;
    bool tieneQueMoverDch    = false;
    bool tieneQueMoverAbajo  = false;
    bool tieneQueMoverArriba = false;
    bool tieneQuePasarTurno  = false;
    string objetivoADnombre = "";
    AudioSource musicaFondo;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        musicaFondo = camara.GetComponentInChildren<AudioSource>();
    }

    public void PreparaTurno()
    {
        tablero.RestauraTablero();
        movimientosEsteTurno = 0;
        objetivo = null;
        log.gameObject.SetActive(true);
        cronometro.gameObject.SetActive(true);
        startTime = tiempoTurnos + 1 + Time.time;

        gestorPartida.GetPersonajeTurno().ComienzoTurno();
        tablero.GestionaEnvenenamientosCasillas(gestorPartida.GetPersonajeTurno().nombre);

        if (gestorPartida.GetPersonajeTurno().EstaMuerto() || gestorPartida.GetPersonajeTurno().EstaParalizado())
        {
            PasarTurno(false);
            return;
        }
        
        // enemigo
        if (gm.IsAliado() != gestorPartida.GetPersonajeTurno().IsAliado())
        {
            menuAcciones.SetActive(false);
            if(!gestorMultiplayer.gameObject.activeSelf)
                PasarTurno(false); // sin ia, para cuando es solo player y pruebas
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

    public void PasarTurno(bool enviaMensaje)
    {
        if (enviaMensaje)
            gestorMultiplayer.SendMensajePasarTurno();

        if(gestorPartida.GetPersonajeTurno().IsAliado())
            gestorPartida.GetPersonajeTurno().SetColor(aliadoAzul);
        else
            gestorPartida.GetPersonajeTurno().SetColor(enemigoRojo);
        camara.RestauraCamara();

        turnos.SetActive(true);
        gestorPartida.SiguienteTurno();
        PreparaTurno();
    }

    public void MueveIzq(bool enviaMensaje)
    {
        if(enviaMensaje)
            gestorMultiplayer.SendMensajeMoverIzq();
        MueveCasilla(-1, 0);
    }
    public void MueveDch(bool enviaMensaje)
    {
        if (enviaMensaje)
            gestorMultiplayer.SendMensajeMoverDch();
        MueveCasilla(1, 0);
    }
    public void MueveArriba(bool enviaMensaje)
    {
        if (enviaMensaje)
            gestorMultiplayer.SendMensajeMoverArriba();
        MueveCasilla(0, 1);
    }
    public void MueveAbajo(bool enviaMensaje)
    {
        if (enviaMensaje)
            gestorMultiplayer.SendMensajeMoverAbajo();
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
            if(gm.IsAliado() == gestorPartida.GetPersonajeTurno().IsAliado())
                tablero.PintaCasillasAmarillas(newPosX, newPosZ, movs- movimientosEsteTurno);
        }
    }

    /////////////////////////////////////////
    public void AtaqueCuerpo(bool enviaMensaje)
    {
        if (enviaMensaje)
            gestorMultiplayer.SendMensajeAtaqueAC();
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
        else if (nombre == "Asier")
            camara.EnfocaCamaraAMAsier(gestorPartida.GetPersonajeTurno().transform.position, gestorPartida.GetPersonajeTurno().IsAliado());
        else
            camara.EnfocaCamaraAC(gestorPartida.GetPersonajeTurno().transform.position, gestorPartida.GetPersonajeTurno().IsAliado());


        DesactivaGUI();
        tablero.RestauraTablero();
        lanzaAnimacionAM = true;
    }

    /////////////////////////////////////////
    public void AtaqueDistancia(bool enviaMensaje)
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
            
            ConfirmadoObjetivoAD(aliadoAdyacente.nombre, enviaMensaje);
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
            ConfirmadoObjetivoAD("", enviaMensaje);
        }
    }

    public void ConfirmadoObjetivoAD(string objetivoName, bool enviaMensaje)
    {
        if (enviaMensaje)
            gestorMultiplayer.SendMensajeAtaqueAD(objetivoName);
        
        objetivo = gestorPartida.GetPersonaje(objetivoName);
        string nombre = gestorPartida.GetPersonajeTurno().nombre;
        if (nombre == "Dani" || nombre == "Laura" || nombre == "Sergio")
            camara.EnfocaCamaraAE2(gestorPartida.GetPersonajeTurno().transform.position, gestorPartida.GetPersonajeTurno().IsAliado());
        else if(nombre == "Reygon")
            camara.EnfocaCamaraAD(objetivo.transform.position, gestorPartida.GetPersonajeTurno().IsAliado());
        else if(nombre != "Asier")
            camara.EnfocaCamaraAD(gestorPartida.GetPersonajeTurno().transform.position, gestorPartida.GetPersonajeTurno().IsAliado());

        DesactivaGUI();
        tablero.RestauraTablero();

        lanzaAnimacionAD = true;
    }

    /////////////////////////////////////////
    public void AtaqueEspecial(bool enviaMensaje)
    {
        if (enviaMensaje)
            gestorMultiplayer.SendMensajeAtaqueAE();

        if (gestorPartida.GetPersonajeTurno().HaJugadoUlti())
            return;

        string nombre = gestorPartida.GetPersonajeTurno().nombre;
        if (nombre == "Alex")
            camara.EnfocaCamaraAD(gestorPartida.GetPersonajeTurno().transform.position, gestorPartida.GetPersonajeTurno().IsAliado());
        else if (nombre == "Sergio")
            camara.EnfocaCamaraAE2(gestorPartida.GetPersonajeTurno().transform.position, gestorPartida.GetPersonajeTurno().IsAliado());
        else
            camara.EnfocaCamaraAE(gestorPartida.GetPersonajeTurno().transform.position, gestorPartida.GetPersonajeTurno().IsAliado());

        DesactivaGUI();
        tablero.RestauraTablero();
        lanzaAnimacionAE = true;
    }

    private void Update()
    {
        if (!gestorPartida.Ready())
            return;

        //ataques
        if(lanzaAnimacionAM)
        {
            if(gestorPartida.GetPersonajeTurno().AnimacionAM(objetivo))
            {
                lanzaAnimacionAM = false;
                PasarTurno(false);
            }
        }
        else if (lanzaAnimacionAD)
        {
            if (gestorPartida.GetPersonajeTurno().AnimacionAD(objetivo))
            {
                lanzaAnimacionAD = false;
                PasarTurno(false);
            }
        }
        else if (lanzaAnimacionAE)
        {
            if (gestorPartida.GetPersonajeTurno().AnimacionAE(objetivo))
            {
                lanzaAnimacionAE = false;
                PasarTurno(false);
            }
        }
        //posible retardo en recibimiento de mensajes
        else if(tieneQueMoverIzq)
        {
            tieneQueMoverIzq = false;
            MueveIzq(false);
        }
        else if (tieneQueMoverDch)
        {
            tieneQueMoverDch = false;
            MueveDch(false);
        }
        else if (tieneQueMoverArriba)
        {
            tieneQueMoverArriba = false;
            MueveArriba(false);
        }
        else if (tieneQueMoverAbajo)
        {
            tieneQueMoverAbajo = false;
            MueveAbajo(false);
        }
        else if (tieneQueAtacarAC)
        {
            tieneQueAtacarAC = false;
            AtaqueCuerpo(false);
        }
        else if (tieneQueAtacarAD)
        {
            tieneQueAtacarAD = false;
            ConfirmadoObjetivoAD(objetivoADnombre, false);
        }
        else if (tieneQueAtacarAE)
        {
            tieneQueAtacarAE = false;
            AtaqueEspecial(false);
        }
        else if(tieneQuePasarTurno)
        {
            tieneQuePasarTurno = false;
            PasarTurno(false);
        }
        // pantalla carga
        else if (loading.activeSelf)
        {
            cont += Time.deltaTime;
            if(!gestorMultiplayer.gameObject.activeSelf || cont > 5)
            {
                loading.SetActive(false);
                startTime = tiempoTurnos + 1 + Time.time;
            }
        }
        //cronometro y musica
        else
        {
            if (!musicaFondo.isPlaying)
                musicaFondo.Play();

            time = startTime - Time.time;
            if (time >= 0)
            {
                if (time > tiempoTurnos)
                    time = tiempoTurnos;
                string  seg;
                if (time == 0)
                    seg = "00";
                else if (time < 10)
                    seg = "0" + ((int)time % tiempoTurnos).ToString();
                else
                    seg = ((int)time).ToString();

                cronometro.text = seg;
            }
            else
            {
                if(gm.IsAliado() == gestorPartida.GetPersonajeTurno().IsAliado())
                    PasarTurno(true);
            }
        }
    }
    private void DesactivaGUI()
    {
        menuAcciones.SetActive(false);
        log.gameObject.SetActive(false);
        cronometro.gameObject.SetActive(false);
        objetivosAD.gameObject.SetActive(false);
        turnos.SetActive(false);
    }
    public void TieneQueAtacarAC()
    {
        tieneQueAtacarAC = true;
    }
    public void TieneQueAtacarAD(string n)
    {
        tieneQueAtacarAD = true;
        objetivoADnombre = n;
    }
    public void TieneQueAtacarAE()
    {
        tieneQueAtacarAE = true;
    }

    public void TieneQueMoverseIzq()    { tieneQueMoverIzq   = true; }
    public void TieneQueMoverseDch()    { tieneQueMoverDch   = true; }
    public void TieneQueMoverseArriba() { tieneQueMoverArriba= true; }
    public void TieneQueMoverseAbajo()  { tieneQueMoverAbajo = true; }
    public void TieneQuePasarTurno()    { tieneQuePasarTurno = true; }
}
