using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ConsoleShell
{
    [System.Serializable]
    public class TextsPool
    {
        [SerializeField] private ConsoleLine prefab;
        [SerializeField] private int poolCount = 50;
        private Transform holder;
        private List<ConsoleLine> pool = new List<ConsoleLine>(50);
        private List<ConsoleLine> spawned = new List<ConsoleLine>(50);

        public List<ConsoleLine> Spawned => spawned;

        public void Init()
        {
            this.holder = prefab.transform.parent;
            prefab.gameObject.SetActive(false);
            for (int i = 0; i < poolCount; i++)
            {
                AddNewItem(holder);
            }
        }

        private void AddNewItem(Transform holder)
        {
            var obj = Object.Instantiate(prefab, holder);
            obj.gameObject.SetActive(false);
            pool.Add(obj);
        }

        public ConsoleLine Get()
        {
            if (pool.Count == 0)
            {
                AddNewItem(holder);
            }

            if (pool.Count != 0)
            {
                var item = pool[0];
                pool.RemoveAt(0);
                Spawned.Add(item);
                return item;
            }

            return null;
        }


        public void ClearAllSpawnedButtons()
        {
            for (int i = 0; i < Spawned.Count; i++)
            {
                pool.Add(Spawned[i]);
                Spawned[i].gameObject.SetActive(false);
            }

            Spawned.Clear();

        }
    }
}