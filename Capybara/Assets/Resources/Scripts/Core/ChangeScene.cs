using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void MoveToScene(string sceneID)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneID);
    }

}