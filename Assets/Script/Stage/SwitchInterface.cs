using UnityEngine;

public class SwitchInterface
{
    public interface IInteractPromptProvider
    {
        Sprite PromptSprite { get; }
        Vector3 PromptWorldPosition { get; }
    }
}
