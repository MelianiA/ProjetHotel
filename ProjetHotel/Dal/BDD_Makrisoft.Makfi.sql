---------------------------------------------
--            Makrisoft.Makfi              --
---------------------------------------------
/*************************************************************************************************/
-- DROP
/*************************************************************************************************/
USE [master]
GO
DROP DATABASE [MakfiBD]
GO

/*************************************************************************************************/
-- DATABASE
/*************************************************************************************************/
CREATE DATABASE [MakfiBD] ON PRIMARY
	( 
	NAME = N'MakfiBD', 
	FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\MakfiBD.mdf'
	)
	LOG ON 
	( 
	NAME = N'MakfiBD_log', 
	FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\MakfiBD_log.ldf'
	)
GO
 
/*************************************************************************************************/
-- CHAMBRE
/*************************************************************************************************/
use MakfiBD
go

CREATE TABLE [dbo].[Chambre]
	(
	[Id] [uniqueidentifier] NOT NULL,
	[Nom] [nvarchar](max) NOT NULL,
	[Etat] [uniqueidentifier] NOT NULL,
	[Commentaire] [nvarchar](max) NULL,
	[Hotel] [uniqueidentifier] NOT NULL,
	CONSTRAINT [PK_Chambre] PRIMARY KEY CLUSTERED 
	(
	[Id] ASC
	)
	)
GO

ALTER TABLE [dbo].[Chambre] ADD  CONSTRAINT [DF_Chambre_Id]  DEFAULT (newid()) FOR [Id]
GO

ALTER TABLE [dbo].[Chambre]  WITH CHECK ADD  CONSTRAINT [FK_Chambre_Etat] FOREIGN KEY([Etat])
REFERENCES [dbo].[Etat] ([Id])
GO

ALTER TABLE [dbo].[Chambre] CHECK CONSTRAINT [FK_Chambre_Etat]
GO

ALTER TABLE [dbo].[Chambre]  WITH CHECK ADD  CONSTRAINT [FK_Chambre_Hotel] FOREIGN KEY([Hotel])
REFERENCES [dbo].[Hotel] ([Id])
GO

ALTER TABLE [dbo].[Chambre] CHECK CONSTRAINT [FK_Chambre_Hotel]
GO
USE [MakfiBD]
GO

/*************************************************************************************************/
-- GROUPECHAMBRE
/*************************************************************************************************/
CREATE TABLE [dbo].[ChambreGroupeChambre](
	[Id] [uniqueidentifier] NOT NULL,
	[Chambre] [uniqueidentifier] NOT NULL,
	[GroupeChambre] [uniqueidentifier] NULL,
 CONSTRAINT [PK_ChambreGroupeChambre] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ChambreGroupeChambre] ADD  CONSTRAINT [DF_ChambreGroupeChambre_Id]  DEFAULT (newid()) FOR [Id]
GO

ALTER TABLE [dbo].[ChambreGroupeChambre]  WITH CHECK ADD  CONSTRAINT [FK_ChambreGroupeChambre_Chambre] FOREIGN KEY([Chambre])
REFERENCES [dbo].[Chambre] ([Id])
GO

ALTER TABLE [dbo].[ChambreGroupeChambre] CHECK CONSTRAINT [FK_ChambreGroupeChambre_Chambre]
GO

ALTER TABLE [dbo].[ChambreGroupeChambre]  WITH CHECK ADD  CONSTRAINT [FK_ChambreGroupeChambre_GroupeChambre] FOREIGN KEY([GroupeChambre])
REFERENCES [dbo].[GroupeChambre] ([Id])
GO

ALTER TABLE [dbo].[ChambreGroupeChambre] CHECK CONSTRAINT [FK_ChambreGroupeChambre_GroupeChambre]
GO
USE [MakfiBD]
GO


CREATE TABLE [dbo].[Employe](
	[Id] [uniqueidentifier] NOT NULL,
	[Nom] [nvarchar](max) NOT NULL,
	[Prenom] [nvarchar](max) NOT NULL,
	[Etat] [uniqueidentifier] NOT NULL,
	[Commentaire] [nvarchar](max) NULL,
 CONSTRAINT [PK_Employe] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
) 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Employe] ADD  CONSTRAINT [DF_Employe_Id]  DEFAULT (newid()) FOR [Id]
GO

ALTER TABLE [dbo].[Employe]  WITH CHECK ADD  CONSTRAINT [FK_Employe_Etat] FOREIGN KEY([Etat])
REFERENCES [dbo].[Etat] ([Id])
GO

ALTER TABLE [dbo].[Employe] CHECK CONSTRAINT [FK_Employe_Etat]
GO
USE [MakfiBD]
GO


CREATE TABLE [dbo].[Etat](
	[Id] [uniqueidentifier] NOT NULL,
	[Libelle] [nvarchar](max) NULL,
	[Icone] [nvarchar](max) NULL,
	[Couleur] [nvarchar](max) NOT NULL,
	[Entite] [tinyint] NOT NULL,
 CONSTRAINT [PK_Etat] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
) 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Etat] ADD  CONSTRAINT [DF_Etat_Id]  DEFAULT (newid()) FOR [Id]
GO
USE [MakfiBD]
GO


CREATE TABLE [dbo].[GroupeChambre](
	[Id] [uniqueidentifier] NOT NULL,
	[Nom] [nvarchar](max) NOT NULL,
	[Commentaire] [nvarchar](max) NULL,
 CONSTRAINT [PK_GroupeChambre] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
) 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[GroupeChambre] ADD  CONSTRAINT [DF_GroupeChambre_Id]  DEFAULT (newid()) FOR [Id]
GO
USE [MakfiBD]
GO


CREATE TABLE [dbo].[Hotel](
	[Id] [uniqueidentifier] NOT NULL,
	[Nom] [nvarchar](max) NOT NULL,
	[Reception] [uniqueidentifier] NULL,
	[Gouvernante] [uniqueidentifier] NULL,
	[Commentaire] [nvarchar](max) NULL,
	[Image] [nvarchar](max) NULL,
 CONSTRAINT [PK_Hotel] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
) 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Hotel] ADD  CONSTRAINT [DF_Hotel_Id]  DEFAULT (newid()) FOR [Id]
GO

ALTER TABLE [dbo].[Hotel]  WITH CHECK ADD  CONSTRAINT [FK_Hotel_Utilisateur] FOREIGN KEY([Reception])
REFERENCES [dbo].[Utilisateur] ([Id])
GO

ALTER TABLE [dbo].[Hotel] CHECK CONSTRAINT [FK_Hotel_Utilisateur]
GO

ALTER TABLE [dbo].[Hotel]  WITH CHECK ADD  CONSTRAINT [FK_Hotel_Utilisateur2] FOREIGN KEY([Gouvernante])
REFERENCES [dbo].[Utilisateur] ([Id])
GO

ALTER TABLE [dbo].[Hotel] CHECK CONSTRAINT [FK_Hotel_Utilisateur2]
GO
USE [MakfiBD]
GO



CREATE TABLE [dbo].[HotelEmploye](
	[Id] [uniqueidentifier] NOT NULL,
	[Hotel] [uniqueidentifier] NOT NULL,
	[Employe] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_HotelEmploye] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
) 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[HotelEmploye] ADD  CONSTRAINT [DF_HotelEmploye_Id]  DEFAULT (newid()) FOR [Id]
GO

ALTER TABLE [dbo].[HotelEmploye]  WITH CHECK ADD  CONSTRAINT [FK_HotelEmploye_Employe] FOREIGN KEY([Employe])
REFERENCES [dbo].[Employe] ([Id])
GO

ALTER TABLE [dbo].[HotelEmploye] CHECK CONSTRAINT [FK_HotelEmploye_Employe]
GO

ALTER TABLE [dbo].[HotelEmploye]  WITH CHECK ADD  CONSTRAINT [FK_HotelEmploye_Hotel] FOREIGN KEY([Hotel])
REFERENCES [dbo].[Hotel] ([Id])
GO

ALTER TABLE [dbo].[HotelEmploye] CHECK CONSTRAINT [FK_HotelEmploye_Hotel]
GO
USE [MakfiBD]
GO

/*************************************************************************************************/
/****** Object:  Table [dbo].[Intervention]    Script Date: 23/11/2020 08:55:22 ******/
/*************************************************************************************************/

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Intervention](
	[Id] [uniqueidentifier] NOT NULL,
	[Commentaire] [nvarchar](max) NULL,
	[Date1] [date] NOT NULL,
	[Libelle] [nvarchar](max) NULL,
	[GroupeChambre] [uniqueidentifier] NULL,
	[Hotel] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Intervention] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
) 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Intervention] ADD  CONSTRAINT [DF_Intervention_Id]  DEFAULT (newid()) FOR [Id]
GO

ALTER TABLE [dbo].[Intervention]  WITH CHECK ADD  CONSTRAINT [FK_Intervention_GroupeChambre] FOREIGN KEY([GroupeChambre])
REFERENCES [dbo].[GroupeChambre] ([Id])
GO

ALTER TABLE [dbo].[Intervention] CHECK CONSTRAINT [FK_Intervention_GroupeChambre]
GO

ALTER TABLE [dbo].[Intervention]  WITH CHECK ADD  CONSTRAINT [FK_Intervention_Hotel] FOREIGN KEY([Hotel])
REFERENCES [dbo].[Hotel] ([Id])
GO

ALTER TABLE [dbo].[Intervention] CHECK CONSTRAINT [FK_Intervention_Hotel]
GO
USE [MakfiBD]
GO
/*************************************************************************************************/
/****** Object:  Table [dbo].[InterventionDetail]    Script Date: 23/11/2020 08:55:30 ******/
/*************************************************************************************************/

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[InterventionDetail](
	[Id] [uniqueidentifier] NOT NULL,
	[EmployeAffecte] [uniqueidentifier] NOT NULL,
	[ChambreAffectee] [uniqueidentifier] NOT NULL,
	[Commentaire] [nvarchar](10) NULL,
	[Intervention] [uniqueidentifier] NOT NULL,
	[Etat] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_InterventionDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
) 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[InterventionDetail] ADD  CONSTRAINT [DF_InterventionDetail_Id]  DEFAULT (newid()) FOR [Id]
GO

ALTER TABLE [dbo].[InterventionDetail] ADD  CONSTRAINT [DF_InterventionDetail_Etat]  DEFAULT ('E9B7DB92-4C5F-4C80-8AD3-760501B12DC5') FOR [Etat]
GO

ALTER TABLE [dbo].[InterventionDetail]  WITH CHECK ADD  CONSTRAINT [FK_InterventionDetail_Chambre] FOREIGN KEY([ChambreAffectee])
REFERENCES [dbo].[Chambre] ([Id])
GO

ALTER TABLE [dbo].[InterventionDetail] CHECK CONSTRAINT [FK_InterventionDetail_Chambre]
GO

ALTER TABLE [dbo].[InterventionDetail]  WITH CHECK ADD  CONSTRAINT [FK_InterventionDetail_Employe] FOREIGN KEY([EmployeAffecte])
REFERENCES [dbo].[Employe] ([Id])
GO

ALTER TABLE [dbo].[InterventionDetail] CHECK CONSTRAINT [FK_InterventionDetail_Employe]
GO

ALTER TABLE [dbo].[InterventionDetail]  WITH CHECK ADD  CONSTRAINT [FK_InterventionDetail_Etat] FOREIGN KEY([Etat])
REFERENCES [dbo].[Etat] ([Id])
GO

ALTER TABLE [dbo].[InterventionDetail] CHECK CONSTRAINT [FK_InterventionDetail_Etat]
GO

ALTER TABLE [dbo].[InterventionDetail]  WITH CHECK ADD  CONSTRAINT [FK_InterventionDetail_Intervention] FOREIGN KEY([Intervention])
REFERENCES [dbo].[Intervention] ([Id])
GO

ALTER TABLE [dbo].[InterventionDetail] CHECK CONSTRAINT [FK_InterventionDetail_Intervention]
GO
USE [MakfiBD]
GO

/*************************************************************************************************/
/****** Object:  Table [dbo].[Message]    Script Date: 23/11/2020 08:55:36 ******/
/*************************************************************************************************/

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Message](
	[Id] [uniqueidentifier] NOT NULL,
	[De] [uniqueidentifier] NOT NULL,
	[A] [uniqueidentifier] NOT NULL,
	[EnvoyeLe] [nchar](10) NOT NULL,
	[Libelle] [nchar](10) NULL,
	[Statut] [nchar](10) NOT NULL,
 CONSTRAINT [PK_Message] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
) 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Message] ADD  CONSTRAINT [DF_Message_Id]  DEFAULT (newid()) FOR [Id]
GO

ALTER TABLE [dbo].[Message]  WITH CHECK ADD  CONSTRAINT [FK_Message_Utilisateur] FOREIGN KEY([De])
REFERENCES [dbo].[Utilisateur] ([Id])
GO

ALTER TABLE [dbo].[Message] CHECK CONSTRAINT [FK_Message_Utilisateur]
GO

ALTER TABLE [dbo].[Message]  WITH CHECK ADD  CONSTRAINT [FK_Message_Utilisateur1] FOREIGN KEY([A])
REFERENCES [dbo].[Utilisateur] ([Id])
GO

ALTER TABLE [dbo].[Message] CHECK CONSTRAINT [FK_Message_Utilisateur1]
GO
USE [MakfiBD]
GO
/*************************************************************************************************/
/****** Object:  Table [dbo].[Utilisateur]    Script Date: 23/11/2020 08:55:43 ******/
/*************************************************************************************************/

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Utilisateur](
	[Id] [uniqueidentifier] NOT NULL,
	[Nom] [nvarchar](max) NOT NULL,
	[Identifiant] [int] NULL,
	[CodePin] [nvarchar](max) NOT NULL,
	[Statut] [tinyint] NOT NULL,
	[Image] [nvarchar](max) NULL,
 CONSTRAINT [PK_Utilisateur] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
) 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Utilisateur] ADD  CONSTRAINT [DF_Utilisateur_Id]  DEFAULT (newid()) FOR [Id]
GO

ALTER TABLE [dbo].[Utilisateur] ADD  CONSTRAINT [DF_Utilisateur_CodePin]  DEFAULT ((0)) FOR [CodePin]
GO

ALTER TABLE [dbo].[Utilisateur] ADD  CONSTRAINT [DF_Utilisateur_Statut]  DEFAULT ((1)) FOR [Statut]
GO











