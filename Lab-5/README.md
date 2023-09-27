# Laboratory work #5

## About

This folder is dedicated to Lab-5.

## Tasks:
_Lab5_ - Build a mathematical model of the system based on the Markov process with
discrete states and discrete time, to find the probabilities of stay
systems in the specified states. 

### Problem:

The technical security department conducts a search of embedded devices in the meeting room.
According to ageny data, it is known that the attackers planted 4 electroacoustic devices.
Specialists of the department plan to carry out 5 consecutive tests. Determine the probability 
of a system of 4 devices to be in the following states:

- _S<sub><sup>1</sub></sup>_ - no device is detected;
- _S<sub><sup>2</sub></sup>_ - 1 device detected;
- _S<sub><sup>3</sub></sup>_ - 2 devices detected;
- _S<sub><sup>4</sub></sup>_ - 3 devices detected;
- _S<sub><sup>5</sub></sup>_ - 4 devices detected;

It is assumed that known transition probabilities are the probabilities of detecting 1, 2, 3 or 4
electroacoustic devices when transitioning from state to state during tests. The system is assumed
to start from state _S<sub><sup>1</sub></sup>_.
|_P<sub><sup>12</sub></sup>_|_P<sub><sup>13</sub></sup>_|_P<sub><sup>14</sub></sup>_|_P<sub><sup>15</sub></sup>_|_P<sub><sup>23</sub></sup>_|_P<sub><sup>24</sub></sup>_|_P<sub><sup>25</sub></sup>_|_P<sub><sup>34</sub></sup>_|_P<sub><sup>35</sub></sup>_|_P<sub><sup>45</sub></sup>_|
--- | --- | --- | --- | --- | --- | --- | --- | --- | ---  
|0.25|0.20|0.10|0.05|0.30|0.25|0.10|0.40|0.15|0.60|

## Graph
![Algorithm photo](https://github.com/FranklinMar/Modelling/blob/main/Lab-5/graph.png)
