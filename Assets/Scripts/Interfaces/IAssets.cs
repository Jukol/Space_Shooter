using UnityEngine;

namespace Interfaces
{
    public interface IAssets : IService
    {
        GameObject Instantiate(GameObject prefab);
        GameObject Instantiate(string path);

        GameObject Instantiate(string path, Transform parent, Quaternion angle);
    }
}
