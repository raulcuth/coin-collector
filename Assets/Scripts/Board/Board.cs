using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board {
    protected int player;

    public Board() {
        player = 1;
    }

    //retrieve next possible moves
    public virtual Move[] GetMoves() {
        return new Move[0];
    }

    //play the move on the board
    public virtual Board MakeMove(Move m) {
        return new Board();
    }

    public virtual bool IsGameOver() {
        return true;
    }

    public virtual int GetCurrentPlayer() {
        return player;
    }

    //test the board's value for a given player
    public virtual float Evaluate(int player) {
        return Mathf.NegativeInfinity;
    }

    //test the board's value for the current player
    public virtual float Evaluate() {
        return Mathf.NegativeInfinity;
    }
}
