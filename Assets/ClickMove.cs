using UnityEngine;
using System.Collections;
/// <summary>
/// 處理屏幕点击到 Floor （因为挂在Floor上）
/// </summary>
public class ClickMove : MonoBehaviour, IClickable
{

    public GameObject player;

    public void OnClick(RaycastHit hit)
    {
        var navigator = player.GetComponent<Navigator>();
        var nMove = player.GetComponent<NetworkMove>();

        navigator.NavigateTo(hit.point);
        //nMove.OnMove(hit.point);
        Network.Move(hit.point);

        //Network.Move (player.transform.position, hit.point);
    }
}
