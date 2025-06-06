# Arena Kits
Give player prefixed kits in arena mode

- Players cannot use /kit command when arena is active
- Players will automatically receive kits when spawning or joining the server
- Selected kit will be saved for each player

### Using
Opening the configs inside Rocket/Plugins/ArenaKits/..., you can add new kits, after creating your kit go to ``Permission.config.xml`` inside Rocket folder, add a new line
```
<Permission Cooldown="0">arenakit.MyKit</Permission>
```

The kits should accept kitnames with spaces

Metadata, used for storing gun mods, to get the metadata simple join the server and give yourself the guns, add the mods you wish, type the command /metadata, take a look in your server console and the metadata should be there

### Configs
- KitCommandOnlyInArea: Players can only use the kit command in configurated areas ``KitCommandAreas``
- KitCommandArenaPlayingCheck: If arena is still running players cannot use the kit command
- KitCommandAreas: Stores the areas for the ``KitCommandOnlyInArea``
- Items: Stores the kits items

# Building

*Windows*: The project uses dotnet 4.8, consider installing into your machine, you need visual studio, simple open the solution file open the Build section and hit the build button (ctrl + shift + b) or you can do into powershell the command dotnet build -c Debug if you have installed dotnet 4.8.

*Linux*: Unfortunately versions lower than 6 of dotnet do not have support for linux, the best thing you can do is install dotnet 6 or the lowest possible version on your distro and try to compile in dotnet 6 using the command dotnet build -c Debug, this can cause problems within rocket loader.

FTM License.
