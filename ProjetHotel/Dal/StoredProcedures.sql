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
create PROC [dbo].[Utilisateur_Read](@data xml=NULL)
AS
DECLARE @Id uniqueidentifier=NULL
select @Id = T.N.value('id[1]', 'uniqueidentifier') from @data.nodes('utilisateur') as T(N)
Select Id, Nom, CodePin, Statut from Utilisateur where @Id is null Or Id=@Id
GO
---------------------------------------------------------------------------------------------------
create PROC [dbo].[Hotel_Read](@data xml=NULL)
AS
DECLARE @id uniqueidentifier=NULL
DECLARE @gouvernante uniqueidentifier=NULL

select 
	@id = T.N.value('id[1]', 'uniqueidentifier'),
	@gouvernante = T.N.value('gouvernante[1]', 'uniqueidentifier') 
from 
	@data.nodes('hotel') as T(N)

Select Id, Nom, [Reception], [Gouvernante] ,[Commentaire] 
from Hotel 
where (@id is null Or Id=@id) and (@gouvernante is null Or gouvernante=@gouvernante)
GO
 
---------------------------------------------------------------------------------------------------
CREATE PROC [dbo].[Employe_Read](@data xml=NULL)
AS
DECLARE @id uniqueidentifier=NULL
DECLARE @hotel uniqueidentifier=NULL

select 
	@id = T.N.value('id[1]', 'uniqueidentifier'),
	@hotel = T.N.value('hotel[1]', 'uniqueidentifier') 
from @data.nodes('employes') as T(N)

select 
	e.Id, Nom,Prenom,Etat,Commentaire 
from 
	HotelEmploye he inner join Employe e on e.Id=he.Employe 
where (@hotel is null Or he.Hotel=@hotel) and (@id is null or he.Employe=@id)
GO
---------------------------------------------------------------------------------------------------
create PROC [dbo].[Etat_Read](@data xml=NULL)
AS
DECLARE @Id uniqueidentifier=NULL
select @Id = T.N.value('id[1]', 'uniqueidentifier') from @data.nodes('etat') as T(N)
Select Id,Libelle,Icone,Couleur,Entite, EtatEtat from Etat where @Id is null Or Id=@Id
GO
---------------------------------------------------------------------------------------------------
create PROC [dbo].[Chambre_Read](@data xml=NULL)
AS
DECLARE @Hotel uniqueidentifier=NULL
select @Hotel = T.N.value('hotel[1]', 'uniqueidentifier') from @data.nodes('chambre') as T(N)
Select Id,Nom,Etat,Commentaire  from Chambre where Hotel=@Hotel
GO
 
---------------------------------------------------------------------------------------------------
create PROC [dbo].[HotelEmploye_Read](@data xml=NULL)
-- Retourne la liste des employés qui correspond à l'hôtel donné dans les paramètres 
AS
DECLARE @hotel uniqueidentifier=NULL
select @hotel = T.N.value('hotel[1]', 'uniqueidentifier') from @data.nodes('hotel') as T(N)
Select Employe from HotelEmploye where Hotel=@hotel
GO
---------------------------------------------------------------------------------------------------
CREATE PROC [dbo].[ChambreGroupeChambre_Read](@data xml=NULL)
AS
DECLARE @hotel uniqueidentifier=NULL 
select @hotel = T.N.value('(hotel/text())[1]', 'uniqueidentifier') from @data.nodes('chambreGroupeChambre') as T(N)
 select c.Id, c.Nom, e.id as Etat,c.Commentaire, STRING_AGG(gc.Nom,', ') GroupeChambre
 from ChambreGroupeChambre cgc
 right join Chambre c on c.Id=cgc.Chambre
 left join GroupeChambre gc on gc.Id = cgc.GroupeChambre
 left join Etat e on c.Etat=e.Id
 where c.Hotel=@hotel
 group by c.Id, c.Nom,e.id,c.Commentaire 
 GO
 ---------------------------------------------------------------------------------------------------
CREATE PROC [dbo].[GroupeChambre_Read](@data xml=NULL)
AS
DECLARE @hotel uniqueidentifier=NULL
select @hotel= T.N.value('hotel[1]', 'uniqueidentifier') from @data.nodes('groupeChambre') as T(N)

Select distinct gc.Id,gc.Nom,gc.Commentaire from GroupeChambre gc
inner join ChambreGroupeChambre cgc on gc.Id = cgc.GroupeChambre
inner join Chambre c on c.Id = cgc.Chambre
where c.Hotel = @hotel
 GO
---------------------------------------------------------------------------------------------------
CREATE PROC [dbo].[ChambreByGroupe_Read](@data xml=NULL)
AS
DECLARE @Hotel uniqueidentifier=NULL
select @Hotel = T.N.value('hotel[1]', 'uniqueidentifier') from @data.nodes('chambreByGroupe') as T(N)
select  c.id as IdDelaChambre ,cgc.GroupeChambre,gc.Nom ,c.Nom as NomChambre, c.Hotel
 from ChambreGroupeChambre cgc
 right join Chambre c on c.Id=cgc.Chambre
 left join GroupeChambre gc on gc.Id = cgc.GroupeChambre
 left join Etat e on c.Etat=e.Id
where c.Hotel=@Hotel
GO
---------------------------------------------------------------------------------------------------
CREATE PROC [dbo].[Intervention_Read](@data xml=NULL)
AS
DECLARE @Hotel uniqueidentifier=NULL
select @Hotel = T.N.value('hotel[1]', 'uniqueidentifier') from @data.nodes('intervention') as T(N)
 select Id,Libelle,Etat,convert(date, Date1, 120) as Date1 , Commentaire, Model from Intervention i
  where i.Hotel=@Hotel
 GO

--exec Intervention_Read '<intervention><hotel>96176477-13a9-46b9-b729-197741a53cc7</hotel></intervention>'
--select  * from Etat
--select * from Intervention

  ---------------------------------------------------------------------------------------------------
CREATE PROC [dbo].[InterventionDetail_Read](@data xml=NULL)
AS
DECLARE @Intervention uniqueidentifier=NULL
select @Intervention = T.N.value('intervention[1]', 'uniqueidentifier') from @data.nodes('interventionDetail') as T(N)

select Id,EmployeAffecte, ChambreAffectee,Etat, Commentaire
from InterventionDetail  
where Intervention=@Intervention
GO

--exec InterventionDetail_Read '<interventionDetail><intervention>48632a88-c16b-4869-a3e5-6d1eae5d8e10</intervention></interventionDetail>'
  ---------------------------------------------------------------------------------------------------
CREATE PROC [dbo].[Info_Read](@data xml=NULL)
AS
select Id,Cle, Valeur from Info
GO

 -- *************************************************************************************************
-- save
-- *************************************************************************************************
 
create PROC [dbo].[Utilisateur_Save](@data xml=NULL)
AS
DECLARE @IDs TABLE(ID uniqueidentifier);
DECLARE @message nvarchar(MAX)
DECLARE @Id uniqueidentifier

select 
		T.N.value('(id/text())[1]', 'uniqueidentifier') id, 
		T.N.value('nom[1]', 'nvarchar(MAX)') Nom, 
		T.N.value('codePin[1]', 'nvarchar(MAX)') CodePin,
		T.N.value('statut[1]', 'tinyint') Statut
into #_utilisateur	
from @data.nodes('utilisateur') as T(N)

-- PARTIE2
select @id=Id from #_utilisateur

-- Update
BEGIN TRY
       update Utilisateur set 
				  Nom = t.Nom,
				  CodePin = t.CodePin,
				  Statut= t.Statut
			output inserted.Id into @IDs(ID)
       from (select Nom, CodePin, Statut  from #_utilisateur where Id is not null) t
       where Utilisateur.Id=@id
END TRY
BEGIN CATCH
       select @message = ERROR_MESSAGE() 
    RAISERROR (@message, 16, 1);  
       RETURN;
END CATCH

-- Insert
insert Utilisateur( Nom, CodePin, Statut )
output inserted.Id into @IDs(ID)
(
select
        Nom, CodePin, Statut 
from #_utilisateur where Id is null
)
select Id from @IDs
GO
 
----------------------------------------------------------------------------------------------------------
create PROC [dbo].[Employe_Save](@data xml=NULL)
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

 ----------------------------------------------------------------------------------------------------------
CREATE PROC [dbo].[Chambre_Save](@data xml=NULL)
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
select @id=Id  from #_chambre

-- Update  
BEGIN TRY
 update Chambre set
			Nom= t.Nom, Etat=t.Etat,Commentaire=t.Commentaire
			       output inserted.Id into @IDs(ID)
			from (select Id, Nom, Etat, Commentaire from #_chambre where Id is not null) t
			where Chambre.Id=t.Id
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

 ----------------------------------------------------------------------------------------------------------
CREATE PROC [dbo].[GroupeChambre_Save](@data xml=NULL)
AS
	DECLARE @IDs TABLE(ID uniqueidentifier);
	DECLARE @message nvarchar(MAX)
	DECLARE @Id uniqueidentifier
	DECLARE @Nom nvarchar(MAX)
	DECLARE @Commentaire nvarchar(MAX)

-- PARTIE recup XML :
select 
 		T.N.value('(id/text())[1]', 'uniqueidentifier') Id, 
		T.N.value('(nom/text())[1]', 'nvarchar(MAX)') Nom, 
 		T.N.value('(commentaire/text())[1]', 'nvarchar(MAX)') Commentaire
   	 into #_groupeChambre
from @data.nodes('groupeChambre') as T(N)
-- PARTIE2
select @id=Id  from #_groupeChambre
-- Update  
BEGIN TRY
	 update GroupeChambre set
			Nom= t.Nom, Commentaire=t.Commentaire
					output inserted.Id into @IDs(ID)
			from (select Id, Nom, Commentaire from #_groupeChambre) t
			where GroupeChambre.Id=t.Id
END TRY
BEGIN CATCH
       select @message = ERROR_MESSAGE() 
    RAISERROR (@message, 16, 1);  
       RETURN;
END CATCH
-- Insert
if @id is null insert GroupeChambre(Nom, Commentaire )
		output inserted.Id into @IDs(ID)
	(select Nom, Commentaire from #_groupeChambre  )
select @id=ID from @IDs
select Id from @IDs
GO
 
 ----------------------------------------------------------------------------------------------------------
create PROC [dbo].[ChambreGroupeChambre_Save](@data xml=NULL)
 as
 	DECLARE @IDs TABLE(ID uniqueidentifier);
	DECLARE @Id uniqueidentifier
	DECLARE @message nvarchar(MAX)
 	DECLARE @Chambre nvarchar(MAX)
	DECLARE @GroupeChambre nvarchar(MAX) 

-- PARTIE recup XML :
select 
 		T.N.value('(chambre/text())[1]', 'nvarchar(MAX)') chambre, 
 		T.N.value('(groupeChambre/text())[1]', 'nvarchar(MAX)') groupeChambre
   	 into #_chambreGroupeChambre
from @data.nodes('chambreGroupeChambre') as T(N)

insert ChambreGroupeChambre(Chambre, GroupeChambre )
	output inserted.Id into @IDs(ID)
(select Chambre, GroupeChambre from #_chambreGroupeChambre  )
IF @Id is null select @id=ID from @IDs
select Id from @IDs
GO
 ----------------------------------------------------------------------------------------------------------
 CREATE PROC [dbo].[Intervention_Save](@data xml=NULL)
 AS
	DECLARE @IDs TABLE(ID uniqueidentifier);
	DECLARE @message nvarchar(MAX)
	DECLARE @Id uniqueidentifier
	DECLARE @Libelle nvarchar(MAX)
	DECLARE @Commentaire nvarchar(MAX)
	DECLARE @Date1 date
	DECLARE @GroupeChambre uniqueidentifier


 	DECLARE @Hotel uniqueidentifier

-- PARTIE recup XML :
select 
 		T.N.value('(id/text())[1]', 'uniqueidentifier') Id, 
		T.N.value('(libelle/text())[1]', 'nvarchar(MAX)') Libelle, 
 		T.N.value('(commentaire/text())[1]', 'nvarchar(MAX)') Commentaire,
  		T.N.value('(hotel/text())[1]', 'uniqueidentifier') Hotel,
		T.N.value('(etat/text())[1]', 'uniqueidentifier') Etat,
  		T.N.value('(date1/text())[1]', 'date') Date1,
  		T.N.value('(model/text())[1]', 'bit') Model
		into #_intervention
from @data.nodes('intervention') as T(N)
-- PARTIE2
select @id=Id  from #_intervention
-- Update  
BEGIN TRY
	 update Intervention set
			Libelle= t.Libelle, Commentaire=t.Commentaire, Date1=t.Date1,Model=t.Model,Etat=t.Etat
					output inserted.Id into @IDs(ID)
			from (select Id, Libelle, Commentaire, Date1, Model, Etat from #_intervention where Id is not null) t
			where Intervention.Id=t.Id
END TRY
BEGIN CATCH
       select @message = ERROR_MESSAGE() 
    RAISERROR (@message, 16, 1);  
       RETURN;
END CATCH
-- Insert
if @id is null insert Intervention(Libelle, Commentaire, Date1, Model, Hotel,Etat )
		output inserted.Id into @IDs(ID)
	(select Libelle, Commentaire, Date1, Model, Hotel,Etat from #_intervention  )
select @id=ID from @IDs
select Id from @IDs
GO

 --exec [Intervention_Save] '<intervention>
	--							<id></id>
	--							<libelle>(A définir ! )</libelle>
	--							<commentaire></commentaire>    
	--							<hotel>96176477-13a9-46b9-b729-197741a53cc7</hotel>
	--							<date1>25/11/2020 19:32:58</date1>    
	--							<model>True</model>   
	--							<etat>b12f9d9c-c0b8-4509-9234-a543c1b777e8</etat> 
	--					 </intervention>' 
	-- select * from Intervention

 ----------------------------------------------------------------------------------------------------------
Create PROC [dbo].[Hotel_Save](@data xml=NULL)
 AS
	DECLARE @IDs TABLE(ID uniqueidentifier);
	DECLARE @message nvarchar(MAX)
	DECLARE @Id uniqueidentifier
	DECLARE @Nom nvarchar(MAX)
	DECLARE @Commentaire nvarchar(MAX)
 	DECLARE @Reception uniqueidentifier
	DECLARE @Gouvernante uniqueidentifier

-- PARTIE recup XML :
select 
 		T.N.value('(id/text())[1]', 'uniqueidentifier') Id, 
		T.N.value('(nom/text())[1]', 'nvarchar(MAX)') Nom, 
 		T.N.value('(reception/text())[1]', 'uniqueidentifier') Reception,
  		T.N.value('(gouvernante/text())[1]', 'uniqueidentifier') Gouvernante,
  		T.N.value('(commentaire/text())[1]', 'nvarchar(MAX)') Commentaire
		into #_hotel
from @data.nodes('hotel') as T(N)
-- PARTIE2
select @id=Id  from #_hotel
-- Update  
BEGIN TRY
	 update Hotel set
			Nom= t.Nom, Commentaire=t.Commentaire, Reception=t.Reception,Gouvernante=t.Gouvernante
					output inserted.Id into @IDs(ID)
			from (select Id, Nom, Commentaire, Reception, Gouvernante from #_hotel) t
			where Hotel.Id=t.Id
END TRY
BEGIN CATCH
       select @message = ERROR_MESSAGE() 
    RAISERROR (@message, 16, 1);  
       RETURN;
END CATCH
-- Insert
if @id is null insert Hotel(Nom, Commentaire, Reception, Gouvernante )
		output inserted.Id into @IDs(ID)
	(select Nom, Commentaire, Reception, Gouvernante from #_hotel )
select @id=ID from @IDs
select Id from @IDs
GO
 ----------------------------------------------------------------------------------------------------------
--create PROC [dbo].[Etat_Save](@data xml=NULL)
-- AS
--	DECLARE @IDs TABLE(ID uniqueidentifier);
---- PARTIE recup XML :
--select 
-- 		T.N.value('(libelle/text())[1]', 'nvarchar(MAX)') Libelle, 
-- 		T.N.value('(icone/text())[1]', 'nvarchar(MAX)') Icone,
--  		T.N.value('(couleur/text())[1]', 'nvarchar(MAX)') Couleur,
--  		T.N.value('(entite/text())[1]', 'tinyint') Entite,
--		T.N.value('(entite/text())[1]', 'tinyint') Entite
--		into #_etat
--from @data.nodes('etats/etat') as T(N)
---- Insert
--insert Etat(Libelle, Icone, Couleur, Entite )
--		output inserted.Id into @IDs(ID)
--	(select Libelle, Icone, Couleur, Entite from #_etat )
--select Id from @IDs
--GO

--exec Etat_Save '  <etats>
--                         <etat><libelle>Fait</libelle>                  <icone>TimelineHelp</icone>             <couleur>gray</couleur>            <entite>3</entite> </etat>
--                         <etat> <libelle>En cours</libelle>             <icone>TableLock</icone>                <couleur>orange</couleur>          <entite>3</entite> </etat>
--                         <etat><libelle>None</libelle>                  <icone>TimelineHelp</icone>             <couleur>green</couleur>           <entite>3</entite> </etat>
--                         <etat> <libelle>Incident</libelle>             <icone>TimelineHelp</icone>             <couleur>red</couleur>             <entite>3</entite> </etat>
--                         <etat><libelle>Disponible</libelle>            <icone>FaceWomanShimmer</icone>         <couleur>green</couleur>           <entite>1</entite> </etat>
--                         <etat><libelle>Arrêt maladie</libelle>         <icone>FaceWomanShimmer</icone>         <couleur>red</couleur>             <entite>1</entite> </etat>
--                         <etat><libelle>Non disponible</libelle>        <icone>FaceWomanShimmer</icone>         <couleur>black</couleur>           <entite>1</entite> </etat>
--                         <etat><libelle>Fait</libelle>                  <icone>TableLock</icone>                <couleur>green</couleur>           <entite>2</entite> </etat>
--                         <etat><libelle>Pas encore fait</libelle>       <icone>TableLock</icone>                <couleur>Red</couleur>             <entite>2</entite> </etat>
--                </etats>'

 ----------------------------------------------------------------------------------------------------------
Create PROC [dbo].[Info_Save](@data xml=NULL)
AS
DECLARE @message nvarchar(MAX)
-- PARTIE recup XML :
select 
 		T.N.value('(id/text())[1]', 'uniqueidentifier') id, 
 		T.N.value('(cle/text())[1]', 'nvarchar(MAX)') cle ,
  		T.N.value('(valeur/text())[1]', 'nvarchar(MAX)') valeur
 		into #_info
from @data.nodes('info') as T(N)
 
-- Update  
BEGIN TRY
	 update Info set
			Valeur=t.valeur 
 			from (select id, cle, valeur from #_info) t
			where Info.Cle=t.cle
END TRY
BEGIN CATCH
       select @message = ERROR_MESSAGE() 
    RAISERROR (@message, 16, 1);  
       RETURN;
END CATCH
 GO

  ----------------------------------------------------------------------------------------------------------
  Create PROC [dbo].[InterventionDetail_Save](@data xml=NULL)
 AS
	DECLARE @IDs TABLE(ID uniqueidentifier);
	DECLARE @message nvarchar(MAX)
	DECLARE @Id uniqueidentifier

-- PARTIE recup XML :
select 
 		T.N.value('(id/text())[1]', 'uniqueidentifier') Id, 
		T.N.value('(employeAffecte/text())[1]', 'uniqueidentifier') EmployeAffecte, 
 		T.N.value('(chambreAffectee/text())[1]', 'uniqueidentifier') ChambreAffectee,
 		T.N.value('(commentaire/text())[1]', 'nvarchar(MAX)') Commentaire,
		T.N.value('(intervention/text())[1]', 'uniqueidentifier') Intervention,
  		T.N.value('(etat/text())[1]', 'uniqueidentifier') Etat
		into #_InterventionDetail
from @data.nodes('interventionDetail') as T(N)
-- PARTIE2
select @id=Id  from #_InterventionDetail
-- Update  
BEGIN TRY
	 update InterventionDetail set
			EmployeAffecte= t.EmployeAffecte, 
			ChambreAffectee=t.ChambreAffectee, 
			Commentaire=t.Commentaire,
			Intervention=t.Intervention,
			Etat=t.Etat
					output inserted.Id into @IDs(ID)
			from (select Id, EmployeAffecte, ChambreAffectee, Commentaire, Intervention,Etat from #_InterventionDetail where Id is not null) t
			where InterventionDetail.Id=t.Id
END TRY
BEGIN CATCH
       select @message = ERROR_MESSAGE() 
    RAISERROR (@message, 16, 1);  
       RETURN;
END CATCH
-- Insert
insert InterventionDetail(EmployeAffecte, ChambreAffectee, Commentaire, Intervention,Etat )
		output inserted.Id into @IDs(ID)
	(select EmployeAffecte, ChambreAffectee, Commentaire, Intervention,Etat from #_InterventionDetail where Id is null )
select @id=ID from @IDs
select Id from @IDs
GO

--exec InterventionDetail_Save '
--                    <interventionDetail>
--                        <id>62e4cb2b-8ffb-4193-bbd1-ed882b4921ec</id>
--                        <employeAffecte>a9531486-348e-4f08-a7c7-102a28026afd</employeAffecte>
--                        <commentaire>Je la modifie</commentaire>    
--						<chambreAffectee>f6dd41d2-527a-455d-b62e-0ffc09cc0d9a</chambreAffectee>
--                        <intervention>01eede37-1732-4f07-be1e-4f3b4a88b26b</intervention>    
--                        <etat>9f7702b6-1f5b-40b9-b663-baa56eeb17d0</etat> 
--                     </interventionDetail>'
 
 -- *************************************************************************************************
-- Delete
-- *************************************************************************************************

CREATE PROC [dbo].[Utilisateur_Delete](@data xml=NULL)
AS
DECLARE @ID uniqueidentifier;

select @ID= T.N.value('id[1]', 'uniqueidentifier') from @data.nodes('utilisateur') as T(N) 

delete from Utilisateur where Id = @ID
go
-----------------------------------------------------------------------
CREATE PROC [dbo].[Hotel_Delete](@data xml=NULL)
AS
DECLARE @ID uniqueidentifier;

select @ID= T.N.value('id[1]', 'uniqueidentifier') from @data.nodes('hotel') as T(N) 

delete from Hotel where Id = @ID
go
 -----------------------------------------------------------------------
CREATE PROC [dbo].[Employe_Delete](@data xml=NULL)
AS
DECLARE @ID uniqueidentifier;

select @ID= T.N.value('id[1]', 'uniqueidentifier') from @data.nodes('employe') as T(N) 

delete from Employe where Id = @ID
go
-----------------------------------------------------------------------
create PROC [dbo].[HotelEmploye_Delete](@data xml=NULL)
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
DECLARE @id uniqueidentifier;
select
	@id= T.N.value('id[1]', 'uniqueidentifier') 
from @data.nodes('chambre') as T(N) 
delete from Chambre where Id = @id
go
-----------------------------------------------------------------------
CREATE PROC [dbo].[ChambreGroupeChambre_Delete](@data xml=NULL)
as
DECLARE @GroupeChambre uniqueidentifier;
DECLARE @Hotel uniqueidentifier;
select
	@GroupeChambre= T.N.value('groupeChambre[1]', 'uniqueidentifier') ,
	@Hotel= T.N.value('hotel[1]', 'uniqueidentifier') 
from @data.nodes('chambreGroupeChambre') as T(N) 

delete from ChambreGroupeChambre
where Id in 
(
select cgc.Id  
from ChambreGroupeChambre cgc
inner join Chambre c on cgc.Chambre = c.Id
where c.Hotel=@Hotel and cgc.GroupeChambre=@GroupeChambre
 )
go

 -----------------------------------------------------------------------
create PROC [dbo].[GroupeChambre_Delete](@data xml=NULL)
 as
DECLARE @Id uniqueidentifier;
 select
	@Id= T.N.value('id[1]', 'uniqueidentifier') 
 from @data.nodes('groupeChambre') as T(N) 

delete from GroupeChambre
where Id = @Id
go
-----------------------------------------------------------------------
create PROC [dbo].[Intervention_Delete](@data xml=NULL)
as
DECLARE @id uniqueidentifier;
select
	@id= T.N.value('id[1]', 'uniqueidentifier') 
 from @data.nodes('intervention') as T(N) 
delete from Intervention
where Id = @id
go
-----------------------------------------------------------------------
create PROC [dbo].InterventionDetails_Delete(@data xml=NULL)
as
DECLARE @id uniqueidentifier;
select
	@id= T.N.value('id[1]', 'uniqueidentifier') 
 from @data.nodes('interventionDetails') as T(N) 
delete from InterventionDetail
where Id = @id
go

-- *************************************************************************************************
-- CanDelete
-- *************************************************************************************************
create PROC [dbo].[Utilisateur_CanDelete](@data xml=NULL)
AS
DECLARE @id uniqueidentifier=NULL
select @id=T.N.value('id[1]', 'uniqueidentifier') from @data.nodes('utilisateur') as T(N)

select 'Hotel' tableName, COUNT(*) n from Hotel where Gouvernante=@id or Reception = @id  
UNION ALL
select 'Message' tableName, COUNT(*) n from [Message] where De=@id or A = @id  
GO
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
CREATE PROC [dbo].[Chambre_CanDelete](@data xml=NULL)
AS
DECLARE @id uniqueidentifier=NULL
select @id=T.N.value('id[1]', 'uniqueidentifier') from @data.nodes('chambre') as T(N)
select 'InterventionDetail' tableName, COUNT(*) n from InterventionDetail where ChambreAffectee=@id  
UNION ALL
select 'ChambreGroupeChambre' tableName, COUNT(*) n from ChambreGroupeChambre where Chambre=@id  
GO
-----------------------------------------------------------------------------------------------------
create PROC [dbo].[GroupeChambre_CanDelete](@data xml=NULL)
AS
DECLARE @id uniqueidentifier=NULL
select @id=T.N.value('id[1]', 'uniqueidentifier') from @data.nodes('groupeChambre') as T(N)
select 'ChambreGroupeChambre' tableName, COUNT(*) n from ChambreGroupeChambre where GroupeChambre=@id  
GO
-----------------------------------------------------------------------------------------------------
create PROC [dbo].[Intervention_CanDelete](@data xml=NULL)
AS
DECLARE @id uniqueidentifier=NULL
select @id=T.N.value('id[1]', 'uniqueidentifier') from @data.nodes('intervention') as T(N)
select 'InterventionDetail' tableName, COUNT(*) n from InterventionDetail where Intervention=@id  
GO