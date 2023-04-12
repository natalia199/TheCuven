using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerListingsMenu : MonoBehaviourPunCallbacks
{
    /*
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private PlayerListing _playerListings;
    */

    public GameObject startBtn;

    // List of room names
    //private List<PlayerListing> _listings = new List<PlayerListing>();
/*
    public override void OnEnable()
    {
        base.OnEnable();
        GetCurrentRoomPlayers();
    }

    
    public override void OnDisable()
    {
        base.OnDisable();
        for (int i = 0; i < _listings.Count; i++)
        {
            Destroy(_listings[i].gameObject);
        }

        _listings.Clear();
    }

    */

    private void GetCurrentRoomPlayers()
    {
        if (!PhotonNetwork.IsConnected)
            return;
        if (PhotonNetwork.CurrentRoom == null || PhotonNetwork.CurrentRoom.Players == null)
            return;

        foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
        {
            //AddPlayerListing(playerInfo.Value);
        }
    }

    /*
    private void AddPlayerListing(Player player)
    {
        int index = _listings.FindIndex(x => x.Player == player);
        if (index != -1)
        {
            _listings[index].SetPlayerInfo(player);
        }
        else
        {
            PlayerListing listing = Instantiate(_playerListings, _content);
            if (listing != null)
            {
                listing.SetPlayerInfo(player);
                _listings.Add(listing);
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerListing(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int index = _listings.FindIndex(x => x.Player == otherPlayer);

        // Room name does exist
        if (index != -1)
        {
            Destroy(_listings[index].gameObject);
            _listings.RemoveAt(index);
        }
    }
    */

    public void OnClick_StartCharSelection()
    {
        GameObject.Find("Scene Manager").GetComponent<SceneManage>().beginCharSelection = true;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        /*
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Intro_Scene");
        }
        */
    }

    public void OnClick_StartGame()
    {
        GameObject.Find("Scene Manager").GetComponent<SceneManage>().beginGame = true;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        /*
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Intro_Scene");
        }
        */
    }

    public void Update()
    {
        if (SceneManager.GetActiveScene().name == "Username 1")
        {

            if (PhotonNetwork.LocalPlayer.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 2)
                startBtn.SetActive(true);
            else
                startBtn.SetActive(false);
        }
        else if (SceneManager.GetActiveScene().name == "Username")
        {
            if (!PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                startBtn.GetComponent<Button>().enabled = false;
            }
        }
    }
}
