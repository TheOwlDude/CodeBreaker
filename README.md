# CodeBreaker
Discover the secret ordered color code by refining guesses given hints.

An F# library provides the game engine.

The web server is hosted in the WebSiteHost project. This the startup project for the solution.

When WebSiteHost is running, CodeBreaker is available at http://localhost:9000/UI/CodeBreaker.html

Even if you happen to be familiar with this game, there are two unusual features. 
The game allows you to include with each guess response, the set of codes that are consistent with
the game state to date. In other words, the codes that could still be the answer. Independently, the 
game allows you to include with each guess response every possible guess ordered by descending quality. 
In other words, the best next guesses.

Enjoy
