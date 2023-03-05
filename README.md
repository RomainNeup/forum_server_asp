# ForumAPI

## Description
ForumAPI is a RESTful API built with ASP.NET Core that allows users to create, read, update, and delete categories, sections, subjects, and messages.

## Installation
1. Clone the repository: git clone https://github.com/your-username/ForumAPI.git
2. Navigate to the project directory: `cd ForumAPI`**
3. Restore the dependencies: dotnet `restore`**
4. Build the project: `dotnet build`**
5. Run the project: `dotnet run`**

## Forum API Routes

### Auth Routes

**🔓 POST `/auth/register`**

*Registers a new user account.*

📄 Payload: 
```json
{
    "username": "string",
    "email": "string",
    "password": "string"
}
```
**🔓 POST `/auth/login`**

*Logs in a user and returns an authentication token.*

📄 Payload: 
```json
{
    "email": "string",
    "password": "string"
}
```

### Category Routes

**🔐 POST `/categories`**

*Creates a new category.*

📄 Payload: 
```json
{
    "name": "string",
    "description": "string"
}
```
**🔓 GET `/categories`**

*Returns a list of all categories.*

**🔓 GET `/categories/{id}`**

*Returns the details of a single category.*

**🔐 PUT `/categories/{id}`**

*Updates an existing category.*

📄 Payload: 
```json
{
    "name": "string",
    "description": "string"
}
```
**🔐 DELETE `/categories/{id}`**

*Deletes an existing category.*

### Section Routes

**🔐 POST `/sections`**

*Creates a new section in the specified category.*

📄 Payload: 
```json
{
    "name": "string",
    "description": "string",
    "categoryId": "integer"
}
```
**🔓 GET `/sections`**

*Returns a list of all sections.*

**🔓 GET `/sections/{id}`**

*Returns the details of a single section.*

**🔐 PUT `/sections/{id}`**

*Updates an existing section.*

📄 Payload: 
```json
{
    "name": "string",
    "description": "string"
}
```
**🔐 DELETE `/sections/{id}`**

*Deletes an existing section.*

### Subject Routes

**🔐 POST `/sections/{sectionId}/subjects`**

*Creates a new subject in the specified section.*

📄 Payload: 
```json
{
    "name": "string",
    "text": "string"
}
```
**🔓 GET `/sections/{sectionId}/subjects`**

*Returns a list of all subjects in the specified section.*

**🔓 GET `/sections/{sectionId}/subjects/{id}`**

*Returns the details of a single subject.*

**🔐 PUT `/sections/{sectionId}/subjects/{id}`**

*Updates an existing subject.*

📄 Payload: 
```json
{
    "name": "string",
    "text": "string"
}
```
**🔐 DELETE `/sections/{sectionId}/subjects/{id}`**

*Deletes an existing subject.*

### Message Routes

**🔐 POST `/subjects/{subjectId}/messages`**

*Creates a new message in the specified subject.*

📄 Payload: 
```json
{
    "text": "string"
}
```
**🔓 GET `/subjects/{subjectId}/messages`**

*Returns a list of all messages in the specified subject.*

**🔓 GET `/subjects/{subjectId}/messages/{id}`**

*Returns the details of a single message.*

**🔐 PUT `/subjects/{subjectId}/messages/{id}`**

*Updates an existing message.*

📄 Payload: 
```json
{
    "text": "string"
}
```
**🔐 DELETE `/subjects/{subjectId}/messages/{id}`**

*Deletes an existing message.*

---
Note: 🔐 represents a protected route, which requires a JWT token to be sent in the request header as Bearer token, while 🔓 represents an unprotected route.