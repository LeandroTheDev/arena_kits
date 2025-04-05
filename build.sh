#!/bin/sh
game_dir=".../SteamLibrary/steamapps/common/U3DS/Servers/TestArena/Rocket/Plugins/"
library_dir=".../SteamLibrary/steamapps/common/U3DS/Servers/TestArena/Rocket/Libraries/"

dotnet build -c Release
cp -r ./bin/Release/net4.8/ArenaKits.dll "$game_dir"