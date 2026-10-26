
# Gift of the Givers Foundation - Prototype Web Application

An enterprise-style web prototype developed in C# using **ASP.NET Core MVC** and connected to SQL Server, designed to support disaster relief coordination, volunteer management, and transparent donation processing. 

This project was built as part of an advanced full-stack software development module to demonstrate clean architecture, robust backend logic, and modern cloud deployment pipelines using **Azure DevOps**.

---

## Key Features
* **Disaster Relief Tracking:** Manage ongoing relief efforts, allocate resources, and log real-time updates.
* **Donation & Tax Certificate Workflow:** Secure processing of financial and physical donations with automated tax certificate generation.
* **Volunteer & Employee Portals:** Role-based dashboards for tracking volunteers, staff shifts, and operational task assignments.
* **Azure Serverless Functions:** Integrated background worker functions for automated task logging and storage synchronization.

---

## Technology Stack & Architecture
* **Backend:** C#, ASP.NET Core MVC, .NET 8, Entity Framework Core
* **Database:** Microsoft SQL Server (Relational Database Design)
* **Cloud & DevOps:** 
  * **Azure App Services:** Cloud hosting and deployment
  * **Azure DevOps:** Automated CI/CD YAML pipelines for continuous build and release
  * **Azure Blob Storage:** Secure media and document management
* **Tooling & Helpers:** Custom NuGet package helper library integration, Git feature-branch workflows

---

## Project Structure
```text
GiftOfTheGivers/
│
├── Data/                     # Entity Framework database context and configurations
├── Models/                   # Domain entities (Donations, Volunteers, Relief Updates, Users)
├── Pages/                    # MVC Views, Controllers, and Account Management pages
├── Services/                 # Business logic and prototype store services
├── GiftOfTheGivers.Functions/# Azure Functions for background logging and tax calculations
└── Program.cs                # Application entry point and service configurations
```

---

## Setup & Local Installation

1. **Clone the Repository:**
   ```bash
   git clone https://github.com/ethan1ol/giftofthegivers.git
   ```
2. **Open the Solution:**
   Open `GiftOfTheGivers.sln` using Visual Studio or Visual Studio Code.
3. **Configure the Database:**
   Update your connection string in `appsettings.json` to point to your local SQL Server instance:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=GiftOfTheGiversDb;Trusted_Connection=True;MultipleActiveResultSets=true"
   }
   ```
4. **Run Entity Framework Migrations:**
   Open the Package Manager Console in Visual Studio and run:
   ```bash
   dotnet ef database update
   ```
5. **Run the Application:**
   Press `F5` in Visual Studio or run `dotnet run` in your terminal.

---

## CI/CD Pipeline & Deployment
The project utilizes **Azure DevOps YAML pipelines** configured for:
* Automatic code compilation and dependency restoration.
* Automated unit testing and build verification on every pull request.
* Seamless continuous deployment directly to **Azure App Services**.
