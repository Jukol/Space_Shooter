using Infrastructure.Services;
using UnityEngine;
namespace Background
{
    public class BackgroundCompositor : MonoBehaviour
    {
        [SerializeField] private int partsInLayer;
        [SerializeField] private GameObject[] prefabs, layers;

        private IBackgroundAdjuster _adjuster;
        private float _height;

        private GameObject[][] _backgroundArray;
        
        private float _offset;
        private float _screenHeight;
        private float _screenWidth;

        private void Awake()
        {
            _adjuster = AllServices.Container.Single<IBackgroundAdjuster>();
            _height = _adjuster.Height;
            _offset = _adjuster.VerticalOffset;
        }

        private void Start() => 
            ComposeBackgrounds();

        private void ComposeBackgrounds()
        {
            GameObject[][] bgrArray = CreateBackgroundArray();
            
            for (int i = 0; i < prefabs.Length; i++)
            {
                InitiateBackgrounds(prefabs[i], layers[i], bgrArray[i]);
                GetBackgroundsToStartPosition(bgrArray[i]);
            }
        }

        private GameObject[][] CreateBackgroundArray()
        {
            GameObject[][] bgrArray = new GameObject[prefabs.Length][];

            for (int i = 0; i < prefabs.Length; i++) 
                bgrArray[i] = new GameObject[partsInLayer];

            return bgrArray;
        }

        private void InitiateBackgrounds(GameObject bgrType, GameObject layer, GameObject[] bgrArray)
        {
            for (int i = 0; i < partsInLayer; i++)
            {
                bgrArray[i] = Instantiate(bgrType, layer.transform);
                bgrArray[i].GetComponent<IMoveUppable>().Init();
                bgrArray[i].GetComponent<IResizable>().Resize();
            }
        }

        
        private void GetBackgroundsToStartPosition(GameObject[] composedBackground)
        {
            for (int i = 0; i < composedBackground.Length; i++)
            {
                if (i == 0)
                    composedBackground[i].transform.position = new Vector2(0, -_offset);
                else
                    composedBackground[i].transform.position = new Vector2(0, composedBackground[i - 1].transform.position.y + _height);
            }
        }
    }
}
