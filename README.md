# graphdeamon
Description: Simple .NET Console app to test app permissions with MS Graph


## Setting up Azure AD
You need to create a new App Registration in Azure AD
- In the App Registrations tab, click on **New Registration*
- Give it a name and press **Register**
- In the **Authentication** tab, add a new platform and select **Mobile and Desktop application**
- Select the msal **Redirect URI**
- Press **Save**

In API permissions, click on **Add a permission**
- Select Microsoft Graph
- Select **Application Permissions**
- Find and select the following permissions
  - Calendars.Read
  - Calanders.ReadWrite
  - User.Read.All
- Make sure to press the **Grant admin consent for <tenantName>**

In the **Certificates and secrets** tab add a new Secret
- Give it a Description and an Expiration and press Add
- Copy the value somewhere safe 

> Ideally you'll be using a certificate instead of secret for authenticating the client. Certificates are highly recommended :)

## Configure you console app
- Open the `appsettings.json` file
- Add your specific information in the placeholders
- Save and run your app
  
