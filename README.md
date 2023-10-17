# FG_CompTech

A simple space shooter. It was first made in Unity using regular GameObjects and MonoBehaviour conventions. Afterwards it was converted (practically rewritten) to use Unity DOTS for huge performance gains.

- Unity 2022.3.10f1
- Packages:
    - com.unity.entities-1.0.16
    - com.unity.platforms-1.0.0-exp.6
    - com.unity.burst-1.8.8
    + (+ all their dependencies)

## Controls

- WASD/Arrow Keys = move & rotate ship
- Space = Fire
- R = Restart
- Q = Quit

------------------------------------

# Making the Game

## "Not DOTS"-version

I first started off making the game using my regular Unity knowledge. I made the enemies prefabs, spawned them in, made them move using their own Movement scripts, saved references wherever possible, and so on. 

Initially I used Physics to detect bullet hits and collision, but later, to make the comparison between the DOTS version and non-DOTS version more fair I disabled Physics.

For my testing, I usually played until I went below 30 FPS. I feel like 30 FPS is the minimum.

Here are the performance numbers for the "Not DOTS"-version, when played in a build (not in the editor):

|Physics?|Avg FPS ~1000 enemies|Avg FPS ~2000 enemies|Avg FPS ~20000 Enemies|Avg FPS ~28000 Enemies|Avg FPS ~32000 Enemies|Comment|
|--------|---------------------|----------------------|---------------------|-----|----|-----|
|Yes|120 FPS (VSync limit)|5 FPS|?|?|?|Seems to hit some kind of hard limit at 2000 enemies, making the framerate insufferable
|No|120 FPS (VSync limit)|120 FPS (VSync limit)|115|50|<30 FPS|

If we look at the profiler inside Unity when there are around 25000 enemies on screen (ignoring the Physics version, as Physics is the obvious culprit there) we can see that the biggest culprit is the enemies movement, which makes sense as there are a lot of enemies that need to be moved:

![Enemy Movement Profiler](https://djsimg.org/Forceful-Downright-Ilsamochadegu.png)

In second place is the Camera Render:

![Camera Render 1](https://djsimg.org/Distant-Coordinated-Monarch.png)

The render taking up 0.7% is not very interesting right now but keep it in mind for when we get to the DOTS version.

Now that I had a base game running and a baseline, I went off to convert the game to DOTS. I didn't bother doing any optimization of the regular GameObject/MonoBehaviour approach (such as Object Pooling) as I knew investing my time in learning DOTS would be more beneficial.

## DOTS Version

Having never used DOTS, ECS, or anything related to these systems I first had to figure out how all this works and what it is. I found that programming in DOTS requires a very different mindset which takes a while to get used to.

### Rant About Terrible DOTS Documentation

I very quickly ran into issues. Googling for things about DOTS is not easy, because people use different terms (some people just say ECS, which is one part of DOTS, for example), and sometimes you get results for "regular" MonoBehaviour approach. On top of that, DOTS changed a lot over the years. I believe the first version came out sometime around 2019, and it changed a lot since then for the 1.0 version we have now.

So the first day or so was spent in frustration trying to find some good resource of information. 

I finally found Unity's own [Kickball](https://github.com/Unity-Technologies/EntityComponentSystemSamples/tree/master/EntitiesSamples/Assets/Tutorials/Kickball) sample project which was pretty similar to what I was trying to do (player movement + firing projectiles), so I started by following that.

I learned that there were two kinds of systems you could use with ECS: `SystemBase` and `ISystem`. As I understood it, ISystem was better for performance, so I stuck with that.
I also learned about the Burst compiler, and learned that not everything is compatible with Burst (especially when interacting with MonoBehaviour), so I set my goal to have everything use ISystem and be Burst compatible, for maximum performance.

### Conversion

The first thing I converted was the enemies movement and spawning, knowing that enemy movement was the biggest culprit. 
I set up a Enemy prefab which had a `EnemyAuthoring` script on it, and then I set up two systems: one for spawning and one for enemy movement.

Initially the EnemyMovement script also did a lot of other things too (like damaing the player) but I later learned/came to the conclusion that each system should do as little as possible to maintain readability.

I tested this and was *blown away* by the performance. I had to significantly bump up the spawn rate of enemies and make them much smaller to not have them fill up the screen immediately (this is why in the "pre-optimization" build they're big hexagons, and now in this optimized build they are tiny dots).

After I had the enemies figured out I converted PlayerMovement. I was afraid this was gonna be very complicated but no it was actually surprisingly simple, as you have access to the regular `Input.GetAxis` functions and such inside ECS, so I got that taken care of quickly. It also helps that I now understood Entities and Systems much better than the day before.

TODO: 

- Write about vector distance vs math.abs (profile it)
- Bullets, (how i tried pooling)
- GameManager being the only mono behaviour now