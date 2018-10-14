//Unity
using UnityEngine;

//Odin
using Sirenix.OdinInspector;

//C#
using System;
using System.Collections.Generic;

//Game
using Common.Util;
using Gameplay.Inventory;

namespace Gameplay.Loot
{
    [ExecuteInEditMode]
    [CreateAssetMenu(fileName = "Loot Config", menuName = "Gameplay/Loot Config")]
    public class LootConfig : SerializedScriptableObject
    {
        [Serializable]
        public class LootDef
        {
            [ReadOnly]
            public Loot.Rarity rarity;
            public Color color;
            [Range(0,25)]
            [Tooltip("Amount to warm in pool for this element")]
            public int pooledWarmAmount = 5;
            [ListDrawerSettings(ShowPaging = false, DraggableItems = true,Expanded =false,ListElementLabelName = "name")]
            public List<LootItemDef> lootItems;

            //private string name;

            public LootDef(Loot.Rarity rarity, string name, Color color)
            {
                this.rarity = rarity;
                this.color = color;
                //this.name = name;
            }
        }

        [Serializable]
        public class LootItemDef
        {
            public Storeable.Type type;
            public string name;
            public BaseLootItem prefab;

            [SerializeField, HideInInspector]
            private int min;
            [SerializeField, HideInInspector]
            private int max;

            [Tooltip("Should this item give you more than one of this item")]
            public bool multipleQuantities;

            [ShowIf("multipleQuantities")]
            [ShowInInspector, MinMaxSlider(1, 100)]
            private Vector2 quantity
            {
                get
                {
                    return new Vector2(min, max);
                }
                set
                {
                    min = (int)value.x;
                    max = (int)value.y;
                }
            }

            public int GetQuantitiy()
            {
                if(multipleQuantities)
                {
                    return UnityEngine.Random.Range(min, max+1);
                }
                return 1;
            }
        }

        [Serializable]
        public class LootDropDef
        {
            [SerializeField, HideInInspector]
            private int min;
            [SerializeField, HideInInspector]
            private int max;

            [ShowInInspector, MinMaxSlider(0, 15)]
            public Vector2 randomDropRange
            {
                get
                {
                    return new Vector2(min, max);
                }
                set
                {
                    min = (int)value.x;
                    max = (int)value.y;
                }
            }

            [Space(10)]

            [ListDrawerSettings(HideAddButton = true, ShowPaging = true, DraggableItems = false)]
            [OnInspectorGUI("OnProbUpdate", append: false)]
            public List<ProbabilityDef> probabilities;

            #region Loot Drop Def Editor
            #if UNITY_EDITOR

            private void OnProbUpdate()
            {
                if (Event.current.type == EventType.Repaint)
                {
                    Loot.Rarity[] probTypes = Enum.GetValues(typeof(Loot.Rarity)) as Loot.Rarity[];
                    if (probabilities.Count != probTypes.Length)
                    {
                        GenerateProbabilities(probTypes);
                    }
                }

                //sum all elements
                float sum = 0;
                int probCount = probabilities.Count;
                int i = 0;
                for (i = 0; i < probCount; i++)
                {
                    sum += probabilities[i].probability;
                }

                ProbabilityDef def = null;
                //divide elements by sum
                for (i = 0; i < probCount; i++)
                {
                    def = probabilities[i];
                    def.probability = def.probability / sum;
                }
            }

            private void GenerateProbabilities(Loot.Rarity[] probTypes)
            {
                probabilities = new List<ProbabilityDef>();
                int index = 0;
                foreach (Loot.Rarity type in probTypes)
                {
                    float prob = index == 0 ? 1 : 0;
                    ProbabilityDef def = new ProbabilityDef(type, prob);
                    probabilities.Add(def);
                    index++;
                }
            }
            #endif
            #endregion
        }

        [Serializable]
        public class ProbabilityDef
        {
            [ReadOnly]
            [GUIColor(1f, 0.5f, 0f, 1f)]
            public Loot.Rarity rarity;
            [Range(0, 1)]
            public float probability;

            public ProbabilityDef(Loot.Rarity rare, float prob)
            {
                rarity = rare;
                probability = prob;
            }
        }

        [Serializable]
        public class LootBoomDef
        {
            [Range(0,10)]
            public float boomHeight = 3;
            [Range(0,2000)]
            public float boomForce = 350;
        }

        [ListDrawerSettings(HideAddButton = true, ShowPaging = false, DraggableItems = false)]
        [OnInspectorGUI("OnLootDefUpdate", append: false)]
        [SerializeField]
        public List<LootDef> lootDefs;

        public LootBoomDef lootBoomDef;

        [Range(5, 25)]
        public float autoDestroyTime = 20;

        [Range(0.01f, 5f)]
        public float collectRange = 2f;

        [Tooltip("Times per frame to refresh")]
        public int refreshRate = 3;

        [Range(0, 1000)]
        public float playerAttractDistance = 150;

        [Range(0, 35)]
        public float playerAttractSpeed = 8;

        #region Loot Defs Editor
#if UNITY_EDITOR
        private void OnLootDefUpdate()
        {
            if (Event.current.type == EventType.Repaint)
            {
                Loot.Rarity[] probTypes = Enum.GetValues(typeof(Loot.Rarity)) as Loot.Rarity[];
                if (lootDefs.Count != probTypes.Length)
                {
                    GenerateDefs(probTypes);
                }
            }
        }

        private void GenerateDefs(Loot.Rarity[] probTypes)
        {
            lootDefs = new List<LootDef>();
            foreach (Loot.Rarity type in probTypes)
            {
                LootDef def = new LootDef(type, type.ToString(), Color.white);
                lootDefs.Add(def);
            }
        }
        #endif
        #endregion

        #region Lookups
        private Dictionary<Loot.Rarity, LootDef> _lootDefLookup = new Dictionary<Loot.Rarity, LootDef>(new FastEnumIntEqualityComparer<Loot.Rarity>());
        private void GenerateLootDefLookup()
        {
            int i = 0;
            int particleLength = lootDefs.Count;
            for (i = 0; i < particleLength; i++)
            {
                LootDef blueprint = lootDefs[i];
                Loot.Rarity type = blueprint.rarity;
                if (!_lootDefLookup.ContainsKey(type))
                {
                    _lootDefLookup.Add(type, blueprint);
                }
            }
        }

        public LootDef GetLootDef(Loot.Rarity key)
        {
            if(_lootDefLookup.Count == 0)
            {
                GenerateLootDefLookup();
            }

            if(_lootDefLookup.ContainsKey(key))
            {
                return _lootDefLookup[key];
            }
            return null;
        }
        #endregion
    }

    public class Loot
    {
        public enum Rarity
        {
            Common = 0,
            Uncommon = 1,
            Rare = 2,
            Legendary = 3,
            Exotic = 4
        }

        public enum State
        {
            Boom = 0,
            Collect = 1,
            Collected = 2
        }
    }
}
