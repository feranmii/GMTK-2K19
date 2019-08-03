using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Utils.Extensions
{
    public static class RandomExtensions<T>
    {

        
    public static bool SplitChance()
    {
        return Random.Range(0, 2) == 0 ? true : false;
    }
 
    public static bool Chance(int nProbabilityFactor, int nProbabilitySpace)
    {
        return Random.Range(0, nProbabilitySpace) < nProbabilityFactor ? true : false;
    }
 
    public static T Choice(T[] array)
    {
        return array[Random.Range(0, array.Length)];
    }
 
    public static T Choice(List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }
 
    public static T WeightedChoice(T[] array, int[] nWeights)
    {
        int nTotalWeight = 0;
        for(int i = 0; i < array.Length; i++)
        {
            nTotalWeight += nWeights[i];
        }
        int nChoiceIndex = Random.Range(0, nTotalWeight);
        for(int i = 0; i < array.Length; i++)
        {
            if(nChoiceIndex < nWeights[i])
            {
                nChoiceIndex = i;
                break;
            }
            nChoiceIndex -= nWeights[i];
        }
 
        return array[nChoiceIndex];
    }
 
    public static T WeightedChoice(List<T> list, int[] nWeights)
    {
        int nTotalWeight = 0;
        for(int i = 0; i < list.Count; i++)
        {
            nTotalWeight += nWeights[i];
        }
        int nChoiceIndex = Random.Range(0, nTotalWeight);
        for(int i = 0; i < list.Count; i++)
        {
            if(nChoiceIndex < nWeights[i])
            {
                nChoiceIndex = i;
                break;
            }
            nChoiceIndex -= nWeights[i];
        }
 
        return list[nChoiceIndex];
    }
    
    
    public static T[] Shuffle(T[] array)
    {
        T[] shuffledArray = new T[array.Length];
        List<int> elementIndices = new List<int>(0);
        for(int i = 0; i < array.Length; i++)
        {
            elementIndices.Add(i);
        }
        int nArrayIndex;
        for(int i = 0; i < array.Length; i++)
        {
            nArrayIndex = elementIndices[Random.Range(0, elementIndices.Count)];
            shuffledArray[i] = array[nArrayIndex];
            elementIndices.Remove(nArrayIndex);
        }
 
        return shuffledArray;
    }
 
    public static List<T> Shuffle(List<T> list)
    {
        List<T> shuffledList = new List<T>(0);
        int nListCount = list.Count;
        int nElementIndex;
        for(int i = 0; i < nListCount; i++)
        {
            nElementIndex = Random.Range(0, list.Count);
            shuffledList.Add(list[nElementIndex]);
            list.RemoveAt(nElementIndex);
        }
 
        return shuffledList;
    }
    }
}

public static class RandomOverrides
{
    public static int GetRandomValue(this Vector2 vector2)
    {
        var retVal = 0;

        retVal = (int) Random.Range(Mathf.Ceil(vector2.x), Mathf.Ceil(vector2.y) + 1);

        return retVal;
    }
    
    public static float GetRandomFloatValue(this Vector2 vector2)
    {
        var retVal = 0f;

        retVal = Random.Range(vector2.x, vector2.y);

        return retVal;
    }
    
    

    
    public static float CurveWeightedRandom(AnimationCurve curve)
    {
        return curve.Evaluate(Random.value);
    }
    

    
}