# TaskAPI
TaskAPI in clean architecture

This is a simple REST API that uses clean architecture with JWT authentication.

## Used Technologies
1. ASP.NET (DOT NET 6)
2. PostgreSql
3. Dapper

## Create Database Tables

  1. Products Table - product_id, product_name, product_description, product_price
  2. Users Table - id, username, name, age, role, password
 
 Change the connection string in "appsettings.json" according to the database.
 
 ## Endpoints
  1. ### Users
  
     * (GET) {url}/api/users - meant for getting all the users in the DB
     * (GET) {url}/api/users/{username} - meant for getting the user by username
     * (POST) {url}/api/users/register - meant for adding a user to the DB
     * (PUT) {url}/api/users - updating user by a user model passed in the body
     * (DELETE) {url}/api/users/{userid} - delete user by user id
  
  2. ### Auth
     
     * (POST) {url}/api/auth/login - login user by passing credintiols in the body
     * (GET) {url}/api/auth/refresh - refresh the token by passing refresh token. (passed in the header)
     * (GET) {url}/api/auth/loggeduser - get the logged user name and role.
  
  3. ### Product
     
     * (GET) {url}/api/products - get all the products
     * (GET) {url}/api/products/search - search products by name (pass the name as a query parameter)
     * (GET) {url}/api/products/{id} - get the product by product id
     * (POST) {url}/api/products - add a product to the DB
     * (PUT) {url}/api/products/{product id} - update the products
     * (DELETE) {url}/api/products/{product id} - delete products
  
 ## Models
   1. User model
        
        {
          "id": 0,
          "username": "string",
          "name": "string",
          "age": 0,
          "role": "string",
          "password": "string"
        }
        
    2. Products
        
      {
        "product_id": 0,
        "product_name": "string",
        "product_description": "string",
        "product_price": 0
      }
      
    3. Login
        
        {
          "username": "string",
          "password": "string"
        }
### NOTE : All Ids are set to auto incremented
     
