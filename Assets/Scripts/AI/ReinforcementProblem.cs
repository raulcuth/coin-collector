public class ReinforcementProblem {

    //retrieves a random state, depending on the type of game, we would be interested in random
    //states concerning the current state of the game
    public virtual GameState GetRandomState() {
        //TODO
        //define your own behaviour
        return new GameState();
    }

    //retireves all the available actions from a given game state
    public virtual GameAction[] GetAvailableActions(GameState s) {
        //TODO
        //define your own behaviour
        return new GameAction[0];
    }

    //carries out an action, and the retreieves the resulting state and reward
    public virtual GameState TakeAction(GameState state,
                                        GameAction action,
                                        ref float reward) {
        //TODO
        //define your own behaviour
        reward = 0f;
        return new GameState();
    }
}
