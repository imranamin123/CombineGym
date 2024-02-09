USE [master]
GO
/****** Object:  Database [CombineGym]    Script Date: 8/5/2021 12:26:19 PM ******/
CREATE DATABASE [CombineGym]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CombineGym', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\CombineGym.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'CombineGym_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\CombineGym_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [CombineGym] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CombineGym].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CombineGym] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CombineGym] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CombineGym] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CombineGym] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CombineGym] SET ARITHABORT OFF 
GO
ALTER DATABASE [CombineGym] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CombineGym] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CombineGym] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CombineGym] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CombineGym] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CombineGym] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CombineGym] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CombineGym] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CombineGym] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CombineGym] SET  DISABLE_BROKER 
GO
ALTER DATABASE [CombineGym] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CombineGym] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CombineGym] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CombineGym] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CombineGym] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CombineGym] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CombineGym] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CombineGym] SET RECOVERY FULL 
GO
ALTER DATABASE [CombineGym] SET  MULTI_USER 
GO
ALTER DATABASE [CombineGym] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CombineGym] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CombineGym] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CombineGym] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [CombineGym] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'CombineGym', N'ON'
GO
ALTER DATABASE [CombineGym] SET QUERY_STORE = OFF
GO
USE [CombineGym]
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [CombineGym]
GO
/****** Object:  UserDefinedFunction [dbo].[fnGetFeeReceiptIsPending]    Script Date: 8/5/2021 12:26:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--fnGetFeeReceiptIsPending 1

CREATE function [dbo].[fnGetFeeReceiptIsPending]
(
	@branchId as int, 
	@studentId as int , 
	@checkDate as date 
)

RETURNS BIT
AS

BEGIN

declare 
	@checkYear as int=0, 
	@checkMonth as int=0,
	@isPending as bit = 0

	select @checkMonth = MONTH(@checkDate)
	select @checkYear = YEAR(@checkDate)

  IF EXISTS(select * from FeeReceipt where BranchId = @branchId and StudentId = @studentId and FeeYear = @checkYear and FeeMonth = @checkMonth)
  BEGIN
	set @isPending = 0
  END
  ELSE
  BEGIN
	declare @feeDueDate date
	declare @feeDueDateFoCurrentMonth date
	declare @feeCheckDateForCurrentMonth date
	
	select @feeDueDate = FeeDueDate from Student where BranchId = @branchId and StudentId = @studentId
	SET @feeDueDateFoCurrentMonth = cast(year(getdate()) as varchar) + '-' + cast(MONTH(getdate()) as varchar) + '-' + cast(day(@feeDueDate) as varchar)
	if @checkDate >= @feeDueDateFoCurrentMonth
	BEGIN
		set @isPending = 1
	END
	ELSE
	BEGIN
		set @isPending = 0
	END
  END
  RETURN @isPending 
END



GO
/****** Object:  UserDefinedFunction [dbo].[fnGetStPrevBal]    Script Date: 8/5/2021 12:26:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[fnGetStPrevBal] 
(
	@branchId as int, 
	@studentId as int, 
	@feeMonth as int, 
	@feeYear as int
)
RETURNS decimal(18,2)
AS
BEGIN


declare @PrevMonth int
declare @PrevYear int
declare @prevBalance decimal(18,2)

if @feeMonth = 1 
BEGIN
	SET @PrevMonth = 12
	SET @PrevYear = @feeYear - 1
END
ELSE
BEGIN
	SET @PrevMonth = @feeMonth - 1
	SET @PrevYear = @feeYear 

END
	select @prevBalance = ISNULL(SUM(BalanceAmount),0)  
	from FeeReceipt 
	where 
		BranchId = @branchId and 
		StudentId = @studentId and 
		FeeMonth = @PrevMonth and 
		FeeYear = @PrevYear

	-- Return the result of the function
	RETURN @prevBalance

END


GO
/****** Object:  Table [dbo].[BloodGroup]    Script Date: 8/5/2021 12:26:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BloodGroup](
	[BloodGroupId] [smallint] NOT NULL,
	[BloodGroupName] [varchar](50) NULL,
 CONSTRAINT [PK_BloodGroup] PRIMARY KEY CLUSTERED 
(
	[BloodGroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Branch]    Script Date: 8/5/2021 12:26:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Branch](
	[BranchId] [int] IDENTITY(1,1) NOT NULL,
	[BranchName] [varchar](100) NULL,
	[Phone1] [varchar](50) NULL,
	[Phone2] [varchar](50) NULL,
	[Address] [varchar](200) NULL,
 CONSTRAINT [PK_Branch] PRIMARY KEY CLUSTERED 
(
	[BranchId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EmpAttendance]    Script Date: 8/5/2021 12:26:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmpAttendance](
	[EmployeeAttendanceId] [bigint] IDENTITY(1,1) NOT NULL,
	[EmployeeId] [int] NULL,
	[AttendanceDate] [datetime] NULL,
	[TimeIn] [datetime] NULL,
	[TimeOut] [datetime] NULL,
	[BranchId] [int] NULL,
 CONSTRAINT [PK_EmpAttendance] PRIMARY KEY CLUSTERED 
(
	[EmployeeAttendanceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Employee]    Script Date: 8/5/2021 12:26:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employee](
	[EmployeeId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[GenderId] [smallint] NULL,
	[FatherName] [varchar](100) NULL,
	[DateOfJoining] [date] NULL,
	[DateOfLeaving] [date] NULL,
	[Salary] [decimal](18, 2) NULL,
	[Phone1] [varchar](50) NULL,
	[Phone2] [varchar](50) NULL,
	[Address] [varchar](200) NULL,
	[Remarks] [varchar](200) NULL,
	[StatusId] [smallint] NULL,
	[BranchId] [int] NULL,
	[GymShiftId] [int] NULL,
	[Picture] [varchar](100) NULL,
 CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED 
(
	[EmployeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EmployeeAttendance]    Script Date: 8/5/2021 12:26:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmployeeAttendance](
	[EmployeeAttendanceId] [bigint] IDENTITY(1,1) NOT NULL,
	[EmployeeId] [int] NULL,
	[AttendanceDate] [datetime] NULL,
	[TimeIn] [datetime] NULL,
	[TimeOut] [datetime] NULL,
	[BranchId] [int] NULL,
 CONSTRAINT [PK_EmployeeAttendance] PRIMARY KEY CLUSTERED 
(
	[EmployeeAttendanceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FeeMonth]    Script Date: 8/5/2021 12:26:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FeeMonth](
	[FeeMonthId] [smallint] NOT NULL,
	[FeeMonth] [varchar](50) NULL,
 CONSTRAINT [PK_FeeMonth] PRIMARY KEY CLUSTERED 
(
	[FeeMonthId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FeeReceipt]    Script Date: 8/5/2021 12:26:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FeeReceipt](
	[FeeReceiptId] [int] IDENTITY(1,1) NOT NULL,
	[ReferenceNo] [varchar](50) NULL,
	[StudentId] [int] NULL,
	[ReceiptDate] [datetime] NULL,
	[FeeYear] [smallint] NULL,
	[FeeMonth] [smallint] NULL,
	[PreviousBalance] [decimal](18, 2) NULL,
	[ReceiptAmount] [decimal](18, 2) NULL,
	[BalanceAmount] [decimal](18, 2) NULL,
	[Remarks] [varchar](100) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[BranchId] [int] NULL,
 CONSTRAINT [PK_FeeReceipt] PRIMARY KEY CLUSTERED 
(
	[FeeReceiptId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FeeYear]    Script Date: 8/5/2021 12:26:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FeeYear](
	[FeeYearId] [smallint] NOT NULL,
	[FeeYear] [smallint] NULL,
 CONSTRAINT [PK_FeeYear] PRIMARY KEY CLUSTERED 
(
	[FeeYearId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Gender]    Script Date: 8/5/2021 12:26:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Gender](
	[GenderId] [smallint] NOT NULL,
	[Description] [varchar](50) NULL,
 CONSTRAINT [PK_Gender] PRIMARY KEY CLUSTERED 
(
	[GenderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GymShift]    Script Date: 8/5/2021 12:26:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GymShift](
	[GymShiftId] [smallint] NOT NULL,
	[Description] [varchar](50) NULL,
	[Timing] [varchar](50) NULL,
 CONSTRAINT [PK_Shift] PRIMARY KEY CLUSTERED 
(
	[GymShiftId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MNU_PARENT]    Script Date: 8/5/2021 12:26:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MNU_PARENT](
	[MAINMNU] [varchar](20) NULL,
	[STATUS] [varchar](1) NULL,
	[MENUPARVAL] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MNU_SUBMENU]    Script Date: 8/5/2021 12:26:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MNU_SUBMENU](
	[MENUPARVAL] [int] NULL,
	[FRM_CODE] [varchar](50) NULL,
	[FRM_NAME] [varchar](20) NULL,
	[STATUS] [varchar](1) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[secMenu]    Script Date: 8/5/2021 12:26:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[secMenu](
	[MenuId] [int] NOT NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_secMenu] PRIMARY KEY CLUSTERED 
(
	[MenuId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[secPage]    Script Date: 8/5/2021 12:26:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[secPage](
	[PageId] [int] IDENTITY(1,1) NOT NULL,
	[MenuId] [int] NULL,
	[PageNo] [int] NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_secPage] PRIMARY KEY CLUSTERED 
(
	[PageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[secUser]    Script Date: 8/5/2021 12:26:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[secUser](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[username] [varchar](50) NULL,
	[password] [nvarchar](50) NULL,
	[EmployeeId] [int] NULL,
	[StatusId] [smallint] NULL,
	[BranchId] [int] NULL,
	[Remarks] [varchar](200) NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_secUser] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[secUserRights]    Script Date: 8/5/2021 12:26:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[secUserRights](
	[UserRightsId] [int] IDENTITY(1,1) NOT NULL,
	[BranchId] [int] NULL,
	[UserId] [int] NULL,
	[MenuId] [int] NULL,
	[PageNo] [int] NULL,
	[IsView] [bit] NULL,
	[IsAdd] [bit] NULL,
	[IsEdit] [bit] NULL,
	[IsDelete] [bit] NULL,
 CONSTRAINT [PK_secUserRights] PRIMARY KEY CLUSTERED 
(
	[UserRightsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Status]    Script Date: 8/5/2021 12:26:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Status](
	[StatusId] [smallint] NOT NULL,
	[Description] [varchar](50) NULL,
 CONSTRAINT [PK_Status] PRIMARY KEY CLUSTERED 
(
	[StatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Student]    Script Date: 8/5/2021 12:26:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student](
	[StudentId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](200) NULL,
	[FaherName] [varchar](200) NULL,
	[GenderId] [smallint] NULL,
	[AdmissionDate] [date] NULL,
	[FeeDueDate] [date] NULL,
	[FeeAmount] [decimal](18, 2) NULL,
	[Phone1] [varchar](50) NULL,
	[Phone2] [varchar](50) NULL,
	[Address] [varchar](200) NULL,
	[BloodGroupId] [smallint] NULL,
	[Picture] [varchar](100) NULL,
	[Remarks] [varchar](500) NULL,
	[StatusId] [smallint] NULL,
	[GymShiftId] [smallint] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[BranchId] [int] NULL,
 CONSTRAINT [PK_Student] PRIMARY KEY CLUSTERED 
(
	[StudentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[StudentAttendance]    Script Date: 8/5/2021 12:26:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudentAttendance](
	[StudentAttendanceId] [bigint] IDENTITY(1,1) NOT NULL,
	[StudentId] [int] NULL,
	[AttendanceDate] [datetime] NULL,
	[TimeIn] [datetime] NULL,
	[TimeOut] [datetime] NULL,
	[TimeSpend] [int] NULL,
	[BranchId] [int] NULL,
 CONSTRAINT [PK_StudentAttendance] PRIMARY KEY CLUSTERED 
(
	[StudentAttendanceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[temp]    Script Date: 8/5/2021 12:26:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[temp](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
 CONSTRAINT [PK_temp] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
INSERT [dbo].[BloodGroup] ([BloodGroupId], [BloodGroupName]) VALUES (1, N'O+')
INSERT [dbo].[BloodGroup] ([BloodGroupId], [BloodGroupName]) VALUES (2, N'O-')
INSERT [dbo].[BloodGroup] ([BloodGroupId], [BloodGroupName]) VALUES (3, N'A+')
INSERT [dbo].[BloodGroup] ([BloodGroupId], [BloodGroupName]) VALUES (4, N'A-')
INSERT [dbo].[BloodGroup] ([BloodGroupId], [BloodGroupName]) VALUES (5, N'B+')
INSERT [dbo].[BloodGroup] ([BloodGroupId], [BloodGroupName]) VALUES (6, N'B-')
INSERT [dbo].[BloodGroup] ([BloodGroupId], [BloodGroupName]) VALUES (7, N'AB+')
INSERT [dbo].[BloodGroup] ([BloodGroupId], [BloodGroupName]) VALUES (8, N'AB-')
SET IDENTITY_INSERT [dbo].[Branch] ON 

INSERT [dbo].[Branch] ([BranchId], [BranchName], [Phone1], [Phone2], [Address]) VALUES (1, N'Imtiaz Fitness', N' 0300 9465197', N'', N'Swiss Centre, Wahdat Rd, Old Muslim Town, Mulsim Town, Lahore, Punjab, 54700')
SET IDENTITY_INSERT [dbo].[Branch] OFF
SET IDENTITY_INSERT [dbo].[EmpAttendance] ON 

INSERT [dbo].[EmpAttendance] ([EmployeeAttendanceId], [EmployeeId], [AttendanceDate], [TimeIn], [TimeOut], [BranchId]) VALUES (1, 2, CAST(N'2021-04-01T13:20:46.710' AS DateTime), CAST(N'2021-04-01T13:20:46.710' AS DateTime), NULL, 1)
SET IDENTITY_INSERT [dbo].[EmpAttendance] OFF
SET IDENTITY_INSERT [dbo].[Employee] ON 

INSERT [dbo].[Employee] ([EmployeeId], [Name], [GenderId], [FatherName], [DateOfJoining], [DateOfLeaving], [Salary], [Phone1], [Phone2], [Address], [Remarks], [StatusId], [BranchId], [GymShiftId], [Picture]) VALUES (1, N'Asif', 2, N'Sajjad Sb', CAST(N'2021-02-23' AS Date), CAST(N'2021-12-23' AS Date), NULL, N'Sajjad Sb', N'Sajjad Sb', N'Lahore', N'', 3, 1, 1, N'EMP_210932397.png')
INSERT [dbo].[Employee] ([EmployeeId], [Name], [GenderId], [FatherName], [DateOfJoining], [DateOfLeaving], [Salary], [Phone1], [Phone2], [Address], [Remarks], [StatusId], [BranchId], [GymShiftId], [Picture]) VALUES (2, N'Moeez', 1, N'Hassam', CAST(N'2021-02-23' AS Date), CAST(N'2021-02-23' AS Date), NULL, N'Hassam', N'Hassam', N'Islam Pura, Lahore', N'', 1, 1, 1, N'EMP_213341010.png')
INSERT [dbo].[Employee] ([EmployeeId], [Name], [GenderId], [FatherName], [DateOfJoining], [DateOfLeaving], [Salary], [Phone1], [Phone2], [Address], [Remarks], [StatusId], [BranchId], [GymShiftId], [Picture]) VALUES (3, N'Kamran Naseem', 2, N'Mr. Naseem', CAST(N'2021-02-01' AS Date), CAST(N'2021-02-25' AS Date), NULL, N'Mr. Naseem', N'0300-7896541', N'Rachna Town, Lahore 555', N'', 1, 1, 1, N'EMP_213112609.png')
INSERT [dbo].[Employee] ([EmployeeId], [Name], [GenderId], [FatherName], [DateOfJoining], [DateOfLeaving], [Salary], [Phone1], [Phone2], [Address], [Remarks], [StatusId], [BranchId], [GymShiftId], [Picture]) VALUES (4, N'Imran Amin', 1, N'Muhammad Amin', CAST(N'2021-03-06' AS Date), CAST(N'2021-03-06' AS Date), NULL, N'Muhammad Amin', N'Muhammad Amin', N'Allama Iqbal Town, Lahore', N'', 1, 1, 1, N'EMP_210322120.png')
SET IDENTITY_INSERT [dbo].[Employee] OFF
INSERT [dbo].[FeeMonth] ([FeeMonthId], [FeeMonth]) VALUES (1, N'Jan')
INSERT [dbo].[FeeMonth] ([FeeMonthId], [FeeMonth]) VALUES (2, N'Feb')
INSERT [dbo].[FeeMonth] ([FeeMonthId], [FeeMonth]) VALUES (3, N'Mar')
INSERT [dbo].[FeeMonth] ([FeeMonthId], [FeeMonth]) VALUES (4, N'Apr')
INSERT [dbo].[FeeMonth] ([FeeMonthId], [FeeMonth]) VALUES (5, N'May')
INSERT [dbo].[FeeMonth] ([FeeMonthId], [FeeMonth]) VALUES (6, N'Jun')
INSERT [dbo].[FeeMonth] ([FeeMonthId], [FeeMonth]) VALUES (7, N'Jul')
INSERT [dbo].[FeeMonth] ([FeeMonthId], [FeeMonth]) VALUES (8, N'Aug')
INSERT [dbo].[FeeMonth] ([FeeMonthId], [FeeMonth]) VALUES (9, N'Sep')
INSERT [dbo].[FeeMonth] ([FeeMonthId], [FeeMonth]) VALUES (10, N'Oct')
INSERT [dbo].[FeeMonth] ([FeeMonthId], [FeeMonth]) VALUES (11, N'Nov')
INSERT [dbo].[FeeMonth] ([FeeMonthId], [FeeMonth]) VALUES (12, N'Dec')
INSERT [dbo].[FeeYear] ([FeeYearId], [FeeYear]) VALUES (2020, 2020)
INSERT [dbo].[FeeYear] ([FeeYearId], [FeeYear]) VALUES (2021, 2021)
INSERT [dbo].[Gender] ([GenderId], [Description]) VALUES (1, N'Male')
INSERT [dbo].[Gender] ([GenderId], [Description]) VALUES (2, N'Female')
INSERT [dbo].[Gender] ([GenderId], [Description]) VALUES (3, N'Other')
INSERT [dbo].[GymShift] ([GymShiftId], [Description], [Timing]) VALUES (1, N'1 st', N'6 AM - 9:30 AM')
INSERT [dbo].[GymShift] ([GymShiftId], [Description], [Timing]) VALUES (2, N'2 nd', N'10 AM - 4 PM')
INSERT [dbo].[GymShift] ([GymShiftId], [Description], [Timing]) VALUES (3, N'3 rd', N'5 PM - 12 AM')
INSERT [dbo].[MNU_PARENT] ([MAINMNU], [STATUS], [MENUPARVAL]) VALUES (N'Transactions', N'Y', 1)
INSERT [dbo].[MNU_PARENT] ([MAINMNU], [STATUS], [MENUPARVAL]) VALUES (N'Inventory', N'Y', 2)
INSERT [dbo].[MNU_SUBMENU] ([MENUPARVAL], [FRM_CODE], [FRM_NAME], [STATUS]) VALUES (1, N'FrmAccount', N'Accounting', N'Y')
INSERT [dbo].[MNU_SUBMENU] ([MENUPARVAL], [FRM_CODE], [FRM_NAME], [STATUS]) VALUES (1, N'FrmFinance', N'Finance', N'Y')
INSERT [dbo].[MNU_SUBMENU] ([MENUPARVAL], [FRM_CODE], [FRM_NAME], [STATUS]) VALUES (1, NULL, N'Despatch', N'Y')
INSERT [dbo].[secMenu] ([MenuId], [Name]) VALUES (1, N'File')
INSERT [dbo].[secMenu] ([MenuId], [Name]) VALUES (2, N'Setup')
INSERT [dbo].[secMenu] ([MenuId], [Name]) VALUES (3, N'Master Data')
INSERT [dbo].[secMenu] ([MenuId], [Name]) VALUES (4, N'Attendance')
INSERT [dbo].[secMenu] ([MenuId], [Name]) VALUES (5, N'Accounts')
INSERT [dbo].[secMenu] ([MenuId], [Name]) VALUES (6, N'Reports')
SET IDENTITY_INSERT [dbo].[secPage] ON 

INSERT [dbo].[secPage] ([PageId], [MenuId], [PageNo], [Name]) VALUES (1, 1, 1, N'Branch')
INSERT [dbo].[secPage] ([PageId], [MenuId], [PageNo], [Name]) VALUES (2, 1, 2, N'Student')
INSERT [dbo].[secPage] ([PageId], [MenuId], [PageNo], [Name]) VALUES (3, 1, 3, N'Employee')
INSERT [dbo].[secPage] ([PageId], [MenuId], [PageNo], [Name]) VALUES (4, 2, 4, N'StudentAttendance')
INSERT [dbo].[secPage] ([PageId], [MenuId], [PageNo], [Name]) VALUES (5, 2, 5, N'EmployeeAttendance')
INSERT [dbo].[secPage] ([PageId], [MenuId], [PageNo], [Name]) VALUES (6, 3, 6, N'FeeReceipt')
SET IDENTITY_INSERT [dbo].[secPage] OFF
SET IDENTITY_INSERT [dbo].[secUser] ON 

INSERT [dbo].[secUser] ([UserId], [username], [password], [EmployeeId], [StatusId], [BranchId], [Remarks], [CreatedDate], [ModifiedDate]) VALUES (1, N'a', N'a', 1, 1, 1, N'', NULL, CAST(N'2021-04-01T21:07:44.283' AS DateTime))
INSERT [dbo].[secUser] ([UserId], [username], [password], [EmployeeId], [StatusId], [BranchId], [Remarks], [CreatedDate], [ModifiedDate]) VALUES (2, N'imran', N'123', 4, 2, 1, N'', CAST(N'2021-04-03T16:56:54.743' AS DateTime), CAST(N'2021-04-03T17:00:22.313' AS DateTime))
SET IDENTITY_INSERT [dbo].[secUser] OFF
SET IDENTITY_INSERT [dbo].[secUserRights] ON 

INSERT [dbo].[secUserRights] ([UserRightsId], [BranchId], [UserId], [MenuId], [PageNo], [IsView], [IsAdd], [IsEdit], [IsDelete]) VALUES (1, 1, 1, 1, 1, 1, 1, 1, 1)
INSERT [dbo].[secUserRights] ([UserRightsId], [BranchId], [UserId], [MenuId], [PageNo], [IsView], [IsAdd], [IsEdit], [IsDelete]) VALUES (2, 1, 1, 1, 2, 1, 1, 1, 1)
INSERT [dbo].[secUserRights] ([UserRightsId], [BranchId], [UserId], [MenuId], [PageNo], [IsView], [IsAdd], [IsEdit], [IsDelete]) VALUES (3, 1, 1, 1, 3, 1, 1, 1, 1)
SET IDENTITY_INSERT [dbo].[secUserRights] OFF
INSERT [dbo].[Status] ([StatusId], [Description]) VALUES (1, N'Active')
INSERT [dbo].[Status] ([StatusId], [Description]) VALUES (2, N'InActive')
INSERT [dbo].[Status] ([StatusId], [Description]) VALUES (3, N'Pending')
SET IDENTITY_INSERT [dbo].[Student] ON 

INSERT [dbo].[Student] ([StudentId], [Name], [FaherName], [GenderId], [AdmissionDate], [FeeDueDate], [FeeAmount], [Phone1], [Phone2], [Address], [BloodGroupId], [Picture], [Remarks], [StatusId], [GymShiftId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [BranchId]) VALUES (1, N'Muhammad Imran Amin', N'Muhammad Amin', 1, CAST(N'2021-03-04' AS Date), CAST(N'2021-04-01' AS Date), CAST(1000.00 AS Decimal(18, 2)), N'042-356987', N'042-356987', N'Ichra, Lahore', 1, N'STD_212729411.png', N'Software Engineer', 1, 3, NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[Student] ([StudentId], [Name], [FaherName], [GenderId], [AdmissionDate], [FeeDueDate], [FeeAmount], [Phone1], [Phone2], [Address], [BloodGroupId], [Picture], [Remarks], [StatusId], [GymShiftId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [BranchId]) VALUES (2, N'Ali Ahmad', N'Muhammad Amin', 1, CAST(N'2021-01-10' AS Date), CAST(N'2021-02-10' AS Date), CAST(2000.00 AS Decimal(18, 2)), N'0321-456321', N'', N'Ichra, Lahore sss', 2, N'STD_212752105.png', N'Software Engineer', 2, 1, NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[Student] ([StudentId], [Name], [FaherName], [GenderId], [AdmissionDate], [FeeDueDate], [FeeAmount], [Phone1], [Phone2], [Address], [BloodGroupId], [Picture], [Remarks], [StatusId], [GymShiftId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [BranchId]) VALUES (3, N'Zainab', N'Haider', 2, CAST(N'2021-04-05' AS Date), CAST(N'2021-05-05' AS Date), CAST(500.00 AS Decimal(18, 2)), N'0300-456987', N'0300-456987', N'Model Town, Lahore', 7, N'STD_212825850.png', N'', 2, 2, NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[Student] ([StudentId], [Name], [FaherName], [GenderId], [AdmissionDate], [FeeDueDate], [FeeAmount], [Phone1], [Phone2], [Address], [BloodGroupId], [Picture], [Remarks], [StatusId], [GymShiftId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [BranchId]) VALUES (4, N'Usman Mughal', N'Hammad Mughal', 1, CAST(N'2021-01-01' AS Date), CAST(N'2021-02-01' AS Date), CAST(2000.00 AS Decimal(18, 2)), N'042-3569874', N'042-3569874', N'Mughal Pura, Lahore', 1, N'STD_211631120.png', N'Software Engineer', 1, 1, NULL, NULL, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[Student] OFF
/****** Object:  StoredProcedure [dbo].[spEmployeeAttendanceList]    Script Date: 8/5/2021 12:26:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [dbo].[spEmployeeAttendanceList]
	@BranchId int,
	@FromDate date ,
	@ToDate date
AS

BEGIN

select 
	e.EmployeeId, 
	e.Name, 
	CAST(a.AttendanceDate as date) AttendanceDate, 
	a.TimeIn, 
	a.TimeOut, 
	DATEDIFF(MINUTE, a.TimeIn,a.TimeOut) TimeSpend 
from Employee e left outer join EmployeeAttendance a on e.BranchId = a.BranchId and e.EmployeeId = a.EmployeeId
where 
	(cast(a.AttendanceDate as date) >= @FromDate and  cast(a.AttendanceDate as date) <= @ToDate ) and a.BranchId = @BranchId

	order by a.TimeIn desc

END

GO
/****** Object:  StoredProcedure [dbo].[spEmployeeList]    Script Date: 8/5/2021 12:26:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[spEmployeeList]
(
	@EmployeeId int=null,
	@Name varchar(100)=null,
	@BranchId int
)
AS
BEGIN
	SELECT e.EmployeeId
		,e.Name
		,e.GenderId
		,g.Description Gender
		,e.FatherName
		,e.DateOfJoining
		,e.DateOfLeaving
		,e.Salary
		,e.Phone1
		,e.Phone2
		,e.Address
		,e.Remarks
		,e.StatusId
		,s.Description 
		,e.BranchId
		,br.BranchName 
		,sh.gymShiftId
		,sh.Description ShiftDescription
		,sh.Timing ShiftTiming
		,Picture
	FROM dbo.Employee e
		left join Gender g on e.GenderId = g.GenderId
		left join Status s on e.StatusId = s.StatusId
		left join Branch br on e.GenderId = br.BranchId
		left join GymShift sh on e.gymShiftId = sh.gymShiftId
		
	WHERE 
		(e.EmployeeId = @EmployeeId OR @EmployeeId = 0 OR @EmployeeId is null) and
		(e.Name like '%' + @Name + '%' OR @Name = '' OR @Name is null) and
		(e.BranchId = @BranchId OR @BranchId = 0 OR @BranchId = null )

END



GO
/****** Object:  StoredProcedure [dbo].[spFeeReceiptHistory]    Script Date: 8/5/2021 12:26:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--spFeeReceiptHistory 1, 2

CREATE procedure [dbo].[spFeeReceiptHistory]
	@BranchId int = null,
	@StudentId int = null
AS

SELECT 
	fr.FeeReceiptId,
	fr.BranchId, 
	fr.StudentId, 
	fr.FeeYear, 
	fr.FeeMonth, 
	fr.ReceiptDate,
	dbo.fnGetStPrevBal (fr.BranchId, fr.StudentId, fr.FeeMonth, fr.FeeYear) PreviousBalance,
	fr.ReceiptAmount, 
	fr.BalanceAmount ,
	dbo.fnGetFeeReceiptIsPending (fr.BranchId,fr.StudentId,GETDATE()) IsPending
FROM FeeReceipt fr

WHERE 
	(BranchId = @BranchId OR @BranchId IS NULL OR @BranchId = 0) AND
	(StudentId = @StudentId OR @StudentId IS NULL OR @StudentId = 0)
ORDER BY BranchId, StudentId, FeeYear desc, FeeMonth desc


GO
/****** Object:  StoredProcedure [dbo].[spFeeReceiptHistorySearch]    Script Date: 8/5/2021 12:26:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--select * from FeeReceipt
--go
--spFeeReceiptHistorySearch 1, null, '2020-01-01', '2021-06-01'
--go

CREATE procedure [dbo].[spFeeReceiptHistorySearch]

	@BranchId int = null,
	@StudentId int = null,
	@FromDate date,
	@ToDate date
AS

SELECT 
	f.FeeReceiptId,
	f.BranchId, 
	f.StudentId, 
	s.Name,
	f.FeeYear, 
	f.FeeMonth, 
	f.ReceiptDate
FROM FeeReceipt f INNER JOIN Student s on f.StudentId = s.StudentId

WHERE 
	(f.BranchId = @BranchId OR @BranchId IS NULL OR @BranchId = 0) AND
	(f.StudentId = @StudentId OR @StudentId IS NULL OR @StudentId = 0) AND
	(CONVERT(Date, f.ReceiptDate) >= @FromDate AND  CONVERT(Date, f.ReceiptDate) <= @ToDate)
ORDER BY f.FeeReceiptId desc


GO
/****** Object:  StoredProcedure [dbo].[spGetStdentPrevBal]    Script Date: 8/5/2021 12:26:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[spGetStdentPrevBal]
	@branchId as int, 
	@studentId as int, 
	@feeMonth as int, 
	@feeYear as int
AS

BEGIN
declare @PrevBal decimal (18,2)
select @PrevBal  = dbo.fnGetStPrevBal (@branchId , @studentId , @feeMonth , @feeYear)
select ISNULL(@PrevBal,0);
END


GO
/****** Object:  StoredProcedure [dbo].[spStudentAttendanceListByDate]    Script Date: 8/5/2021 12:26:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE procedure [dbo].[spStudentAttendanceListByDate]
	@BranchId int,
	@FromDate date ,
	@ToDate date
AS

BEGIN

select 
	s.StudentId, 
	s.Name, 
	CAST(sa.AttendanceDate as date) AttendanceDate, 
	sa.TimeIn, 
	sa.TimeOut, 
	DATEDIFF(MINUTE, sa.TimeIn,sa.TimeOut) TimeSpend ,
	s.FeeDueDate,
	dbo.fnGetFeeReceiptIsPending (sa.BranchId,sa.StudentId,sa.AttendanceDate) IsPending
from Student s inner join StudentAttendance sa on s.BranchId = sa.BranchId and s.StudentId = sa.StudentId
where 
	(cast(sa.AttendanceDate as date) >= @FromDate and  cast(sa.AttendanceDate as date) <= @ToDate ) and sa.BranchId = @BranchId

	order by sa.TimeIn desc

END

GO
/****** Object:  StoredProcedure [dbo].[spStudentList]    Script Date: 8/5/2021 12:26:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[spStudentList]



 @branchId int=null,
 @studentId int=null,
 @name varchar(200)=null,
 @phone varchar(50)=null,
 @status smallint=null,
 @gender smallint=null,
 @shift smallint = null

AS 

SELECT [StudentId]
      ,[Name]
      ,[FaherName]
      ,s.[GenderId]
	  ,g.Description 'Gender'
      ,[AdmissionDate]
      ,[FeeDueDate]
      ,[FeeAmount]
      ,s.[Phone1]
      ,s.[Phone2]
      ,s.[Address]
      ,s.[BloodGroupId] 
	  ,bg.[BloodGroupId] 'BloodGroup'
      ,[Picture]
      ,[Remarks]
      ,s.[StatusId]
	  ,st.Description 'Status'
      ,s.[GymShiftId]
	  ,gs.Description 'Shift'
	  ,gs.Timing 'ShiftTimings'
      ,[CreatedBy]
      ,[CreatedDate]
      ,[ModifiedBy]
      ,[ModifiedDate]
      ,s.[BranchId]
	  ,b.BranchName
  FROM [dbo].[Student] s
	  left outer join Gender g on s.GenderId = g.GenderId
	  left outer join Status st on s.StatusId = st.StatusId
	  left outer join GymShift gs on s.GymShiftId = gs.GymShiftId
	  left outer join Branch b on s.BranchId = b.BranchId
	  left outer join BloodGroup bg on s.BloodGroupId = bg.BloodGroupId
  WHERE
	(s.BranchId = @branchId OR @branchId = 0 OR @branchId is null) AND
	(s.studentId = @studentId OR @studentId = 0 OR @studentId is null) AND
	(s.Name like '%' + @name + '%' OR @name = '' OR @name is null) AND
	(s.Phone1 like '%' + @phone + '%' OR @phone = '' OR @phone is null ) AND
	(s.StatusId = @status OR @status = 0 OR @status is null) AND
	(s.GenderId = @gender OR @gender = 0 OR @gender is null) AND
	(s.GymShiftId = @shift OR @shift = 0 OR @shift is null)
	




GO
/****** Object:  StoredProcedure [dbo].[spUserList]    Script Date: 8/5/2021 12:26:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[spUserList]
(
	@UserId int=null,
	@Name varchar(100)=null,
	@BranchId int
)
AS
BEGIN
	SELECT u.UserId
		,e.Name
		,u.username
		,u.BranchId
		,br.BranchName 
		,s.StatusId
		,s.Description Status

	FROM dbo.secUser u
		left join Status s on u.StatusId = s.StatusId
		left join Branch br on u.BranchId = br.BranchId 
		left join Employee e on u.EmployeeId = e.EmployeeId and u.BranchId = e.BranchId
	WHERE 
		(u.UserId = @UserId OR @UserId = 0 OR @UserId is null) and
		(e.Name like '%' + @Name + '%' OR @Name = '' OR @Name is null) and
		(u.BranchId = @BranchId OR @BranchId = 0 OR @BranchId = null )

END



GO
USE [master]
GO
ALTER DATABASE [CombineGym] SET  READ_WRITE 
GO
