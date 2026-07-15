# Persons API

The Persons API is a service that provides contact information and basic personal details of key individuals responsible for various organisations associated with the Department for Education.

## Technologies

.NET, JSON, OpenAPI, Web API

## Authenticating for Swagger

The Swagger UI requires an authentication token to access the API. You can obtain a token via Postman. Import the Postman collection and environment from the `Postman` folder.
You will need to ask a colleague to provide you with the `Secret` and `ClientId` values.
You should now be able to generate a new access token. You can either call the endpoints in Postman or use the token in the Swagger UI.

## Running the API locally

You can run a local instance of the Academies database, or alternatively, you can connect to the Dev or Test instance of the database.

Replace the "DefaultConnection" string in `appsettings.Development.json` with the appropriate connection string for your environment, e.g.

`"Server=srv-t1dv-sips.database.windows.net;Database=sip;User Id=<id_here>;Password=<password_here>"`

Build and run the application as you usually would.

Create a token using Postman (see above) and paste it into the Swagger UI, or call your locally hosted endpoints in Postman.