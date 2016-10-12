# CodeBreaker
Discover the secret ordered color code by refining guesses given hints.

The F# game engine has been swapped out with a pure javascript implementation. Now all processing is client side. The F# code is still present on the SelfHosted branch but has been removed from master.

The game is available on-line to play at theowldude.net/CodeBreaker/CodeBreaker.html

Even if you happen to be familiar with this game, there are two unusual features. 
The game allows you to include with each guess response, the set of codes that are consistent with
the game state to date. In other words, the codes that could still be the answer. Independently, the 
game allows you to include with each guess response every possible guess ordered by descending quality. 
In other words, the best next guesses.

Enjoy
