using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Map_Wap : PointBase
{
    [SerializeField] Transform mainTr;
    [SerializeField] Vector3 unit;
    [SerializeField] GameObject main;
    [SerializeField] TextMesh textMesh;
    [SerializeField] Transform tr;
    Material mat;
    GameObject obj;

    private void Awake()
    {
        Material material = new Material(Shader.Find("Standard"));
        mat = GetMain<MeshRenderer>().material = material;
    }
    public T GetMain<T>() where T : class
    {
        T ret = null;
        if (main != null)
        {
            ret = main.GetComponent<T>();
        }
        return ret;
    }
    public void Setobj(GameObject obj)
    {
        this.obj = obj;
        if (obj != null)
        {
            obj.transform.position = tr.position;
        }
    }
    public bool TryGetobj<T>(out T cs) where T : class
    {
        bool ret = false;
        cs = null;
        cs = obj?.GetComponent<T>();
        if (cs != null)
        {
            ret = true;
        }
        return ret;
    }
    public void DestroyObject()
    {
        Destroy(obj);
        Setobj(null);
    }
    public int GetObjLayerMask()
    {
        int ret = 0;
        if (obj != null)
        {
            ret = (int)Mathf.Pow(2, obj.layer);
        }
        return ret;
    }
    public Vector3 GetPosition()
    {
        return mainTr.position;
    }

    public Vector3 GetPostion()
    {
        return mainTr.position;
    }
    public Vector3 GetUnit()
    {
        return unit;
    }
    public void SetPosition(Vector3 pos)
    {
        SetPoint(pos);
        var point = GetPoint();
        textMesh.text = $"({(int)point.x},{(int)point.z})";
    }

    private void OnMouseUpAsButton()
    {
        foreach (var item in MapManager.Instance.GetMap())
        {
            item.Value.SetColor(Color.white);
        }

        if (TryGetobj<Transform>(out Transform tr))
        {
            return;
        }
        var player = MapManager.Instance.GetPlayer();
        var point = player.GetPoint();
        var toPoint = GetPoint();

        //StartCoroutine(MapManager.Instance.BreadthPath(point, toPoint));


        StartCoroutine(Base.Instance.BreadthPath(point, toPoint, MapManager.Instance.GetMap(),
            startEvent: () =>
            {
                player.StopMove();
            },
            sizer: (list) =>
            {
                bool ret = true;
                if (list.TryGetobj(out Transform tr))
                {
                    ret = false;
                }
                return ret;
            },
            endEvent: (path) =>
            {
                var map = MapManager.Instance.GetMap();
                foreach (var item in path)
                {
                    map[item].SetColor(Color.red * 0.2f);
                }
                player.StartMove(path, MapManager.Instance.GetMap());
            }));
        player.SetPoint(point);
    }
    public void SetColor(Color toColor)
    {
        mat.color = toColor;
    }
}
