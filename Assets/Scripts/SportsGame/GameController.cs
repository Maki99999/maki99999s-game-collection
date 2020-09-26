using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SportsGame {
public abstract class GameController : MonoBehaviour {
    
    public static BooleanWrapper paused = new BooleanWrapper();
    public GameType type;
    public Bounds area;

    public GameObject player;
    public Transform playerCam;

    protected MainController mainController;

    public bool inGame = false;
    protected Vector3 playerInitPos;
    protected Quaternion playerInitRot;

    public BooleanWrapper getPaused() {
        return paused;
    }
    
    public abstract void Init();
    
    public abstract void Reset();

    public abstract void Loose();

    public abstract void Hack();
}

public enum GameType {
    Tag, Dodge, Parkour, Throw, Bonus
}
}