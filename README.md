# StudentManagementSystem
A web-based Student Management System using ASP.NET Core MVC with Identity. Features include CRUD operations for students, courses, teachers, enrollments, image upload, and role-based access. Built for the Software Engineering with AI course by ICT Bangladesh.
echo # 🎓 Student Management System - ASP.NET Core MVC

A full-featured Student Management System built using **ASP.NET Core MVC**, **Entity Framework Core**, and **ASP.NET Identity**. This project allows administrators, teachers, and students to manage data efficiently with role-based access control, custom user registration, and user-friendly dashboards.

---

## 📖 Project Description

This system was developed as a course project for the **Software Engineering with AI** program under ICT Bangladesh. It demonstrates core enterprise application features using modern web development practices including:

- MVC Architecture
- Database-first design using EF Core
- ASP.NET Identity-based authentication and authorization
- Dynamic UI rendering based on user roles
- File/image upload and profile management

---

## 🚀 Features

- 👨‍🏫 Admin panel for managing Students, Teachers, and Courses
- 🧑‍🎓 Student dashboard with GPA and course enrollment
- 👩‍🏫 Teacher panel for viewing assigned students and courses
- 🔐 Role-based registration and navigation
- 📂 Image upload for user profiles
- 📊 GPA calculation and grade management
- 🗂️ Organized views and responsive layout

---

## 🧠 Technologies Used

- ASP.NET Core MVC (.NET 6+)
- Entity Framework Core (EF Core)
- ASP.NET Core Identity
- SQL Server / LocalDB
- Razor Pages & Bootstrap
- LINQ, ViewModels
- Visual Studio 2022

---

## 🧩 How to Run the Project (Local Setup)

1. **Clone the repository:**

   git clone https://github.com/Rukaiya2009/StudentManagementSystem.git
   cd StudentManagementSystem

2. **Open in Visual Studio**

   - Double-click on the `.sln` file to open the solution.

3. **Configure database:**
   - Open `appsettings.json` and set your own SQL Server connection string.

4. **Apply migrations and create database:**

   Update-Database

   (From Package Manager Console)

5. **Seed default users and roles:**
   - Ensure the `Program.cs` or `DataSeeder.cs` seeds roles like **Admin**, **Student**, and **Teacher**

6. **Run the application** (`F5` or `Ctrl+F5`)

---

## 📁 Folder Structure (Key Parts)

Controllers/, Models/, Views/, Data/, Program.cs, wwwroot/

---

## 👨‍🏫 Instructors

- Israfeel Masum
- Mahinur Rahaman Hridoy
- Rakibul Islam

---

## 🏫 Batch Info

- Batch Name: SE 5/25
- Course Title: Software Engineering with AI
- Organization: ICT Bangladesh
- Group Name: ICTBD AI Soft Eng 25/5

---

## ✍️ Author

Rukaiya Binte Shafique  
GitHub: https://github.com/Rukaiya2009

---

## 📌 License

This project is open source for educational purposes under the MIT license. > README.md
