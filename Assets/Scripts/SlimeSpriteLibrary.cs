using UnityEngine;

[CreateAssetMenu(fileName = "SlimeSpriteLibrary", menuName = "Slime/Slime Sprite Library")]
public class SlimeSpriteLibrary : ScriptableObject
{
    [Header("红色史莱姆")]
    public Sprite[] redNormalFramesA;
    public Sprite[] redNormalFramesB;
    public Sprite[] redJumpFrames;
    public Sprite[] redCorrectFrames;
    public Sprite[] redWrongFrames;

    [Header("黄色史莱姆")]
    public Sprite[] yellowNormalFramesA;
    public Sprite[] yellowNormalFramesB;
    public Sprite[] yellowJumpFrames;
    public Sprite[] yellowCorrectFrames;
    public Sprite[] yellowWrongFrames;

    [Header("蓝色史莱姆")]
    public Sprite[] blueNormalFramesA;
    public Sprite[] blueNormalFramesB;
    public Sprite[] blueJumpFrames;
    public Sprite[] blueCorrectFrames;
    public Sprite[] blueWrongFrames;

    [Header("绿色史莱姆")]
    public Sprite[] greenNormalFramesA;
    public Sprite[] greenNormalFramesB;
    public Sprite[] greenJumpFrames;
    public Sprite[] greenCorrectFrames;
    public Sprite[] greenWrongFrames;

    [Header("紫色史莱姆")]
    public Sprite[] purpleNormalFramesA;
    public Sprite[] purpleNormalFramesB;
    public Sprite[] purpleJumpFrames;
    public Sprite[] purpleCorrectFrames;
    public Sprite[] purpleWrongFrames;
}