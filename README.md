# YSD Test Task 

## Description
Current implementation of AuthenticationService uses default .NET identity implementation.

CLI tool is used as client interface to interact with service.

## Project Structure

1. **YSD.Client.Cli** - Command Line Interface used to interact with WebService - AuthenticationService.

> Uses _Shared/YSD.AuthenticationService.Integration for communication
> 
> with WebService.

2. _Shared/**YSD.AuthenticationService.Integration** - Package for 3rd party integration with main AuthenticationService. Can be used in ClientApps and other WebServices as well.

> Implements Factory for convenient client initialisation.
> Implements DependencyInjection extensions for ease of usage.

3. Folder WebServices - Contains all WebServices in subfolders.

4. WebServices/AuthenticationService/**YSD.AuthenticationService.Web** - AuthenticationService Web Application itself.

5. WebServices/AuthenticationService/**YSD.AuthenticationService.Application** - Domain logic for AuthenticationService.

> Implements DependencyInjection extensions for ease of usage

6. WebServices/AuthenticationService/**YSD.AuthenticationService.DAL** - DAL assembly. Containing AppDbContext and Migrations.

> Implements DependencyInjection extensions for ease of usage
