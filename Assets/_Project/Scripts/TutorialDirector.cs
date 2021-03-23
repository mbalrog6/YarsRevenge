
using UnityEngine;

public class TutorialDirector : MonoBehaviour
{

   public void Tutorial()
   {
       /*
      if (TutorialActive && GameStateMachine.Instance.CurrentState == States.PLAY)
              {
                  if (_tutorialIndex == 0 && DialogueManager.Instance.IsVisible == false )
                  {
                      Mediator.Instance.Publish<ShowDialogueCommand>(new ShowDialogueCommand());
                      DialogueManager.Instance.ShowNextStoryElement();
                      _tutorialIndex = 1;
                  }
      
                  if (_tutorialIndex == 1 && DialogueManager.Instance.IsVisible == false )
                  {
                      if (_tutorialTimerSet == false)
                      {
                          _tutorialTimer = Time.time + 5f;
                          _tutorialTimerSet = true;
                      }
                      else if (Time.time > _tutorialTimer)
                      {
                          if (_player.Ammo <= 0)
                          {
                              Mediator.Instance.Publish<ShowDialogueCommand>(new ShowDialogueCommand());
                              DialogueManager.Instance.ShowNextStoryElement();
                              
                          }
                          _tutorialIndex = 2;
                          _tutorialTimerSet = false;
                      }
                  }
      
                  if (_tutorialIndex == 2 && DialogueManager.Instance.IsVisible == false )
                  {
                      if (_player.Ammo >= 1)
                      {
                          Mediator.Instance.Publish<ShowDialogueCommand>(new ShowDialogueCommand());
                          DialogueManager.Instance.ShowNextStoryElement();
                          _tutorialIndex = 3;
                      }
                  }
      
                  if (_tutorialIndex == 3 && DialogueManager.Instance.IsVisible == false )
                  {
                      if( _player.Ammo > 15 )
                      {
                          Mediator.Instance.Publish<ShowDialogueCommand>(new ShowDialogueCommand());
                          DialogueManager.Instance.ShowNextStoryElement();
                          _tutorialIndex = 4;
                      }
                  }
              }
              */
   }
   
}
