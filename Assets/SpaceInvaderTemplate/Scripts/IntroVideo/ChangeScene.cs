using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private VideoPlayer _videoPLayer;

    private AsyncOperation _asyncOperation;
    
    private void Start()
    {
        _asyncOperation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        _asyncOperation.allowSceneActivation = false;

        _videoPLayer.loopPointReached += OnVideoEnd;
    }

    private void OnVideoEnd(VideoPlayer source)
    {
        _asyncOperation.allowSceneActivation = true;
    }
}
