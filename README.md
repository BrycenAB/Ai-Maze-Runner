# AI Maze Runner

Welcome to the AI Maze Runner project! This interactive Unity-based simulation allows you to train and test neural networks (NNs) to control bots navigating a 3D maze. The bots receive feedback from checkpoints, enabling them to learn and optimize their paths through the maze. As an observer, you can control various aspects of the experiment, monitor progress, and explore the environment in first-person.

---

## Features

- **Neural Network Training**:
  - Train and test neural networks to control bots in a 3D environment.
  - Feedback is provided via checkpoints in the maze to facilitate learning.
  
- **Maze and Observation**:
  - 3D maze with teleportation to three observation floors.
  - First-person player mode for real-time observation of bot behavior.

- **Customizable Parameters**:
  - Adjust parameters like mutation chance, mutation rate, number of bots per generation, simulation timeframe, and game speed.

- **Simulation Management**:
  - Reset tests while keeping the current neural network intact or start fresh by wiping the neural network.
  - Track detailed simulation metrics, including:
    - Number of bots that collided (died).
    - Furthest position reached by the current and overall generations.

---

## Usage

1. **Start the Simulation**:
   - Unlock/lock your mouse by pressing the `I` key.
   - Default test settings are displayed in the UI. Modify these parameters before starting the test if needed.

2. **Navigate the Environment**:
   - Use the first-person player to observe bots in the maze.
   - Teleport between three observation floors using the buttons on the left side.

3. **Monitor Progress**:
   - View simulation metrics on the top-right UI panel, such as:
     - Number of bots that have died (collided with walls).
     - Furthest position reached by bots.

4. **Manage the Test**:
   - To restart the current generation, press the "Reset" button.
   - To start completely fresh, check the box under the "Reset" button and press "Reset."

---

## Bot Neural Network Overview

- **Neural Network Architecture**:
  - **Input Layer**:
    - 5 neurons receiving input from raycasts extending 10 meters to detect surrounding obstacles.
  - **Hidden Layer**:
    - 3 neurons for intermediate processing.
  - **Output Layer**:
    - 2 neurons:
      - One controls the bot's movement speed.
      - The other controls its rotation.

---

## To-Do List

- Convert plain text save data to an SQLite embedded database.
- Add a settings menu for player and camera configurations.

---

## Contributing

This project is open for contributions! If you encounter bugs or have feature suggestions, please open an issue on the GitHub repository. If you wish to use this project in any public-facing applications, I kindly request that you inform me beforehand.

---

## License

This project is licensed under the [MIT License](LICENSE). Feel free to use, modify, and distribute the code with appropriate attribution.

---

Thank you for exploring AI Maze Runner! 🚀

