# Willem's Fizzy Logic

This repository contains the sources for my personal website. I use this code
in production to run my blog. 

## Goals for this repository

* Provide reference for people looking to learn from a real-world website.
* Provide a place to store the code and configuration for my website.

Please, keep in mind that this code is optimized for the way I like my website
to work. I could have used a standard tool for my blog. But I didn't because I
wanted a learning challenge. This is not meant to be a product that you can 
use yourself without modifications.

## System requirements

* .NET 5 SDK
* Visual Studio Code
* Postgresql

In addition to these tools you'll need to have the Entity Framework Core tools
installed, you can install these using the following command:

```
dotnet tool install -g dotnet-ef
```

## Getting started

Please follow these steps to clone the repository and run the website on your 
machine:

* `git clone https://github.com/wmeints/fizzylogic`
* `cd fizzylogic/src/FizzyLogic`
* `dotnet user-secrets set "ConnectionStrings:DefaultDatabase" "<connection-string>"`
* `dotnet ef database update`
* `dotnet run`

**Note:** Be sure to have postgres running on your machine. I'm using Docker, but
you can run postgres directly if you want.  The connection string is of the format
`host=127.0.0.1;database=fizzylogic_dev;username=postgres;password=<your-password>`.

## Deploying

The deployment process for my website involves a few manual steps. This is not ideal
but it will do for now. Please find the description of the deployment process below.

### Generating deployment files
For deployment purposes I'm using a PowerShell script to clean the deployment
folder and copy in all the necessary bits for my webserver.

You can find the script in `Generate-Deployment.ps1`. It performs the following
tasks:

* Remove the dist folder if it exists.
* Run the publish command on the website project to the `dist/website` folder.
* Remove the `content` folder from `dist/website/wwwroot` so we don't copy any test content.
* Copy the migration scripts to the `dist/migrations` folder.

**Note:** I'm assuming that the configuration files don't contain any secrets.
These are all set through variables in the service configuration.

### Deploying the website files to the webserver

I'm using `scp` to copy the deployment files to the webserver. After I've done
that, I login using SSH and copy them to the right location, which 
is `/var/www/blog`. 

### Service configuration on the webserver

After I've deployed the files to the server, I need to restart the website.
I'm running the website behind a reverse proxy (Nginx). The ASP.NET process
is running as a separate service on my webserver.

So to finalize the deployment I run the command `sudo service fizzylogic restart`.
And that's it! The new version of the website is live!

For your convenience I've included a cleaned up version of the `fizzylogic.service`
in the `config` folder of the repository. It's missing the secrets that I use,
but I've added a placeholder for those, so you know where to add them yourself.

Typically, you copy the service configuration file to `/etc/systemd/system/`
and run `systemctl reload-daemon` to reload the service configuration.

### Database configuration

I'm using Postgres on my webserver with a user that is specific to the application.
The database creation and upgrade process, again, involves a few manual steps.

To create the database I use the following commands:

* Execute `sudo -i -u postgres` to get access to the postgres user.
* Execute `psql` to open the postgres terminal.
* Execute `CREATE DATABASE fizzylogic;` to create the database.
* Execute `\q` to close the postgres terminal.

I don't use the EF core tooling to run migrations. Instead I use the following command
to script the migrations from the `src/FizzyLogic` folder to a specific sql file:

```
dotnet ef migrations script -i -o ../../migrations/<output-file>.sql
```

The `-i` option makes the script idem-potent, so I can run it multiple times 
without wrecking the database. The `-o` option lets me specify the output file.
I like to number my migrations and add some type of description for them.

After generating the migration script, I copy the SQL file to the webserver and 
then use the following steps to migrate the database:

* `sudo -i -u postgres`
* `psql -d fizzylogic -f <output-file>.sql`

The website uses a specific postgres user to access the database. This user 
has limited permissions to ensure that nothing bad can happen in production.

I've used the following steps to create the user and give it the right permissions.

* Execute `sudo -i -u postgres` to login as the postgres user.
* Execute `createuser --interactive` to create a new user for postgres.
* Execute `psql` to start the postgres terminal
* Execute `ALTER USER <your-user> WITH PASSWORD <your-password>` to set a password for the user.
* Execute `GRANT CONNECT ON fizzylogic TO <your-user>;`
* Execute `\connect fizzylogic_prod` to switch to the new database.
* Execute `GRANT USAGE ON SCHEMA public TO <your-user>;` to grant usage rights for the new user.
* Execute `GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO <your-user>;` to give the new user access to the tables in the database.

## Contributing

I don't take contributions. This code is here for reference, hope you like it!
