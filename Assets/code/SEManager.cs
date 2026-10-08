using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SEManager : MonoBehaviour
{
    public static SEManager Instance;

    [Header("UI")]
    [SerializeField]
    private AudioClip titleClickSE;

    [SerializeField]
    private AudioClip emptyClickSE;

    [SerializeField]
    private AudioClip decideSE;

    [SerializeField]
    private AudioClip backSE;

    [Header("ゲーム")]
    [SerializeField]
    private AudioClip correctSE;

    [SerializeField]
    private AudioClip missSE;

    [SerializeField]
    private AudioClip startSE;

    [SerializeField]
    private AudioClip finishSE;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource =
            GetComponent<AudioSource>();
    }

    /*
     * 共通再生
     */
    private void Play(AudioClip clip)
    {
        if (clip == null)
        {
            return;
        }

        audioSource.PlayOneShot(clip);
    }

    /*
     * タイトルクリック
     */
    public void PlayTitleClick()
    {
        Play(titleClickSE);
    }

    /*
     * 何もない所クリック
     */
    public void PlayEmptyClick()
    {
        Play(emptyClickSE);
    }

    /*
     * 決定
     */
    public void PlayDecide()
    {
        Play(decideSE);
    }

    /*
     * 戻る
     */
    public void PlayBack()
    {
        Play(backSE);
    }

    /*
     * 正解
     */
    public void PlayCorrect()
    {
        Play(correctSE);
    }

    /*
     * 不正解
     */
    public void PlayMiss()
    {
        Play(missSE);
    }

    /*
     * スタート
     */
    public void PlayStart()
    {
        Play(startSE);
    }

    /*
     * 終了
     */
    public void PlayFinish()
    {
        Play(finishSE);
    }

    /*
     * タイトルSE終了後に処理
     */
    public IEnumerator PlayTitleClickThen(Action next)
    {
        Play(titleClickSE);

        yield return new WaitForSecondsRealtime(
            titleClickSE.length
        );

        next?.Invoke();
    }

    /*
     * 決定SE終了後に処理
     */
    public IEnumerator PlayDecideThen(Action next)
    {
        Play(decideSE);

        yield return new WaitForSecondsRealtime(
            decideSE.length
        );

        next?.Invoke();
    }

    /*
     * 戻るSE終了後に処理
     */
    public IEnumerator PlayBackThen(Action next)
    {
        Play(backSE);

        yield return new WaitForSecondsRealtime(
            backSE.length
        );

        next?.Invoke();
    }
}