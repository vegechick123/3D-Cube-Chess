using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class BeginPannel : MonoBehaviour
{
    public bool ignoreLoadScene=false;
    public MonoBehaviour coroutineProxy;
    public Animation text1;
    public Animation text2;
    public Animation text3;
    public void LoadScene(int index)
    {
        int curIndex=SceneManager.GetActiveScene().buildIndex;
        if (curIndex != index && !ignoreLoadScene)
        {
            Camera.main.GetComponent<Animation>().Play("CameraOut");
            if (index == 1)
            {
                coroutineProxy.InvokeAfter(() => { text1.Play(); }, 3.0f);
                coroutineProxy.InvokeAfter(() => { text2.Play(); }, 8.0f);
                coroutineProxy.InvokeAfter(() => { text3.Play(); }, 10.0f);
                coroutineProxy.InvokeAfter(() =>
                {
                    SceneManager.LoadScene(index);
                }, 14.0f);
            }
            else
            {
                coroutineProxy.InvokeAfter(() =>
                {
                    SceneManager.LoadScene(index);
                }, 3.0f);
            }
        }
       
    }
    public void Quit()
    {
        Application.Quit();
    }
    IEnumerator LoadYourAsyncScene(int index)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
