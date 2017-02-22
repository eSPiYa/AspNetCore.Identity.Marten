# Sample ASP.NET Core application using AspNetCore.Identity.Marten

## To make it work
Add this with proper values in your 'appsettings.{env.EnvironmentName}.json' or in your 'secrets'
```json
  "Data": {
    "Store": {
      "Host": "hostname",
      "Database": "database-name",
      "Username": "db-user",
      "Password": "db-user-password"
    }
  }
```

## To enable Facebook authentication
Add this with proper values in your 'appsettings.{env.EnvironmentName}.json' or in your 'secrets'
```json
  "Authentication": {
    "Facebook": {
      "Enabled" : true,
      "AppId": "1354410337939024",
      "AppSecret": "72006c8e0e6d34a3d69c7d3b4d811d4c"
    }
  }
```