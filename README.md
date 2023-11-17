
# Trailblaze: Road Crossing Simulation

The "Trailblaze : Road Crossing Stimulation" is a machine learning
initiative designed to train an agent to safely cross a road. Leveraging Unity ML-Agents, a robust machine learning toolkit tailored for developing AI agents capable of learning tasks within simulated environments, this project employs a reinforcement learning paradigm. 

Reinforcement learning entails rewarding the agent for actions leading to desired outcomes while penalizing actions yielding undesired results. Through a trial-and-error process, the agent incrementally refines its ability to safely navigate road crossings over time. "Road Crossing Stimulation: Trailblaze" utilizes Unity ML-Agents and Proximal Policy Optimization (PPO), to craft a vibrant and immersive virtual environment. 

Within this virtual environment, players find themselves in a dynamic setting, navigating through busy roads, encountering intelligent agents, and strategically avoiding obstacles.In this project the agents are trained using PPO which is fine-tuned for optimal learning and decision-making outcomes. Furthermore, the integration of Behavioral Cloning (BC) enriches the training process, refining agent behavior through expert demonstrations.
## Background

To build the game simulation environment we use Unity 3D and to train the agent for this environment we use Unity ML 

Unity 3D is a popular game engine used to create interactive 3D games for various platforms including mobile, web, consoles, and more. It is developed and maintained by Unity Technologies and is beginning to gain popularity as an educational tool. Some unique features of Unity 3D include a built-in physics engine, support for animation, and cross-platform development capabilities. 

Unity ML is a toolset included in Unity, a game engine, for creating and using machine learning models in the projects. It allows us to train, import, and use machine learning models in real-time. Unity ML also has a built-in visual ML editor, which makes it easy to create and experiment with models, and includes pre-built ML assets that can be used in the projects. 

To train the agent with the help of Unity ML we use Proximal Policy Optimization (PPO) and Behavioral Cloning (BC). 

PPO is a policy optimization algorithm in reinforcement learning that aims to update policies incrementally while ensuring limited divergence from the previous policy, enhancing stability and sample efficiency. 

Behavioral cloning is a supervised learning technique in machine learning where a model learns a policy by mimicking the behavior of an expert through  imitation learning.

## Methodology

<b>Environment</b>
![f7cb2833-178e-41e0-8668-3b7669adae56](https://github.com/Pranav-Programmer/Trailblaze-Road-Crossing-Stimulation/assets/79044490/d2e3d518-2528-40f1-b4ae-74bed865f4c6)


To build the environment in Unity 3d we have made and used prefabs available to us in the Unity Asset Store. The 4 major components of the game
environment are:
1. Road - We have imported this prefab from unity store.

2. Cars - We have imported many cars from unity stores and then customized their textures in Unity 3D.

3. Human Agent - This prefab is also taken from Unity store and already has the necessary animations which gives it a more humanoid movement.

4. EndRoad Block - This is a transparent gameobject perpendicular to the lane of the road which is used to navigate the agent.

Rest of the prefabs are mainly for visuals such as streetlight, trees, building, petrol pump, hospital ,etc.

Initially we have spawned 30 human agents in the game scene which are trying to cross the road. The aim of the agents is to cross the road while avoiding the cars present as soon as possible and if it reaches the other side of the road the episode will end and a new episode will begin.

● Components of Human Agents
Components attached to the human agent are -

- RayCast Sensor - to help it detect moving objects.

- Decision Requestor - Necessary for ML agent training.

- Behaviour Parameter - Necessary for ML agent training.

- Demonstration Recorder - Only when recording experts' games.

- Attached Scripts and other necessary components like MeshRenderer, etc.

● Observations of Human Agents

Following observations are recorded by the Human agent for ml-agent:

1. Human Agent Position - Size of 3
2. Human Agent Speed - Size of 4
3. Current Distance (distance between agent and the other side of the road) - Size of 1
4. Nearby obstacles (like Car, Tree, etc)- Ray Cast Sensor

Thus the size of VectorSensor is 8.

● Actions of Human Agents

The Human agent can take 2 types of actions:
1. Movement - Can move Forward, Backward or stay still.
2. Rotate - Can move 45 degrees Clockwise, AntiClockwise or stay
still.

● Scripts Used

The game has a total of 3 scripts attached to different gameobjects:

1. PlayerAgent.cs - Attached to the Human Agent and controls its behaviour and collects observations for Ml agent.

2. CarAI.cs - Attached to the Car prefab and controls its speed and behaviour in the game.

3. CarManager.cs - Attached to the Car Manager and spawn different cars in different lanes randomly.

● Rewards

Different rewards assigned to the Human Agents are -

1. +5 for crossing the road.
2. -5 if the agent collided with a car.
3. +0.05 if the current distance is less than the last.
4. -0.005 if the current distance is greater or equal to the last 
one.

● ML Algorithms
We are using 2 different approach to train our ML agent -
1. PPO (Proximal Policy Optimization)
2. PPO + BC (Behavioural Cloning)
For the PPO + BC we used a demonstration of about 20 minutes for the training
of our agent. Yaml file configuration of both the algorithms are :

● PPO : Proximal Policy Optimization :

Learning rate = 3e-4

Beta = 0.001

Gamma = 0.99

Epsilon = 0.2

max_steps = 1.5 million

● PPO + BC (Behavioural Cloning) :

behavioural_cloning:

    Strength = 0.5
    Steps = 200k

Extrinsic Reward :

    Gamma = 0.99
    Strength = 1.00

## Results

<b>PPO: Proximal Policy Optimisation</b>
![ed6b33af-8bd2-45b6-a268-6a9bcbff8db6](https://github.com/Pranav-Programmer/Trailblaze-Road-Crossing-Stimulation/assets/79044490/ea7657f5-be5d-489a-8a8d-89ab2d096447)


- The agent's performance improves significantly at the beginning, as seen by the steep slope until around 400k steps.

- After the initial phase, the learning curve flattens, indicating that the agent has likely learned a policy that performs consistently over time.

- There are fluctuations in the reward, which suggests that the environment may have some level of complexity or variability that the agent is still learning to handle optimally.

<b>BC: Behaviour Cloning + PPO</b>
![7e133ec0-f373-4b99-b037-05b7001c4187](https://github.com/Pranav-Programmer/Trailblaze-Road-Crossing-Stimulation/assets/79044490/f3e18db8-158a-4eca-9d3f-21dd8bd87855)


- Similar to the first graph(Fig.2), there's an initial phase of rapid improvement, followed by a plateau. However, the overall reward levels are lower compared to the first graph.

- The performance of this agent seems to stabilize quicker but at a lower reward threshold than the solely PPO-trained agent.

- The smoother curve could indicate that behavior cloning helped in stabilizing the learning process, but it might have also limited the potential of the agent to explore and achieve higher rewards.

● Comparison and Inference:
PPO vs. Behavior Cloning + PPO

- Comparing both, the pure PPO agent seems to achieve a higher cumulative reward than the agent trained with behavior cloning followed by PPO.

- The behavior cloning + PPO agent shows less variability in its performance, which is due to the initial behavior cloning phase providing a more stable but possibly less explorative policy.

- It is possible that behavior cloning has provided a good starting point, but may have introduced some bias or constrained exploration, preventing the agent from achieving higher rewards that pure PPO might reach through more extensive exploration.

## Features

1. Unity 3D environment with intelligent agents

2. Reinforcement learning with Proximal Policy Optimization

3. Behavioral Cloning for improved agent training

4. Realistic road-crossing challenges

## Getting Started

To run this project on your local system, follow the instructions below:

### Prerequisites

- Unity 3D installed on your system
- Basic understanding of Unity ML-Agents, PPO, and BC
## Running Locally

- Clone the repository.
- Open the project in Unity.
- Set up Unity ML-Agents.
- Explore and modify the game as needed.
## Feedback and Support

We welcome feedback and contributions from the community to improve Trailblaze. If you encounter any issues, have suggestions for new features, or wish to contribute to the project, please feel free to open an issue or submit a pull request on our GitHub repository.

For any feedback or support related inquiries, please contact me at https://pranav-programmer.github.io/Contact-Form/


## 

GitHub Repository: https://github.com/Pranav-Programmer/Trailblaze-Road-Crossing-Stimulation
