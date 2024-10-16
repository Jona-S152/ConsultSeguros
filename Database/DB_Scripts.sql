USE [DB_Seguros]
GO
/****** Object:  Table [dbo].[Insurance]    Script Date: 14/10/2024 20:46:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Insurance](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[InsuranceName] [varchar](50) NULL,
	[InsuranceCode] [varchar](6) NULL,
	[InsuredAmount] [decimal](12, 6) NULL,
	[Prima] [decimal](12, 6) NULL,
	[Status] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InsuranceInsured]    Script Date: 14/10/2024 20:46:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InsuranceInsured](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Id_Insurance] [int] NULL,
	[Id_Insured] [int] NULL,
	[Status] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Insured]    Script Date: 14/10/2024 20:46:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Insured](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Identification] [varchar](10) NULL,
	[InsuredName] [varchar](50) NULL,
	[PhoneNumber] [varchar](10) NULL,
	[Age] [int] NULL,
	[Status] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[InsuranceInsured]  WITH CHECK ADD FOREIGN KEY([Id_Insurance])
REFERENCES [dbo].[Insurance] ([Id])
GO
ALTER TABLE [dbo].[InsuranceInsured]  WITH CHECK ADD FOREIGN KEY([Id_Insured])
REFERENCES [dbo].[Insured] ([Id])
GO
