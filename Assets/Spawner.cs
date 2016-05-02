using UnityEngine;
using System.Collections.Generic;
using SocketIO;

public class Spawner : MonoBehaviour {

    public GameObject myPlayer;
    public GameObject playerPrefab;
	public SocketIOComponent socket;

	Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();

	public GameObject SpawnPlayer(string id) {
		var player = Instantiate (playerPrefab, Vector3.zero, Quaternion.identity) as GameObject;

        player.GetComponent<ClickFollow>().myPlayer = myPlayer;
        //player.GetComponent<ClickFollow>().myPlayerFollower = myPlayer.GetComponent<Targeter>();
        //player.GetComponent<NetworkFollower>().socket = socket;

        player.GetComponent<NetworkEntity> ().id = id;

        AddPlayer(id, player);

		return player;
	}

	public GameObject FindPlayer(string id){
		return players[id];
	}

	public void AddPlayer(string id, GameObject player) {
		players.Add(id, player);
	}

	public void Remove (string id){
		var player = players [id];
		Destroy (player);
		players.Remove (id);
	}
}
