CREATE TABLE Admins (
    AdminID INT PRIMARY KEY IDENTITY(1,1),
    Password NVARCHAR(max) NOT NULL,  -- Store hashed password
    Email NVARCHAR(100) UNIQUE NOT NULL,
    FullName NVARCHAR(100) NOT NULL,
	adminImage NVARCHAR(max)
);

-- Create Users table for regular website users
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Password NVARCHAR(max) NOT NULL,  -- Store hashed password
    Email NVARCHAR(100) UNIQUE NOT NULL,
    FullName NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20),
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- Create Tours table (simplified)
CREATE TABLE Tours (
    TourID INT PRIMARY KEY IDENTITY(1,1),
    TourName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX),
    Price DECIMAL(10,2) NOT NULL,
    Duration NVARCHAR(50),  -- e.g., "2 hours", "Half day"
    IsActive BIT DEFAULT 1,
	TourImage NVARCHAR(max)

);

-- Create Bookings table (simplified)
CREATE TABLE Bookings (
    BookingID INT PRIMARY KEY IDENTITY(1,1),
    TourID INT FOREIGN KEY REFERENCES Tours(TourID),
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    BookingDate DATE NOT NULL,
    NumberOfPeople INT NOT NULL,
    TotalPrice DECIMAL(10,2) NOT NULL,
    Status NVARCHAR(20) DEFAULT 'Pending',  -- Pending, Confirmed, Cancelled
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- Create ContactMessages table
CREATE TABLE ContactMessages (
    MessageID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Subject NVARCHAR(200),
    Message NVARCHAR(MAX) NOT NULL,
    SubmittedAt DATETIME DEFAULT GETDATE(),
    IsRead BIT DEFAULT 0
);

-- Create Newsletter table
CREATE TABLE NewsletterSubscribers (
    SubscriberID INT PRIMARY KEY IDENTITY(1,1),
    Email NVARCHAR(100) UNIQUE NOT NULL,

    SubscribedAt DATETIME DEFAULT GETDATE()
);