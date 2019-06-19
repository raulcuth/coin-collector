using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {
    public LevelPredictor predictor;
    public List<LevelSlice> pattern;
    public List<LevelSlice> result;
    private bool isInit;

    private void Start() {
        isInit = false;
    }

    public void Init() {
        result = new List<LevelSlice>();
        predictor = new LevelPredictor(3);
        predictor.RegisterSequence(pattern.ToArray());
    }

    public void Build() {
        if (isInit) {
            return;
        }
        for (int i = 0; i < pattern.Count - 1; i++) {
            LevelSlice[] input = pattern.GetRange(0, i + 1).ToArray();
            LevelSlice slice = predictor.GetMostLikely(input);
            result.Add(slice);
        }
    }
}
