USE MakfiBD;
-- *************************************************************************************************
-- DROP
-- *************************************************************************************************
Declare @sql nvarchar(MAX)='';
select @sql = @sql + 'DROP PROC [' + name + '];' from sys.objects where type='p' and name not like 'sp[_]%'  order by name;
exec(@sql);
GO
 
-- *************************************************************************************************
-- _Read_
-- *************************************************************************************************
CREATE PROC [dbo].[Utilisateur_Read](@data xml=NULL)
AS
DECLARE @Id uniqueidentifier=NULL
select @Id = T.N.value('id[1]', 'uniqueidentifier') from @data.nodes('utilisateur') as T(N)
Select Id, Nom, Identifiant, CodePin, Statut, [Image] from Utilisateur where @Id is null Or Id=@Id
GO
---------------------------------------------------------------------------------------------------
CREATE PROC [dbo].[Hotel_Read](@data xml=NULL)
AS
DECLARE @id uniqueidentifier=NULL
DECLARE @gouvernante uniqueidentifier=NULL

select 
	@id = T.N.value('id[1]', 'uniqueidentifier'),
	@gouvernante = T.N.value('gouvernante[1]', 'uniqueidentifier') 
from 
	@data.nodes('hotel') as T(N)

Select Id, Nom, [Reception], [Gouvernante] ,[Commentaire],[Image] 
from Hotel 
where (@id is null Or Id=@id) and (@gouvernante is null Or gouvernante=@gouvernante)
GO
Exec Hotel_Read
Exec Hotel_Read '<hotel><id>C65BFB16-6DBE-4BC9-8314-0DEABABB0404</id></hotel>'
Exec Hotel_Read '<hotel><gouvernante>D4867483-472E-4432-AF36-28037FCD7FC7</gouvernante></hotel>'
---------------------------------------------------------------------------------------------------
CREATE PROC [dbo].[Employe_Read](@data xml=NULL)
AS
DECLARE @Id uniqueidentifier=NULL
select @Id = T.N.value('id[1]', 'uniqueidentifier') from @data.nodes('employe') as T(N)
Select Id, Nom,Prenom,Etat,Commentaire from Employe where @Id is null Or Id=@Id
GO
---------------------------------------------------------------------------------------------------

exec Employe_Read
---------------------------------------------------------------------------------------------------
alter PROC [dbo].[Etat_Read](@data xml=NULL)
AS
DECLARE @Id uniqueidentifier=NULL
select @Id = T.N.value('id[1]', 'uniqueidentifier') from @data.nodes('etat') as T(N)
Select Id,Libelle,Icone,Couleur,Entite from Etat where @Id is null Or Id=@Id
GO
---------------------------------------------------------------------------------------------------
create PROC [dbo].[Chambre_Read](@data xml=NULL)
AS
DECLARE @Id uniqueidentifier=NULL
select @Id = T.N.value('id[1]', 'uniqueidentifier') from @data.nodes('chambre') as T(N)
Select Id,Nom,Etat,Commentaire,Hotel from Chambre where @Id is null Or Id=@Id
GO
---------------------------------------------------------------------------------------------------
alter PROC [dbo].[HotelEmploye_Read](@data xml=NULL)
-- Retourne la liste des employés qui correspond à l'hôtel donné dans les paramètres 
AS
DECLARE @hotel uniqueidentifier=NULL
select @hotel = T.N.value('hotel[1]', 'uniqueidentifier') from @data.nodes('hotel') as T(N)
Select Employe from HotelEmploye where Hotel=@hotel
GO

exec HotelEmploye_Read '<hotel><hotel>6F04A94F-8129-4903-9506-2BAA05C4F0F2</hotel></hotel>'
-- *************************************************************************************************
-- save
-- *************************************************************************************************

  
CREATE PROC [dbo].[Utilisateur_Save](@data xml=NULL)
AS
DECLARE @IDs TABLE(ID uniqueidentifier);
--
update Utilisateur set Nom=t.nom, Statut=t.Statut
	output inserted.Id into @IDs(ID)
from 
	(
	select 
		T.N.value('id[1]', 'uniqueidentifier') id, 
		T.N.value('nom[1]', 'nvarchar(MAX)') Nom, 
		T.N.value('statut[1]', 'nvarchar(MAX)') Statut
		from @data.nodes('utilisateur') as T(N)
	where T.N.value('id[1]', 'uniqueidentifier') is not null
	)t
where Utilisateur.Id = t.id
---
insert Utilisateur (Nom, Statut) 
	output inserted.Id into @IDs(ID)
	(
	select 
		T.N.value('nom[1]', 'nvarchar(MAX)'), 
		T.N.value('statut[1]', 'nvarchar(MAX)') 
	from @data.nodes('utilisateur') as T(N)
	where T.N.value('id[1]', 'uniqueidentifier') is null
	);
SELECT ID FROM @IDs;
GO

exec Utilisateur_Save '<utilisateur><id>6FBA0732-FF0E-4AE0-871F-A710534F98FC</id><nom>teste</nom><statut>4</statut></utilisateur>'
exec Utilisateur_Save '<utilisateur><nom>csdf</nom><statut>4</statut></utilisateur>'
----------------------------------------------------------------------------------------------------------
alter PROC [dbo].[Employe_Save](@data xml=NULL)
AS
DECLARE @IDs TABLE(ID uniqueidentifier);
DECLARE @message nvarchar(MAX)
DECLARE @Id uniqueidentifier
DECLARE @Nom nvarchar(MAX)
DECLARE @Prenom nvarchar(MAX)
DECLARE @Etat uniqueidentifier
DECLARE @Commentaire nvarchar(MAX)

-- PARTIE recup XML :
select 
		T.N.value('(id/text())[1]', 'uniqueidentifier') Id, 
		T.N.value('(nom/text())[1]', 'nvarchar(MAX)') Nom, 
		T.N.value('(prenom/text())[1]', 'nvarchar(MAX)') Prenom, 
		T.N.value('(etat/text())[1]', 'uniqueidentifier') Etat,
		T.N.value('(commentaire/text())[1]', 'nvarchar(MAX)') Commentaire
  	 into #_employe
	 from @data.nodes('employe') as T(N)

-- PARTIE2
select @id=Id from #_employe

-- Update
BEGIN TRY
       update Employe set 
              Nom = t.Nom,
			  Prenom = t.Prenom,
			  Etat= t.Etat,
			  Commentaire=t.Commentaire
       output inserted.Id into @IDs(ID)
       from (select Nom, Prenom, Etat, Commentaire from #_employe where Id is not null) t
       where Employe.Id=@id
END TRY
BEGIN CATCH
       select @message = ERROR_MESSAGE() 
    RAISERROR (@message, 16, 1);  
       RETURN;
END CATCH

-- Insert
insert Employe(Nom, Prenom, Etat, Commentaire )
output inserted.Id into @IDs(ID)
(
select
       Nom, Prenom, Etat, Commentaire
from #_employe where Id is null
)
IF @Id is null select @id=ID from @IDs
select Id from @IDs
GO

exec [Employe_Save] '<employe>
                        <id>3E92EBC2-9105-4B81-844D-18BE2E105278</id>
                        <nom>aaaa</nom>
                        <prenom>bbbbb</prenom>
                        <etat>37DDF78D-E0D5-40B8-BE19-E47F2235B839</etat>
                        <commentaire>Commentaire ... 123 vive le code :)</commentaire>       
                    </employe>'
 ----------------------------------------------------------------------------------------------------------
Create PROC [dbo].[HotelEmploye_Save](@data xml=NULL)
AS
DECLARE @IDs TABLE(ID uniqueidentifier);
DECLARE @message nvarchar(MAX)
DECLARE @Id uniqueidentifier
DECLARE @Hotel uniqueidentifier
DECLARE @Employe uniqueidentifier

-- PARTIE recup XML :
select 
 		T.N.value('(hotel/text())[1]', 'uniqueidentifier') Hotel,
		T.N.value('(employe/text())[1]', 'nvarchar(MAX)') Employe
  	 into #_hotelEmploye
	 from @data.nodes('hotelEmploye') as T(N)

-- Insert
insert HotelEmploye(Hotel, Employe )
output inserted.Id into @IDs(ID)
(
select
       Hotel, Employe
from #_hotelEmploye  
)
IF @Id is null select @id=ID from @IDs
select Id from @IDs
GO

exec [HotelEmploye_Save] '<hotelEmploye>
							<hotel>c65bfb16-6dbe-4bc9-8314-0deababb0404</hotel>
							<employe>Makrisoft.Makfi.Models.Employe</employe>
						  </hotelEmploye>'
 ----------------------------------------------------------------------------------------------------------
create PROC [dbo].[Chambre_Save](@data xml=NULL)
AS
DECLARE @IDs TABLE(ID uniqueidentifier);
DECLARE @message nvarchar(MAX)
DECLARE @Id uniqueidentifier
DECLARE @Nom nvarchar(MAX)
DECLARE @Etat uniqueidentifier
DECLARE @Commentaire nvarchar(MAX)
DECLARE @Hotel uniqueidentifier

-- PARTIE recup XML :
select 
		T.N.value('(id/text())[1]', 'uniqueidentifier') Id, 
		T.N.value('(nom/text())[1]', 'nvarchar(MAX)') Nom, 
 		T.N.value('(etat/text())[1]', 'uniqueidentifier') Etat,
		T.N.value('(commentaire/text())[1]', 'nvarchar(MAX)') Commentaire,
		T.N.value('(hotel/text())[1]', 'uniqueidentifier') Hotel
  	 into #_chambre
	 from @data.nodes('chambre') as T(N)

-- PARTIE2
select @id=Id from #_chambre

-- Update
BEGIN TRY
       update Chambre set 
              Nom = t.Nom,
			  Etat = t.Etat,
			  Commentaire= t.Commentaire,
			  Hotel=t.Hotel
       output inserted.Id into @IDs(ID)
       from (select Nom, Etat, Commentaire, Hotel from #_chambre where Id is not null) t
       where Chambre.Id=@id
END TRY
BEGIN CATCH
       select @message = ERROR_MESSAGE() 
    RAISERROR (@message, 16, 1);  
       RETURN;
END CATCH

-- Insert
insert Chambre(Nom, Etat, Commentaire, Hotel )
output inserted.Id into @IDs(ID)
(
select
       Nom, Etat, Commentaire, Hotel 
from #_chambre where Id is null
)
IF @Id is null select @id=ID from @IDs
select Id from @IDs
GO

exec Chambre_Save '<chambre>
                        <id>86CAE02F-E402-4DAF-948C-AAF77133921C</id>
                        <nom>Chambre33</nom>
                         <etat>9C6166BB-9C34-4B3A-B9A0-C59B0B9E2D83</etat>
                        <commentaire>Commentaire ... 123 vive le code :)</commentaire>    
						<hotel>30E545B7-C604-4ED7-A6EB-20C3A1BE1730</hotel>
                    </chambre>'
select * from Chambre
 -- *************************************************************************************************
-- Delete
-- *************************************************************************************************

CREATE PROC [dbo].[Utilisateur_Delete](@data xml=NULL)
AS
DECLARE @ID uniqueidentifier;

select @ID= T.N.value('id[1]', 'uniqueidentifier') from @data.nodes('utilisateur') as T(N) 

delete from Utilisateur where Id = @ID

exec Utilisateur_Delete '<utilisateur><id>6FBA0732-FF0E-4AE0-871F-A710534F98FC</id></utilisateur>'
select * from Utilisateur
select * from Hotel
-----------------------------------------------------------------------
CREATE PROC [dbo].[Hotel_Delete](@data xml=NULL)
AS
DECLARE @ID uniqueidentifier;

select @ID= T.N.value('id[1]', 'uniqueidentifier') from @data.nodes('hotel') as T(N) 

delete from Hotel where Id = @ID

exec Utilisateur_Delete '<hotel><id>6FBA0732-FF0E-4AE0-871F-A710534F98FC</id></hotel>'
-----------------------------------------------------------------------
CREATE PROC [dbo].[Employe_Delete](@data xml=NULL)
AS
DECLARE @ID uniqueidentifier;

select @ID= T.N.value('id[1]', 'uniqueidentifier') from @data.nodes('employe') as T(N) 

delete from Employe where Id = @ID
go
-----------------------------------------------------------------------
alter PROC [dbo].[HotelEmploye_Delete](@data xml=NULL)
AS
DECLARE @Employe uniqueidentifier;
DECLARE @Hotel uniqueidentifier;
DECLARE @n int;
select 
	   @Employe= T.N.value('(employe/text())[1]', 'uniqueidentifier')  ,
	   @Hotel= T.N.value('(hotel/text())[1]', 'uniqueidentifier') 
from @data.nodes('hotelEmploye') as T(N) 
delete from HotelEmploye where Employe = @Employe and Hotel =@Hotel

select @n = count(*) from HotelEmploye where Employe=@Employe
IF @n=0
BEGIN
  delete from Employe where Id=@Employe
END
go
-----------------------------------------------------------------------

CREATE PROC [dbo].[Chambre_Delete](@data xml=NULL)
AS
DECLARE @ID uniqueidentifier;
select @ID= T.N.value('id[1]', 'uniqueidentifier') from @data.nodes('chambre') as T(N) 
delete from Chambre where Id = @ID
go
-- *************************************************************************************************
-- CanDelete
-- *************************************************************************************************
alter PROC [dbo].[Utilisateur_CanDelete](@data xml=NULL)
AS
DECLARE @id uniqueidentifier=NULL
select @id=T.N.value('id[1]', 'uniqueidentifier') from @data.nodes('utilisateur') as T(N)

select 'Hotel' tableName, COUNT(*) n from Hotel where Gouvernante=@id or Reception = @id  
UNION ALL
select 'Message' tableName, COUNT(*) n from [Message] where De=@id or A = @id  
GO

exec Utilisateur_CanDelete '<utilisateur><id>6FBA0732-FF0E-4AE0-871F-A710534F98FC</id></utilisateur>'

exec Utilisateur_CanDelete '<utilisateur><id>D4867483-472E-4432-AF36-28037FCD7FC7</id></utilisateur>'
-----------------------------------------------------------------------------------------------------
Create PROC [dbo].[Hotel_CanDelete](@data xml=NULL)
AS
DECLARE @id uniqueidentifier=NULL
select @id=T.N.value('id[1]', 'uniqueidentifier') from @data.nodes('hotel') as T(N)

select 'HotelEmploye' tableName, COUNT(*) n from HotelEmploye where Hotel=@id  
UNION ALL
select 'Intervention' tableName, COUNT(*) n from Intervention where Hotel=@id  
UNION ALL
select 'Chambre' tableName, COUNT(*) n from Chambre where Hotel=@id  
GO
select * from Hotel
exec Hotel_CanDelete '<hotel><id>B9F20D14-F9BC-4384-A3AD-57B1847CD1C3</id></hotel>'
-----------------------------------------------------------------------------------------------------
Create PROC [dbo].[Employe_CanDelete](@data xml=NULL)
AS
DECLARE @id uniqueidentifier=NULL
select @id=T.N.value('id[1]', 'uniqueidentifier') from @data.nodes('employe') as T(N)

select 'HotelEmploye' tableName, COUNT(*) n from HotelEmploye where Employe=@id  
UNION ALL
select 'InterventionDetail' tableName, COUNT(*) n from InterventionDetail where EmployeAffecte=@id  
GO
-----------------------------------------------------------------------------------------------------
Create PROC [dbo].[Chambre_CanDelete](@data xml=NULL)
AS
DECLARE @id uniqueidentifier=NULL
select @id=T.N.value('id[1]', 'uniqueidentifier') from @data.nodes('chambre') as T(N)

select 'ChambreGroupeChambre' tableName, COUNT(*) n from ChambreGroupeChambre where Chambre=@id  
UNION ALL
select 'InterventionDetail' tableName, COUNT(*) n from InterventionDetail where ChambreAffectee=@id  
GO
 