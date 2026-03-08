#!/bin/bash
alacritty --title "Pokemon Game" -e bash -c "cd \"$(dirname "$0")/PokemonGame\" && dotnet run; read -p 'Spausk Enter išeiti...'"
