using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
    void Update () {
        if (Input.anyKey) {
            GameManager.Instance.StartNewGame();
        }
    }
}
