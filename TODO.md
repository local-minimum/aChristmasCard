Major Issues
============

Game Logic
----------

* Need a secondary button, or way of showing that player wants to cancel applying currently active/using object.
* Need to have intuitive way of cancelling zoom view
* New game needs to refresh all instances in scene, not only the save state
* Save state needs to know character position and where things are / what is solved / state of things.
* Room broadcasts to self about player leaving and entering, used for getting mail at game start as well as stove/fire thing.
* Transitioning between rooms need to transport player to actual ip position in new room or save/resume won't work correctly

Minor Issues
============

Graphics
--------

* Specific shader for applying words on sprite textures
* Letter view bg should have words, one small text still, one big lettters
* Active word when solving letter neads to be clearer that it is active
* Transition to new room should have slower cam or cam follow player.

Game Logic
----------

* When solving word, putting word in wrong place should invoke sigh
