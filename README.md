# Customer-Management

![App Logo](./Customer_Management.Web/wwwroot/assets/images/customers-logo.png)

## Description
With this application you can manage Customers and Companies. Basically, each customer belongs to a company. Keeping that in mind, you can have a good manage for your entries and so on.

## Features
- CRUD (Create - Read - Update - Delete) Actions for companies
- CRUD (Create - Read - Update - Delete) Actions for customers (but each client is linked to a company)
- An entire collection where all the customers of a company are listed

## Technologies used
- **.NET Core 6:** It is the LTS version at the moment for .NET Core.
- **SQL Server:** A Database Server to store the data.
- **Entity Framework Core:** An ORM to mapping and manage the entities of the Database.
- **NToastNotify:** A Package that allow the developers show a Toast Notification when necessary.

## Techniques and good practice implemented
- POO Principles
- SOLID Principles
- Design Patterns
- Code First
- Logging
- Toast Notifications

## Project Structure
Basically this is a Web Project that connects to a Local Database using SQL Server. The project was designed with a **MVC Architecture** but implementing the **DAO** and **DTO** patterns.

_These are the principal folders for the application functionality._
- **Customer_Management.Web**
  - **Classes:** Contains useful classes that can be used in the application, but these elements don't represent Database Entities.
    
  - **Contracts:** Contains the contracts (Interfaces) that specific classes has to implement.
  - **Controllers:** Contains the elements that handle the communication between the DAO objects and Views and request from the views.
  - **DAO:** Contains the classes that are defined to be used for the communication with the **Models**. In summary, the instances of these classes can access the data stored in the Database using the **DbContext**.
  - **DTO:** Contains the elements that the **Views** receive.
  - **Data:** Contains the **Application's DbContext**
  - **Helpers:** Defines static classes that Developers can use to handle some scenarios. For example: A class to show a **Toast Notification** when an action has done.
  - **Migrations:** A folder that keep the migrations created in the **Development Process**.
  - **Models:** Contains the entities that are defined in the Database.
  - **StaticValues:** Defines some **const values** within **static classes**. Basically, these are values that will be used in the entire application.
  - **Views:** Contains the different views and pages used for the entities (Companies and Customers)
  - **wwwroot:** It stores some folders and files for JS functionalites, CSS styles and images used in the project.

## How to execute the project locally?

  1. Create a Fork of the application to have the access from your repo.
  2. Go to the folder **Database** located in the Repo root then open the **DDL.sql** file and execute it in a RDBMS that support **SQL Server connections**. For example: **SQL Server Management Studio (SSMS)**
  3. After that, go to the **appsettings.json** file and modify the value of the Connection String named **DbCustomerManagement**.
  4. You can open the project using **Visual Studio** and run the app with it.
  5. Once the application is running you can go tp your preferred browser and open the following URL _http://localhost:5014_
