using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuildRecord : IComparable<GuildRecord> {

    public Vertex location;
    public float strength;
    public Guild guild;

    public override bool Equals(object obj) {
        GuildRecord other = (GuildRecord)obj;
        return location == other.location;
    }

    public bool Equals(GuildRecord o) {
        return location == o.location;
    }

    public override int GetHashCode() {
        return base.GetHashCode();
    }

    public int CompareTo(GuildRecord other) {
        if (location == other.location) {
            return 0;
        }
        //the substraction is inverse for having a descending binary heap
        return (int)(other.strength - strength);
    }
}
