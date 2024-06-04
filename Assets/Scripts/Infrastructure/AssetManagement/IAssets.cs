using Infrastructure.Services;
using UnityEngine;
namespace Infrastructure.AssetManagement
{
    public interface IAssets : IService
    {
        GameObject Instantiate(string path);

        GameObject Instantiate(string path, Vector2 at);

        GameObject Instantiate(string path, Transform parent, Quaternion angle);
    }
}
