using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;
using System;
using Homebrew;

namespace TowerFight
{
    public abstract class ActorBase : MonoBehaviour, IAwake
    {
        [SerializeField]
        public List<DataBase> data = new List<DataBase>();
        [SerializeField]
        public List<BehaviourBase> behaviours = new List<BehaviourBase>();

        private Dictionary<Type, BehaviourBase> behaviourTypes = new Dictionary<Type, BehaviourBase>();
        private Dictionary<Type, DataBase> dataTypes = new Dictionary<Type, DataBase>();

        public bool debug = false;

        public void OnAwake()
        {
            if(debug)
                Debug.Log($"{GetType()}");

            foreach (var item in data)
            {
                if (item == null)
                    continue;

                if (!dataTypes.ContainsKey(item.GetType()))
                    dataTypes.Add(item.GetType(), item);
            }

            foreach (var item in behaviours)
            {
                if (item == null)
                    continue;

                var behaviour = ScriptableObject.CreateInstance(item.GetType()) as BehaviourBase;//item;

                behaviour.AddActor(this);

                if (behaviour is ITick
                    || behaviour is ITickFixed
                    || behaviour is ITickLate)
                {
                    ManagerUpdate.AddTo(behaviour);
                }

                behaviourTypes.Add(behaviour.GetType(), behaviour);

                if (behaviour is IAwake)
                    (behaviour as IAwake).OnAwake();
            }

            if (this is ITick
                    || this is ITickFixed
                    || this is ITickLate)
                ManagerUpdate.AddTo(this);


            OnStart();
        }

        protected virtual void OnStart()
        {
        } 
            
        public void AddTo<T>(T item)
        {
            if (item is DataBase)
            {
                var add = Instantiate(item as DataBase);
                data.Add(add);
                if (!dataTypes.ContainsKey(item.GetType()))
                    dataTypes.Add(item.GetType(), add);
            }

            if (item is BehaviourBase)
            {
                var add = Instantiate(item as BehaviourBase);
                behaviours.Add(add);
                if (!behaviourTypes.ContainsKey(item.GetType()))
                    behaviourTypes.Add(item.GetType(), add);
            }
        }


        public void RemoveFrom<T>(T item)
        {
            if (item is DataBase)
            {             
                if (!dataTypes.ContainsKey(item.GetType()))
                    dataTypes.Remove(item.GetType());
            }

            if (item is BehaviourBase)
            {               
                if (!behaviourTypes.ContainsKey(item.GetType()))
                    behaviourTypes.Remove(item.GetType());
            }
        }


        public T GetData<T>() where T : DataBase
        {
            DataBase resolve;
            dataTypes.TryGetValue(typeof(T), out resolve);
            return (T)resolve;
        }

        public T GetBehaviour<T>() where T : BehaviourBase
        {
            BehaviourBase resolve;
            behaviourTypes.TryGetValue(typeof(T), out resolve);
            return (T)resolve;
        }


        private void OnDestroy()
        {
            if (this is ITick
                     || this is ITickFixed
                     || this is ITickLate)
            {
                if(Toolbox.Instance)
                    ManagerUpdate.RemoveFrom(this);
            }
            foreach (var item in behaviourTypes)
            {
                Destroy(item.Value);
            }
        }
    }
}