using UnityEngine;

namespace EnemyScripts
{
    [CreateAssetMenu(fileName = "New Ship", menuName = "Enemy Ship")]
    public class EnemyScriptableObject : ScriptableObject
    {
        public int wounded;
        public int damaged;
    }
}
