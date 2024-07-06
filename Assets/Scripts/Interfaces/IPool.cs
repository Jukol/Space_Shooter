using UnityEngine;

namespace Interfaces
{
    public interface IPool : IService
    {
        public GameObject Request();
    }
}
