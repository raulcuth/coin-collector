using UnityEngine;
using System.Collections;
using System;

public class HierarchicalNGramP<T> {
    public int threshold;
    public NGramPredictor<T>[] predictors;
    private int nValue;

    public HierarchicalNGramP(int windowSize) {
        nValue = windowSize + 1;
        predictors = new NGramPredictor<T>[nValue];
        for (int i = 0; i < nValue; i++) {
            predictors[i] = new NGramPredictor<T>(i + 1);
        }
    }

    //registers a sequence, just like its predecessor
    public void RegisterSequence(T[] actions) {
        for (int i = 0; i < nValue; i++) {
            T[] subactions = new T[i + 1];
            Array.Copy(actions, nValue - i - 1, subactions, 0, i + 1);
            predictors[i].RegisterSequence(subactions);
        }
    }

    //computes the prediction
    public T GetMostLikely(T[] actions) {
        T bestAction = default(T);
        for (int i = 0; i < nValue; i++) {
            NGramPredictor<T> p=predictors[nValue-i-1];
            T[] subactions = new T[i + 1];
            Array.Copy(actions, nValue - i - 1, subactions, 0, i + 1);
            int numActions = p.GetActionsNum(ref actions);
            if (numActions > threshold) {
                bestAction = p.GetMostLikely(actions);
            }
        }
        return bestAction;
    }
}
