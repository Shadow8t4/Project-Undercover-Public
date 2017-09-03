# The Dream Machine

This repository houses Dream Machine's code and project files for CSCE 443 / VIST 487 - Game Development.

## Special Unity Settings

[This post on Stack Overflow](https://stackoverflow.com/questions/18225126/how-to-use-git-for-unity3d-source-control)
has lots of useful suggestions for making Unity3D play nice with Git.

Namely, make sure you the Unity project is setup like this:

1. Enable the `External` option in `Unity -> Preferences -> Packages -> Repository`.
2. Open the `Edit` menu and pick `Project Settings -> Editor`.
3. Switch `Version Control Mode` to `Visible Meta Files`.
4. Switch `Asset Serialization Mode` to `Force Text`.
5. Save the scene and project from the `File` menu.
