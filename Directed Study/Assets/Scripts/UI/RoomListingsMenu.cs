using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomListingsMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private RoomListing _roomListing;

    // List of room names
    private List<RoomListing> _listings = new List<RoomListing>();

    public override void OnJoinedRoom()
    {
        _content.DestroyChildren();
        _listings.Clear();

        PhotonNetwork.LoadLevel("Room");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // Going through every single room listing
        foreach (RoomInfo info in roomList)
        {
            // Removed from room list
            if (info.RemovedFromList)
            {
                // Returns the index of the room name in the room list which has the same name as the room name we recieved
                int index = _listings.FindIndex(x => x.RoomInfo.Name == info.Name);

                // Room name does exist
                if (index != -1)
                {
                    Destroy(_listings[index].gameObject);
                    _listings.RemoveAt(index);
                }
            }
            // Added to room list
            else
            {
                int index = _listings.FindIndex(x => x.RoomInfo.Name == info.Name);
                if (index == -1)
                {
                    // Instantiating a room list inside the content GameObject
                    RoomListing listing = Instantiate(_roomListing, _content);

                    // It should never = null
                    if (listing != null)
                    {
                        listing.SetRoomInfo(info);
                        _listings.Add(listing);
                    }
                }
                else
                {

                }
            }            
        }
    }

}
