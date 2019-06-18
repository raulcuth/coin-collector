using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLPNetwork : MonoBehaviour {
    public Perceptron[] inputPer;
    public Perceptron[] hiddenPer;
    public Perceptron[] outputPer;

    public void Learn(Perceptron[] inputs, Perceptron[] outputs) {
        GenerateOutput(inputs);
        BackProp(outputs);
    }

    //transmits inputs from one end of the neural network to the other
    public void GenerateOutput(Perceptron[] inputs) {
        for (int i = 0; i < inputs.Length; i++) {
            inputPer[i].state = inputs[i].input;
        }
        for (int i = 0; i < hiddenPer.Length; i++) {
            hiddenPer[i].FeedForward();
        }
        for (int i = 0; i < outputPer.Length; i++) {
            outputPer[i].FeedForward();
        }
    }

    //propell the computation that actually emulates learning
    public void BackProp(Perceptron[] outputs) {
        int i;
        //traverse the output layer for computing values
        for (i = 0; i < outputPer.Length; i++) {
            Perceptron p = outputPer[i];
            float state = p.state;
            float error = state * (1f - state);
            error *= outputs[i].state - state;
            p.AdjustWeights(error);
        }
        //traverse the internal perceptron layers
        for (i = 0; i < hiddenPer.Length; i++) {
            Perceptron p = outputPer[i];
            float state = p.state;
            float sum = 0f;
            for (i = 0; i < outputs.Length; i++) {
                float incomingW = outputs[i].GetIncomingWeight();
                sum += incomingW * outputs[i].error;
                float error = state * (1f - state) * sum;
                p.AdjustWeights(error);
            }

        }
    }
}
