#!/bin/sh
game_dir="/home/bobs/System/Devices/Hard Drive/SteamLibrary/steamapps/common/U3DS/Servers/playtoearn3/Rocket/Plugins/"
library_dir="/home/bobs/System/Devices/Hard Drive/SteamLibrary/steamapps/common/U3DS/Servers/playtoearn3/Rocket/Libraries/"

dotnet build -c Release
cp -r ./bin/Release/net4.8/ArenaKits.dll "$game_dir"
cp -r ./Libs/MySql.Data.dll "$library_dir"
cp -r ./Libs/Ubiety.Dns.Core.dll "$library_dir"