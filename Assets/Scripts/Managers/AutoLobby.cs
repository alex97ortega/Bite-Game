using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using ExitGames.Client.Photon;

namespace Photon.Pun.Demo.PunBasics
{
    public class AutoLobby : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        public Text Log;
        public Text PlayerCount;
        public int playersCount;

        public byte maxPlayersPerRoom = 6;
        public byte minPlayersPerRoom = 2;
        public Button buttonJoinRoom;
        public Button buttonLoadJuego;
        public Button buttonComenzarPartida;
        public Button buttonElegir;
        public Text nombrePlayer, nombreServer, turnoElegir;
        public GameObject menuMultijugador, menuPersonajes, screenNombre, screenJoin, screenServer;
        public SelectManager selectManager;
        
        private bool aliado = true;
        private string[] turnosElegir;
        private int turno = 0;

        enum Eventos
        {
            JUGADOR_UNIDO,
            JUGADOR_DESCONECTADO,
            MENU_SELECCION_PERSONAJE,
            SET_EQUIPO_ROJO,
            SELECCIONAR_ALIADO,
            SELECCIONAR_ENEMIGO,
            NUM_EVENTOS
        }

        void Start()
        {
            // La IP del servidor al que se conectará el player será almacenada en las PlayerPrefs.
            // Debemos resetear las PlayerPrefs para evitar ciertos problemas de conexión
            PlayerPrefs.DeleteAll();
            ConnectToPhoton();
            buttonJoinRoom.interactable = false;
            buttonLoadJuego.interactable = false;
        }

        void Awake()
        {
            // La siguiente línea nos permite sincronizar la escena para todos los players de la room
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        void ConnectToPhoton()
        {
            Log.text = "Conectando...";
            PhotonNetwork.GameVersion = "" + 1; //1
            if (PhotonNetwork.ConnectUsingSettings())
            {
                //Log.text += "\n Connected to server";
            }
            else
            {
                Log.text += "\n Error al conectar con servidor";
            }
        }

        // Photon Methods
        public override void OnConnected()
        {
            base.OnConnected();
            Log.text += "\nConectado!";

            buttonJoinRoom.interactable = true;
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
        }

        public void CheckNombre()
        {
            if (nombrePlayer.GetComponent<Text>().text == "")
            {
                Log.text += "\nEscribe un nombre...";
                return;
            }

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.LocalPlayer.NickName = nombrePlayer.GetComponent<Text>().text;
                screenNombre.SetActive(false);
                screenJoin.SetActive(true);
            }
        }
        public void JoinRoom()
        {
            if (PhotonNetwork.IsConnected)
            {
                screenJoin.SetActive(false);
                screenServer.SetActive(true);
                string roomName = nombreServer.GetComponent<Text>().text;
                RoomOptions roomOptions = new RoomOptions();
                TypedLobby typedLobby = new TypedLobby(roomName, LobbyType.Default);
                PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, typedLobby);
            }
        }

        public override void OnJoinedRoom()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                buttonJoinRoom.interactable = false;
                buttonLoadJuego.interactable = true;
                Log.text += "\nCreada sala " + PhotonNetwork.CurrentRoom.Name + "\nTu eres el anfitrión de sala";
            }
            else
            {
                buttonJoinRoom.interactable = false;
                Log.text += "\nConectado a la sala";
                //comprobamos que el nombre no sea exactamente igual que alguno que ya tenemos
                string firstNickName = PhotonNetwork.LocalPlayer.NickName;
                int addN = 2;
                int i = 0;
                do
                {
                    if(PhotonNetwork.PlayerList[i] != PhotonNetwork.LocalPlayer &&
                        PhotonNetwork.PlayerList[i].NickName == PhotonNetwork.LocalPlayer.NickName)
                    {
                        PhotonNetwork.LocalPlayer.NickName = firstNickName + " " + addN.ToString();
                        addN++;
                        i = 0;
                    }
                    else
                        i++;
                } while (i<PhotonNetwork.PlayerList.Length);

                PhotonNetwork.RaiseEvent((byte)Eventos.JUGADOR_UNIDO, PhotonNetwork.LocalPlayer.NickName,
                    new Photon.Realtime.RaiseEventOptions { Receivers = Photon.Realtime.ReceiverGroup.All }, 
                    new ExitGames.Client.Photon.SendOptions() { });
            }
        }

        public void LoadJuego()
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2 || PhotonNetwork.CurrentRoom.PlayerCount == 6)
            {
                PhotonNetwork.RaiseEvent((byte)Eventos.MENU_SELECCION_PERSONAJE, PhotonNetwork.LocalPlayer.NickName,
                    new Photon.Realtime.RaiseEventOptions { Receivers = Photon.Realtime.ReceiverGroup.All },
                    new ExitGames.Client.Photon.SendOptions() { });
            }
            else
            {
                Log.text += "\nSe comienza partida con 2 o 6 jugadores";
            }
        }

        void Update()
        {
            if (PhotonNetwork.InRoom)
            {
                playersCount = PhotonNetwork.CurrentRoom.PlayerCount;
                PlayerCount.text = "Sala " + PhotonNetwork.CurrentRoom.Name + "\nJugadores: "+playersCount + "/" + maxPlayersPerRoom;
            }
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Log.text += "\nNo Rooms to Join, creating one...";
            if (PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions() { MaxPlayers = maxPlayersPerRoom }))
            {
                Log.text += "\n Room Created";
            }
            else
            {
                Log.text += "\nFail Creating Room";
            }
        }
        public void Desconectar()
        {
            PhotonNetwork.RaiseEvent((byte)Eventos.JUGADOR_DESCONECTADO, PhotonNetwork.LocalPlayer.NickName,
                    new Photon.Realtime.RaiseEventOptions { Receivers = Photon.Realtime.ReceiverGroup.All },
                    new ExitGames.Client.Photon.SendOptions() { });
            PhotonNetwork.LeaveRoom();
            FindObjectOfType<ScenesManager>().ChangeScene("MainMenu");
        }

        public void OnEvent(EventData eventData)
        {
            Eventos ev = (Eventos)eventData.Code;

            switch(ev)
            {
                case Eventos.JUGADOR_UNIDO:
                    Log.text += "\n" + eventData.CustomData + " se unió a la sala";
                    if (PhotonNetwork.CurrentRoom.PlayerCount % 2 == 0)
                    {
                        PhotonNetwork.RaiseEvent((byte)Eventos.SET_EQUIPO_ROJO, eventData.CustomData,
                            new Photon.Realtime.RaiseEventOptions { Receivers = Photon.Realtime.ReceiverGroup.Others },
                            new ExitGames.Client.Photon.SendOptions() { });
                    }
                    break;
                case Eventos.JUGADOR_DESCONECTADO:
                    Log.text += "\n" + eventData.CustomData + " se ha desconectado";
                    break;
                case Eventos.MENU_SELECCION_PERSONAJE:
                    turnosElegir = new string[PhotonNetwork.CurrentRoom.PlayerCount];
                    int i = 0;
                    foreach(var p in PhotonNetwork.PlayerList)
                    {
                        turnosElegir[i] = p.NickName;
                        i++;
                    }
                    menuMultijugador.SetActive(false);
                    menuPersonajes.SetActive(true);
                    buttonComenzarPartida.interactable = false;
                    turnoElegir.text = "Le toca elegir personaje a " + turnosElegir[turno % PhotonNetwork.CurrentRoom.PlayerCount];
                    break;
                case Eventos.SET_EQUIPO_ROJO:
                    if(eventData.CustomData.ToString() == PhotonNetwork.LocalPlayer.NickName)
                    {
                        aliado = false;
                        selectManager.SetEnemigo();
                    }
                    break;
                case Eventos.SELECCIONAR_ALIADO:
                    selectManager.Seleccionar((int)eventData.CustomData, true);
                    PasarTurno();
                    break;
                case Eventos.SELECCIONAR_ENEMIGO:
                    selectManager.Seleccionar((int)eventData.CustomData, false);
                    PasarTurno();
                    break;
                default:
                    break;
            }
        }
        public bool IsAliado()
        {
            return aliado;
        }

        public void SeleccionadoPersonaje(int boton)
        {
            Eventos evt = Eventos.SELECCIONAR_ALIADO;
            if (!aliado)
                evt = Eventos.SELECCIONAR_ENEMIGO;
            PhotonNetwork.RaiseEvent((byte)evt, boton,
                             new Photon.Realtime.RaiseEventOptions { Receivers = Photon.Realtime.ReceiverGroup.Others },
                             new ExitGames.Client.Photon.SendOptions() { });
            PasarTurno();
        }
        public bool PuedeSeleccionar()
        {
            return (turno < 6)&&(PhotonNetwork.LocalPlayer.NickName == turnosElegir[turno % PhotonNetwork.CurrentRoom.PlayerCount]);
        }

        private void PasarTurno()
        {
            turno++;
            
            if (turno == 6)
            {
                if (PhotonNetwork.IsMasterClient)
                    buttonComenzarPartida.interactable = true;

                buttonElegir.interactable = false;
                turnoElegir.text = "El anfitrión ya puede comenzar la partida";
            }
            else
            {
                buttonElegir.interactable = PuedeSeleccionar();
                turnoElegir.text = "Le toca elegir personaje a " + turnosElegir[turno % PhotonNetwork.CurrentRoom.PlayerCount];
            }
        }
        public void ComenzarPartida()
        {
            PhotonNetwork.LoadLevel("GamePlay");
        }
    }
}
