using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private void Start()
    {
        InputKeyManager.Instance.AddMouse1DownClick(()=>
        {
            Camera camera = GetComponent<Camera>();
            var ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast( ray,out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent(out Map_Wap obj))
                {
                    var dic_map = MapManager.Instance.GetMap();
                    var map_wap = dic_map[obj.GetPoint()];
                    if (map_wap.TryGetobj(out Transform target))
                    {
                        map_wap.DestroyObject();
                    }
                    else
                    {
                        string path = "Wall";
                        var wall = Resources.Load<GameObject>(path);
                        var wallObj = Instantiate(wall);
                        obj.Setobj(wallObj);
                    }
                    Debug.Log(obj.GetPoint());
                }
            }
        });
    }
}
