﻿using UnityEngine;

public class Character : MonoBehaviour {
    public Location location;

    public void SetTarget(Location location) {
        this.location = location;
    }
}
