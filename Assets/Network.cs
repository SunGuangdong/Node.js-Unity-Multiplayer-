using UnityEngine;
using System.Collections.Generic;
using SocketIO;

public class Network : MonoBehaviour {


    static SocketIOComponent socket;

    public GameObject myPlayer;

    public Spawner spawner;

    void Start()
    {
        socket = GetComponent<SocketIOComponent>();
        socket.On("open", OnConnected);
        socket.On("spawn", OnSpawned);
        socket.On("move", OnMove);
        socket.On("registered", OnRegistered);
        socket.On("disconnected", OnDisconnected);
        socket.On("requestPosition", OnRequestPosition);
        socket.On("updatePosition", OnUpdatePosition);
        socket.On("follow", OnFollow);
        socket.On("register", OnRegister);
        socket.On("attack", OnAttack);
    }

    void OnConnected(SocketIOEvent e)
    {
        Debug.Log("connected");
    }

    private void OnSpawned(SocketIOEvent e)
    {
        Debug.Log("spawned" + e.data);
        var player = spawner.SpawnPlayer(e.data["id"].str);

        if (e.data["x"])
        {
            var movePosition = GetVectorFromJson(e);

            var navigatePos = player.GetComponent<Navigator>();
            navigatePos.NavigateTo(movePosition);
        }
    }

    void OnRegister(SocketIOEvent e)
    {
        Debug.Log("Succesfully registered, with id: " + e.data);
        spawner.AddPlayer(e.data["id"].str, myPlayer);
    }

    // 响应服务器的 move 事件
    void OnMove(SocketIOEvent e)
    {
        Debug.Log("player is moving" + e.data);

        var position = GetVectorFromJson(e);

        var id = e.data["id"].str;
        var player = spawner.FindPlayer(id);

        var navigatePos = player.GetComponent<Navigator>();

        navigatePos.NavigateTo(position);
    }

    void OnFollow(SocketIOEvent e)
    {
        Debug.Log("follow request " + e.data);

        var player = spawner.FindPlayer(e.data["id"].str);

        var targetTransform = spawner.FindPlayer(e.data["targetId"].str).transform;

        var target = player.GetComponent<Targeter>();

        target.target = targetTransform;
    }

    void OnAttack(SocketIOEvent e)
    {
        Debug.Log("recieved attack " + e.data);

        var targetPlayer = spawner.FindPlayer(e.data["targetId"].str);

        targetPlayer.GetComponent<Hittable>().OnHit();

        var attackingPlayer = spawner.FindPlayer(e.data["id"].str);
        attackingPlayer.GetComponent<Animator>().SetTrigger("Attack");
    }

    void OnRegistered(SocketIOEvent e)
    {
        Debug.Log("registered id: " + e.data);
    }

    void OnRequestPosition(SocketIOEvent e)
    {
        Debug.Log("server is requesting position");
        // 请求其他客户端更新  我的位置
        socket.Emit("updatePosition", VectorToJson(myPlayer.transform.position));
    }

    void OnDisconnected(SocketIOEvent e)
    {
        Debug.Log("player disconnected: " + e.data);

        var id = e.data["id"].str;
        spawner.Remove(id);
    }

    void OnUpdatePosition(SocketIOEvent e)
    {
        Debug.Log("updating position " + e.data);

        var position = GetVectorFromJson(e);

        var id = e.data["id"].str;
        var player = spawner.FindPlayer(id);

        player.transform.position = position;
    }

    //static public void Move (Vector3 current, Vector3 destination)
    //{
    //	Debug.Log ("sending position to node" + Network.VectorToJson(destination));


    //	JSONObject j = new JSONObject (JSONObject.Type.OBJECT);
    //	j.AddField ("c", Network.VectorToJson(current));
    //	j.AddField ("d", Network.VectorToJson(destination));

    //	socket.Emit("move", j);
    //}

    static public void Move(Vector3 position)
    {
        Debug.Log("sending position to node " + Network.VectorToJson(position));
        socket.Emit("move", Network.VectorToJson(position));
    }

    static public void Follow(string id)
    {
        Debug.Log("sending follow player id" + Network.PlayerIdToJson(id));
        socket.Emit("follow", Network.PlayerIdToJson(id));
    }

    static public void Attack(string targetId)
    {
        Debug.Log("attacking player: " + Network.PlayerIdToJson(targetId));
        socket.Emit("attack", Network.PlayerIdToJson(targetId));
    }

    static Vector3 GetVectorFromJson(SocketIOEvent e)
    {
        return new Vector3(e.data["x"].n, 0, e.data["y"].n);
    }

    public static JSONObject VectorToJson(Vector3 vector)
    {
        JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
        j.AddField("x", vector.x);
        j.AddField("y", vector.z);
        return j;
    }

    public static JSONObject PlayerIdToJson(string id)
    {
        JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
        j.AddField("targetId", id);
        return j;
    }
}
