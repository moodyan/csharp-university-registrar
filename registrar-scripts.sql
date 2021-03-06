USE [master]
GO
/****** Object:  Database [registrar]    Script Date: 6/14/17 11:35:57 AM ******/
CREATE DATABASE [registrar]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'registrar', FILENAME = N'C:\Users\alyssamoody\registrar.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'registrar_log', FILENAME = N'C:\Users\alyssamoody\registrar_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [registrar] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [registrar].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [registrar] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [registrar] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [registrar] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [registrar] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [registrar] SET ARITHABORT OFF 
GO
ALTER DATABASE [registrar] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [registrar] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [registrar] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [registrar] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [registrar] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [registrar] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [registrar] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [registrar] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [registrar] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [registrar] SET  ENABLE_BROKER 
GO
ALTER DATABASE [registrar] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [registrar] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [registrar] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [registrar] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [registrar] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [registrar] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [registrar] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [registrar] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [registrar] SET  MULTI_USER 
GO
ALTER DATABASE [registrar] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [registrar] SET DB_CHAINING OFF 
GO
ALTER DATABASE [registrar] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [registrar] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [registrar] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [registrar] SET QUERY_STORE = OFF
GO
USE [registrar]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [registrar]
GO
/****** Object:  Table [dbo].[courses]    Script Date: 6/14/17 11:35:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[courses](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[course_name] [varchar](100) NULL,
	[course_number] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[departments]    Script Date: 6/14/17 11:35:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[departments](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[department_name] [varchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[students]    Script Date: 6/14/17 11:35:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[students](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[first_name] [varchar](50) NULL,
	[last_name] [varchar](50) NULL,
	[enrollment_date] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[students_courses]    Script Date: 6/14/17 11:35:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[students_courses](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[students_id] [int] NULL,
	[courses_id] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[university]    Script Date: 6/14/17 11:35:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[university](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[departments_id] [int] NULL,
	[courses_id] [int] NULL,
	[students_id] [int] NULL
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[courses] ON 

INSERT [dbo].[courses] ([id], [course_name], [course_number]) VALUES (1, N'English 101', N'ENG101')
INSERT [dbo].[courses] ([id], [course_name], [course_number]) VALUES (2, N'Biology 101', N'BIO101')
SET IDENTITY_INSERT [dbo].[courses] OFF
SET IDENTITY_INSERT [dbo].[departments] ON 

INSERT [dbo].[departments] ([id], [department_name]) VALUES (1, N'English Department')
INSERT [dbo].[departments] ([id], [department_name]) VALUES (3, N'Biology Department')
SET IDENTITY_INSERT [dbo].[departments] OFF
SET IDENTITY_INSERT [dbo].[students] ON 

INSERT [dbo].[students] ([id], [first_name], [last_name], [enrollment_date]) VALUES (11, N'Garrett', N'Hays', CAST(N'2017-02-11T00:00:00.000' AS DateTime))
INSERT [dbo].[students] ([id], [first_name], [last_name], [enrollment_date]) VALUES (12, N'Alyssa', N'Moody', CAST(N'2014-04-10T00:00:00.000' AS DateTime))
INSERT [dbo].[students] ([id], [first_name], [last_name], [enrollment_date]) VALUES (13, N'David', N'Rolfs', CAST(N'2017-08-14T00:00:00.000' AS DateTime))
SET IDENTITY_INSERT [dbo].[students] OFF
SET IDENTITY_INSERT [dbo].[students_courses] ON 

INSERT [dbo].[students_courses] ([id], [students_id], [courses_id]) VALUES (1, 1, 1)
INSERT [dbo].[students_courses] ([id], [students_id], [courses_id]) VALUES (2, 2, 2)
INSERT [dbo].[students_courses] ([id], [students_id], [courses_id]) VALUES (3, 3, 2)
INSERT [dbo].[students_courses] ([id], [students_id], [courses_id]) VALUES (4, 6, 1)
INSERT [dbo].[students_courses] ([id], [students_id], [courses_id]) VALUES (5, 5, 1)
INSERT [dbo].[students_courses] ([id], [students_id], [courses_id]) VALUES (10, 8, 2)
INSERT [dbo].[students_courses] ([id], [students_id], [courses_id]) VALUES (11, 8, 1)
INSERT [dbo].[students_courses] ([id], [students_id], [courses_id]) VALUES (12, 9, 2)
INSERT [dbo].[students_courses] ([id], [students_id], [courses_id]) VALUES (9, 6, 2)
INSERT [dbo].[students_courses] ([id], [students_id], [courses_id]) VALUES (13, 11, 2)
INSERT [dbo].[students_courses] ([id], [students_id], [courses_id]) VALUES (14, 13, 1)
SET IDENTITY_INSERT [dbo].[students_courses] OFF
SET IDENTITY_INSERT [dbo].[university] ON 

INSERT [dbo].[university] ([id], [departments_id], [courses_id], [students_id]) VALUES (1, 1, 8, 8)
INSERT [dbo].[university] ([id], [departments_id], [courses_id], [students_id]) VALUES (2, 1, 9, 9)
INSERT [dbo].[university] ([id], [departments_id], [courses_id], [students_id]) VALUES (3, 1, 10, 10)
SET IDENTITY_INSERT [dbo].[university] OFF
USE [master]
GO
ALTER DATABASE [registrar] SET  READ_WRITE 
GO
