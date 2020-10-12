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
        public Text nombre;
        public GameObject menuMultijugador, menuPersonajes;
        public SelectManager selectManager;

        private string roomName = "salaBite";
        private bool aliado = true;
        enum Eventos
        {
            JUGADOR_UNIDO,
            MENU_SELECCION_PERSONAJE,
            SET_EQUIPO_ROJO,
            SELECCIONAR_ALIADO,
            DESELECCIONAR_ALIADO,
            SELECCIONAR_ENEMIGO,
            DESELECCIONAR_ENEMIGO,
            NUM_EVENTOS
        }

        void Start()
        {
            // La IP del servidor al que se conectará el player será almacenada en las PlayerPrefs.
            // Debemos resetear las PlayerPrefs para evitar ciertos problemas de conexión
            PlayerPrefs.DeleteAll();
            ConnectToPhoton();
            PlayerCount.text = "Jugadores: " + playersCount + "/" + maxPlayersPerRoom;
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
            Log.text += "\nConexión correcta!";

            buttonJoinRoom.interactable = true;
            buttonLoadJuego.interactable = false;
        }

        public void JoinRoom()
        {
            if(nombre.GetComponent<Text>().text == "")
            {
                Log.text += "\nEscribe un nombre...";
                return;
            }

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.LocalPlayer.NickName = nombre.GetComponent<Text>().text; //1
                //Log.text += "\nPhotonNetwork.IsConnected! | Trying to Create/Join Room ";
                RoomOptions roomOptions = new RoomOptions(); //2
                TypedLobby typedLobby = new TypedLobby(roomName, LobbyType.Default); //3
                PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, typedLobby); //4

                Log.text += "\n" + PhotonNetwork.LocalPlayer.NickName + " se unió a la sala";
            }
        }

        public override void OnJoinedRoom()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                buttonJoinRoom.interactable = false;
                buttonLoadJuego.interactable = true;
                Log.text += "\nTu eres el anfitrión de sala";
            }
            else
            {
                buttonJoinRoom.interactable = false;
                Log.text += "\nConectado a la sala";
                PhotonNetwork.RaiseEvent((byte)Eventos.JUGADOR_UNIDO, PhotonNetwork.LocalPlayer.NickName,
                    new Photon.Realtime.RaiseEventOptions { Receivers = Photon.Realtime.ReceiverGroup.Others }, 
                    new ExitGames.Client.Photon.SendOptions() { });
            }
        }

        public void LoadJuego()
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2 || PhotonNetwork.CurrentRoom.PlayerCount == 6)
            {
                //PhotonNetwork.LoadLevel("SelectPlayers");
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
                PlayerCount.text = "Jugadores: "+playersCount + "/" + maxPlayersPerRoom;
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
            //PhotonNetwork.Disconnect();
            FindObjectOfType<ScenesManager>().ChangeScene("MainMenu");
            //Destroy(gameObject);
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
                case Eventos.MENU_SELECCION_PERSONAJE:
                    menuMultijugador.SetActive(false);
                    menuPersonajes.SetActive(true);
                    break;
                case Eventos.SET_EQUIPO_ROJO:
                    if(eventData.CustomData.ToString() == PhotonNetwork.LocalPlayer.NickName)
                    {
                        aliado = false;
                    }
                    break;
                case Eventos.SELECCIONAR_ALIADO:
                    selectManager.Seleccionar((int)eventData.CustomData, true);
                    break;
                case Eventos.DESELECCIONAR_ALIADO:
                    selectManager.Seleccionar((int)eventData.CustomData, true);
                    break;
                case Eventos.SELECCIONAR_ENEMIGO:
                    selectManager.Seleccionar((int)eventData.CustomData, false);
                    break;
                case Eventos.DESELECCIONAR_ENEMIGO:
                    selectManager.Seleccionar((int)eventData.CustomData, false);
                    break;
                default:
                    break;
            }
        }
        public bool IsAliado()
        {
            return aliado;
        }

        public void SeleccionaAliado(int boton)
        {
            PhotonNetwork.RaiseEvent((byte)Eventos.SELECCIONAR_ALIADO, boton,
                             new Photon.Realtime.RaiseEventOptions { Receivers = Photon.Realtime.ReceiverGroup.Others },
                             new ExitGames.Client.Photon.SendOptions() { });
        }
        public void DeseleccionaAliado(int boton)
        {
            PhotonNetwork.RaiseEvent((byte)Eventos.DESELECCIONAR_ALIADO, boton,
                             new Photon.Realtime.RaiseEventOptions { Receivers = Photon.Realtime.ReceiverGroup.Others },
                             new ExitGames.Client.Photon.SendOptions() { });
        }

        public void SeleccionaEnemigo(int boton)
        {
            PhotonNetwork.RaiseEvent((byte)Eventos.SELECCIONAR_ENEMIGO, boton,
                             new Photon.Realtime.RaiseEventOptions { Receivers = Photon.Realtime.ReceiverGroup.Others },
                             new ExitGames.Client.Photon.SendOptions() { });
        }
        public void DeseleccionaEnemigo(int boton)
        {
            PhotonNetwork.RaiseEvent((byte)Eventos.DESELECCIONAR_ENEMIGO, boton,
                             new Photon.Realtime.RaiseEventOptions { Receivers = Photon.Realtime.ReceiverGroup.Others },
                             new ExitGames.Client.Photon.SendOptions() { });
        }

    }
}
