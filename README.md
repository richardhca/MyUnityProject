# Personal Indie Game Project
***Unity Version: 2019.2.18f1***

(Project name temporary untitled)

*** ***The Project is still in-progress*** ***

Control Config:
-----------------------
UP - Numpad 5,

DOWN - Numpad 2

LEFT - Numpad 1

RIGHT - Numpad 3

Camera UP - Numpad 7

Camera DOWN - Numpad 9

Camera LEFT - Numpad 4

Camera RIGHT - Numpad 6

Camera ZoomIn - Numpad 8

Canera ZoomOut - Numpad 0

Camera Reset - Z

Attack - F

Jump - D

Run - S (Press down while walking)

Restart - ESC

-----------------------


************** Dev Log ***************
------------------------------------------------

Features Completed:
 - Character Move
 - Camera adjust and rotation
 - Player Jumps
 - Character attacks
 - Character stat properties(e.g. HP, ATK, AGI, attack types)
 - Basic NPC (enemy) AI's
 - Ability to attack/kill each other or get hit/die 

Feature to be done:
 - Add HP Gauge
 - Add more enemy characters
 - Add attack effects
 - Add Title Scenes
 - Design levels
 
/************** Detail ***************/

-----2020/12/16-----

Implementation / Optimization:
 - Link character stat features so that player and enemy can hit and kill each other

Develop Note:
 - issue found: collider detection doesn't work on high speed, even continuous type also not working. So currently for melee type enemy, add animation event on attack anime instead of add trigger collider on enemy's weapon
 

-----2020/11/28-----

Implementation / Optimization:
 - Add basic AI logic to enemy, so the enemy will move towards player and attack player (use NavMeshAgent)


-----2020/11/27-----

Implementation / Optimization:
 - Camera Adjust feature optimize:
 --- Move the camera object out of character object in Unity
 --- Set camera container object position to character position in each frame
 --- when rotate camera, rotate the camera container angle
 - Optimize Rotate character speed depending on angle difference, the larger angle difference the faster rotate speed


-----2020/11/24-----

Implementation / Optimization:
 - Implement character stat properties and features (e.g. HP, ATK, AGI, attack types)
 - Refactor some functions / code components and move them to the proper places
 - Rename some animation names in Unity animator (animationController)


-----2020/11/23-----

 - Just add in a bunch of enemy models, animations and textures for future development works


-----2020/11/18----- 

Implementation / Optimization:
 - Group the scripts by namespace and function (script and file management)
 - Optimize the character turn so that character can turn smoothly instead of instantly (angle += value per frame until reach target angle instead of set to target angle directly)

Develop Note:
 - Rotate character must give positive angle value (e.g. 270 instead of -90), otherwise the character will have glitch while moving. (angle values shifting back and forth)


-----2020/11/05----- 

Implementation / Optimization:
 - Move Character
 - Rotate Character
 - Camera Rotate
 - Camera zoom in/out
 - Use Queue to handle a series of action animations
 - Jump feature (can Perform second jump)
 - Ability to shoot arrow (can perform triple continuous attack), press attack button on current attack animation to queue next continuous attack

Develop Note:
 - Move Character: use position += new Vector3(x,y,z) instead of velocity = new Vector3(x,y,z) because if use velocity, once character stop I have to do extra velocity = Vector3.zero
 - Move Character Algorithm: += Vector3(Sin(deg) * moveSpeed, 0, Cos(deg) * moveSpeed)
 - Camera rotate up and down algorithm: camera height in range [1.0, 6.0], within the range, on every camera height change by 0.25, camera x rotate change by 3.0
 - Camera zoom: range on local z-axis: [-7.5, -2.5]
 - Correct exact Arrow Spawn position value input manually (different attack animators respectively)
