using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance = null;
    [SerializeField] Vector3 value;
    [SerializeField] Transform parent;
    [SerializeField] Vector3 distance;
    [SerializeField] Player player;

    public static Dictionary<Vector3, Map_Wap> dic_map = new Dictionary<Vector3, Map_Wap>();

    private void Awake()
    {
        Instance = this;

        CreateMap();
    }
    public Player GetPlayer()
    {
        return player;
    }
    public void CreateMap()
    {
        //dic_map = new Dictionary<Vector3, Map_Wap>();
        IE_CreateMap();
    }
    public Map_Wap GetWapToPoint(Vector3 point)
    {
        Map_Wap ret = null;
        if (dic_map.ContainsKey(point))
        {
            ret = dic_map[point];
        }
        return ret;
    }
    public Dictionary<Vector3, Map_Wap> GetMap()
    {
        return dic_map;
    }

    void IE_CreateMap()
    {
        var wapPath = "Map_Wap";
        var parPosition = parent.position;
        for (int i = 0; i < value.y; i++)
        {
            for (int j = 0; j < value.x; j++)
            {
                for (int k = 0; k < value.z; k++)
                {
                    var wapOriginal = Resources.Load<Map_Wap>(wapPath);
                    var wap = GameObject.Instantiate(wapOriginal, parent);
                    var wapUnit = wap.GetUnit();

                    float z = parPosition.z + (wapUnit.z + distance.z) * j;
                    float x = parPosition.x + (wapUnit.x + distance.x) * k;
                    float y = parPosition.y + (wapUnit.y + distance.y) * i;

                    var point = new Vector3(j, i, k);
                    wap.SetPosition(point);
                    wap.transform.position = new Vector3(x, y, -z);

                    dic_map.Add(point, wap);
                }
            }
        }


        CreatePlayer();
        player.SetUpPoint(new Vector3(8, 0, 8));
    }

    public void CreatePlayer()
    {
        var playerOriginal = Resources.Load<Player>("Player");
        var player = GameObject.Instantiate(playerOriginal, null);
        this.player = player;
    }
    public T ResLoad<T>(string path, Transform parent = null) where T : MonoBehaviour, new()
    {
        T ret = new T();
        object obj = Resources.Load(path, ret.GetType());
        ret = GameObject.Instantiate(ret, parent);
        return ret;
    }

    public IEnumerator DepthPath(Vector3 point, Vector3 toPoint)
    {
        var ret = new List<Vector3>();
        bool isEnd = false;
        Dictionary<Vector3, bool> dic_pathed = new Dictionary<Vector3, bool>();


        dic_pathed.Add(point, true);
        if (dic_map.ContainsKey(point))
        {
            Loop(point);
        }
        //Task.Run(() => { });
        //Thread thread = new Thread(() => { });

        void Loop(Vector3 point)
        {
            if (point == toPoint)
            {
                isEnd = true;

                return;
            }
            if (isEnd)
            {
                return;
            }
            dic_map[point].SetColor(Color.red);
            var list = GetPoint(point);
            foreach (var item in list)
            {
                Loop(item);
            }
        }

        List<Vector3> GetPoint(Vector3 point)
        {
            List<Vector3> ret = new List<Vector3>();
            var left = point + new Vector3(-1, 0, 0);
            var right = point + new Vector3(1, 0, 0);
            var top = point + new Vector3(0, 0, -1);
            var bottom = point + new Vector3(0, 0, 1);
            if (dic_map.ContainsKey(left) && !dic_pathed.ContainsKey(left))
            {
                dic_pathed.Add(left, true);
                ret.Add(left);
            }
            if (dic_map.ContainsKey(right) && !dic_pathed.ContainsKey(right))
            {
                dic_pathed.Add(right, true);
                ret.Add(right);
            }
            if (dic_map.ContainsKey(top) && !dic_pathed.ContainsKey(top))
            {
                dic_pathed.Add(top, true);
                ret.Add(top);
            }
            if (dic_map.ContainsKey(bottom) && !dic_pathed.ContainsKey(bottom))
            {
                dic_pathed.Add(bottom, true);
                ret.Add(bottom);
            }
            return ret;
        }
        yield return 0;
    }

    public IEnumerator BreadthPath(Vector3 point, Vector3 toPoint)
    {
        Dictionary<Vector3, bool> dic_pathed = new Dictionary<Vector3, bool>();
        bool isEnd = false;

        dic_pathed.Add(point, true);
        var list = GetPoint(point);
        Loop(list);


        void Loop(List<Vector3> list)
        {
            if (isEnd)
            {
                return;
            }

            List<Vector3> newList = new List<Vector3>();
            foreach (var item in list)
            {
                if (item == toPoint)
                {
                    isEnd = true;
                    return;
                }
                dic_map[item].SetColor(Color.red);
                newList.AddRange(GetPoint(item));
            }
            if (newList.Count == 0)
            {
                return;
            }
            Loop(newList);
        }






        List<Vector3> GetPoint(Vector3 point)
        {
            List<Vector3> ret = new List<Vector3>();
            var left = point + new Vector3(-1, 0, 0);
            var right = point + new Vector3(1, 0, 0);
            var top = point + new Vector3(0, 0, -1);
            var bottom = point + new Vector3(0, 0, 1);
            if (dic_map.ContainsKey(left) && !dic_pathed.ContainsKey(left))
            {
                dic_pathed.Add(left, true);
                ret.Add(left);
            }
            if (dic_map.ContainsKey(right) && !dic_pathed.ContainsKey(right))
            {
                dic_pathed.Add(right, true);
                ret.Add(right);
            }
            if (dic_map.ContainsKey(top) && !dic_pathed.ContainsKey(top))
            {
                dic_pathed.Add(top, true);
                ret.Add(top);
            }
            if (dic_map.ContainsKey(bottom) && !dic_pathed.ContainsKey(bottom))
            {
                dic_pathed.Add(bottom, true);
                ret.Add(bottom);
            }
            return ret;
        }

        yield return 0;
    }
}
