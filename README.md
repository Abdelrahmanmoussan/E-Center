# Education Center Management System

A desktop application (C# / .NET) designed to help education centers manage entities like students, courses, enrollments, instructors, and payments efficiently.

---

## Features

- **Student Management**
  - Add, update, delete, search student records
  - Store personal details and enrollment history

- **Course Management**
  - Create, edit, and remove courses
  - Manage descriptions, schedules, and fees

- **Instructor Management**
  - Add and manage instructor profiles
  - Assign instructors to courses

- **Enrollment Management**
  - Enroll students into courses
  - Track enrollment dates and statuses

- **Payment Handling**
  - Record course payments per student
  - Monitor outstanding balances and payment history

- **Reporting**
  - View student rosters per course
  - Track financial reports (completed vs. pending payments)

- **User Interface**
  - Clean and intuitive Windows Forms (WinForms) interface
  - Data presentation using DataGridViews
  - Forms with input validation and search/filter functionality

- **Data Persistence**
  - Backend supported by SQL Server (or compatible database)
  - Clean architecture separating Data Access, Business Logic, and Presentation

---

## Project Structure

```
EducationCenter.sln
├── .gitattributes
├── .gitignore
├── EducationCenter.DataAccess/
│   └── DAOs, Repository classes, DB models
├── EducationCenter.Models/
│   └── Entity classes: Student, Course, Instructor, Enrollment, Payment
└── EducationCenter/ (WinForms)
    ├── Forms for managing each entity
    └── Program entry point
```

---

## Prerequisites

- Windows OS
- .NET Framework (version used in the project)
- SQL Server or compatible RDBMS
- Visual Studio (2019 or later) for development

---

## Installation & Setup

1. **Clone** the project:
   ```bash
   git clone https://github.com/Abdelrahmanmoussan/E-Center.git
   cd E-Center
   ```

2. **Open** the `.sln` solution in Visual Studio.

3. **Configure** the database connection:
   - Locate `App.config` or settings file in the DataAccess project.
   - Update the connection string with your server/database credentials.

4. **Initialize** the database:
   - Use SQL scripts provided (if any) or set up tables manually.
   - Ensure relationships between entities are respected.

5. **Build & Run**:
   - Build the solution.
   - Run the application using the `EducationCenter` (WinForms) project.

---

## Usage Guide

- Use the menu or tabs to access features.
- Add or edit data via respective forms.
- Use filters and searches for quick access.
- Check reports for financial summaries or enrollments.

---

## Architecture & Key Components

- **Separation of Concerns**: 
  - `DataAccess`: Database interaction (ADO.NET or raw SQL).
  - `Models`: POCO classes for Students, Courses, etc.
  - `EducationCenter`: WinForms UI.

- **Validation & Error Handling**:
  - Input validation on UI fields.
  - Try-catch blocks for DB transactions.

- **Scalability**:
  - Can be extended with modules like attendance tracking, teacher payroll, or online student portal.

---

## Contributing

Contributions are welcome!

1. Fork the repository.
2. Create a new branch (`feature/your-feature-name`).
3. Commit and push changes.
4. Open a Pull Request with a description.

---

## License

Currently unlicensed. For reuse, modifications, or distribution, please consult the author.

---

## Contact

**Author:** Abdelrahman Moussan  
**GitHub:** [https://github.com/Abdelrahmanmoussan/E-Center](https://github.com/Abdelrahmanmoussan/E-Center)
