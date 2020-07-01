using System.Collections;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace a_player
{
    public class can_move
    {
        [UnityTest]
        public IEnumerator right()
        {
            yield return LoadMovementTestScene();

            var player = Object.FindObjectOfType<Player>();
            player.PlayerInput = Substitute.For<IPlayerInput>();

            var playerPositionX = player.transform.position.x;
            
            Assert.AreEqual(playerPositionX, player.transform.position.x);

            player.PlayerInput.Horizontal.Returns(1f);
            yield return new WaitForSeconds(2f);
            
            Assert.Less(playerPositionX, player.transform.position.x);
        }
        
        [UnityTest]
        public IEnumerator left()
        {
            yield return LoadMovementTestScene();

            var player = Object.FindObjectOfType<Player>();
            player.PlayerInput = Substitute.For<IPlayerInput>();

            var playerPositionX = player.transform.position.x;
            
            Assert.AreEqual(playerPositionX, player.transform.position.x);

            player.PlayerInput.Horizontal.Returns(-1f);
            yield return new WaitForSeconds(2f);
            
            Assert.Greater(playerPositionX, player.transform.position.x);
        }
        
        [UnityTest]
        public IEnumerator up()
        {
            yield return LoadMovementTestScene();

            var player = Object.FindObjectOfType<Player>();
            player.PlayerInput = Substitute.For<IPlayerInput>();

            var playerPositionY = player.transform.position.y;
            
            Assert.AreEqual(playerPositionY, player.transform.position.y);

            player.PlayerInput.Vertical.Returns(1f);
            yield return new WaitForSeconds(2f);
            
            Assert.Less(playerPositionY, player.transform.position.y);
        }
        
        [UnityTest]
        public IEnumerator down()
        {
            yield return LoadMovementTestScene();

            var player = Object.FindObjectOfType<Player>();
            player.PlayerInput = Substitute.For<IPlayerInput>();

            var playerPositionY = player.transform.position.y;
            
            Assert.AreEqual(playerPositionY, player.transform.position.y);

            player.PlayerInput.Vertical.Returns(-1f);
            yield return new WaitForSeconds(2f);
            
            Assert.Greater(playerPositionY, player.transform.position.y);
        }
        
        private IEnumerator LoadMovementTestScene()
        {
            var operation = SceneManager.LoadSceneAsync("MovementTests");
            while (!operation.isDone)
            {
                yield return null;
            }
        }
    }
}
