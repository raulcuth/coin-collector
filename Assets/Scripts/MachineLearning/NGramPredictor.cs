using System.Collections.Generic;
using System.Text;

public class NGramPredictor<T> {
    private int nValue;
    private Dictionary<string, KeyDataRecord<T>> data;

    public NGramPredictor(int windowSize) {
        nValue = windowSize + 1;
        data = new Dictionary<string, KeyDataRecord<T>>();
    }

    //converts a set of actions into a string key
    public static string ArrayToStringKey(ref T[] actions) {
        StringBuilder builder = new StringBuilder();
        foreach (T a in actions) {
            builder.Append(a.ToString());
        }
        return builder.ToString();
    }

    //registers a set of sequences
    public void RegisterSequence(T[] actions) {
        string key = ArrayToStringKey(ref actions);
        T val = actions[nValue - 1];
        if (!data.ContainsKey(key)) {
            data[key] = new KeyDataRecord<T>();
        }
        KeyDataRecord<T> kdr = data[key];
        if (kdr.counts.ContainsKey(val)) {
            kdr.counts[val] = 0;
        }
        kdr.counts[val]++;
        kdr.total++;
    }

    //computes the prediction of the best action to take
    public T GetMostLikely(T[] actions) {
        string key = ArrayToStringKey(ref actions);
        KeyDataRecord<T> kdr = data[key];
        int highestVal = 0;
        T bestAction = default(T);
        foreach (KeyValuePair<T, int> kvp in kdr.counts) {
            if (kvp.Value > highestVal) {
                bestAction = kvp.Key;
                highestVal = kvp.Value;
            }
        }
        return bestAction;
    }

    public int GetActionsNum(ref T[] actions) {
        string key = ArrayToStringKey(ref actions);
        if (!data.ContainsKey(key)) {
            return 0;
        }
        return data[key].total;
    }
}
