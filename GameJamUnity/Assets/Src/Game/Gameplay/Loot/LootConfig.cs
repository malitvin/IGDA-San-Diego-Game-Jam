//Unity
//Odin
using Sirenix.OdinInspector;
//C#
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Loot
{
    [ExecuteInEditMode]
    [CreateAssetMenu(fileName = "Loot Config", menuName = "Gameplay/Loot Config")]
    public class LootConfig : ScriptableObject
    {
        [System.Serializable]
        public class LootDef
        {
            [SerializeField, HideInInspector]
            private int min;
            [SerializeField, HideInInspector]
            private int max;

            [ShowInInspector, MinMaxSlider(0, 15)]
            private Vector2 randomDropRange
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

            [ListDrawerSettings(HideAddButton = true, ShowPaging = false, DraggableItems = false, OnBeginListElementGUI = "onElementUpdate")]
            [OnInspectorGUI("onGUIUpdate", append: false)]
            public List<ProbabilityDef> probabilities;

            private Dictionary<int, float> changeMap;

#if UNITY_EDITOR
            private void onGUIUpdate()
            {
                Loot.Rarity[] probTypes = Enum.GetValues(typeof(Loot.Rarity)) as Loot.Rarity[];
                if (probabilities.Count != probTypes.Length)
                {
                    GenerateProbabilities(probTypes);
                }
            }

            private void GenerateProbabilities(Loot.Rarity[] probTypes)
            {
                probabilities = new List<ProbabilityDef>();
                changeMap = new Dictionary<int, float>();
                int index = 0;
                foreach (Loot.Rarity type in probTypes)
                {
                    float prob = index == 0 ? 1 : 0;
                    ProbabilityDef def = new ProbabilityDef(type, prob);
                    probabilities.Add(def);
                    changeMap[index] = prob;
                    index++;
                }
            }

            private void onElementUpdate(int index)
            {
                if(changeMap == null)
                {
                    Loot.Rarity[] probTypes = Enum.GetValues(typeof(Loot.Rarity)) as Loot.Rarity[];
                    GenerateProbabilities(probTypes);
                }
                else if(changeMap != null && changeMap.ContainsKey(index))
                {
                    float previousProbabliity = changeMap[index];
                    ProbabilityDef def = probabilities[index];
                    if (previousProbabliity != def.probability)
                    {
                        //sum all elements
                        float sum = 0;
                        int probCount = probabilities.Count;
                        int i = 0;
                        for (i=0; i < probCount; i++)
                        {
                            sum += probabilities[i].probability;
                        }

                        //divide elements by sum
                        for (i = 0; i < probCount; i++)
                        {
                            def = probabilities[i];
                            def.probability = def.probability / sum;
                            changeMap[i] = def.probability;
                        }
                    }
                }
            }
#endif
        }

        [System.Serializable]
        public class ProbabilityDef
        {
            [ReadOnly]
            public Loot.Rarity rarity;
            [Range(0, 1)]
            public float probability;

            public ProbabilityDef(Loot.Rarity rare, float prob)
            {
                rarity = rare;
                probability = prob;
            }
        }

        public LootDef def;
    }

    public class Loot
    {
        public enum Rarity
        {
            Common,
            Uncommon,
            Rare,
            Legendary,
            Exotic
        }
    }
}
