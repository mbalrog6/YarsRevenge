using UnityEngine;

public interface IMenuButton
{ 
    void OnEnter();
    void OnExit();
    void OnClick();
    void SetIndex(int index);
    
    int MouseOverMenuButton(Vector2 mousePosition); 
}