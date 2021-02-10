# LinkedOut
<p align=center>
    <a href="https://github.com/brendenblack/LinkedOut"><img alt="LinkedOut build status" src="https://github.com/brendenblack/LinkedOut/workflows/build/badge.svg" /></a>
</p>

> ## *Submitting résumés to the void*

LinkedOut can help you keep track of all of the fruitless applications you submit during a job hunt. Keep detailed records of the job description, the résumé and cover letter you sent in, and any contact you miraculously had with the company.

## Technology stack
LinkedOut was built with [server-side Blazor]("https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor") as the UI on top of patterns informed mostly by Jason Taylor's [Clean Architecture](https://github.com/jasontaylordev/CleanArchitecture/tree/main/src).

### Authentication
The app is built to authenticate with an OIDC authority. I happen to be developing against Keycloak but it doesn't matter too much. The following blog posts were instrumental
in figuring out how to wire up authentication:

1. [Blazor Authentication with OpenID Connect](https://mcguirev10.com/2019/12/15/blazor-authentication-with-openid-connect.html)
1. [Blazor Login Expiration with OpenID Connect](https://mcguirev10.com/2019/12/16/blazor-login-expiration-with-openid-connect.html)


## Setting up the dev environment
1. Set up database access

appsettings.Development.json is preconfigured to use MSSQLLocalDB

2. Set up an OIDC authority

You can start up a local instance of Keycloak with the following command:

`docker run -d -p 8080:8080 -e KEYCLOAK_USER=admin -e KEYCLOAK_PASSWORD=admin quay.io/keycloak/keycloak:12.0.1`

Then you have to add a realm & client, and populate appsettings.Development.json with the appropriate values.

### A note on migrations
This application can support SqlServer and PostgreSQL, and so it has to maintain two sets of migrations. There is a script in the root, `create_migration.bat` that is
configured to make both sets for you. The script requires a migration name as a parameter, e.g. `create_migration InitialCommit`.

There is an issue with the current EF version where migrations are being created that touch all identity columns unneccessarily, tracked in [this issue](https://github.com/dotnet/efcore/issues/23755) and [this issue](https://github.com/dotnet/efcore/issues/23456).

The latter identifies the proper fix to host each provider in separate projects which is a planned improvement, but the workaround for now is to remove all of the AlterColumn statements that aren't required, e.g.:
```C#
migrationBuilder.AlterColumn<int>(
    name: "Id",
    table: "JobSearches",
    type: "int",
    nullable: false,
    oldClrType: typeof(int),
    oldType: "int")
    .Annotation("SqlServer:Identity", "1, 1");
```