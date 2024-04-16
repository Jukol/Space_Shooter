using UnityEditor;
using UnityEngine;

namespace Infrastructure.Editor
{
    public class Tools
    {
        [MenuItem("Tools/Clear Player Prefs")]
        public static void ClearPrefs()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }
}
