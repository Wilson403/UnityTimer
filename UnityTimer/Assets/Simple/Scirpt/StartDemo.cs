using UnityEngine;
using UnityEngine.UI;
using UnityTimer;

public class StartDemo : MonoBehaviour
{
    public Text _textSecond;
    public Button _btnStart4Second;
    public Button _btnPause4Second;
    public Button _btnStop4Second;

    public Text _textMinute;
    public Button _btnStart4Minute;
    public Button _btnPause4Minute;
    public Button _btnStop4Minute;

    public Text _textFrame;
    public Button _btnStart4Frame;
    public Button _btnPause4Frame;
    public Button _btnStop4Frame;

    void Start ()
    {
        var clock1 = UnityTimerMgr.CreateSecondClock ((v) =>
        {
            _textSecond.text = $"{v}秒";
        });

        _btnStart4Second.onClick.AddListener (() =>
        {
            clock1.Start ();
        });

        _btnPause4Second.onClick.AddListener (() =>
        {
            clock1.Pause ();
        });

        _btnStop4Second.onClick.AddListener (() =>
        {
            clock1.Stop ();
        });

        clock1.Wait (3).Then (() => 
        {
            Debug.Log ($"过了3秒");
        });

        //也可以这样不绑定Aciton,只等待
        UnityTimerMgr.CreateSecondClock ().Wait (10).Then (() => 
        {
            Debug.Log ($"过了10秒");
        });

        //==============================================================

        var clock2 = UnityTimerMgr.CreateMinuteClock ((v) =>
        {
            _textMinute.text = $"{v}分";
        });

        _btnStart4Minute.onClick.AddListener (() =>
        {
            clock2.Start ();
        });

        _btnPause4Minute.onClick.AddListener (() =>
        {
            clock2.Pause ();
        });

        _btnStop4Minute.onClick.AddListener (() =>
        {
            clock2.Stop ();
        });

        clock2.Wait (1).Then (() =>
        {
            Debug.Log ($"过了1分钟");
        });

        //==============================================================

        var clock3 = UnityTimerMgr.CreateFrameClock ((v) =>
        {
            _textFrame.text = $"{v}帧";
        });

        _btnStart4Frame.onClick.AddListener (() =>
        {
            clock3.Start ();
        });

        _btnPause4Frame.onClick.AddListener (() =>
        {
            clock3.Pause ();
        });

        _btnStop4Frame.onClick.AddListener (() =>
        {
            clock3.Stop ();
        });
    }
}