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

            player.PlayerInput.Inputs.Returns(
                new InputDTO(0f, 1f, false,
                    CardinalDirection.EAST,
                    CardinalDirection.EAST,
                CardinalDirection.EAST));
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

            player.PlayerInput.Inputs.Returns(
                new InputDTO(0f, -1f, false, 
                    CardinalDirection.WEST,
                    CardinalDirection.WEST,
                    CardinalDirection.WEST));
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

            player.PlayerInput.Inputs.Returns(
                new InputDTO(1f, 0f, false, 
                    CardinalDirection.NORTH,
                    CardinalDirection.NORTH,
                    CardinalDirection.NORTH));
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

            player.PlayerInput.Inputs.Returns(
                new InputDTO(-1f, 0f, false,
                    CardinalDirection.SOUTH,
                    CardinalDirection.SOUTH,
                    CardinalDirection.SOUTH));
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
