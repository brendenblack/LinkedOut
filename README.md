# LinkedOut
<p align=center>
    <a href="https://github.com/brendenblack/LinkedOut"><img alt="LinkedOut build status" src="https://github.com/brendenblack/LinkedOut/workflows/build/badge.svg" /></a>
</p>

> ## *Submitting resumés to the void*

LinkedOut can help you keep track of all of the fruitless applications you submit during a job hunt. Keep detailed records of the job description, the resumé and cover letter you sent in, and any contact you miraculously had with the company.

## Technology stack
LinkedOut was built with [server-side Blazor]("https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor") as the UI on top of patterns informed mostly by Jason Taylor's [Clean Architecture](https://github.com/jasontaylordev/CleanArchitecture/tree/main/src).


## Setting up the dev environment
1. Set up database access

appsettings.Development.json is preconfigured to use MSSQLLocalDB

2. Set up an OIDC authority

You can start up a local instance of Keycloak with the following command:

`docker run -d -p 8080:8080 -e KEYCLOAK_USER=admin -e KEYCLOAK_PASSWORD=admin quay.io/keycloak/keycloak:12.0.1`

Then you have to add a realm & client, and populate appsettings.Development.json with the appropriate values.