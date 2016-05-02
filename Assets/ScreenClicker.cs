using UnityEngine;
using System.Collections;

public class ScreenClicker : MonoBehaviour {

    // Update is called once per frame
    void Update()
    {
        // left alt  或者 鼠标右键
        if (Input.GetButtonDown("Fire2"))
            Clicked();

        //if (Input.GetButtonDown("Fire1"))
        //{
        //    Network.Move(new Vector3(500, 0, 500), new Vector3(200, 0, 200));
        //}
    }

    void Clicked()
    {
        // 射线碰撞检测
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.collider.gameObject.name);

            var clickable = hit.collider.gameObject.GetComponent<IClickable>();  // 这个为什么要这样写呢？ 继续看就知道了
            clickable.OnClick(hit);
        }
    }
}
