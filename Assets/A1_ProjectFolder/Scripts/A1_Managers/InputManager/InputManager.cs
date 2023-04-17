public class InputManager : Manager
{
    public static InputManager instance;
    public GetInputOfficer getInputOfficer;
    public InputTouchOfficer InputTouchOfficer;

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

    public override void PostGameStartProcess()
    {
        getInputOfficer.touchable = true;
    }
}
