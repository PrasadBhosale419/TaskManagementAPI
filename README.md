Here’s the updated **Steps to Run the Project Locally**:

---

## **Steps to Run the Project Locally**

### **1. Clone the Repository**  
Clone the project to your local machine:  
```bash
git clone <repository-url>
cd <project-folder>
```

---

### **2. Install .NET SDK**  
Ensure you have the correct .NET version installed. (Replace `8.0` with your project’s version):  
```bash
dotnet --version
```

If the .NET SDK is not installed, download it from the [official .NET download page](https://dotnet.microsoft.com/download).

---

### **3. Restore Dependencies**  
Restore the NuGet packages required by the project:  
```bash
dotnet restore
```

---

### **4. Set Up Environment Variables**  
Update the `appsettings.json` file with the required configuration. Example:  
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=ServerName;Database=DataBaseName;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

---

### **5. Run Database Migrations (if applicable)**  
Use Entity Framework Core to apply migrations to the database:  
```bash
dotnet ef database update
```

If using the NuGet Package Manager Console, run:  
```bash
update-database
```

---

### **6. Run the Application**  
Start the project (ensure the startup file is **TaskManagement.Web**):  
```bash
dotnet run
```

---

### **7. Access the Application**  
Open your browser and navigate to:  
```
http://localhost:5000
```
- If the default port is occupied, check the command prompt output for the actual port being used.

To view Swagger UI, append `/swagger` to the URL:  
```
http://localhost:5000/swagger
```

---

# **API Documentation**

## **Base URL**  
```
http://<your-domain>/api/Task
```

---

### **1. Get All Tasks**  
Fetch all tasks from the database.

- **Endpoint**: `GET /api/Task`
- **Response**:  
  - **200 OK**: Returns a list of tasks.  
  - **404 Not Found**: No tasks found.  
  - **500 Internal Server Error**: Error retrieving tasks.  

---

### **2. Get All Tasks with Pagination**  
Fetch tasks with pagination support.  

- **Endpoint**: `GET /api/Task/Pagination?pageNumber={pageNumber}&pageSize={pageSize}`  
- **Query Parameters**:  
  - `pageNumber` (required): Page number.  
  - `pageSize` (required): Number of tasks per page.  
- **Response**:  
  - **200 OK**: Returns a paginated list of tasks, along with metadata:
    ```json
    {
      "TotalCount": 50,
      "TotalPages": 5,
      "CurrentPage": 1,
      "PageSize": 10,
      "Tasks": [list of tasks]
    }
    ```
  - **500 Internal Server Error**: Error retrieving paginated tasks.

---

### **3. Get Task by ID**  
Fetch a specific task by its ID.  

- **Endpoint**: `GET /api/Task/{id}`  
- **Path Parameter**:  
  - `id` (required): ID of the task.  
- **Response**:  
  - **200 OK**: Returns the task.  
  - **404 Not Found**: Task not found.  
  - **500 Internal Server Error**: Error retrieving the task.

---

### **4. Get Tasks by Status**  
Fetch tasks filtered by status.  

- **Endpoint**: `GET /api/Task/GetTasksByStatus/{status}`  
- **Path Parameter**:  
  - `status` (required): Status of the tasks (e.g., Pending, Completed).  
- **Response**:  
  - **200 OK**: Returns a list of tasks.  
  - **404 Not Found**: No tasks found with the specified status.  
  - **500 Internal Server Error**: Error retrieving tasks by status.

---

### **5. Add a New Task**  
Add a new task to the database.  

- **Endpoint**: `POST /api/Task/AddTask`  
- **Request Body**:  
  ```json
  {
    "Title": "Task Title",
    "Description": "Task Description",
    "DueDate": "2024-12-31"
  }
  ```
- **Response**:  
  - **201 Created**: Task added successfully.  
  - **400 Bad Request**: Validation error (e.g., missing required fields).  
  - **500 Internal Server Error**: Error adding the task.

---

### **6. Delete Task by ID**  
Delete a task by its ID.  

- **Endpoint**: `DELETE /api/Task/DeleteTask/{id}`  
- **Path Parameter**:  
  - `id` (required): ID of the task.  
- **Response**:  
  - **204 No Content**: Task deleted successfully.  
  - **404 Not Found**: Task not found.  
  - **500 Internal Server Error**: Error deleting the task.

---

### **7. Update a Task**  
Update an existing task by its ID.  

- **Endpoint**: `PUT /api/Task/UpdateTask/{id}`  
- **Path Parameter**:  
  - `id` (required): ID of the task.  
- **Request Body**:  
  ```json
  {
    "Title": "Updated Task Title",
    "Description": "Updated Task Description",
    "DueDate": "2024-12-31",
    "Status": 1
  }
  ```
- **Response**:  
  - **204 No Content**: Task updated successfully.  
  - **400 Bad Request**: Validation error (e.g., missing required fields).  
  - **404 Not Found**: Task not found.  
  - **500 Internal Server Error**: Error updating the task.

---

### **8. Export Tasks as CSV**  
Download a CSV file containing all tasks.  

- **Endpoint**: `GET /api/Task/ExportTasksAsCsv`  
- **Response**:  
  - **200 OK**: Returns the CSV file.  
  - **500 Internal Server Error**: Error exporting tasks.  

---

### **Bonus Features**

#### **1. Swagger/OpenAPI Documentation**  
Implemented Swagger/OpenAPI documentation using **Swashbuckle** to provide an interactive API interface for developers.  
- **Feature Details**: 
  - The `SwaggerGen` service is added to the DI container to generate the OpenAPI specification.
  - The Swagger UI allows testing API endpoints interactively in the browser.
  - Configuration:
    ```csharp
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
    });
    ```
  - **Access URL**: Once the application is running, Swagger UI is accessible at `http://localhost:<port>/swagger`.

---

#### **2. Dependency Injection (DI) for Services**  
Used **Dependency Injection** to decouple the application's layers and improve testability and maintainability.
- **Injected Services**:
  - **Repositories**:  
    `ITaskRepository` is injected for database interactions.  
  - **Services**:  
    `ITaskService` is injected to handle business logic.  
  - **Database Context**:  
    `ITaskDbContext` is injected for accessing the database using `TaskDbContext`.

  **Implementation**:  
  ```csharp
  builder.Services.AddScoped<ITaskDbContext, TaskDbContext>();
  builder.Services.AddScoped<ITaskRepository, TaskRepository>();
  builder.Services.AddScoped<ITaskService, TaskService>();
  ```

---

#### **3. Export Tasks as a CSV File**  
Added a feature to export all tasks into a **CSV file** for easier data portability.
- **Feature Details**:
  - A new endpoint (`GET /api/Task/ExportTasksAsCsv`) generates and downloads a CSV file containing all task details.
  - Implementation creates a CSV format using `StringBuilder` and streams the content as a downloadable file.
  - **Example Output**:
    ```
    Id,Title,Description,Status,DueDate
    1,Task 1,Description of Task 1,Pending,2024-12-10
    2,Task 2,Description of Task 2,Completed,2024-12-15
    ```

---

#### **4. Unit Testing with xUnit**  
Created **unit tests** for critical API endpoints using the **xUnit** framework to ensure correctness and reliability.
- **Below are some of the Tests Implemented**:
  - `GetAllTasks`: Validates fetching all tasks returns the expected list.
  - `GetTaskById`: Confirms that tasks are retrieved correctly by ID and handles cases where the task doesn't exist.
  - `AddTask`: Ensures tasks are correctly added to the database.
  - `DeleteTask`: Verifies tasks are deleted successfully and checks the behavior when the task doesn't exist.

  **Example Test Structure**:
  ```csharp
  [Fact]
  public async Task GetAllTasks_ReturnsTasks_WhenTasksExist()
  {
      // Arrange
      var mockService = new Mock<ITaskService>();
      mockService.Setup(service => service.GetAllAsync())
                 .ReturnsAsync(GetSampleTasks());
      var controller = new TaskController(mockService.Object);

      // Act
      var result = await controller.GetAllTasks();

      // Assert
      Assert.IsType<OkObjectResult>(result);
  }
  ```

---
