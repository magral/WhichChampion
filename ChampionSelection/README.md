# WhichChampion

When you don't know what champion you want to play, let the champion selector decide for you! Just input your preferred lane, damage, playstyle, 
whether you want to play something familiar or not, and your summoner and get a champion back! 
Compatible for Mac & Windows. And probably Linux, but I don't have a machine to test it on.


Project Thoughts:
- A more appropriate way to get the API key would be to host a config of on a server, but for something so small it seemed unnecessary
- For a production application I would have created a local file that saves a list of champions from the API. It would then only re-ping the
API on a 2 week cadence (Riot champion release schedule).
