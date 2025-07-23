🛠️ Project Description: MongoDB CRUD with Dynamic & Static Documents
📄 This documentation was written with the help of AI.

This ASP.NET Core Web API project demonstrates two approaches to working with MongoDB:

🔹 1. UsersController: Strongly-Typed CRUD (Static)
Handles well-defined documents using a predefined C# model (User) with properties like Id, Name, Email, and Age.

✅ Suitable for:

Predictable data structures

Compile-time validation

Clear contract between API and client

🔸 2. DynamicUsersController: Flexible Document Handling (Dynamic)
Allows fully dynamic JSON documents to be stored and queried using dynamic types. No need to define a C# model upfront.

✅ Suitable for:

Evolving data structures

Flexible schema scenarios

Prototyping and admin tools

⚙️ Features
Add, get, update, and delete both static and dynamic MongoDB documents

Smart query type detection (e.g., automatically treats "true" as bool)

Query by any field, including _id

Fully testable via Swagger
