# Arena Fight!

This is my solution for the programming test provided by Reim Entertainment. It is a 3-player, local multiplayer game. Each player controls one fighter in the arena, and last one standing wins! (More information about controls can be found in the game's main menu.)

About Building for Local Networks:

I have included a build that works on one windows machine. However, it is possible to build the project in a way that lets you play across multiple windows boxes that are all on the same local network. Below are the steps to do so.
  1. Download the project and open it in Unity (version 2019.4)
  2. In the Menu scene, open the NetworkingManager object and scroll down to its UnetTransport component
  3. Find the Connect Address, which is currently 127.0.0.1
  4. Change this address to your own IP address. This address will host the game server on your network
  5. Save, and then build the project by going to File->Build and selecting Windows (it may work on other platforms, but there is no guarantee)
  6. Now you can play on different devices on your local network! Enjoy!
 
About Running the Project in Unity:

If you wish to run the game in the Unity Editor, make sure you start in the Menu scene. Otherwise, it won't run properly. Also, a side note: someone playing in the editor can connect to someone playing the build. Also, you can change the number of players by going into the Arena scene and looking at the Arena Manager. There's a variable that lets you set the number of players required to play the game. However, since I built the arena for three players, if you try to add more players, it won't work unless you add more spawn locations (these must have the "Spawn" tag).
