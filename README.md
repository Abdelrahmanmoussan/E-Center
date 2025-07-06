# Education Center Management System

A desktop application built with C# and .NET, designed to help education centers manage students, courses, instructors, enrollments, and payments efficiently.

---

## Features

- **Student Management**  
  Add, update, delete, and search student records. Maintain student profiles and track enrollment history.

- **Course Management**  
  Create and manage courses, including schedules, descriptions, and fees.

- **Instructor Management**  
  Add instructor profiles and assign them to specific courses.

- **Enrollment Management**  
  Enroll students in courses, manage enrollment status, and view course participation.

- **Payment Tracking**  
  Record course payments, monitor payment history, and view outstanding balances.

- **Reports**  
  Display students per course and generate simple financial reports.

- **User Interface**  
  Built with Windows Forms (WinForms) using clean and intuitive DataGridViews. Forms include input validation and search features.

- **Database Integration**  
  SQL Server backend. Structured architecture separating data access, models, and UI layers.

---

## Requirements

- Windows OS  
- Visual Studio 2019 or newer  
- SQL Server  
- Appropriate .NET Framework (as used in the project)

---

## Installation & Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/Abdelrahmanmoussan/E-Center.git
   cd E-Center
   ```

2. Open the solution file `EducationCenter.sln` in Visual Studio.

3. Update the database connection string in `App.config` to match your local SQL Server instance.

4. Run any provided SQL scripts (if available) to create necessary tables.

5. Build and run the solution.

---

## Project Structure

```
EducationCenter.sln
├── EducationCenter.DataAccess/
│   └── Database operations and repository classes
├── EducationCenter.Models/
│   └── Entity classes: Student, Course, Instructor, Enrollment, Payment
└── EducationCenter/ (WinForms UI)
    ├── Forms for each management module
    └── Program entry point
```

---

## Usage

- Use the main menu to navigate between Students, Courses, Instructors, Enrollments, and Payments.
- Add or modify records through dedicated forms.
- View reports for enrolled students and financial summaries.

---

## Contribution

Feel free to contribute:

1. Fork the repository  
2. Create a feature branch (`feature/your-feature`)  
3. Commit your changes  
4. Push to your fork  
5. Submit a Pull Request

---

## Contact

**Author**: Abdelrahman Moussan  
**GitHub**: [https://github.com/Abdelrahmanmoussan/E-Center](https://github.com/Abdelrahmanmoussan/E-Center)
