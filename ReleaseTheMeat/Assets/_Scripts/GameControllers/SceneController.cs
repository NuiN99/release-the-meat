using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//You see these grey using statements? if you hover over them and then press [ctrl + .] then it will prompt you to have them removed
//You don't want to include using statements for libraries that you won't use


//Where is the namespace?
//ALWAYS use a namespace

public class SceneController : MonoBehaviour
{

    //If you aren't using the Start method then remove it. Unity calls it even if it's empty. It's not much but good practice
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
