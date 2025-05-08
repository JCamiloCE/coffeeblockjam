<h1 align="center"> Coffee Block Jam </h1>
<h4 align="center">
:construction: Project under development :construction:
</h4>

This project started as a technical test but has gradually evolved into a more complete game aimed at expanding my portfolio. 
It's a mobile-oriented video game developed in Unity 3D, also playable with a mouse. The core mechanic involves moving trays 
that collide with each other and the level walls. Additionally, the project includes an in-editor tool that allows for designing 
levels directly within the game.

<h4 align="center">
:construction: Project under development :construction:
</h4>

## :hammer: Project Features

- `Feature - Tray Movement`: Move the trays, which are dragged (by mouse or touch), the movement is by physics.
- `Feature - Create level`: It allows the design and creation of a level through the development of a tool that generates a JSON
  with the level information. On the other hand, the level reads the generated JSON and can create the level components.
- `Feature - Unit Tests`: Some of the code is supported by Unit Tests, using NUnit.

## :construction_worker: Next Steps

- `Unit Tests`: Add more Unit Tests to have a good code average.
- `EColorTray`: Make the EColorTray enum clearer and eliminate unnecessary code.
- `Pull out the Trays`: The trays must be able to “exit” through the wall that is painted the same color as the trays.
