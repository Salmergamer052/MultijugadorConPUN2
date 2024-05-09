using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    private PhotonView pv;

    void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (pv.IsMine)
        {
            CreatePlayerController();
        }
    }

    public void CreatePlayerController()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","PlayerController"), Vector3.zero, Quaternion.identity);
    }

}
