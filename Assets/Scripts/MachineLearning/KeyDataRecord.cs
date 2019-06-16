using System.Collections.Generic;

//data type for holding the actions and theirprobabilities
public class KeyDataRecord<T> {
    public Dictionary<T, int> counts;
    public int total;
    public KeyDataRecord() {
        counts = new Dictionary<T, int>();
    }
}
