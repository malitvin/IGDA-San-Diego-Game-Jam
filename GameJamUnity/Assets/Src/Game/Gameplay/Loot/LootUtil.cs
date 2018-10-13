//C#
using System.Collections.Generic;

//C#
using System;

namespace Gameplay.Loot
{
    public class LootUtil 
    {
        private static Random randomGen = new Random();

        public static Loot.Rarity GetWeightedRarity(List<LootConfig.ProbabilityDef> probabilities,float totalSum)
        {
            float randomValue = UnityEngine.Random.Range(0, totalSum);
            float cumulative = 0.0f;
            for (int i=0; i < probabilities.Count; i++)
            {
                cumulative += probabilities[i].probability;
                if (randomValue < cumulative)
                {
                    return probabilities[i].rarity;
                }
            }

            return Loot.Rarity.Common;
        }

        public static int GetLootDropCount(LootConfig.LootDropDef def)
        {
            UnityEngine.Vector2 range = def.randomDropRange;
            return (int)UnityEngine.Random.Range(range.x, range.y + 1);
        }

        public static float GetTotalSum(List<LootConfig.ProbabilityDef> probabilities)
        {
            float totalSum = 0;
            int probCount = probabilities.Count;
            for (int i = 0; i < probCount; i++)
            {
                totalSum += probabilities[i].probability;
            }

            return totalSum;
        }

        public static int CompareLootByProbability(LootConfig.ProbabilityDef a, LootConfig.ProbabilityDef b)
        {
            if (a.probability > b.probability)
            {
                return 1;
            }
            else if (a.probability < b.probability)
            {
                return -1;
            }
            return 0;
        }
    }

    public struct SumStruct
    {
        public float totalSum;
        public float[] cumulativeSum;
    }
}
