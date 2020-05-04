using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerFight;
using Homebrew;
using UniRx;

namespace TowerFight
{
    public class Stand : ActorBase
    {
        private Dictionary<Transform, Unit> stands { get; set; } = new Dictionary<Transform, Unit>();
        private List<Transform> points { get; set; } = new List<Transform>();

        public Subject<Dictionary<string, object>> OnUnitClick { get; private set; } = new Subject<Dictionary<string, object>>();

        public bool Activate
        {
            get
            {
                if (stands.Count > 0)
                {

                    return points[0].gameObject.activeSelf;
                }

                else return false;
            }
            set
            {
                gameObject.SetActive(value);              
            }
        }

        public void SetTower(DataUnit dataUnit)
        {

            if (stands.Count > 0)
            {
                if (stands[points[0]])
                    Destroy(stands[points[0]].gameObject);
                stands[points[0]] = Instantiate(dataUnit.prefab, points[0].position, Quaternion.identity).GetComponent<Unit>();
                stands[points[0]].GetComponent<BoxCollider2D>().enabled = false;
                stands[points[0]].transform.SetParent(points[0]);
                stands[points[0]].AddTo(dataUnit);
                points[0].GetComponent<StandPoint>().dataUnit = stands[points[0]].GetData<DataUnit>();

            }
        }

        public void SetUnit(DataUnit dataUnit, int pos)
        {
            if (stands.Count > 0)
            {
                if (stands[points[pos + 1]])
                    Destroy(stands[points[pos + 1]]);
                stands[points[pos + 1]] = Instantiate(dataUnit.prefab, points[pos + 1].position, Quaternion.identity).GetComponent<Unit>();
                stands[points[pos + 1]].GetComponent<BoxCollider2D>().enabled = false;
                stands[points[pos + 1]].transform.SetParent(points[pos + 1]);
                stands[points[pos + 1]].AddTo(dataUnit);
                points[pos + 1].GetComponent<StandPoint>().dataUnit = dataUnit;                
            }
        }

        public void SetUnits(List<DataUnit> dataUnits, int startPos)
        {
            int count = 0;
            foreach (var item in dataUnits)
            {
                SetUnit(item, startPos + count);
                count++;
            }
        }

        public void RemoveUnit(int pos)
        {
            if (stands[points[pos + 1]])
                Destroy(stands[points[pos + 1]]);
        }
        

        public void UpdateStand()
        {
            foreach (var item in stands)
            {
                if (item.Value)
                    Destroy(item.Value.gameObject);
            }
            var gameManager = Toolbox.Get<GameManager>();
            SetUnits(gameManager.dataPlayer.squad, 0);
            SetTower(gameManager.dataPlayer.dataTower);
        }

        private void OnStandClick(DataUnit dataUnit,Transform transform)
        {
            var data = new Dictionary<string, object>();
            data.Add(typeof(DataUnit).FullName, dataUnit);
            data.Add(typeof(Transform).FullName, transform);
            OnUnitClick.OnNext(data);
        }

        protected override void OnStart()
        {

            var dataPoint = GetData<DataStandPoint>();
            if (dataPoint)
            {                
                foreach (var item in dataPoint.standPoints)
                {
                    var point = Instantiate(item);
                    point.SetParent(transform);
                    var standPoint = point.GetComponent<StandPoint>();
                    standPoint.OnTabsUnit.AddListener(OnStandClick);
                   
                    stands.Add(point, null);
                    points.Add(point);
                }
                
            }
            var gameManager = Toolbox.Get<GameManager>();
            SetUnits(gameManager.dataPlayer.squad,0);
            SetTower(gameManager.dataPlayer.dataTower);

            gameManager.ToMenu();
        }



        public void ActivateUnit(DataUnit dataUnit)
        {
            if(dataUnit)
                foreach (var item in points)                
                    if (stands[item])
                        if (!stands[item].GetData<DataUnit>().stats.name.Equals(dataUnit.stats.name))                        
                            item.gameObject.SetActive(false);                        
                
        }

        public void ActivateAllUnit( bool value)
        {
            foreach (var item in points)
            {
                item.gameObject.SetActive(value);
            }
        }

    }
}