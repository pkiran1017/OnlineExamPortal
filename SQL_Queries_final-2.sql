-- User Table
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    UserName VARCHAR(50),
    Password VARCHAR(50),
    Email VARCHAR(100),
    Mobile VARCHAR(20),
    UserRole VARCHAR(20)
);

-- Topic Table
CREATE TABLE Topic (
    TopicID INT IDENTITY(1,1) PRIMARY KEY,
    TopicName VARCHAR(50)
);

-- Questions Table
CREATE TABLE Questions (
    QuestionID INT IDENTITY(1,1) PRIMARY KEY,
    Type VARCHAR(20),
    Question VARCHAR(500),
    Option1 VARCHAR(100),
    Option2 VARCHAR(100),
    Option3 VARCHAR(100),
    Option4 VARCHAR(100),
    Option5 VARCHAR(100),
    CorrectOption VARCHAR(50),
    TopicID INT,
    DifficultyLevel VARCHAR(50),
    FOREIGN KEY (TopicID) REFERENCES Topic(TopicID)
);

-- Exam Table
CREATE TABLE Exam (
    ExamID INT IDENTITY(1,1) PRIMARY KEY,
    ExamStartDateTime DATETIME,
    ExamEndDateTime DATETIME,
    ExamDuration INT,
    NoOfAttempts INT,
    UserID INT,
    TopicID INT,
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (TopicID) REFERENCES Topic(TopicID)
);

-- Result Table
CREATE TABLE Result (
    ResultID INT IDENTITY(1,1) PRIMARY KEY,
    ExamID INT,
    QuestionID INT,
    SelectedOption VARCHAR(50),
    CorrectOption VARCHAR(50),
    Result INT,
    FOREIGN KEY (ExamID) REFERENCES Exam(ExamID),
    FOREIGN KEY (QuestionID) REFERENCES Questions(QuestionID)
);




-- Inserting data into the USER table
INSERT INTO Users (UserName, Password, Email, Mobile, UserRole)
VALUES 
    ('RahulG', 'password1', 'rahul@example.com', '+91 9876543210', 'Admin'),
    ('PriyaS', 'pword123', 'priya@emailprovider.in', '+91 8765432109', 'User'),
    ('RohanK', 'secret456', 'rohan_k@exampledomain.com', '+91 7654321098', 'User'),
    ('MeenaP', 'mypass789', 'meena_p@samplemail.com', '+91 6543210987', 'User'),
    ('SanjayM', 'passSan56', 'sanjaym@webmailprovider.in', '+91 5432109876', 'Admin');

-- Inserting data into the TOPIC table
INSERT INTO Topic (TopicName)
VALUES 
    ('.NET'),
    ('HTML'),
    ('CSS'),
    ('JavaScript'),
    ('SQL');

-- Insert into Questions Table
INSERT INTO Questions (Type, Question, Option1, Option2, Option3, Option4, Option5, CorrectOption, TopicID, DifficultyLevel)
VALUES
('Multiple', 'Which type of integrity preserves the defined relationship between tables when records are entered or deleted?', 'Entity integrity', 'Domain integrity', 'Referential integrity', 'User-defined integrity', NULL, 'C', 5, 'Easy'),
('Multiple', 'The command to remove rows from a table CUSTOMER is:', 'REMOVE FROM CUSTOMER ...', 'DROP FROM CUSTOMER ...', 'DELETE FROM CUSTOMER WHERE ...', 'UPDATE FROM CUSTOMER ...', NULL, 'C', 5, 'Medium'),
('Multiple', 'Which of the following is the original purpose of SQL?', 'To specify the syntax and semantics of SQL data definition language', 'To specify the syntax and semantics of SQL manipulation language', 'To define the data structures', 'All of the above', NULL, 'D', 5, 'Easy'),
('Multiple', 'Default is a type of constraint although it does not really enforce anything', 'Yes', 'No', NULL, NULL, NULL, 'A', 5, 'Easy'),
('Multiple', 'A relation scheme is said to be in ______________ form if the values in the domain of each attribute of the relation are atomic?', 'Unnormalized', 'First Normal', 'BoyceCODD', 'None of these', NULL, 'B', 5, 'Hard'),
('Multiple', 'What are the different events in Triggers?', 'Define, Create', 'Drop, Comment', 'Insert, Update, Delete', 'Select, Commit', NULL, 'C', 5, 'Easy'),
('Multiple', 'Which of the following SQL command can be used to modify existing data in a database table?', 'MODIFY', 'UPDATE', 'CHANGE', 'NEW', NULL, 'B', 5, 'Hard'),
('Multiple', 'Identify the relationship between a Movie table and Stars table:', 'One to one', 'One to many', 'Many to many', 'None of above', NULL, 'C', 5, 'Easy'),
('Multiple', 'The SQL keyword(s) ________ is used with wildcards.', 'LIKE only', 'IN only', 'NOT IN only', 'IN and NOT IN', NULL, 'A', 5, 'Medium'),
('Multiple', 'The JOIN which does the Cartesian Product is called?', 'Left Join', 'Left Outer Join', 'Right Outer Join', 'Cross Join', NULL, 'D', 5, 'Easy');

-- Insert into Exam Table
INSERT INTO Exam (ExamStartDateTime, ExamEndDateTime, ExamDuration, NoOfAttempts, UserID, TopicID)
VALUES
('2023-12-01 09:00:00', '2023-12-01 10:30:00', 90, 1, 1, 5),
('2023-12-02 10:30:00', '2023-12-03 12:30:00', 120, 1, 2, 5),
('2023-12-04 11:00:00', '2023-12-05 12:15:00', 75, 1, 3, 5),
('2023-12-03 13:45:00', '2023-12-03 14:45:00', 60, 1, 4, 5),
('2023-12-05 14:20:00', '2023-12-05 16:05:00', 105, 1, 5, 5);


    -- Inserting data into the RESULT table
INSERT INTO RESULT (ExamID, QuestionID, SelectedOption, CorrectOption, Result)
VALUES 
    (1, 1, 'Option1', 'Option1', 1),
    (1, 2, 'Option1', 'Option1', 1),
    (1, 3, 'Option1', 'Option2', 0),
    (1, 4, 'Option1', 'Option1', 1),
    (1, 5, 'Option1', 'Option1', 1),
    (1, 6, 'Option1', 'Option1', 1),
    (1, 7, 'Option1', 'Option2', 0),
    (1, 8, 'Option1', 'Option1', 1),
    (1, 9, 'Option1', 'Option1', 1),
    (1, 10, 'Option1', 'Option4', 0);

SELECT * FROM Users
SELECT * FROM Topic
SELECT * FROM Questions
SELECT * FROM Exam
SELECT * FROM RESULT