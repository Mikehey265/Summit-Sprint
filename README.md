# Summit-Sprint
After 4 weeks of collaborating within a team of 16 very talented people, I am proud to present our accomplishment. Summit Sprint is a high-octane rock climbing game. As a climber, your objective is to reach the peak of the mountains as fast as possible. Climb and jump among rocks, manage to get enough chalk and not to fall from the cliff. </br>

## Table of contents
* [General info](#general-info)
* [Controls](#controls)
* [My contribution](#my-contribution)
* [Development cycle](#development-cycle)
* [Links](#links)
* [Screenshots](#screenshots)

## General info

Our objective was to develop a mobile game centered around the theme of extreme sports. After making some research, we went with extreme climbing as our project, as we noticed a notable lack of realistic yet arcade-style climbing games in the mobile games market. <br><br>
The game was designed for mobile devices with intuitive interaction for players. It is accessible and easy to learn, yet demands the skills of a pro gamer to truly master. Summit Sprint features a tutorial level along with three very well designed levels. Players can try to beat their own high scores, race against their personal best ghost, and try to complete a set of side objectives that unlock developer ghost to race with for an additional challege. Additionally, we implemented a chalk functionality that requires players to manage their chalk level to prevent from falling.

## Controls
* Slide on the left/right half of the screen to control the left/right hand of the climber to grab on rocks.
* Touch the climber to decide the direction of the jump, and release to jump. Remember to touch the screen to grab a rock after jumping, otherwise you can fall.

## My contribution
In this project, one of my responsibilities was to create and manage a game manager. Its primary function was to integrate everyone's work into a fully functional game. I created various game states to facilitate easier control of the game's behavior across different phases. Thanks to this solution it was very easy to come back and add additional functionalities to specific states.<br><br>
I was also responsible for creating saving system, score manager and side objectives. Ensuring that player scores persisted between sessions was a top priority for us. To achieve this, I implemented an efficient saving system using JSON files, which functioned precisely as we wanted. The score system was time-based, making it relatively straightforward. Players were rewarded with gold, silver, or bronze medals depending on whether they completed the level within the specified time. Implementing side objectives was a late addition to our project, as we were closing to our deadline. Despite the time constraints, I utilized scriptable objects to streamline the process, allowing designers to easily modify the side objectives to suit their needs. I successfully developed fully functional side objectives manager, enriching our game with yet another exciting feature. At the end I integrated side objectives, current time, best time, medals etc. into our UI, ensuring that players could easily view and track their progress during gameplay.<br><br>

## Development cycle

Our first working demo of the project includes input controls for maneuvering individual arms and snapping them into the rocks. This was a crucial initial milestone.

![first](https://github.com/Mikehey265/Summit-Sprint/assets/101410858/e8878ba4-2aa1-4569-92b2-98dc203d19e6)

After the initial implementation of the input controls was working, we decided to use a ragdoll model for the player. However, we quickly realized that this might not be the best idea.

![ragdoll](https://github.com/Mikehey265/Summit-Sprint/assets/101410858/57f8661d-e18d-44d6-9c67-8e11dff7e674)

After more than a week, we had our first alpha build. This build was given to local high school students and our mentors, who provided us with valuable feedback. We already had one functioning level, complete with working input, a stamina system, win and lose conditions, and basic pickups. We were halfway through the development process.

![alpha](https://github.com/Mikehey265/Summit-Sprint/assets/101410858/c4afe52d-3f54-47ec-9253-be81f243c335)

At that point, we decided to try adding a level editor for the player. Since our "mountain" was made from modular cylinders, it was fairly easy to create a basic shape for the level editor. Unfortunately, the more features we wanted to add, the more time it consumed. After two days, we scrapped the idea due to time constraints. The final version of the level editor included a rotating and zooming camera, the ability to add and remove cylinders, and options to save, load, and clear the entire level. It was a really nice experience, and even though it wasn't used in the final build, I'm proud of it.

![leveleditor](https://github.com/Mikehey265/Summit-Sprint/assets/101410858/b3444073-7f9b-4aea-b8f5-a6c173079a6e)

After four weeks in the making, we finally brought this game to life. The game received some very positive feedback from the jury members as well as other students that have played it, and I encourage everyone to try it!

![final](https://github.com/Mikehey265/Summit-Sprint/assets/101410858/b17da7d7-b186-4636-aba0-00fab9b02c81)
 
## Links
* https://playerzongying.itch.io/summit-sprint
* https://www.linkedin.com/feed/update/urn:li:activity:7167925900741656576/

## Screenshots
![skh](https://github.com/Mikehey265/Summit-Sprint/assets/101410858/9dd810cc-81a6-40bf-802e-3e5ba5f58b57)
![sko](https://github.com/Mikehey265/Summit-Sprint/assets/101410858/90c263b6-6842-4a77-9576-f2b67ca598db)
![sss](https://github.com/Mikehey265/Summit-Sprint/assets/101410858/33c82e87-4854-4762-916c-cc0336599c2a)
![scc](https://github.com/Mikehey265/Summit-Sprint/assets/101410858/e7e39472-eda3-4546-aab9-e73f8910564a)
![screen](https://github.com/Mikehey265/Summit-Sprint/assets/101410858/78ae69e9-e25c-4027-9ed9-e50789d74622)
![image_2024-03-18_143932936](https://github.com/Mikehey265/Summit-Sprint/assets/101410858/6d52263f-8df4-4d92-b62a-cef8f93d01cb)
