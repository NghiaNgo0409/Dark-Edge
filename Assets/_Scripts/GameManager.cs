using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    [SerializeField] GameObject loadingCanvas;
    [SerializeField] Slider loadingSlider;
    [SerializeField] TextMeshProUGUI loadingText; 
    [SerializeField] GameObject winCanvas;
    bool isWin;
    void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadToScene(int index) 
    {
        StartCoroutine(LoadSceneAsync(index));
    }

    IEnumerator LoadSceneAsync(int index)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(index);

        loadingCanvas.SetActive(true);

        while(!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            loadingSlider.value = progressValue;
            loadingText.text = "LOADING SCENES..." + progressValue * 100f + "%";

            yield return null;
        }
    }

    public bool GetIsWin()
    {
        return isWin;
    }

    public void ShowWinCanvas()
    {
        isWin = true;
        winCanvas.SetActive(true);
    }
}
