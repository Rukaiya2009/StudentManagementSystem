INSERT INTO Departments (DepartmentName, Description)
VALUES 
('Computer Science', 'Focuses on programming, systems, and AI.'),
('Mathematics', 'Focuses on pure and applied math.'),
('Physics', 'Covers mechanics, optics, and thermodynamics.'),
('History', 'Teaches world and national historical events.');

INSERT INTO Teachers (Name, Email, DepartmentName)
VALUES
('Dr. Mahinur Rahaman', 'mahinur.rahman@university.edu', 'Mathematics'),
('Prof. Israfeel Masum', 'israfeel.masum@university.edu', 'Computer Science'),
('Dr. Rakibul Islam', 'rakibul.islam@university.edu', 'Physics'),
('Ms. Nusrat Jahan', 'nusrat.jahan@university.edu', 'History');

INSERT INTO Courses (CourseName, Credit, DepartmentId, TeacherId)
VALUES
('Programming Fundamentals', 3, 1, 2),
('Discrete Mathematics', 3, 2, 1),
('Quantum Mechanics', 4, 3, 3),
('Bangladesh History', 2, 4, 4),
('Data Structures', 3, 1, 2);

INSERT INTO Students (FullName, DateOfBirth, Gender, Email, Phone, Address, ProfilePicture)
VALUES 
('Ayesha Siddiqua', '2003-08-15', 'Female', 'ayesha.siddiqua@student.edu', '01712345678', 'Dhaka, Bangladesh', NULL),
('Tanvir Hasan', '2002-11-30', 'Male', 'tanvir.hasan@student.edu', '01898765432', 'Chattogram, Bangladesh', NULL),
('Farzana Akter', '2004-01-20', 'Female', 'farzana.akter@student.edu', '01911223344', 'Rajshahi, Bangladesh', NULL);

INSERT INTO Enrollments (StudentId, CourseId, Grade)
VALUES
(1, 1, 'A'),
(1, 2, 'B+'),
(2, 3, 'A-'),
(2, 1, 'B'),
(3, 4, 'A'),
(3, 5, 'A+');

SELECT * FROM Departments;
SELECT * FROM Teachers;
SELECT * FROM Courses;
SELECT * FROM Students;
SELECT * FROM Enrollments;
