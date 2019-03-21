using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Com.Capra314Cabra.Project_2048Ex
{
    public class BlockManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject blockModel;

        #region  Material Fields

        [Header("Materials")]
        [SerializeField]
        private Material materialColor0;
        [SerializeField]
        private Material materialColor2;
        [SerializeField]
        private Material materialColor4;
        [SerializeField]
        private Material materialColor8;
        [SerializeField]
        private Material materialColor16;
        [SerializeField]
        private Material materialColor32;
        [SerializeField]
        private Material materialColor64;
        [SerializeField]
        private Material materialColor128;
        [SerializeField]
        private Material materialColor256;
        [SerializeField]
        private Material materialColorMore;

        #endregion

        private MeshRenderer meshRenderer;

        private const int MAX_VALUE_OF_BLOCK = 256;
        private Dictionary<int, Material> materialCatalog = new Dictionary<int, Material>();

        // Start is called before the first frame update
        void Start()
        {
            meshRenderer = blockModel.GetComponent<MeshRenderer>();
            InitMaterials();
        }

        void InitMaterials()
        {
            materialCatalog.Add(0, materialColor0);
            materialCatalog.Add(2, materialColor2);
            materialCatalog.Add(4, materialColor4);
            materialCatalog.Add(8, materialColor8);
            materialCatalog.Add(16, materialColor16);
            materialCatalog.Add(32, materialColor32);
            materialCatalog.Add(64, materialColor64);
            materialCatalog.Add(128, materialColor128);
            materialCatalog.Add(256, materialColor256);
            materialCatalog.Add(-1, materialColorMore);
        }

        public void ChangeMaterial(int val)
        {
            val = val > MAX_VALUE_OF_BLOCK ? -1 : val;
            meshRenderer.material = materialCatalog[val];
        }
    }
}
