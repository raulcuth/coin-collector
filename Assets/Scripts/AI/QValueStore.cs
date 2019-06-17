using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QValueStore : MonoBehaviour {
    private Dictionary<GameState, Dictionary<GameAction, float>> store;

    public QValueStore() {
        store = new Dictionary<GameState, Dictionary<GameAction, float>>();
    }

    //get the resulting value of taking an action in a game state
    //carefully craft this so that an action cannot be taken in that particular state
    public virtual float GetQValue(GameState state, GameAction action) {
        //TODO: your behaviour here
        return 0f;
    }

    //retrieve the best action to take in a certain state
    public virtual GameAction GetBestAction(GameState state) {
        //TODO: your behaviour here
        return new GameAction();
    }

    public void StoreQValue(GameState state,
                            GameAction action,
                            float val) {
        if (!store.ContainsKey(state)) {
            Dictionary<GameAction, float> d = new Dictionary<GameAction, float>();
            store.Add(state, d);
        }
        if (!store[state].ContainsKey(action)) {
            store[state].Add(action, 0f);
        }
        store[state][action] = val;
    }
}
