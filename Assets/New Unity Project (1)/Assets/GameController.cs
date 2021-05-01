using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
    // Start is called before the first frame update()
    void Start()
    {
        //testing purposes, proof that our scene has restarted when play again button is clicked
        print("This scence has started");


    }

    public void RestartScene() {


        //resets scene
        print("")
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    
    }

}
