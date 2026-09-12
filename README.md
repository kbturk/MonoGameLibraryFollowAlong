## MonogameLibrary Follow Along

This is a follow along for MonoGame's new tutorial. The end product of this
module is a reusable library that is build on the monogame framework. Originally
this pairs with the DungeonSlime repo under this user account.

### Personal TODO:
Move the Gum UI custom modules from DungeonSlime to this repo and rewrite the
init function to take custom texts, atlas, and texture objects so it can be reused.

Review the Scene classes. Move those into the library as well (if needed.)

There's a bug where when you come back from the options screen, bot the start
and options buttons have handles. (fixed in final rewrite.)

Look up EventHandler class. I'm not that familiar with the workings of it.

The slime body is different then the head. - I'd like to color the head differently.
The color of the slime changes during the animation??

Fix the Options text
Add exit button on the home page.

The collision logic isn't working when the head crosses the body.
Add lives and a way to save high scores.
Add the ability to add a name to a high score page.
Add easy/med/hard?? (with different score amounts.)

Scores can be written using a xml serializer. Look at this tutorial
https://gamedevbeginner.com/how-to-keep-score-in-unity-with-loading-and-saving/#save_high_score
