# Education Center Management System

A full-featured desktop application built with C# and .NET (WinForms), designed to help educational institutions manage their daily operations — including students, courses, instructors, enrollments, and payments — with a user-friendly interface and a structured backend architecture.

---

## 📌 Features

### 🎓 Student Management
- Add, update, delete, and search for student records
- View student profiles and enrollment history

### 📚 Course Management
- Create and manage courses with fees, descriptions, and schedules
- Assign instructors to courses

### 👨‍🏫 Instructor Management
- Maintain instructor profiles
- Link instructors with one or more courses

### 📝 Enrollment System
- Register students into courses
- Track enrollment status and dates

### 💰 Payment Management
- Record and monitor course payments
- View paid and unpaid balances per student

### 📊 Reports & Statistics
- Generate lists of enrolled students per course
- Track income from course payments

### 🖥️ UI & Data Views
- Clean WinForms interface using DataGridViews
- Search, filter, and validate input data efficiently

---

## 🛠️ Technologies Used

- **Language:** C# (.NET Framework)
- **UI:** Windows Forms (WinForms)
- **Database:** SQL Server
- **Architecture:** Layered (UI, DataAccess, Models)

---

## 📂 Project Structure

```
EducationCenter.sln
├── EducationCenter/             → WinForms application (UI layer)
│   └── Forms/                   → Forms for Students, Courses, Enrollments, etc.
├── EducationCenter.DataAccess/ → Database access logic and SQL methods
├── EducationCenter.Models/     → Entity classes (POCOs)
└── App.config                   → Connection string configuration
```

---

## ✅ Requirements

- Windows OS
- Visual Studio 2019 or later
- .NET Framework (compatible with project version)
- SQL Server (Express or full version)

---

## 🚀 Installation & Setup

1. **Clone the repository:**
   ```bash
   git clone https://github.com/Abdelrahmanmoussan/E-Center.git
   cd E-Center
   ```

2. **Open the solution:**
   - Open `EducationCenter.sln` in Visual Studio.

3. **Configure the database:**
   - Update the connection string in `App.config` to match your SQL Server instance.
   - Example:
     ```
     <connectionStrings>
       <add name="EducationDb" connectionString="Data Source=YOUR_SERVER;Initial Catalog=YOUR_DB;Integrated Security=True"/>
     </connectionStrings>
     ```

4. **Initialize the database:**
   - Create the required tables manually or run any included `.sql` setup script.
   - Ensure relationships are respected: Students → Enrollments → Courses → Instructors.

5. **Run the application:**
   - Set `EducationCenter` as the startup project.
   - Press F5 to build and run the application.

---

## 👨‍💻 Usage

- Navigate using the main menu to manage:
  - Students
  - Courses
  - Instructors
  - Enrollments
  - Payments

- Use forms to add, edit, delete, or search records.
- Use search boxes to filter data in real-time.
- Reports can be generated via the relevant views (DataGridViews).

---

## 📈 Future Improvements (Optional Ideas)

- Attendance tracking per student
- Course scheduling and calendar view
- Exporting reports to Excel/PDF
- Login system with user roles (Admin, Instructor)

---

## 🤝 Contributing

Contributions are welcome!  
To contribute:

1. Fork the repo  
2. Create a new branch (`feature/my-feature`)  
3. Commit your changes  
4. Push to your branch  
5. Open a pull request

---

## 📩 Contact

**Author:** Abdelrahman Moussan  
**GitHub:** [https://github.com/Abdelrahmanmoussan/E-Center](https://github.com/Abdelrahmanmoussan/E-Center)

---

## 📄 License

This project is currently not licensed. For reuse or distribution, please contact the author.
