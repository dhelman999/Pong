using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    Button singlePlayer;
    Button twoPlayer;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadSinglePlayer()
    {
        PlayerPrefs.SetInt("Players", 1);
        SceneManager.LoadScene("PongMain", LoadSceneMode.Single);
    }

    public void LoadTwoPlayers()
    {
        PlayerPrefs.SetInt("Players", 2);
        SceneManager.LoadScene("PongMain", LoadSceneMode.Single);
    }
}
