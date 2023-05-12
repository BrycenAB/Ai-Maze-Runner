# Ai_Sandbox_Maze


Welcome to the Ai_Sandbox_Maze project, where you can train and test neural networks to control small bots in a 3D lab facility. In this lab, you will find a maze and 3 viewing floors that you can teleport to. Inside the maze are checkpoints that act as feedback for the neural network, helping it learn how to navigate the maze.

# Features
- Train and test neural networks to control small bots in a 3D lab facility
- Maze with checkpoints to provide feedback for the neural network
- Teleportation to 3 viewing floors
- Control over several aspects of the test, such as mutation chance, mutation rate, number of bots in the maze per generation, timeframe in which they have to move through the maze, and the speed at which time passes in the game
- Option to reset a test and wipe the neural network to start from scratch
- First person player to observe the bots moving through the maze
- Information such as number of bots that have died (i.e. ran into a wall), furthest position of the current generation, and overall furthest position

# Usage
   1.) Once you have the project open you can unlock/lock your mouse with the "i" key. 

   2.) The default settings for the test are the ones displayed on the ui for changing parameters, the parameters must be changed before starting the test if you wish to do so.

   3.) Use the first person player to observe the bots moving through the maze.

   4.) You have access to 3 teleport buttons on the left side that will take you to diffrent levels.

   5.) Monitor the information displayed in the top right UI, such as number of bots that have died and furthest position.

   6.) If you wish to start the test over on the same generation simply press the reset button, if you wish to completly start fresh select the check box under the reset button and press reset.
   
   
# Bot Overview
   - The bots NN consists of 3 layers
      - Layer 1 is the input layer, fed by 5 raycasts (5 neurons) that go out 10 meters in front of them to observer the surrounding area
      - layer 2 is the hidden layer consisting of 3 Neurons
      - Layer 3 is the output layer consisting of 2 Neurons, one controlling the speed of the bot and the other controlling rotation


# Contributing
This project is open for contributions. If you find bugs or have a feature request, please open an issue on the GitHub repository. If you would like to use the project feel free to do so with my request of being informed if it is used in any public facing projects.

# License
This project is licensed under the MIT License. See the LICENSE file for more information.
