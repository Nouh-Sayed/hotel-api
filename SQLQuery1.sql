CREATE DATABASE HotelReservationDb;
GO

USE HotelReservationDb;
GO

CREATE TABLE Hotels (
    HotelId INT IDENTITY(1,1) PRIMARY KEY,
    ExternalId INT NOT NULL UNIQUE,
    Name NVARCHAR(200) NOT NULL,
    City NVARCHAR(100) NOT NULL,
    Country NVARCHAR(100) NOT NULL,
    CountryCode NVARCHAR(10) NULL,
    Address NVARCHAR(500) NULL,
    Rating INT NOT NULL DEFAULT 0,
    Lat FLOAT NULL,
    Lng FLOAT NULL,
    PricePerNightTry DECIMAL(18,2) NOT NULL DEFAULT 0,
    Currency NVARCHAR(10) NOT NULL DEFAULT 'TRY',
    PriceType NVARCHAR(100) NULL
);
GO

CREATE TABLE Amenities (
    AmenityId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL UNIQUE
);
GO

CREATE TABLE HotelAmenities (
    HotelId INT NOT NULL,
    AmenityId INT NOT NULL,
    PRIMARY KEY (HotelId, AmenityId),
    FOREIGN KEY (HotelId) REFERENCES Hotels(HotelId) ON DELETE CASCADE,
    FOREIGN KEY (AmenityId) REFERENCES Amenities(AmenityId) ON DELETE CASCADE
);
GO

CREATE TABLE HotelImages (
    HotelImageId INT IDENTITY(1,1) PRIMARY KEY,
    HotelId INT NOT NULL,
    ImageUrl NVARCHAR(1000) NOT NULL,
    FOREIGN KEY (HotelId) REFERENCES Hotels(HotelId) ON DELETE CASCADE
);
GO

CREATE TABLE PhotoSources (
    PhotoSourceId INT IDENTITY(1,1) PRIMARY KEY,
    HotelId INT NOT NULL UNIQUE,
    GoogleImagesSearch NVARCHAR(1000) NULL,
    GoogleMapsSearch NVARCHAR(1000) NULL,
    FOREIGN KEY (HotelId) REFERENCES Hotels(HotelId) ON DELETE CASCADE
);
GO

CREATE TABLE Rooms (
    RoomId INT IDENTITY(1,1) PRIMARY KEY,
    HotelId INT NOT NULL,
    RoomNumber NVARCHAR(50) NOT NULL,
    RoomType NVARCHAR(100) NOT NULL,
    Capacity INT NOT NULL,
    PricePerNight DECIMAL(18,2) NOT NULL,
    IsAvailable BIT NOT NULL DEFAULT 1,
    FOREIGN KEY (HotelId) REFERENCES Hotels(HotelId) ON DELETE CASCADE
);
GO

CREATE UNIQUE INDEX IX_Rooms_HotelId_RoomNumber
ON Rooms(HotelId, RoomNumber);
GO

CREATE TABLE Customers (
    CustomerId INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(150) NOT NULL,
    Phone NVARCHAR(30) NOT NULL,
    Email NVARCHAR(150) NOT NULL,
    Nationality NVARCHAR(100) NULL,
    IdentityNumber NVARCHAR(100) NULL
);
GO

CREATE TABLE Bookings (
    BookingId INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT NOT NULL,
    HotelId INT NOT NULL,
    RoomId INT NOT NULL,
    CheckInDate DATETIME NOT NULL,
    CheckOutDate DATETIME NOT NULL,
    Adults INT NOT NULL,
    Children INT NOT NULL DEFAULT 0,
    TotalPrice DECIMAL(18,2) NOT NULL,
    Status NVARCHAR(50) NOT NULL DEFAULT 'Confirmed',
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId),
    FOREIGN KEY (HotelId) REFERENCES Hotels(HotelId),
    FOREIGN KEY (RoomId) REFERENCES Rooms(RoomId)
);
GO

CREATE TABLE Payments (
    PaymentId INT IDENTITY(1,1) PRIMARY KEY,
    BookingId INT NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    PaymentMethod NVARCHAR(50) NOT NULL DEFAULT 'Cash',
    PaymentStatus NVARCHAR(50) NOT NULL DEFAULT 'Pending',
    PaymentDate DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (BookingId) REFERENCES Bookings(BookingId) ON DELETE CASCADE
);
GO

INSERT INTO Hotels
(ExternalId, Name, City, Country, CountryCode, Address, Rating, Lat, Lng, PricePerNightTry, Currency, PriceType)
VALUES
(793202, N'Büyük Abant Hotel', N'Abant', N'Turkey', N'TR', N'Abant Gol Kenari 14030 BoluAbant', 5, 40.612098, 31.280425, 20800, N'TRY', N'random_realistic'),
(793209, N'Sheraton Grand Adana', N'Adana', N'Turkey', N'TR', N'Haci Sabanci Boulevard No. 7 01220 Adana', 5, 36.995144, 35.341053, 17350, N'TRY', N'random_realistic'),
(793243, N'Birlik Sahin Hotel', N'Agri', N'Turkey', N'TR', N'Kagizman Caddesi 32 04000 Agri', 3, 39.72137, 43.05096, 5450, N'TRY', N'random_realistic');
GO

INSERT INTO PhotoSources (HotelId, GoogleImagesSearch, GoogleMapsSearch)
VALUES
(
    (SELECT HotelId FROM Hotels WHERE ExternalId = 793202),
    N'https://www.google.com/search?tbm=isch&q=B%C3%BCy%C3%BCk+Abant+Hotel+Abant+Turkey',
    N'https://www.google.com/maps/search/?api=1&query=B%C3%BCy%C3%BCk+Abant+Hotel+Abant+Turkey'
),
(
    (SELECT HotelId FROM Hotels WHERE ExternalId = 793209),
    N'https://www.google.com/search?tbm=isch&q=Sheraton+Grand+Adana+Adana+Turkey',
    N'https://www.google.com/maps/search/?api=1&query=Sheraton+Grand+Adana+Adana+Turkey'
),
(
    (SELECT HotelId FROM Hotels WHERE ExternalId = 793243),
    N'https://www.google.com/search?tbm=isch&q=Birlik+Sahin+Hotel+Agri+Turkey',
    N'https://www.google.com/maps/search/?api=1&query=Birlik+Sahin+Hotel+Agri+Turkey'
);
GO

INSERT INTO Amenities (Name)
VALUES
(N'bar'),
(N'free_wifi'),
(N'front_desk_24h'),
(N'garden'),
(N'gym'),
(N'laundry'),
(N'meeting_rooms'),
(N'pool'),
(N'spa'),
(N'wheelchair_access'),
(N'parking'),
(N'restaurant');
GO

INSERT INTO HotelAmenities (HotelId, AmenityId)
SELECT h.HotelId, a.AmenityId
FROM Hotels h
CROSS JOIN Amenities a
WHERE h.ExternalId = 793202
AND a.Name IN (
    N'bar', N'free_wifi', N'front_desk_24h', N'garden', N'gym',
    N'laundry', N'meeting_rooms', N'pool', N'spa', N'wheelchair_access'
);
GO

INSERT INTO HotelAmenities (HotelId, AmenityId)
SELECT h.HotelId, a.AmenityId
FROM Hotels h
CROSS JOIN Amenities a
WHERE h.ExternalId = 793209
AND a.Name IN (
    N'bar', N'free_wifi', N'front_desk_24h', N'garden', N'gym',
    N'laundry', N'meeting_rooms', N'parking', N'pool', N'restaurant',
    N'spa', N'wheelchair_access'
);
GO

INSERT INTO HotelAmenities (HotelId, AmenityId)
SELECT h.HotelId, a.AmenityId
FROM Hotels h
CROSS JOIN Amenities a
WHERE h.ExternalId = 793243
AND a.Name IN (
    N'free_wifi', N'front_desk_24h'
);
GO

INSERT INTO Rooms (HotelId, RoomNumber, RoomType, Capacity, PricePerNight, IsAvailable)
VALUES
((SELECT HotelId FROM Hotels WHERE ExternalId = 793202), N'101', N'Single', 1, 20800, 1),
((SELECT HotelId FROM Hotels WHERE ExternalId = 793202), N'102', N'Double', 2, 22000, 1),
((SELECT HotelId FROM Hotels WHERE ExternalId = 793209), N'201', N'Deluxe', 2, 17350, 1),
((SELECT HotelId FROM Hotels WHERE ExternalId = 793209), N'202', N'Suite', 4, 25000, 1),
((SELECT HotelId FROM Hotels WHERE ExternalId = 793243), N'301', N'Standard', 2, 5450, 1);
GO

INSERT INTO Customers (FullName, Phone, Email, Nationality, IdentityNumber)
VALUES
(N'Nouh Sayed', N'05555555555', N'nouh@example.com', N'Egyptian', N'12345678901');
GO

INSERT INTO Bookings
(CustomerId, HotelId, RoomId, CheckInDate, CheckOutDate, Adults, Children, TotalPrice, Status)
VALUES
(
    1,
    (SELECT HotelId FROM Hotels WHERE ExternalId = 793202),
    1,
    '2026-05-01 14:00:00',
    '2026-05-05 12:00:00',
    2,
    1,
    83200,
    N'Confirmed'
);
GO

INSERT INTO Payments (BookingId, Amount, PaymentMethod, PaymentStatus)
VALUES
(1, 83200, N'Card', N'Paid');
GO

CREATE VIEW View_HotelDetails AS
SELECT
    h.HotelId,
    h.ExternalId,
    h.Name,
    h.City,
    h.Country,
    h.Address,
    h.Rating,
    h.PricePerNightTry,
    h.Currency,
    ps.GoogleImagesSearch,
    ps.GoogleMapsSearch
FROM Hotels h
LEFT JOIN PhotoSources ps ON h.HotelId = ps.HotelId;
GO

CREATE PROCEDURE AddBooking
    @CustomerId INT,
    @HotelId INT,
    @RoomId INT,
    @CheckInDate DATETIME,
    @CheckOutDate DATETIME,
    @Adults INT,
    @Children INT
AS
BEGIN
    SET NOCOUNT ON;

    IF @CheckOutDate <= @CheckInDate
    BEGIN
        RAISERROR(N'Check-out must be after check-in.', 16, 1);
        RETURN;
    END

    IF EXISTS (
        SELECT 1
        FROM Bookings
        WHERE RoomId = @RoomId
          AND Status <> N'Cancelled'
          AND @CheckInDate < CheckOutDate
          AND @CheckOutDate > CheckInDate
    )
    BEGIN
        RAISERROR(N'Room is already booked for this period.', 16, 1);
        RETURN;
    END

    DECLARE @PricePerNight DECIMAL(18,2);
    DECLARE @Nights INT;
    DECLARE @TotalPrice DECIMAL(18,2);

    SELECT @PricePerNight = PricePerNight
    FROM Rooms
    WHERE RoomId = @RoomId AND HotelId = @HotelId;

    IF @PricePerNight IS NULL
    BEGIN
        RAISERROR(N'Room not found in this hotel.', 16, 1);
        RETURN;
    END

    SET @Nights = DATEDIFF(DAY, @CheckInDate, @CheckOutDate);
    SET @TotalPrice = @Nights * @PricePerNight;

    INSERT INTO Bookings
    (CustomerId, HotelId, RoomId, CheckInDate, CheckOutDate, Adults, Children, TotalPrice, Status)
    VALUES
    (@CustomerId, @HotelId, @RoomId, @CheckInDate, @CheckOutDate, @Adults, @Children, @TotalPrice, N'Confirmed');
END;
GO