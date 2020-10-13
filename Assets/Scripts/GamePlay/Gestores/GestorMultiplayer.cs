using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;


public class GestorMultiplayer : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public GestorPartida gestorPartida;
    bool haRecibidoTurnos = false;

    enum Eventos
    {
        SELECCION_TURNOS,
        ACLAMAR_TURNOS,
        CREA_EQUIPO_AZUL,
        CREA_EQUIPO_ROJO,
        NUM_EVENTOS
    }
    private void Awake()
    {
        if (!PhotonNetwork.IsConnected)
            gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        if(!EsAnfitrion() && !haRecibidoTurnos)
            PhotonNetwork.RaiseEvent((byte)Eventos.ACLAMAR_TURNOS, null,
                           new Photon.Realtime.RaiseEventOptions { Receivers = Photon.Realtime.ReceiverGroup.Others },
                           new ExitGames.Client.Photon.SendOptions() { });
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

    public void SendOrdenTurnos(int turno)
    {
        PhotonNetwork.RaiseEvent((byte)Eventos.SELECCION_TURNOS, turno,
                       new Photon.Realtime.RaiseEventOptions { Receivers = Photon.Realtime.ReceiverGroup.Others },
                       new ExitGames.Client.Photon.SendOptions() { });
    }
    public void SendPosicionesAliados(int[]posiciones)
    {
        PhotonNetwork.RaiseEvent((byte)Eventos.CREA_EQUIPO_AZUL, posiciones,
                       new Photon.Realtime.RaiseEventOptions { Receivers = Photon.Realtime.ReceiverGroup.Others },
                       new ExitGames.Client.Photon.SendOptions() { });
    }
    public void SendPosicionesEnemigos(int[] posiciones)
    {
        PhotonNetwork.RaiseEvent((byte)Eventos.CREA_EQUIPO_ROJO, posiciones,
                       new Photon.Realtime.RaiseEventOptions { Receivers = Photon.Realtime.ReceiverGroup.Others },
                       new ExitGames.Client.Photon.SendOptions() { });
    }
}