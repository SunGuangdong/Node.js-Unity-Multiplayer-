using UnityEngine;
using System.Collections;

/// <summary>
/// 處理屏幕点击到 Player （因为挂在Player身上）
/// </summary>
public class ClickFollow : MonoBehaviour, IClickable {

    public GameObject myPlayer;
    //public Follower myPlayerFollower;

    public NetworkEntity networkEntity;

    Targeter myPlayerTarget;

    void Start() { 
    	networkEntity = GetComponent<NetworkEntity> ();
        myPlayerTarget = myPlayer.GetComponent<Targeter>();
    }

    public void OnClick (RaycastHit hit)
	{
		Debug.Log ("following " + hit.collider.gameObject.name);

        //myPlayerFollower.targeter = transform;

        // GetComponent<NetworkFollower>().OnFollow(networkEntity.id);

        Network.Follow (networkEntity.id);

        myPlayerTarget.target = transform;
    }
}
