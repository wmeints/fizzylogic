# Willem's Fizzy Logic

This repository contains the sources for my personal website. It's still in the 
making after a very long time. I've restarted it a couple of times because I
wasn't happy with the results. But this time it's here to stay.

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

Please follow these steps to clone the repository and run the website on your machine:

* `git clone https://github.com/wmeints/fizzylogic`
* `cd fizzylogic/src/FizzyLogic`
* `dotnet user-secrets set "ConnectionStrings:DefaultDatabase" "<connection-string>"`
* `dotnet ef database update`
* `dotnet run`

**Note:** Be sure to have postgres running on your machine. I'm using Docker, but
you can run postgres directly if you want. 

## Contributing

I don't take contributions. This code is here for reference, hope you like it!
