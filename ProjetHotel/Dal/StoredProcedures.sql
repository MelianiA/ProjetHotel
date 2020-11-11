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

exec Employe_Read
---------------------------------------------------------------------------------------------------
alter PROC [dbo].[Etat_Read](@data xml=NULL)
AS
DECLARE @Id uniqueidentifier=NULL
select @Id = T.N.value('id[1]', 'uniqueidentifier') from @data.nodes('etat') as T(N)
Select Id,Libelle,Icone,Couleur,Entite from Etat where @Id is null Or Id=@Id
GO
	

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
select * from Hotel
