using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityDemo.Utils;
using UnityDemo.Managers;

namespace UnityDemo.World
{
    /// <summary>
    /// 基本类型
    /// </summary>
    public class BaseModel
    {
        public delegate void OnCreateModelComplete();

        #region public
        public BaseModel(BaseData data)
        {
            mData = data;
            isLoadingModel = false;
            gameObject = null;
            mEquipDict = new Dictionary<int, EquipModel>();
            mReplaceEquipDict = new Dictionary<int, EquipModel>();
            mNakeBonesDict = new Dictionary<int, BonesData>();
        }

        public void AsyncCreateModel(OnCreateModelComplete onComplete)
        {
            if (isLoadingModel)
            {
                DebugInfo.Log("[BaseModel] AsyncCreateModel Called Aagin!!");
                return;
            }

            isLoadingModel = true;
            //TODO：read table.
            //model_configItem itemProto = model_config.Instance.Find((int)mData.GetValue(AttrKeyDefine.ATTR_DISPLAYID_ENTITY).ToString());
            //string prefab = itemProto.prefab;
            string prefab = "";

           /* Global.assetMgr.LoadAsset(Global.pathMgr.GetCreatureModelPrefab(prefab), 
                (name, obj)=>
                {
                    if (!isLoadingModel)
                    {
                        Global.assetMgr.Release(name);
                        return;
                    }

                    gameObject = GameObject.Instantiate(obj) as GameObject;
                   
                    InitNakedBones();
                    float scale = (float)mData.GetValue(AttrKeyDefine.ATTR_MODEL_SCALE);
                    if (scale > 0)
                    {
                        gameObject.transform.localScale = new Vector3(scale, scale, scale);
                    }

                    SetSkinnedMeshRendererActive(false);

                    InitAllEquips(() =>
                        {
                            SetSkinnedMeshRendererActive(true);
                            isLoadingModel = false;
                            onComplete();
                        });
                });*/

        }

        public void UpdateEquip(int key, int protoid, Action<bool> onComplete)
        {
            if (protoid == 0) return;
            if (IsHaveEquip(key, protoid))
            {
                return;
            }

            mReplaceEquipDict[key] = mEquipDict[key];
            mData.Update(key, protoid);
            var equip = EquipModel.Create(key, protoid);
            mEquipDict[equip.key] = equip;

            AddEquip(equip, (value) =>
                {
                    //delete old equip.
                    RemoveEquip(mReplaceEquipDict[key]);
                    mReplaceEquipDict.Remove(key);

                    onComplete(value);
                });
        }

        public void RemoveEquip(int key)
        {
            EquipModel equip;
            if (mEquipDict.TryGetValue(key, out equip))
            {
                mData.Update(key, 0);
                RemoveEquip(equip);
                mEquipDict.Remove(key);
            }
        }
        #endregion

        #region private mothod
        //private static BaseData MakeModelBaseData(int modelId, float scale, int head, int arm, int armor, int shoulder, int leg, int shoes, int weapon, int wing)
        //{
        //    var data = new BaseData();
        //    data.Init(AttrKeyDefine.ATTR_DISPLAYID_ENTITY, modelId);
        //    data.Init(AttrKeyDefine.ATTR_MODEL_SCALE, scale);
        //    data.Init(AttrKeyDefine.ATTR_DISPLAYID_HEAD, head);
        //    data.Init(AttrKeyDefine.ATTR_DISPLAYID_ARM, arm);
        //    data.Init(AttrKeyDefine.ATTR_DISPLAYID_ARMOR, armor);
        //    data.Init(AttrKeyDefine.ATTR_DISPLAYID_SHOULDER, shoulder);
        //    data.Init(AttrKeyDefine.ATTR_DISPLAYID_LEG, leg);
        //    data.Init(AttrKeyDefine.ATTR_DISPLAYID_SHOES, shoes);
        //    data.Init(AttrKeyDefine.ATTR_DISPLAYID_WEAPON, weapon);
        //    data.Init(AttrKeyDefine.ATTR_DISPLAYID_WING, wing);
        //    return data;
        //}

        /// <summary>
        /// 初始化NakedBones
        /// </summary>
        private void InitNakedBones()
        {
            string partName;
            for (int key = AttrKeyDefine.ATTR_DISPLAYID_PART_MIN; key <= AttrKeyDefine.ATTR_DISPLAYID_PART_MAX; key += 2)
            {
                if (key == AttrKeyDefine.ATTR_DISPLAYID_WING)
                    continue;

                partName = ModelDefine.GetSlotName(key);
                var equipPart = GameUtils.GetChild(gameObject.transform, partName);
                if (equipPart == null) continue;

                var renderer = equipPart.GetComponent<SkinnedMeshRenderer>();
                renderer.castShadows = false;
                renderer.receiveShadows = false;
                renderer.useLightProbes = true;

                var bonesData = new BonesData();
                bonesData.material = renderer.sharedMaterial;
                bonesData.mesh = renderer.sharedMesh;
                bonesData.bones = GetBones(gameObject, renderer);
                mNakeBonesDict[key] = bonesData;
            }
        }

        /// <summary>
        /// 初始化装备
        /// </summary>
        /// <param name="onComplete"></param>
        private void InitAllEquips(Action<bool> onComplete)
        {
            if (mEquipDict.Count != 0)
            {
                DebugInfo.LogError("[BaseModel.InitAllEquips] equips already inited.");
                return;
            }

            int total = 0;
            int finish = 0;
            for (int key = AttrKeyDefine.ATTR_DISPLAYID_PART_MIN; key <= AttrKeyDefine.ATTR_DISPLAYID_PART_MAX; key += 2)
            {
                if ((int)mData.GetValue(key) != 0)
                {
                    total++;
                    AddEquip(EquipModel.Create(key, (int)mData.GetValue(key)),
                        (bool value) =>
                        {
                            ++finish;
                            if (finish >= total){
                                onComplete(value);
                            }
                        });
                }
            }
        }

        /// <summary>
        /// 添加一件装备
        /// </summary>
        /// <param name="equip"></param>
        private void AddEquip(EquipModel equip, Action<bool> onComplete = null)
        {
            string slotName = ModelDefine.GetSlotName(equip.key);
            var equipSlot = GameUtils.GetChild(gameObject.transform, slotName);
            if (equipSlot == null)
            {
                DebugInfo.LogError(String.Format("cann't find equip slot:{0}, key:{1}", slotName, equip.key));
                return;
            }

            //model_configItem itemProto = model_config.Instance.Find((int)mData.GetValue(AttrKeyDefine.ATTR_DISPLAYID_ENTITY).ToString());
            //string prefab = itemProto.fbx;
            string prefab = "";
            AssetManager.Instance.LoadAsset(PathManager.Instance.GetEquipModelPrefab(prefab), 
                (name, obj)=>
                {
                    if (!IsHaveEquip(equip.key, equip.protoid))  //已经被卸掉
                    {
                        AssetManager.Instance.Release(name);
                        return;
                    }

                    GameObject TmpObj = GameObject.Instantiate(obj) as GameObject;
                    equip.equipPartList.Add(TmpObj);
                    equip.renderer = TmpObj.transform.GetComponent<SkinnedMeshRenderer>();
                    equip.material = equip.renderer.sharedMaterial;

                    if (equip.isMaterial)
                    {
                        var renderer = equipSlot.GetComponent<SkinnedMeshRenderer>();
                        renderer.castShadows = false;
                        renderer.receiveShadows = false;
                        renderer.useLightProbes = true;
                        renderer.sharedMaterial = equip.material;
                        renderer.sharedMesh = equip.renderer.sharedMesh;
                        renderer.bones = GetBones(gameObject, equip.renderer);
                    }else
                    {
                        for (int index = 0; index < equip.equipPartList.Count; ++index )
                        {
                            string partSlot = ModelDefine.GetSlotName(equip.key, index);
                            var equipPartSlot = GameUtils.GetChild(gameObject.transform, partSlot);
                            if (equipPartSlot == null)
                            {
                                DebugInfo.LogError(String.Format("cann't find equip part slot:{0}, key:{1}", partSlot, equip.key));
                                continue;
                            }

                            var equipPart = equip.equipPartList[index];
                            Vector3 scale = equipPart.transform.localScale;
                            equipPart.transform.parent = equipPartSlot;
                            equipPart.transform.localPosition = Vector3.zero;
                            equipPart.transform.localEulerAngles = Vector3.zero;
                            equipPart.transform.localScale = scale;
                        }
                    }

                    if (onComplete != null)
                        onComplete(true);
                }, null);
        }

        private void RemoveEquip(EquipModel equip)
        {
            string slotName = ModelDefine.GetSlotName(equip.key);
            var equipSlot = GameUtils.GetChild(gameObject.transform, slotName);
            if (equipSlot == null)
            {
                DebugInfo.LogError(String.Format("cann't find equip slot:{0}, key:{1}", slotName, equip.key));
                return;
            }

            //替换材质
            if (equip.isMaterial)
            {
                BonesData bonesData;
                if (!mNakeBonesDict.TryGetValue(equip.key, out bonesData))
                {
                    return;
                }

                var renderer = equipSlot.GetComponent<SkinnedMeshRenderer>();
                renderer.sharedMaterial = bonesData.material;
                renderer.sharedMesh = bonesData.mesh;
                renderer.bones = bonesData.bones;
            }
            else
            {
                for (int index = 0; index < equip.equipPartList.Count; ++index)
                {
                    var equipPart = equip.equipPartList[index];
                    equipPart.transform.parent = null;
                    NGUITools.Destroy(equipPart);
                }
            }
            ReleaseEquip(equip);
        }

        /// <summary>
        /// 判断是否有某个装备
        /// </summary>
        /// <param name="key"></param>
        /// <param name="protoid"></param>
        /// <returns></returns>
        private bool IsHaveEquip(int key, int protoid)
        {
            EquipModel equip;
            if (mEquipDict.TryGetValue(key, out equip))
            {
                if (equip.protoid == protoid)
                    return true;
            }

            return false;
        }

        private void SetSkinnedMeshRendererActive(bool value, bool includeInactive = true) 
        {
            foreach (var renderer in gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive))
            {
                renderer.enabled = value;
            }
        }

        private Transform[] GetBones(GameObject gameObject, SkinnedMeshRenderer renderer)
        {
            List<Transform> bones = new List<Transform>();
            for (int i = 0; i < renderer.bones.Length; i++)
            {
                bones.Add(GameUtils.GetChild(gameObject.transform, renderer.bones[i].name));
            }

            return bones.ToArray();
        }

        private void ReleaseEquip(EquipModel equip)
        {
            //TODO:release.
            equip = null;
        }
        #endregion

        #region variable
        private BaseData mData;
        private Dictionary<int, EquipModel> mEquipDict;             //装备列表
        private Dictionary<int, EquipModel> mReplaceEquipDict;      //替换装备列表
        private Dictionary<int, BonesData> mNakeBonesDict;          //原模型骨骼信息

        public GameObject gameObject { get; private set; }
        public bool isLoadingModel { get; private set; }
        #endregion

        #region nested classes
        //SkinnedMeshRenderer 对应的信息
        class BonesData
        {
            public Mesh mesh;           //网格
            public Material material;   //材质
            public Transform[] bones;   //骨骼 骨骼模型对应的SkinnedMeshRenderer组件的bones信息
        }

        class EquipModel
        {
            public int key;                            //属性key：ARM/SHOULDER/...
            public int protoid;                        //模板id
            public int loadingNum;                     //当前正在加载个数
            public Material material;
            public SkinnedMeshRenderer renderer;        //蒙皮网格渲染器
            public List<GameObject> equipPartList;      //装备挂件
            public bool isLoaded;                       //是否已经加载
      
            //材质类型,其他为model
            public bool isMaterial
            {
                get { return key != AttrKeyDefine.ATTR_DISPLAYID_WING; }
            }

            public static EquipModel Create(int key, int protoid)
            {
                var data = new EquipModel();
                data.key = key;
                data.protoid = protoid;
                data.loadingNum = -1;
                data.material = null;
                data.renderer = null;
                data.equipPartList = new List<GameObject>();
                return data;
            }
        }

        #endregion
    }
}
