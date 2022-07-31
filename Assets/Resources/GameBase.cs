using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameBase : PointBase
{

    //tween
    Tween tween_move = null;

    public void SetUpPoint(Vector3 point)
    {
        SetPoint(point);
        var wap = MapManager.Instance.GetWapToPoint(point);
        var position = wap.GetPosition();
        tween_move?.Kill();
        tween_move = transform.DOMove(position, 0.3f, false).SetEase(Ease.Linear);
        tween_move.Play();
    }

    public void StopMove()
    {
        //targetPoints = new List<Vector3>();
    }
    List<Vector3> targetPoints = new List<Vector3>();
    bool isMove = false;
    int index_move = 0;
    public void StartMove(List<Vector3> path, Dictionary<Vector3, Map_Wap> dic_map)
    {
        if (isMove)
        {
            targetPoints.RemoveRange(index_move, targetPoints.Count - index_move - 1);
            targetPoints.AddRange(path);
            return;
        }
        index_move = 0;
        Vector3 endPos = Vector3.zero;
        targetPoints = path;
        Move();

        void Move()
        {
            if (index_move >= targetPoints.Count)
            {
                return;
            }
            var point = targetPoints[index_move];
            SetPoint( point);
            endPos = dic_map[point].GetPosition();
            transform.DOMove(endPos, 0.5f, false)
                .SetEase(Ease.Linear)
                .OnStart(() =>
                {
                    isMove = true;
                })
                .OnComplete(() =>
                {
                    index_move++;
                    isMove = false;
                    Move();
                });
            //点成求角度
            float angle = 0.0f;

            //dot
            Vector3 dir = (endPos - transform.position).normalized;
            //Vector3 playerDir = transform.TransformDirection(new Vector3(0,0,1));
            float dot = Mathf.Acos(Vector3.Dot(dir, Vector3.forward)) * Mathf.Rad2Deg;

            //cross
             Vector3 cross = Vector3.Cross(Vector3.forward, dir);
            cross.y = cross.y < 0.01f && cross.y > -0.01f ? 0.01f : cross.y;
            float dic = cross.y / Mathf.Abs(cross.y);

            angle = dot * dic;

            angle = Base.Instance.GetAngle( Base.Dirction.Forward, endPos - transform.position);

            transform.DORotate(new Vector3(0, angle, 0), 0.3f).SetEase(Ease.Linear);
            //--

        }
    }
}
