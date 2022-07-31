using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base
{
    class FourTree
    {
        public Vector3 value;

        public FourTree left;
        public FourTree right;
        public FourTree top;
        public FourTree bottom;

        public FourTree parent;

        public FourTree(Vector3 value, FourTree parent = null)
        {
            this.value = value;
            this.parent = parent;
        }
    }
    public static Base Instance = new Base();

    public enum Dirction
    {
        Up,
        Right,
        Forward
    }
    Dictionary<Dirction, Vector3> dic_dirction = new Dictionary<Dirction, Vector3>
    {
        { Dirction.Up, new Vector3(0, 1, 0)},
        { Dirction.Right, new Vector3(1, 0, 0)},
        { Dirction.Forward, new Vector3(0, 0, 1)},
    };
    public float GetAngle(Dirction dir1, Vector3 toDir2)
    {
        float ret;
        Vector3 dir = dic_dirction[dir1];

        //dot
        toDir2 = (toDir2).normalized;
        //Vector3 playerDir = transform.TransformDirection(new Vector3(0,0,1));
        float dot = Mathf.Acos(Vector3.Dot(toDir2, dir)) * Mathf.Rad2Deg;

        //cross
        Vector3 cross = Vector3.Cross(dir, toDir2);
        cross.y = cross.y < 0.01f && cross.y > -0.01f ? 0.01f : cross.y;
        float dic = cross.y / Mathf.Abs(cross.y);

        ret = dot * dic;

        return ret;
    }
    float GetDirction(Vector3 start, Vector3 toPoint)
    {
        float ret = 0;
        ret = Mathf.Abs(start.x - toPoint.x) + Mathf.Abs(start.y - toPoint.y) + Mathf.Abs(start.z - toPoint.z);
        return ret;
    }

    public IEnumerator AShapPath<T>(Vector3 point, Vector3 toPoint, Dictionary<Vector3,T> dic_map, 
        Action startEvent, 
        Func<T, bool> sizer,
        Action<List<Vector3>> endEvent)
    {
        startEvent.Invoke();
        Dictionary<Vector3, bool> dic_pathed = new Dictionary<Vector3, bool>();
        List<Vector3> path = new List<Vector3>();
        dic_pathed.Add(point, true);
        Loop(point);
        for (int i = 0; i < 5; i++)
        {
            if (path[path.Count - 1] == toPoint)
            {
                break;
            }
            path = new List<Vector3>();
            dic_pathed = new Dictionary<Vector3, bool>();
            Loop(point);
        }
        void Loop(Vector3 start)
        {
            if (start == toPoint)
            {
                return;
            }
            var list = GetPoint(start);
            for (int i = 0; i < list.Count; i++)
            {
                if (dic_pathed.ContainsKey(list[i]))
                {
                    list.Remove(list[i]);
                    i--;
                }
            }
            for (int i = 0; i < list.Count; i++)
            {
                if (!sizer.Invoke(dic_map[list[i]]))
                {
                    list.Remove(list[i]);
                    i--;
                }
            }
            if (list.Count == 0)
            {
                return;
            }
            float dic = float.MaxValue;
            Vector3 next = Vector3.zero;
            foreach (var item in list)
            {
                var targetDic = GetDirction(toPoint, item);
                var length = targetDic;
                if (length < dic)
                {
                    dic = length;
                    next = item;
                }
                else if (length == dic)
                {
                    #region random
                    var range = UnityEngine.Random.Range(0.0f, 1.0f);
                    if (range >= 0.5f)
                    {
                        next = item;
                    }
                    #endregion
                    #region no random
                    //var hItemDic = Mathf.Abs(toPoint.x - item.x);
                    //var vItemDic = Mathf.Abs(toPoint.z - item.z);
                    //var hNextDic = Mathf.Abs(toPoint.x - next.x);
                    //var vNextDic = Mathf.Abs(toPoint.z - next.z);
                    //if (hItemDic > vItemDic)
                    //{
                    //    if (hItemDic < hNextDic)
                    //    {
                    //        next = item;
                    //    }
                    //}
                    //else
                    //{
                    //    if (vItemDic < vNextDic)
                    //    {
                    //        next = item;
                    //    }
                    //}
                    #endregion
                }
            }
            path.Add(next);
            dic_pathed.Add(next, true);
            Loop(next);
        }

        List<Vector3> GetPoint(Vector3 point)
        {
            List<Vector3> ret = new List<Vector3>();
            var left = point + new Vector3(-1, 0, 0);
            var right = point + new Vector3(1, 0, 0);
            var top = point + new Vector3(0, 0, -1);
            var bottom = point + new Vector3(0, 0, 1);
            if (dic_map.ContainsKey(left))
            {
                ret.Add(left);
            }
            if (dic_map.ContainsKey(right))
            {
                ret.Add(right);
            }
            if (dic_map.ContainsKey(top))
            {
                ret.Add(top);
            }
            if (dic_map.ContainsKey(bottom))
            {
                ret.Add(bottom);
            }
            return ret;
        }
        endEvent.Invoke(path);
        yield return 0;
    }

    public IEnumerator BreadthPath<T>(Vector3 point, Vector3 toPoint, Dictionary<Vector3, T> dic_map,
        Action startEvent,
        Func<T, bool> sizer,
        Action<List<Vector3>> endEvent)
    {
        
        startEvent.Invoke();

        Dictionary<FourTree, bool> dic_pathed = new Dictionary<FourTree, bool>();
        List<Vector3> endPath = new List<Vector3>();
        bool isEnd = false;

        FourTree parent = new FourTree(point, null);
        dic_pathed.Add(parent, true);

        var list = GetPoint(parent);
        Loop(list);


        void Loop(List<FourTree> list)
        {
            if (isEnd)
            {
                return;
            }

            List<FourTree> newList = new List<FourTree>();
            foreach (var item in list)
            {
                if (item.value == toPoint)
                {
                    isEnd = true;
                    endPath.Add(item.value);
                    FourTree p = item;
                    while (p.parent != null)
                    {
                        p = p.parent;
                        endPath.Add(p.value);
                    }
                    endPath.RemoveAt(endPath.Count - 1);
                    endPath.Reverse();
                    return;
                }
                var path = GetPoint(item);
                for (int i = 0; i < path.Count; i++)
                {
                    if (!dic_pathed.ContainsKey(path[i]) && sizer.Invoke(dic_map[path[i].value]))
                    {
                        dic_pathed.Add(path[i], true);
                    }
                    else
                    {
                        path.Remove(path[i]);
                        i--;
                    }
                }
                newList.AddRange(path);
            }
            if (newList.Count == 0)
            {
                return;
            }
            Loop(newList);
        }

        List<FourTree> GetPoint(FourTree p)
        {
            var point = p.value;
            List<FourTree> ret = new List<FourTree>();
            var t_left = point + new Vector3(-1, 0, 0);
            var t_right = point + new Vector3(1, 0, 0);
            var t_top = point + new Vector3(0, 0, -1);
            var t_bottom = point + new Vector3(0, 0, 1);
            if (dic_map.ContainsKey(t_left))
            {
                FourTree node = new FourTree(t_left, p);
                ret.Add(node);
                p.left = node;
            }
            if (dic_map.ContainsKey(t_right))
            {
                FourTree node = new FourTree(t_right, p);
                ret.Add(node);
                p.right = node;
            }
            if (dic_map.ContainsKey(t_top))
            {
                FourTree node = new FourTree(t_top, p);
                ret.Add(node);
                p.top = node;
            }
            if (dic_map.ContainsKey(t_bottom))
            {
                FourTree node = new FourTree(t_bottom, p);
                ret.Add(node);
                p.bottom = node;
            }
            return ret;
        }
        endEvent.Invoke(endPath);
        yield return 0;
    }
}
