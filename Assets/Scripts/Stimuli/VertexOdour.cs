using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexOdour : MonoBehaviour {
    private Dictionary<int, OdourParticle> odourDictionary;

    private void Start() {
        odourDictionary = new Dictionary<int, OdourParticle>();
    }

    private void OnCollisionEnter(Collision coll) {
        OdourParticle op = coll.gameObject.GetComponent<OdourParticle>();
        if (op == null) {
            return;
        }
        int id = op.parent;
        odourDictionary.Add(id, op);
    }

    private void OnCollisionExit(Collision coll) {
        OdourParticle op = coll.gameObject.GetComponent<OdourParticle>();
        if (op == null) {
            return;
        }
        int id = op.parent;
        odourDictionary.Remove(id);
    }

    public bool HasOdour() {
        if (odourDictionary.Values.Count == 0) {
            return false;
        }
        return true;
    }

    public bool OdourExists(int id) {
        return odourDictionary.ContainsKey(id);
    }
}
