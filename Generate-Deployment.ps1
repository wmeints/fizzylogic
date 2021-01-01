##
## This script generates the deployment fileset for the website.
##

# Make sure we have a nice and clean deployment slate.
rm -recurse -force dist

# Run a regular deployment. This will automatically generate the JS + CSS 
# content in the output folder as well.
dotnet publish src/FizzyLogic/FizzyLogic.csproj -c Release -o dist/website

# Make sure we copy the SQL Files for the database migrations as well.
# May not always need them, but it's nice to generate a complete deployment package.
cp -recurse migrations dist/

# I don't want to include the content foldder in any deployment.
# I have this content on my machine for testing purposes, but the webserver 
# should have its own copy of this content.
rm -recurse -force dist/website/wwwroot/content