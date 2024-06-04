using UnityEngine;

namespace EnemyScripts
{
    [CreateAssetMenu(fileName = "New Ship", menuName = "Enemy Ship")]
    public class EnemyScriptableObject : ScriptableObject
    {
        public string shipName;
        public float speed;
        public int wounded;
        public int damaged;
    }
}
