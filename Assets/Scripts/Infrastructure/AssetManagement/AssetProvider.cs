using UnityEngine;
using Zenject;

namespace Infrastructure.AssetManagement

{
    public class AssetProvider : IAssets
    {
        [Inject] private DiContainer _container;

        [Inject]
        public AssetProvider()
        {
            
        }
        public GameObject Instantiate(string path)
        {
            GameObject prefab = Resources.Load<GameObject>(path);
            return _container.InstantiatePrefab(prefab);
        }

        public GameObject Instantiate(string path, Vector2 at)
        {
            GameObject prefab = Resources.Load<GameObject>(path);
            var obj = _container.InstantiatePrefab(prefab);
            obj.transform.position = at;
            obj.transform.rotation = Quaternion.identity;
            return obj;
        }


        public GameObject Instantiate(string path, Transform parent, Quaternion angle)
        {
            GameObject prefab = Resources.Load<GameObject>(path);
            var obj = _container.InstantiatePrefab(prefab);
            obj.transform.parent = parent;
            obj.transform.rotation = angle;
            return obj;
        }
    }
}
