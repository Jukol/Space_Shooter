using UnityEngine;
namespace Infrastructure.AssetManagement

{
    public class AssetProvider : IAssets
    {
        public GameObject Instantiate(string path)
        {
            GameObject prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab);
        }

        public GameObject Instantiate(string path, Vector2 at)
        {
            GameObject prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab, at, Quaternion.identity);
        }


        public GameObject Instantiate(string path, Transform parent, Quaternion angle)
        {
            GameObject prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab, parent.position, angle);
        }
    }
}
