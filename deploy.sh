#!/bin/sh

# Build a release from the sources.
# This will compile the .NET binaries, and stylesheets.
dotnet publish -c Release src/FizzyLogic.sln -o dist

# Synchronize the sources to the remote server.
rsync -r -zP -o -g dist/ willem@fizzylogic.nl:/var/www/blog

# Restart the backend on the server.
# Note: This relies on the fact that you can run this command without sudo.
ssh -i ~/.ssh/id_rsa willem@fizzylogic.nl 'sudo systemctl restart fizzylogic'
