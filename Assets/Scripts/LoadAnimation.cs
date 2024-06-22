using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadAnimation : MonoBehaviour
{
    Text LoadingText;

    // Update is called once per frame

    void Start()
    {
        LoadingText = gameObject.GetComponent<Text>();
        StartCoroutine(CoroutinePrintText());

    }

    IEnumerator CoroutinePrintText()
    {
        int index = 0;
        while (index < 5)
        {
            if (LoadingText.text != "...")
                LoadingText.text += ".";
            else
                LoadingText.text = ".";
            yield return new WaitForSeconds(1f);
            index++;
        }
        SceneManager.LoadScene("GameScene");
    }
}
