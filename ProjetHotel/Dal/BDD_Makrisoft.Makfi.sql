---------------------------------------------
--            Makrisoft.Makfi              --
---------------------------------------------
/*************************************************************************************************/
-- DROP
/*************************************************************************************************/
USE [master]
GO
ALTER DATABASE [MakfiBD] SET  SINGLE_USER WITH ROLLBACK IMMEDIATE
GO
DROP DATABASE [MakfiBD] 
GO

--SP_WHO 
--KILL 53
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
USE [MakfiBD]
GO

/****** Object:  Table [dbo].[Chambre]    Script Date: 04/01/2021 10:21:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Chambre](
	[Id] [uniqueidentifier] NOT NULL,
	[Nom] [nvarchar](max) NOT NULL,
	[Etat] [uniqueidentifier] NOT NULL,
	[Commentaire] [nvarchar](max) NULL,
	[Hotel] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Chambre] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
USE [MakfiBD]
GO

/****** Object:  Table [dbo].[ChambreGroupeChambre]    Script Date: 04/01/2021 10:22:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ChambreGroupeChambre](
	[Id] [uniqueidentifier] NOT NULL,
	[Chambre] [uniqueidentifier] NOT NULL,
	[GroupeChambre] [uniqueidentifier] NULL,
 CONSTRAINT [PK_ChambreGroupeChambre] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
USE [MakfiBD]
GO

/****** Object:  Table [dbo].[Employe]    Script Date: 04/01/2021 10:22:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
USE [MakfiBD]
GO

/****** Object:  Table [dbo].[Etat]    Script Date: 04/01/2021 10:22:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Etat](
	[Id] [uniqueidentifier] NOT NULL,
	[Libelle] [nvarchar](max) NULL,
	[Icone] [nvarchar](max) NULL,
	[Couleur] [nvarchar](max) NOT NULL,
	[Entite] [tinyint] NOT NULL,
	[EtatEtat] [bit] NOT NULL,
 CONSTRAINT [PK_Etat] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
USE [MakfiBD]
GO

/****** Object:  Table [dbo].[GroupeChambre]    Script Date: 04/01/2021 10:23:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GroupeChambre](
	[Id] [uniqueidentifier] NOT NULL,
	[Nom] [nvarchar](max) NOT NULL,
	[Commentaire] [nvarchar](max) NULL,
	[Hotel] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_GroupeChambre] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
USE [MakfiBD]
GO

/****** Object:  Table [dbo].[Hotel]    Script Date: 04/01/2021 10:23:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
USE [MakfiBD]
GO

/****** Object:  Table [dbo].[HotelEmploye]    Script Date: 04/01/2021 10:23:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HotelEmploye](
	[Id] [uniqueidentifier] NOT NULL,
	[Hotel] [uniqueidentifier] NOT NULL,
	[Employe] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_HotelEmploye] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
USE [MakfiBD]
GO

/****** Object:  Table [dbo].[Info]    Script Date: 04/01/2021 10:24:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Info](
	[Id] [uniqueidentifier] NOT NULL,
	[Cle] [nvarchar](max) NOT NULL,
	[Valeur] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Info] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
USE [MakfiBD]
GO

/****** Object:  Table [dbo].[Intervention]    Script Date: 04/01/2021 10:24:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Intervention](
	[Id] [uniqueidentifier] NOT NULL,
	[Commentaire] [nvarchar](max) NULL,
	[Date1] [datetime] NOT NULL,
	[Libelle] [nvarchar](max) NULL,
	[Model] [bit] NOT NULL,
	[Hotel] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Intervention] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
USE [MakfiBD]
GO

/****** Object:  Table [dbo].[InterventionDetail]    Script Date: 04/01/2021 10:25:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[InterventionDetail](
	[Id] [uniqueidentifier] NOT NULL,
	[EmployeAffecte] [uniqueidentifier] NOT NULL,
	[ChambreAffectee] [uniqueidentifier] NOT NULL,
	[Commentaire] [nvarchar](max) NULL,
	[Intervention] [uniqueidentifier] NOT NULL,
	[Etat] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_InterventionDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
USE [MakfiBD]
GO

/****** Object:  Table [dbo].[Message]    Script Date: 04/01/2021 10:25:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Message](
	[Id] [uniqueidentifier] NOT NULL,
	[IdHisto] [uniqueidentifier] NOT NULL,
	[De] [uniqueidentifier] NOT NULL,
	[A] [uniqueidentifier] NOT NULL,
	[EnvoyeLe] [datetime] NOT NULL,
	[Objet] [nvarchar](max) NULL,
	[Libelle] [nvarchar](max) NULL,
	[Etat] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Message] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
USE [MakfiBD]
GO

/****** Object:  Table [dbo].[Utilisateur]    Script Date: 04/01/2021 10:25:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Utilisateur](
	[Id] [uniqueidentifier] NOT NULL,
	[Nom] [nvarchar](max) NOT NULL,
	[Identifiant] [int] NULL,
	[CodePin] [nvarchar](max) NULL,
	[Statut] [tinyint] NOT NULL,
	[Image] [nvarchar](max) NULL,
 CONSTRAINT [PK_Utilisateur] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
 
/**********************************************************************/
/**********                  Clés                     *****************/
/**********************************************************************/

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

ALTER TABLE [dbo].[Employe] ADD  CONSTRAINT [DF_Employe_Id]  DEFAULT (newid()) FOR [Id]
GO

ALTER TABLE [dbo].[Employe]  WITH CHECK ADD  CONSTRAINT [FK_Employe_Etat] FOREIGN KEY([Etat])
REFERENCES [dbo].[Etat] ([Id])
GO

ALTER TABLE [dbo].[Employe] CHECK CONSTRAINT [FK_Employe_Etat]
GO

ALTER TABLE [dbo].[Etat] ADD  CONSTRAINT [DF_Etat_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[GroupeChambre] ADD  CONSTRAINT [DF_GroupeChambre_Id]  DEFAULT (newid()) FOR [Id]
GO

ALTER TABLE [dbo].[GroupeChambre]  WITH CHECK ADD  CONSTRAINT [FK_GroupeChambre_Hotel] FOREIGN KEY([Hotel])
REFERENCES [dbo].[Hotel] ([Id])
GO

ALTER TABLE [dbo].[GroupeChambre] CHECK CONSTRAINT [FK_GroupeChambre_Hotel]
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

ALTER TABLE [dbo].[Info] ADD  CONSTRAINT [DF_Info_Id]  DEFAULT (newid()) FOR [Id]
GO

ALTER TABLE [dbo].[Intervention] ADD  CONSTRAINT [DF_Intervention_Id]  DEFAULT (newid()) FOR [Id]
GO

ALTER TABLE [dbo].[Intervention]  WITH CHECK ADD  CONSTRAINT [FK_Intervention_Hotel] FOREIGN KEY([Hotel])
REFERENCES [dbo].[Hotel] ([Id])
GO

ALTER TABLE [dbo].[Intervention] CHECK CONSTRAINT [FK_Intervention_Hotel]
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
ALTER TABLE [dbo].[Message] ADD  CONSTRAINT [DF_Message_Id]  DEFAULT (newid()) FOR [Id]
GO

ALTER TABLE [dbo].[Message] ADD  CONSTRAINT [DF_Message_EnvoyeLe]  DEFAULT (getdate()) FOR [EnvoyeLe]
GO

ALTER TABLE [dbo].[Message]  WITH CHECK ADD  CONSTRAINT [FK_Message_Etat] FOREIGN KEY([Etat])
REFERENCES [dbo].[Etat] ([Id])
GO

ALTER TABLE [dbo].[Message] CHECK CONSTRAINT [FK_Message_Etat]
GO

ALTER TABLE [dbo].[Message]  WITH CHECK ADD  CONSTRAINT [FK_Message_Message] FOREIGN KEY([IdHisto])
REFERENCES [dbo].[Message] ([Id])
GO

ALTER TABLE [dbo].[Message] CHECK CONSTRAINT [FK_Message_Message]
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

ALTER TABLE [dbo].[Utilisateur] ADD  CONSTRAINT [DF_Utilisateur_Id]  DEFAULT (newid()) FOR [Id]
GO

ALTER TABLE [dbo].[Utilisateur] ADD  CONSTRAINT [DF_Utilisateur_CodePin]  DEFAULT ((0)) FOR [CodePin]
GO

ALTER TABLE [dbo].[Utilisateur] ADD  CONSTRAINT [DF_Utilisateur_Statut]  DEFAULT ((1)) FOR [Statut]
GO
/************************************************************************************************************/
									/* Insertions importantes */
/************************************************************************************************************/
 /*****Insertion dans le table Info ******/
 insert into Info(Cle,Valeur)
 values
 ('PasswordAdmin','#69!'),
 ('Version','1'),
 ('PasswordChange','#11#'),
 ('VoirMsgArchives','false')

 /*****Insertion de l'admin ******/
 insert into Utilisateur(Nom,CodePin,Statut)
 values('Admin','#69!','255')

 /*****Insertion de le table Etat ******/
GO
INSERT [dbo].[Etat] ([Id], [Libelle], [Icone], [Couleur], [Entite], [EtatEtat]) 
VALUES 
  (N'33755851-c85a-4ca3-a52e-21eaf6683dc1', N'Envoyé', N'Mail', N'Navy', 5, 1)
, (N'72d86e99-737d-4a84-9547-2245e4e4935d', N'None', N'TimelineHelp', N'gray', 4, 0)
, (N'04d2e6b4-0e57-4eda-ad60-256d3f59f714', N'Disponible', N'TableLock', N'green', 2, 1)
, (N'51e53e1d-e307-4a10-8722-258621a4645a', N'Brouillon', N'Mail', N'gray', 5, 0)
, (N'dbaac456-7a2c-4895-9e3b-4890131fc375', N'Incident-T', N'TimelineHelp', N'red', 4, 1)
, (N'fb6b9a88-7b33-42c7-b8ba-4c787abab027', N'None', N'TimelineHelp', N'gray', 3, 0)
, (N'241cad4b-5249-4044-8b73-51a85ab73e0f', N'Supprimé', N'Mail', N'red', 5, 0)
, (N'8f89591b-0672-49ba-8e0a-51f0ebbd4d21', N'Non lu', N'Mail', N'orange', 5, 0)
, (N'f731a036-8c44-4974-ba52-723fe9fbb54d', N'En cours', N'TimelineHelp', N'orange', 4, 0)
, (N'd3813796-0707-4425-9301-7d36ecb5eeb4', N'Arrêt maladie', N'FaceWomanShimmer', N'red', 1, 0)
, (N'0501d5d5-3b8e-4429-883b-877ac9691057', N'Non disponible', N'FaceWomanShimmer', N'black', 1, 0)
, (N'bf7b7684-2823-4927-bec9-8db85f466060', N'Non disponible', N'TableLock', N'Red', 2, 0)
, (N'3b6dcc06-1c7c-4213-9535-973beeb6d91f', N'Disponible', N'FaceWomanShimmer', N'green', 1, 1)
, (N'a3ca8733-48c5-4d64-a0e0-98bf1157c880', N'Fait', N'TimelineHelp', N'green', 4, 1)
, (N'3179b40c-020b-44f8-b756-99bcb287f599', N'Non terminée', N'TimelineHelp', N'orange', 3, 0)
, (N'ceaf6469-3dec-4cd7-9489-c5fd364f17cb', N'Incident-NT', N'TimelineHelp', N'red', 4, 0)
, (N'7457fdc9-e252-4c22-a665-cdcd8ebbf80e', N'Lu', N'Mail', N'green', 5, 1)
, (N'052706fa-04bb-4bf7-ad65-fe2b7f83871c', N'Terminée', N'TimelineHelp', N'green', 3, 1)
GO
 