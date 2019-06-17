using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaiveBayesClassifier : MonoBehaviour {
    public int numAttributes;
    public int numExamplesPositive;
    public int numExamplesNegative;

    public List<bool> attrCountPositive;
    public List<bool> attrCountNegative;

    private void Awake() {
        attrCountPositive = new List<bool>();
        attrCountNegative = new List<bool>();
    }

    public void UpdateClassifier(bool[] attributes, NBCLabel label) {
        if (label == NBCLabel.POSITIVE) {
            numExamplesPositive++;
            attrCountPositive.AddRange(attributes);
        } else {
            numExamplesNegative++;
            attrCountNegative.AddRange(attributes);
        }
    }

    public float NaiveProbabilities(ref bool[] attributes,
                                    bool[] counts,
                                    float m,
                                    float n) {
        float prior = m / (m + n);
        float p = 1f;
        for (int i = 0; i < numAttributes; i++) {
            p /= m;
            if (attributes[i] == true) {
                p *= counts[i].GetHashCode();
            } else {
                p *= m - counts[i].GetHashCode();
            }
        }
        return prior * p;
    }

    public bool Predict(bool[] attributes) {
        float nep = numExamplesPositive;
        float nen = numExamplesNegative;
        float x = NaiveProbabilities(ref attributes, attrCountPositive.ToArray(), nep, nen);
        float y = NaiveProbabilities(ref attributes, attrCountNegative.ToArray(), nep, nen);
        if (x >= y) {
            return true;
        }
        return false;
    }
}
