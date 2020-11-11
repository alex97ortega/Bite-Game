using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;


public class GestorMultiplayer : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public GestorPartida gestorPartida;
    public GestorAcciones gestorAcciones;
    bool haRecibidoTurnos = false;
    string proxObjetivoAD = "";

    enum Eventos
    {
        SELECCION_TURNOS,
        ACLAMAR_TURNOS,
        CREA_EQUIPO_AZUL,
        CREA_EQUIPO_ROJO,
        MUEVE_IZQUIERDA,
        MUEVE_DERECHA,
        MUEVE_ARRIBA,
        MUEVE_ABAJO,
        ATAQUE_CUERPO,
        ATAQUE_DISTANCIA,
        ATAQUE_ESPECIAL,
        PASAR_TURNO,
        NUM_EVENTOS
    }
    Queue<Eventos> colaEventos;

    private void Awake()
    {
        colaEventos = new Queue<Eventos>();
        if (!PhotonNetwork.IsConnected)
            gameObject.SetActive(false);      
            
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!EsAnfitrion() && !haRecibidoTurnos)
            SendMensajeToOthers(Eventos.ACLAMAR_TURNOS, null);
    }
    
    public void OnEvent(EventData eventData)
    {
        Eventos ev = (Eventos)eventData.Code;

        switch (ev)
        {
            case Eventos.SELECCION_TURNOS:
                haRecibidoTurnos = true;
                gestorPartida.AddTurno((int)eventData.CustomData);
                break;
            case Eventos.ACLAMAR_TURNOS:
                if (EsAnfitrion())
                {
                    gestorPartida.LanzaMensajeTurnos();
                }
                break;
            case Eventos.CREA_EQUIPO_AZUL:
                gestorPartida.ColocaAliados((int[])eventData.CustomData);
                break;
            case Eventos.CREA_EQUIPO_ROJO:
                gestorPartida.ColocaEnemigos((int[])eventData.CustomData);
                break;
            case Eventos.ATAQUE_DISTANCIA:
                proxObjetivoAD = (string)eventData.CustomData;
                AddEventoToTheQueue(ev);
                break;
            case Eventos.MUEVE_IZQUIERDA:
            case Eventos.MUEVE_DERECHA:
            case Eventos.MUEVE_ARRIBA:
            case Eventos.MUEVE_ABAJO:
            case Eventos.ATAQUE_CUERPO:
            case Eventos.ATAQUE_ESPECIAL:
            case Eventos.PASAR_TURNO:
                AddEventoToTheQueue(ev);
                break;
            default:
                break;
        }
    }

    public bool ModoMultijugador()
    {
        return PhotonNetwork.IsConnected;
    }
    public bool EsAnfitrion()
    {
        return PhotonNetwork.IsMasterClient;
    }
    //INIT PARTIDA
    public void SendOrdenTurnos(int turno)
    {
        SendMensajeToOthers(Eventos.SELECCION_TURNOS, turno);
    }
    public void SendPosicionesAliados(int[]posiciones)
    {
        SendMensajeToOthers(Eventos.CREA_EQUIPO_AZUL, posiciones);
    }
    public void SendPosicionesEnemigos(int[] posiciones)
    {
        SendMensajeToOthers(Eventos.CREA_EQUIPO_ROJO, posiciones);
    }
    //MOVIMIENTOS
    public void SendMensajeMoverIzq()
    {
        SendMensajeToOthers(Eventos.MUEVE_IZQUIERDA, null);
    }
    public void SendMensajeMoverDch()
    {
        SendMensajeToOthers(Eventos.MUEVE_DERECHA, null);
    }
    public void SendMensajeMoverArriba()
    {
        SendMensajeToOthers(Eventos.MUEVE_ARRIBA, null);
    }
    public void SendMensajeMoverAbajo()
    {
        SendMensajeToOthers(Eventos.MUEVE_ABAJO, null);
    }
    public void SendMensajePasarTurno()
    {
        SendMensajeToOthers(Eventos.PASAR_TURNO, null);
    }

    //ATAQUES
    public void SendMensajeAtaqueAC()
    {
        SendMensajeToOthers(Eventos.ATAQUE_CUERPO, null);
    }
    public void SendMensajeAtaqueAD(string personajeName)
    {
        SendMensajeToOthers(Eventos.ATAQUE_DISTANCIA, personajeName);
    }
    public void SendMensajeAtaqueAE()
    {
        SendMensajeToOthers(Eventos.ATAQUE_ESPECIAL, null);
    }

    private void SendMensajeToOthers(Eventos evt, object parametros)
    {
        if(PhotonNetwork.IsConnected)
        {
            PhotonNetwork.RaiseEvent((byte)evt, parametros,
                      new Photon.Realtime.RaiseEventOptions { Receivers = Photon.Realtime.ReceiverGroup.Others },
                      new ExitGames.Client.Photon.SendOptions() { });
        }
    }
    public bool HayAlgunMensajeSinProcesar()
    {
        return (colaEventos.Count != 0);
    }
    private void AddEventoToTheQueue(Eventos evt)
    {
        colaEventos.Enqueue(evt);
    }
    public void GestionaProximoEvento()
    {
        Eventos proxEvento = colaEventos.Dequeue();

        switch (proxEvento)
        {            
            case Eventos.MUEVE_IZQUIERDA:
                gestorAcciones.TieneQueMoverseIzq();
                break;
            case Eventos.MUEVE_DERECHA:
                gestorAcciones.TieneQueMoverseDch();
                break;
            case Eventos.MUEVE_ARRIBA:
                gestorAcciones.TieneQueMoverseArriba();
                break;
            case Eventos.MUEVE_ABAJO:
                gestorAcciones.TieneQueMoverseAbajo();
                break;
            case Eventos.ATAQUE_CUERPO:
                gestorAcciones.TieneQueAtacarAC();
                break;
            case Eventos.ATAQUE_DISTANCIA:
                gestorAcciones.TieneQueAtacarAD(proxObjetivoAD);
                break;
            case Eventos.ATAQUE_ESPECIAL:
                gestorAcciones.TieneQueAtacarAE();
                break;
            case Eventos.PASAR_TURNO:
                gestorAcciones.TieneQuePasarTurno();
                break;
            default:
                break;
        }
    }
}