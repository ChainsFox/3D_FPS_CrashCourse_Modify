3DCrashCourse
04/02/2025:
- Created project and push into github using git hub lfs - small test

05/02/2025:
FPS Full Game Tutorial - Mike's Code
part 1 done
part 2 - 6:41

06/02/2025 - part 3 done
07/02/2025 - part 4 done
09/02/2025 - part 6 done
10/02/2025 - part 7 done
11/02/2025 - p8 done
12/02/2025 - p9 done
15/02/2025 - p10 8:35 - outline raycast not working

17/02/2025:
+)Added a box collider to fix the outline bug, its not working not because of the logic, but because of the gun component(It need a collider) 
p10 - 19:33

18/02/2025:
- 24:10 - he said he fix the outline but if we try we can still bug it out(need to be fix) - he fixed it in later episode
p10 - 28:40

19/02/2025
p10 - done

20/02/2025
+)2:41 - BLACKSCREEN SOLUTION ----  make sure your WeaponRenderCamera layer is default then to to camera --> Render Type --> then change Base to Overlay. THEN go to your Main Camera --> Stack --> Cameras --> then press the + then select the WeaponRenderCamera
p10 - 11:56

23/02/2025
p11 - done
p12 - 3:57

24/02/2025
p12 - 15:35(almost done)

25/02/2025
p12 - done
p13 - done
- CalculateDirectionAndSpread function might be bugging because it is not working like the video

Mess around with this code here:
float z = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

27/02/2025
p14 - 8:42 - effect start

28/02/2025
p14 - done(grenade)

01/03/2025
p15 - done 
-one typo/mistake is all you need to go crazy :)

04/03/2025
p16 - 7:05

05/03/2025 
p16 done
p17 - 12:42
->Remember to use gitbash to push to github(Done, use git lfs track "*.FBX")

05/03/2025 
p17 done

06/03/2025
p18 - 19:17

10/03/2025
p18 done

11/03/2025
p19 done

12/03/2025
p20 - 19:17

16/03/2025
p20 - done

17/03/2025
p21 - 12:22(ui part)

06/04/2025
p21 - done

09/04/2025
p22 - 21:04
-)Weird outline error for smoke and ammobox if you point at them first they will bug out  -> Fixed: By disabling the smoke and ammobox outline enabled = false code(?)

10/04/2025 
p22 - done
