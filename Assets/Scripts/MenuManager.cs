using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Dropdown gridDropdown;
    public GameObject me;
    public GameManager gm;

    void Start()
    {
        // Optional: Populate dropdown dynamically
        gridDropdown.ClearOptions();
        gridDropdown.AddOptions(new System.Collections.Generic.List<string> {
            "2x2", "3x4", "4x4", "5x2", "5x6", "6x6"
        });
    }

    public void OnStartGameButtonClicked()
    {
        string gridSize = gridDropdown.options[gridDropdown.value].text;
        string[] parts = gridSize.Split('x');
        int rows = int.Parse(parts[0]);
        int cols = int.Parse(parts[1]);
        gm.CreateBoard(rows, cols);
        me.SetActive(false);
    }
}
