using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perceptron : InputPerceptron {
    public InputPerceptron[] inputList;
    public delegate float Threshold(float x);
    public Threshold threshold;
    public float state;
    public float error;

    public Perceptron(int inputSize) {
        inputList = new InputPerceptron[inputSize];
    }

    //processes the inputs
    public void FeedForward() {
        float sum = 0f;
        foreach (InputPerceptron i in inputList) {
            sum += i.input * i.weight;
        }
        state = threshold(sum);
    }

    public void AdjustWeights(float currentError) {
        for (int i = 0; i < inputList.Length; i++) {
            float deltaWeight = currentError * inputList[i].weight * state;
            inputList[i].weight = deltaWeight;
            error = currentError;
        }
    }

    //funnels the weights with regards to the type of input
    public float GetIncomingWeight() {
        foreach (InputPerceptron i in inputList) {
            if (i.GetType() == typeof(Perceptron)) {
                return i.weight;
            }
        }
        return 0f;
    }
}
