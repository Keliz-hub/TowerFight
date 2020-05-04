using System.Collections.Generic;
using TowerFight;
using UnityEngine;
namespace Homebrew
{
    [CreateAssetMenu(fileName = "ManagerUnits", menuName = "Managers/ManagerUnits")]

    public class ManagerUnits : ManagerBase, IAwake
    {
        public DataUnits unitBase;

        public static ManagerUnits instans { get { return Toolbox.Get<ManagerUnits>(); } }

        public Transform rightPointPrefab;
        public Transform leftPointPrefab;

        private float yMaxValue = -1.6f;
        private float yMinValue = -2.4f;

        public List<Unit> left { get; private set; } = new List<Unit>();
        public List<Unit> right { get; private set; } = new List<Unit>();
        public Transform rightPoint { get; private set; }
        public Transform leftPoint { get; private set; }



        public LayerMask rightUnitMask;
        public LayerMask leftUnitMask;



        public void OnAwake()
        {
            rightPoint = Instantiate(rightPointPrefab) as Transform;
            leftPoint = Instantiate(leftPointPrefab) as Transform;


        }
        public static GameObject Spawn(DataUnit data, Side team)
        {
            if (!data)
            {
                Debug.LogError("Trying to spawn unit but DataUnit is null");
                return null;
            }
            var manager = instans;
            var roadpoint = RoadPoint(team);
            var gameObject = Instantiate(data.prefab, roadpoint, Quaternion.identity);

            var unit = gameObject.GetComponent<Unit>();

            var stats = new Stats(data.stats, team);

            var newdata = CreateInstance(typeof(DataUnit)) as DataUnit;
            newdata.SetData(data, stats);
            newdata.layer = team == Side.right ? manager.rightUnitMask : manager.leftUnitMask;

            gameObject.layer = team == Side.right ? LayerMaskToLayer(manager.rightUnitMask) : LayerMaskToLayer(manager.leftUnitMask);
            unit.AddTo(newdata);


            if (team == Side.left)
                manager.left.Add(unit);
            else
                manager.right.Add(unit);

            unit.OnAwake();
            return gameObject;
        }


        public static RangeUnit SpawnTower(DataUnit data, DataPlayer player)
        {
            if (!data)
            {
                Debug.LogError("Trying to spawn tower but DataUnit is null");
                return null;
            }
            var manager = instans;
            var gameObject = Instantiate(data.prefab, GetPoint(player.side), Quaternion.identity);

            var unit = gameObject.GetComponent<RangeUnit>();

            var stats = new Stats(data.stats, player.side);

            var newdata = CreateInstance(typeof(DataUnit)) as DataUnit;
            newdata.SetData(data, stats);
            newdata.layer = player.side == Side.right ? manager.rightUnitMask : manager.leftUnitMask;
            newdata.stats.hitPoint = newdata.stats.level;

            gameObject.layer = player.side == Side.right ? LayerMaskToLayer(manager.rightUnitMask) : LayerMaskToLayer(manager.leftUnitMask);
            unit.AddTo(newdata);
            unit.AddTo(player);


            unit.OnAwake();
            return unit;
        }


        public static int LayerMaskToLayer(LayerMask layerMask) => (int)Mathf.Log(layerMask.value, 2);


        public static void DestroyAllUnits()
        {
            var manager = instans;
            foreach (var item in manager.left)
            {
              
                Destroy(item.gameObject);
            }
            manager.left.Clear();

            foreach (var item in manager.right)
            {
                Destroy(item.gameObject);
            }
            manager.right.Clear();
        }



        public static void Remove(Unit unit)
        {
            var manager = instans;
            if (unit.GetData<DataUnit>())
                if (unit.GetData<DataUnit>().stats.side == Side.left)
                    manager.left.Remove(unit);
                else
                    manager.right.Remove(unit);
        }

        private static Vector3 RoadPoint(Side side)
        {
            float y = Random.Range(instans.yMinValue, instans.yMaxValue);

            return new Vector3(GetPoint(side).x, y, y);
        }

        public static Vector2 GetPoint(Side side)
        {
            var manager = instans;
            return side == Side.left ? manager.leftPoint.position : manager.rightPoint.position;
        }
    }
}