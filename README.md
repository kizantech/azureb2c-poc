# azureb2c-poc


* ​Develop .NET 8 / Blazor Server-Side POC App 
* ​Integrate Microsoft Work Accounts 
* ​Integrate Microsoft B2C Personal Accounts 
* ​Integrate Google OAuth 
* ​Integrate Local Account Support 
* ​Data segregation based on Customer Tenant ID 
* ​Custom Sign-Up flow based on link validation 
* ​Identify customer id for non-work accounts 
* ​Blazor Dashboard Sample 
* ​PowerBI Embedded Report Sample

## Samples and Assistive tutorials:
* [Azure B2C](https://learn.microsoft.com/en-us/azure/active-directory-b2c/)
* [Blazor with B2C](https://blazorhelpwebsite.com/ViewBlogPost/55)
* [Multitenancy with Blazor and EF Core](https://blog.jeremylikness.com/blog/multitenancy-with-ef-core-in-blazor-server-apps/)
* [Multitenancy with EF Core](https://learn.microsoft.com/en-us/ef/core/miscellaneous/multitenancy)
* [Finbuckle Multitenancy](https://www.finbuckle.com/MultiTenant/Docs/v6.13.1/Identity)
* [Configuring Custom Policies in Azure B2C](https://www.youtube.com/watch?v=aL1kKAH5Sa8&list=PL4svy-vB4AaxRunWQkxOe8h3zP9jAzS5Z&index=7)
* [Setup B2C + B2B Together](https://www.kallemarjokorpi.fi/blog/how-to-configure-azure-ad-b2c-and-b2b-work-together.html)

## Setting up the Azure B2C Sample App

### Azure Required Resources:
* Azure App Service
* Azure SQL Database
* Azure B2C Tenant
* Azure App Registration
  
Update the appsettings.json AzureAdB2c with your configuration:
```json
{
  "AzureAdB2C": {
    "Instance": "https://{{Your Azure B2C Domain}}.b2clogin.com/tfp/",
    "ClientId": "{{Your Application Client ID}}", // Azure B2C Application Client Id
    "CallbackPath": "/signin-oidc",
    "Domain": "{{Your Azure B2C Domain}}.onmicrosoft.com",
    "SignUpSignInPolicyId": "B2C_1A_SIGNUP_SIGNIN",
    "ResetPasswordPolicyId": "{{Optional Reset Password policy}}",
    "EditProfilePolicyId": "{{Optional Edit profile policy}}",
    "B2cExtensionsAppClientId": "{{b2c-extensions-app Client Id}}",
    "B2cCorsDomain": "{{Your Azure B2C Domain}}.b2clogin.com"
  },
  ...
}
```

Update your database connection strings (in Azureb2C.AuthApi, AzureB2C.Blazor, AzureB2C.Data.Migrator appsettings.json files):
```json
{
    ...
  "Finbuckle:MultiTenant:Stores:ConfigurationStore": {
    "Defaults": {
      "ConnectionString": "{{Your Azure SQL Server connection string}}"
    }
  },
  "ConnectionStrings": {
    "IdentityDatabase": "{{Your Azure SQL Server connection string goes here}}"
  }
}
```

Run ef core database initialization
```powershell
dotnet run -- migrate
```

This will deploy the Finbuckle Multitenant datastore to your Azure SQL Database

Next up, deploy the application to your Azure App Service. We recommend using the Dockerfile build to build a container and publish it to an Azure Container Registry, and then deploy the App Service using the Docker container. You can also use the VS Publish, or Azure DevOps deployment to deploy the App Service using traditional methods. Any of these methods are supported, and documentation for them can be found here:
* [Deploy using Dockerfile](https://learn.microsoft.com/en-us/azure/app-service/tutorial-custom-container?tabs=azure-cli&pivots=container-linux)
* [Publish using Visual Studio](https://learn.microsoft.com/en-us/azure/app-service/quickstart-dotnetcore?tabs=net70&pivots=development-environment-vs)
* [Deployment using Azure Pipelines](https://learn.microsoft.com/en-us/azure/app-service/deploy-azure-pipelines?tabs=yaml)

## Power BI Embedded Config & Help
* [PowerBI REST APIs](https://learn.microsoft.com/en-us/rest/api/power-bi/)
* [PowerBI Service Principle Profile SDK](https://learn.microsoft.com/en-us/power-bi/developer/embedded/service-principal-profile-sdk)
* [PowerBI Multi-tenancy](https://learn.microsoft.com/en-us/power-bi/developer/embedded/embed-multi-tenancy)

### 1. Configure Power BI Embedded

#### Create a Power BI Embedded Capacity

1. Sign in to the [Azure portal](https://portal.azure.com/).
2. Create a new Power BI Embedded resource.
3. Assign the capacity to your workspace in Power BI.

#### Create Power BI Workspace

1. Sign in to [Power BI](https://app.powerbi.com/).
2. Create a new workspace for your reports and datasets.

### 2. Set Up Role-Level Security (RLS) in Power BI

This can be done either through the PowerBI Desktop Application, or through the PowerBI REST API's. Once you setup RLS groups for each customer, then you will need to create Service Principal Profiles, and assign each profile to the RLS group. Then in the `CustomerInfo` database, you will need to assign the ProfileId based on the PowerBI config.

### References

For detailed documentation and further API capabilities, visit the [Power BI REST API documentation](https://docs.microsoft.com/en-us/rest/api/power-bi/).

### 3. Integrate Power BI Embedded in the Blazor Server Application

The project already includes a `PbiEmbedService` class in the `AzureB2C.Blazor.Services` namespace.

#### Configure Power BI Embedded in `appsettings.json`

Update your `appsettings.json` file with the necessary Power BI configuration.

```json
{
  "PowerBI": {
    "WorkspaceId": "your-workspace-id",
    "ReportId": "your-report-id",
  }
}
```

