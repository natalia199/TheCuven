using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomListingsMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform _content;                                                                 // Where room names will be listed and displayed
    [SerializeField]
    private RoomListing _roomListing;                                                           // Single room item, which will be added to the list of rooms

    private List<RoomListing> _listings = new List<RoomListing>();                              // List of all created RoomListing instantiations

    // When player successfully joins the created or existing room
    public override void OnJoinedRoom()
    {
        _content.DestroyChildren();                                                             // Destroying all rooms in the list (no longer needed)
        _listings.Clear();                                                                      // Clearing list

        PhotonNetwork.LoadLevel("Room");                                                        // Sending user to the room
    }


    // Updating the room list in real-time to show available existing rooms
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // Going through every single room listing within the displayed list
        foreach (RoomInfo info in roomList)
        {
            // Removing non-existent or closed room from the list so it can't be seen anymore
            if (info.RemovedFromList)
            {
                // Returns the index of the room name in the room list which has the same name as the room name we recieved
                int index = _listings.FindIndex(x => x.RoomInfo.Name == info.Name);

                // Room name does exist
                if (index != -1)
                {
                    Destroy(_listings[index].gameObject);                                       // Remove matching room from the list to keep listed updated
                    _listings.RemoveAt(index);
                }
            }

            // Adding new room to the list so it can be seen by everyone in the lobby
            else
            {
                int index = _listings.FindIndex(x => x.RoomInfo.Name == info.Name);

                // Room doesn't exist in list
                if (index == -1)
                {
                    // Instantiating a Room List prefab inside the content GameObject where rooms will be listed
                    RoomListing listing = Instantiate(_roomListing, _content);

                    // It should never = null
                    if (listing != null)
                    {
                        listing.SetRoomInfo(info);
                        _listings.Add(listing);
                    }
                }
            }
        }
    }

}
