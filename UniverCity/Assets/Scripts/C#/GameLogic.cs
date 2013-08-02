using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour 
{
    public int playerScore = 0;
    private FlyCam player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<FlyCam>();
    }

    void OnGUI()
    {
        GUI.Label(new Rect(50, 50, 500, 20), "Enemies Captured: " + playerScore);
        if (!player.isHidden)
            GUI.Label(new Rect(50, 100, 1000, 1000),
                "CONTROLS\n" +
                "W - Forward\n" +
                "A - Left\n" +
                "S - Backwards\n" +
                "D - Right\n" +
                "Q - Down\n" +
                "E - Up\n" +
                "H - Hide\n" +
                "M - Go to main menu\n" +
                "Left-click to fire laser. Once enemy is caught,\n" +
                "you can let go of the mouse button.");
        else
        {
            GUI.Label(new Rect(50, 100, 1000, 1000),
                "CONTROLS\n" +
                "W - Forward\n" +
                "A - Left\n" +
                "S - Backwards\n" +
                "D - Right\n" +
                "Q - Down\n" +
                "E - Up\n" +
                "H - Unhide\n" +
                "M - Go to main menu\n" +
                "Left-click to fire laser. Once enemy is caught,\n" +
                "you can let go of the mouse button.");
            GUI.Label(new Rect(300, 100, 500, 30), "YOU ARE HIDDEN.");
        }
    }

    public void EnemyCaptured()
    {
        ++playerScore;
    }
}