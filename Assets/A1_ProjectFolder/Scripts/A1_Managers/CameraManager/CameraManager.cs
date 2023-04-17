using UnityEngine;
public class CameraManager : Manager
{
    public static CameraManager instance;

    public override void LevelInstantiateProcess()
    {

    }

    public enum CameraState     
    {
        inGame, Finish
    }

    private void Awake()
    {
        SingletonCheck();
    }
    
    void SingletonCheck()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }

}
