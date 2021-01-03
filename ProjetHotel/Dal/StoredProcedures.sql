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
---------------------------------------------------------------------------------------------------
CREATE PROC Chambre_Read(@data xml=NULL)
AS
-- DECLARE
DECLARE @hotel uniqueidentifier=NULL
DECLARE @groupeChambre uniqueidentifier=NULL
DECLARE @notGroupeChambre uniqueidentifier=NULL

-- PARAMS
select 
	@hotel            = T.N.value('hotel[1]',            'uniqueidentifier'),
	@groupeChambre    = T.N.value('groupeChambre[1]',    'uniqueidentifier'),
	@notGroupeChambre = T.N.value('notGroupeChambre[1]', 'uniqueidentifier') 
from @data.nodes('chambres') as T(N)

--CAS 1 : <chambres><hotel>@hotel</hotel><groupeChambre>@groupeChambre</groupeChambre></chambres>
IF @groupeChambre is not null
	select c.Id,c.Nom,c.Etat,c.Commentaire, cgc.GroupeChambre  from Chambre c
	left outer join ChambreGroupeChambre cgc on cgc.Chambre=c.Id
	left outer join GroupeChambre gc on cgc.GroupeChambre=gc.Id
	where (@hotel is null or c.Hotel=@hotel) and gc.Id=@groupeChambre
--CAS 2 : <chambres><hotel>@hotel</hotel><notGroupeChambre>@notGroupeChambre</notGroupeChambre></chambres>
ELSE IF @notGroupeChambre is not null
	select c.Id,c.Nom,c.Etat,c.Commentaire, cgc.GroupeChambre from Chambre c
	left outer join ChambreGroupeChambre cgc on cgc.Chambre=c.Id
	left outer join GroupeChambre gc on cgc.GroupeChambre=gc.Id
	where (@hotel is null or c.Hotel=@hotel) and (gc.Id is null or gc.Id<>@notGroupeChambre)
--CAS 3 : <chambres><hotel>@hotel</hotel></chambres>
ELSE
	Select c.Id,Nom,Etat,Commentaire, null GroupeChambre  
		from Chambre c
		where (@hotel is null or Hotel=@hotel) 
GO
---------------------------------------------------------------------------------------------------
CREATE PROC Employe_Read(@data xml=NULL)
AS
-- DECLARE
DECLARE @hotel uniqueidentifier=NULL

-- PARAMS
select 
	@hotel = T.N.value('hotel[1]', 'uniqueidentifier') 
from @data.nodes('employes') as T(N)
-- CAS 1 :  <employes><hotel>@hotel</hotel></employes>
select 
	e.Id, e.Nom, e.Prenom, e.Etat, e.Commentaire 
from 
	HotelEmploye he inner join Employe e on e.Id=he.Employe 
where (@hotel is null Or he.Hotel=@hotel) 
GO
---------------------------------------------------------------------------------------------------
create PROC Etat_Read(@data xml=NULL)
AS
Select e.Id, e.Libelle, e.Icone, e.Couleur, e.Entite, e.EtatEtat from Etat e
GO
---------------------------------------------------------------------------------------------------
CREATE PROC GroupeChambre_Read(@data xml=NULL)
AS
-- DECLARE
DECLARE @hotel uniqueidentifier=NULL
-- PARAMS
select @hotel= T.N.value('hotel[1]', 'uniqueidentifier') from @data.nodes('groupeChambres') as T(N)
-- CAS 1 : <groupeChambres><hotel>@hotel</hotel></groupeChambres>
Select distinct gc.Id,gc.Nom,gc.Commentaire from GroupeChambre gc where gc.Hotel = @hotel
 GO
----------------------------------------------------------------------------------------------	-----
create PROC Hotel_Read(@data xml=NULL)
AS
-- DECLARE
DECLARE @gouvernante uniqueidentifier=NULL
-- PARAMS
select @gouvernante = T.N.value('gouvernante[1]', 'uniqueidentifier') from @data.nodes('hotels') as T(N)

-- CAS 1 : <hotels><gouvernante>@gouvernante</gouvernante></hotels>
Select h.Id, h.Nom, h.Reception, h.Gouvernante ,h.Commentaire from Hotel h
where @gouvernante is null Or gouvernante=@gouvernante
GO
---------------------------------------------------------------------------------------------------
CREATE PROC Info_Read(@data xml=NULL)
AS
select Id,Cle, Valeur from Info
GO
---------------------------------------------------------------------------------------------------
CREATE PROC Intervention_Read(@data xml=NULL)
AS
-- DECLARE
DECLARE @hotel uniqueidentifier=NULL
DECLARE @delete uniqueidentifier=NULL

-- PARAMS
select 
	@hotel = T.N.value('hotel[1]', 'uniqueidentifier') ,
	@delete = T.N.value('delete[1]', 'uniqueidentifier') 
from @data.nodes('interventions') as T(N)

-- CAS 1 : <interventions><hotel>@hotel</hotel></interventions>
--		   <interventions><hotel>@hotel</hotel><delete>@delete</delete></interventions>
 select Id,Libelle,Etat,convert(date, Date1, 120) as Date1 , Commentaire, Model from Intervention i
  where (@hotel is null or i.Hotel=@hotel) and (@delete is null or Id <> @delete)
 GO
---------------------------------------------------------------------------------------------------
CREATE PROC InterventionDetail_Read(@data xml=NULL)
AS
-- DECLARE
DECLARE @Intervention uniqueidentifier=NULL
-- PARAMS
select @Intervention = T.N.value('intervention[1]', 'uniqueidentifier') from @data.nodes('interventionDetails') as T(N)

-- CAS 1 : <interventionDetails><intervention>@Intervention</intervention></interventionDetails>
select 
	id.Id,EmployeAffecte, ChambreAffectee,id.Etat, id.Commentaire, c.Nom ChambreNom,e.Nom EmployeNom, e.Prenom EmployePrenom
from InterventionDetail  id 
inner join Employe e  on e.Id = id.EmployeAffecte
inner join Chambre c  on c.Id = id.ChambreAffectee
where @Intervention is null or Intervention=@Intervention
GO  
---------------------------------------------------------------------------------------------------
CREATE PROC Message_Read(@data xml=NULL)
AS
-- DECLARE
DECLARE @deOuA uniqueidentifier=NULL
DECLARE @exclude nvarchar(MAX)=NULL
-- PARAMS
select 
	@deOuA = T.N.value('deOuA[1]', 'uniqueidentifier'),
	@exclude = T.N.value('exclude[1]', 'nvarchar(MAX)') from @data.nodes('messages') as T(N)

-- CAS 1 : <messages><deOuA>@deOuA</deOuA></messages>
--		   <messages><exclude>Supprimé</exclude><deOuA>@deOuA</deOuA></messages>
select m.Id, IdHisto, De, A, EnvoyeLe, m.Libelle, Etat, Objet
from Message m 
inner join Etat e on m.Etat=e.Id
where 
	(@exclude is null or  e.Libelle <> @exclude) and 
	(@deOuA is null or (De=@deOuA or A=@deOuA))
GO
---------------------------------------------------------------------------------------------------
create PROC Utilisateur_Read(@data xml=NULL)
AS
-- DECLARE
DECLARE @Id uniqueidentifier=NULL
DECLARE @statut tinyint=NULL
-- PARAMS
select 
	@Id = T.N.value('id[1]', 'uniqueidentifier'),
	@statut = T.N.value('statut[1]', 'tinyint')
from @data.nodes('utilisateurs') as T(N)

-- CAS 1 : <utilisateurs><statut>@statut</statut></utilisateurs>
Select Id, Nom, CodePin, Statut from Utilisateur 
where (@Id is null Or Id=@Id) and (@statut is null or Statut=@statut)
GO

 -- *************************************************************************************************
-- save
-- *************************************************************************************************
CREATE PROC Chambre_Save(@data xml=NULL)
AS
-- DECLARE
DECLARE @IDs TABLE(ID uniqueidentifier, insere bit);
DECLARE @message nvarchar(MAX)

-- #DATA
select 
 		T.N.value('(id/text())[1]', 'uniqueidentifier') Id, 
		T.N.value('(nom/text())[1]', 'nvarchar(MAX)') Nom, 
 		T.N.value('(etat/text())[1]', 'uniqueidentifier') Etat,
		T.N.value('(commentaire/text())[1]', 'nvarchar(MAX)') Commentaire,
		T.N.value('(hotel/text())[1]', 'uniqueidentifier') Hotel
  	 into #data
from @data.nodes('chambres/chambre') as T(N)

-- CAS 1
--	<chambres>
--	  <chambre>
--       <id>@id</id>
--       <nom>nom</nom>
--       <etat>etat</etat>
--       <commentaire>commentaire</commentaire>    
--       <hotel>hotel</hotel>
--	  </chambre>
--   </chambres>

-- UPDATE  
BEGIN TRY
	merge into Chambre
	using (select Id, Nom, Etat, Commentaire, hotel from #data) t
	on Chambre.Id=t.Id
		when matched then
		update set 
			Nom = t.Nom,
			Etat= t.Etat,
			Hotel= t.Hotel,
			Commentaire= t.Commentaire
		output inserted.Id, 0 into @IDs(ID, insere);
END TRY
BEGIN CATCH
	select @message = ERROR_MESSAGE() 
	RAISERROR (@message, 16, 1);  
	RETURN;
END CATCH

-- INSERT
BEGIN TRY
	merge into Chambre 
	using (select Nom, Etat, Commentaire, Hotel from #data where Id is null) as t
	on 1=0
		when not matched then
		insert (Nom, Etat, Commentaire, Hotel) 
		values (t.Nom, t.Etat, t.Commentaire, t.Hotel)
		output inserted.Id, 1 into @IDs(ID, insere);
END TRY
BEGIN CATCH
	select @message = ERROR_MESSAGE() 
	RAISERROR (@message, 16, 1);  
	RETURN;
END CATCH

-- RETURN 
select Id, insere from @IDs
GO
----------------------------------------------------------------------------------------------------------
CREATE PROC Employe_Save(@data xml=NULL)
AS
-- DECLARE
DECLARE @IDs TABLE(ID uniqueidentifier, insere bit, hotel uniqueidentifier);
DECLARE @message nvarchar(MAX)

-- #DATA
select 
		T.N.value('(id/text())[1]', 'uniqueidentifier') Id, 
		T.N.value('(hotel/text())[1]', 'uniqueidentifier') hotel, 
		T.N.value('(nom/text())[1]', 'nvarchar(MAX)') Nom, 
		T.N.value('(prenom/text())[1]', 'nvarchar(MAX)') Prenom, 
		T.N.value('(etat/text())[1]', 'uniqueidentifier') Etat,
		T.N.value('(commentaire/text())[1]', 'nvarchar(MAX)') Commentaire
  	 into #data
	 from @data.nodes('employes/employe') as T(N)

-- CAS 1
-- <employes>
--	 <employe>
--     <id>id</id>
--     <nom>nom</nom>
--     <hotel>hotel</hotel>
--     <prenom>prenom</prenom>
--     <etat>etat</etat>
--     <commentaire>commentaire</commentaire>       
-- 	<employe>
-- </employes>

-- UPDATE
BEGIN TRY
	merge into Employe
	using (select Id, Nom, Prenom, Etat, Commentaire, hotel from #data) t
	on Employe.Id=t.Id
		when matched then
		update set 
			Nom = t.Nom,
			Prenom = t.Prenom,
			Etat= t.Etat,
			Commentaire= t.Commentaire
		output inserted.Id, 0, t.hotel into @IDs(ID, insere, hotel);
END TRY
BEGIN CATCH
	select @message = ERROR_MESSAGE() 
	RAISERROR (@message, 16, 1);  
	RETURN;
END CATCH

-- INSERT
BEGIN TRY
	merge into Employe 
	using (select Nom, Prenom, Etat, Commentaire, hotel from #data where Id is null) as t
	on 1=0
		when not matched then
		insert (Nom, Prenom, Etat, Commentaire) 
		values (t.Nom, t.Prenom, t.Etat, t.Commentaire)
		output inserted.Id, 1, t.hotel into @IDs(ID, insere, hotel);
END TRY
BEGIN CATCH
	select @message = ERROR_MESSAGE() 
	RAISERROR (@message, 16, 1);  
	RETURN;
END CATCH

-- TABLES ANNEXES : HotelEmploye
delete HotelEmploye from HotelEmploye he inner join #data d on he.Employe = d.Id and he.Hotel=d.hotel
insert HotelEmploye (Hotel, Employe) select Hotel, Id from @iDS

-- RETURN
select Id, insere,hotel from @IDs
GO
----------------------------------------------------------------------------------------------------------
CREATE PROC GroupeChambre_Save(@data xml=NULL)
AS
-- DECLARE
DECLARE @IDs TABLE(ID uniqueidentifier, insere bit);
DECLARE @message nvarchar(MAX)

-- #DATA
select 
	T.N.value('(id/text())[1]', 'uniqueidentifier') Id, 
	T.N.value('(hotel/text())[1]', 'uniqueidentifier') hotel, 
	T.N.value('(nom/text())[1]', 'nvarchar(MAX)') Nom, 
	T.N.value('(commentaire/text())[1]', 'nvarchar(MAX)') Commentaire
into #data
from @data.nodes('groupeChambres') as T(N)

-- CAS 1
--  <groupeChambres><groupeChambre>
--  	<id>}</id>
--  	<nom></nom>
--  	<hotel></hotel>
--  	<chambres>
--        <chambre></chambre>
--        ...
--        <chambre></chambre>
--      </chambres>
--  	<commentaire></commentaire>    
--  </groupeChambre></groupeChambres>

-- UPDATE
BEGIN TRY
	merge into GroupeChambre
	using (select Id, Nom, Commentaire, Hotel from #data) t
	on GroupeChambre.Id=t.Id
		when matched then
		update set 
			Nom = t.Nom,
			Hotel= t.Hotel,
			Commentaire= t.Commentaire
		output inserted.Id, 0 into @IDs(ID, insere);
END TRY
BEGIN CATCH
	select @message = ERROR_MESSAGE() 
	RAISERROR (@message, 16, 1);  
	RETURN;
END CATCH

-- INSERT
BEGIN TRY
	merge into GroupeChambre 
	using (select Nom, hotel, Commentaire, hotel from #data where Id is null) as t
	on 1=0
		when not matched then
		insert (Nom, hotel, Commentaire) 
		values (t.Nom, t.hotel, t.Commentaire)
		output inserted.Id, 1 into @IDs(ID, insere);
END TRY
BEGIN CATCH
	select @message = ERROR_MESSAGE() 
	RAISERROR (@message, 16, 1);  
	RETURN;
END CATCH

-- RETURN
select Id, insere from @IDs
GO

----------------------------------------------------------------------------------------------------------
Create PROC Hotel_Save(@data xml=NULL)
AS
-- DECLARE 
DECLARE @IDs TABLE(ID uniqueidentifier, insere bit);
DECLARE @message nvarchar(MAX)

-- #DATA
select 
	T.N.value('(id/text())[1]', 'uniqueidentifier') Id, 
	T.N.value('(nom/text())[1]', 'nvarchar(MAX)') Nom, 
	T.N.value('(reception/text())[1]', 'uniqueidentifier') Reception,
	T.N.value('(gouvernante/text())[1]', 'uniqueidentifier') Gouvernante,
	T.N.value('(commentaire/text())[1]', 'nvarchar(MAX)') Commentaire
into #data
from @data.nodes('hotels') as T(N)

-- CAS 1 
--<hotels><hotel>
--	<id>{CurrentDgSource.Id}</id>
--	<nom>{CurrentDgSource.Nom}</nom> 
--	<reception>{CurrentDgSource.Reception?.Id}</reception>
--	<gouvernante>{CurrentDgSource.Gouvernante?.Id}</gouvernante>
--	<commentaire>{CurrentDgSource.Commentaire}</commentaire>       
--</hotel></hotels>

-- UPDATE
BEGIN TRY
	merge into Hotel
	using (select Id, Nom, Commentaire, Reception, Gouvernante from #data) t
	on Hotel.Id=t.Id
		when matched then
		update set 
			Nom = t.Nom,
			Reception = t.Reception,
			Gouvernante = t.Gouvernante,
			Commentaire= t.Commentaire
		output inserted.Id, 0 into @IDs(ID, insere);
END TRY
BEGIN CATCH
	select @message = ERROR_MESSAGE() 
	RAISERROR (@message, 16, 1);  
	RETURN;
END CATCH

-- INSERT
BEGIN TRY
	merge into Hotel 
	using (select Nom, Commentaire, Reception, Gouvernante from #data where Id is null) as t
	on 1=0
		when not matched then
		insert (Nom, Commentaire, Reception, Gouvernante) 
		values (t.Nom, t.Commentaire, t.Reception, t.Gouvernante)
		output inserted.Id, 1 into @IDs(ID, insere);
END TRY
BEGIN CATCH
	select @message = ERROR_MESSAGE() 
	RAISERROR (@message, 16, 1);  
	RETURN;
END CATCH

-- RETURN
select Id, insere from @IDs
GO
----------------------------------------------------------------------------------------------------------
 CREATE PROC Intervention_Save(@data xml=NULL)
 AS
-- DECLARE
DECLARE @IDs TABLE(ID uniqueidentifier, insere bit);
DECLARE @message nvarchar(MAX)

-- #DATA
select 
	T.N.value('(id/text())[1]', 'uniqueidentifier') Id, 
	T.N.value('(libelle/text())[1]', 'nvarchar(MAX)') Libelle, 
	T.N.value('(commentaire/text())[1]', 'nvarchar(MAX)') Commentaire,
	T.N.value('(hotel/text())[1]', 'uniqueidentifier') Hotel,
	T.N.value('(etat/text())[1]', 'uniqueidentifier') Etat,
	T.N.value('(date1/text())[1]', 'date') Date1,
	T.N.value('(model/text())[1]', 'bit') Model
into #data
from @data.nodes('intervention') as T(N)

-- CAS 1
--<interventions><intervention>
--	<id></id>
--	<libelle></libelle>
--	<commentaire></commentaire>    
--	<hotel></hotel>
--	<date1></date1>    
--	<model></model>   
--	<etat></etat> 
--</intervention></interventions>

-- UPDATE
BEGIN TRY
	merge into Intervention
	using (select Id, Libelle, Commentaire, Date1, Model, Etat from #data) t
	on Intervention.Id=t.Id
		when matched then
		update set 
			Libelle = t.Libelle,
			Date1 = t.Date1,
			Model = t.Model,
			Etat= t.Etat,
			Commentaire= t.Commentaire
		output inserted.Id, 0, t.hotel into @IDs(ID, insere, hotel);
END TRY
BEGIN CATCH
	select @message = ERROR_MESSAGE() 
	RAISERROR (@message, 16, 1);  
	RETURN;
END CATCH

-- INSERT
BEGIN TRY
	merge into Intervention 
	using (select Libelle, Commentaire, Date1, Model, Etat from #data where Id is null) as t
	on 1=0
		when not matched then
		insert (Libelle, Commentaire, Date1, Model, Etat) 
		values (t.Libelle, t.Commentaire, t.Date1, t.Model, t.Etat)
		output inserted.Id, 1 into @IDs(ID, insere);
END TRY
BEGIN CATCH
	select @message = ERROR_MESSAGE() 
	RAISERROR (@message, 16, 1);  
	RETURN;
END CATCH

-- RETURN
select Id, insere from @IDs
GO
   ----------------------------------------------------------------------------------------------------------
 Create PROC InterventionDetail_Save(@data xml=NULL)
 AS
-- DECLARE
DECLARE @IDs TABLE(ID uniqueidentifier, insere bit, hotel uniqueidentifier);
DECLARE @message nvarchar(MAX)

-- #DATA
select 
 		T.N.value('(../intervention/text())[1]', 'uniqueidentifier') intervention,
 		T.N.value('(id/text())[1]', 'uniqueidentifier') Id, 
		T.N.value('(employeAffecte/text())[1]', 'uniqueidentifier') EmployeAffecte, 
 		T.N.value('(chambreAffectee/text())[1]', 'uniqueidentifier') ChambreAffectee,
 		T.N.value('(commentaire/text())[1]', 'nvarchar(MAX)') Commentaire,
  		T.N.value('(etat/text())[1]', 'uniqueidentifier') Etat
into #data
from @data.nodes('interventionDetails/interventionDetail') as T(N)

-- CAS 1
--<interventionDetails>
--	<intervention></intervention> 
--	<interventionDetail>
--		<id></id>
--		<employeAffecte></employeAffecte>
--		<commentaire></commentaire>    
--		<chambreAffectee></chambreAffectee>
--		<etat></etat> 
--	</interventionDetail>
--</interventionDetails>

-- UPDATE
BEGIN TRY
	merge into InterventionDetail
	using (select Id, EmployeAffecte, ChambreAffectee, Commentaire, Intervention, Etat from #data) t
	on InterventionDetail.Id=t.Id
		when matched then
		update set 
			EmployeAffecte = t.EmployeAffecte,
			EmployeAffecte = t.EmployeAffecte,
			Intervention = t.Intervention,
			Etat= t.Etat,
			Commentaire= t.Commentaire
		output inserted.Id, 0 into @IDs(ID, insere);
END TRY
BEGIN CATCH
	select @message = ERROR_MESSAGE() 
	RAISERROR (@message, 16, 1);  
	RETURN;
END CATCH

-- INSERT
BEGIN TRY
	merge into InterventionDetail 
	using (select EmployeAffecte, ChambreAffectee, Commentaire, Intervention, Etat from #data where Id is null) as t
	on 1=0
		when not matched then
		insert (EmployeAffecte, ChambreAffectee, Commentaire, Intervention, Etat) 
		values (t.EmployeAffecte, t.ChambreAffectee, t.Commentaire, t.Intervention, t.Etat)
		output inserted.Id, 1 into @IDs(ID, insere);
END TRY
BEGIN CATCH
	select @message = ERROR_MESSAGE() 
	RAISERROR (@message, 16, 1);  
	RETURN;
END CATCH

-- RETURN
select Id, insere from @IDs
GO
----------------------------------------------------------------------------------------------------------
CREATE PROC Message_Save(@data xml=NULL)
AS
-- DECLARE
DECLARE @IDs TABLE(ID uniqueidentifier, insere bit, hotel uniqueidentifier);
DECLARE @message nvarchar(MAX)

-- #DATA
select 
	T.N.value('(archive/text())[1]', 'uniqueidentifier') archive, 
	T.N.value('(id/text())[1]', 'uniqueidentifier') id, 
	T.N.value('(idHisto/text())[1]', 'uniqueidentifier') idHisto, 
	T.N.value('(de/text())[1]', 'uniqueidentifier') de, 
	T.N.value('(a/text())[1]', 'uniqueidentifier') a,
	T.N.value('(envoyeLe/text())[1]', 'datetime') envoyeLe,
	T.N.value('(libelle/text())[1]', 'nvarchar(MAX)') libelle,
	T.N.value('(objet/text())[1]', 'nvarchar(MAX)') objet,
	T.N.value('(etat/text())[1]', 'uniqueidentifier') etat
into #data	
from @data.nodes('messages') as T(N)

-- CAS 1
--<messages><message>
--	<id></id>
--	<de></de>
--	<a></a>
--	<envoyeLe></envoyeLe>
--	<libelle></libelle>
--	<objet></objet>
--	<etat></etat>
--</message></messages>

-- UPDATE
BEGIN TRY
	merge into [Message]
	using (select Id, idHisto, de, a, envoyeLe, libelle, objet, etat from #data) t
	on [Message].Id=t.Id
		when matched then
		update set 
			idHisto = t.idHisto,
			De = t.De,
			A = t.A,
			envoyeLe = t.envoyeLe,
			libelle = t.libelle,
			objet = t.objet,
			Etat= t.Etat
		output inserted.Id, 0 into @IDs(ID, insere);
END TRY
BEGIN CATCH
	select @message = ERROR_MESSAGE() 
	RAISERROR (@message, 16, 1);  
	RETURN;
END CATCH

-- INSERT
BEGIN TRY
	merge into [Message] 
	using (select idHisto, de, a, envoyeLe, libelle, objet, etat from #data where Id is null) as t
	on 1=0
		when not matched then
		insert (idHisto, de, a, envoyeLe, libelle, objet, etat) 
		values (t.idHisto, t.de, t.a, t.envoyeLe, t.libelle, t.objet, t.etat)
		output inserted.Id, 1 into @IDs(ID, insere);
END TRY
BEGIN CATCH
	select @message = ERROR_MESSAGE() 
	RAISERROR (@message, 16, 1);  
	RETURN;
END CATCH

-- RETURN
select Id, insere,hotel from @IDs
GO
 
----------------------------------------------------------------------------------------------------------
CREATE PROC Utilisateur_Save(@data xml=NULL)
AS
-- DECLARE
DECLARE @IDs TABLE(ID uniqueidentifier, insere bit);
DECLARE @message nvarchar(MAX)

-- #DATA
select 
		T.N.value('(id/text())[1]', 'uniqueidentifier') id, 
		T.N.value('nom[1]', 'nvarchar(MAX)') Nom, 
		T.N.value('codePin[1]', 'nvarchar(MAX)') CodePin,
		T.N.value('statut[1]', 'tinyint') Statut
into #data	
from @data.nodes('utilisateur') as T(N)

-- CAS 1
--<utilisateurs><utilisateur>
--	<id></id>
--	<nom></nom>
--	<codePin></codePin>
--	<statut></statut>
--</utilisateur></utilisateur>

-- UPDATE
BEGIN TRY
	merge into Utilisateur
	using (select Id, Nom, CodePin, Statut from #data) t
	on Utilisateur.Id=t.Id
		when matched then
		update set 
			Nom = t.Nom,
			CodePin = t.CodePin,
			Statut= t.Statut
		output inserted.Id, 0 into @IDs(ID, insere);
END TRY
BEGIN CATCH
	select @message = ERROR_MESSAGE() 
	RAISERROR (@message, 16, 1);  
	RETURN;
END CATCH

-- INSERT
BEGIN TRY
	merge into Utilisateur 
	using (select Nom, CodePin, Statut from #data where Id is null) as t
	on 1=0
		when not matched then
		insert (Nom, CodePin, Statut) 
		values (t.Nom, t.CodePin, t.Statut)
		output inserted.Id, 1 into @IDs(ID, insere);
END TRY
BEGIN CATCH
	select @message = ERROR_MESSAGE() 
	RAISERROR (@message, 16, 1);  
	RETURN;
END CATCH

-- RETURN
select Id, insere from @IDs
GO
  
 -- *************************************************************************************************
-- Delete
-- *************************************************************************************************

CREATE PROC Utilisateur_Delete(@data xml=NULL)
AS
-- DECLARE
DECLARE @IDs TABLE(ID uniqueidentifier, insere bit, hotel uniqueidentifier);
DECLARE @message nvarchar(MAX)

-- #DATA
select 
		T.N.value('(id/text())[1]', 'uniqueidentifier') id
into #data	
from @data.nodes('utilisateurs/utilisateur') as T(N)

-- CAS 1
-- <utilisateurs><utilisateur><id></id></utilisateur></utilisateurs>

-- DELETE
BEGIN TRY
	delete from Utilisateur 
	output deleted.Id, 0 into @IDs(ID, insere)
	where Id in (select Id from #data)
END TRY
BEGIN CATCH
	select @message = ERROR_MESSAGE() 
	RAISERROR (@message, 16, 1);  
	RETURN;
END CATCH

-- RETURN
select Id, insere from @IDs
go
-----------------------------------------------------------------------
CREATE PROC Hotel_Delete(@data xml=NULL)
AS
-- DECLARE
DECLARE @IDs TABLE(ID uniqueidentifier, insere bit, hotel uniqueidentifier);
DECLARE @message nvarchar(MAX)

-- #DATA
select 
		T.N.value('(id/text())[1]', 'uniqueidentifier') id
into #data	
from @data.nodes('utilisateurs/utilisateur') as T(N)

-- CAS 1
-- <hotels><hotel><id></id></hotel></hotels>

-- DELETE
BEGIN TRY
	delete from Hotel 
	output deleted.Id, 0 into @IDs(ID, insere)
	where Id in (select Id from #data)
END TRY
BEGIN CATCH
	select @message = ERROR_MESSAGE() 
	RAISERROR (@message, 16, 1);  
	RETURN;
END CATCH

-- RETURN
select Id, insere from @IDs
go
 -----------------------------------------------------------------------
CREATE PROC Employe_Delete(@data xml=NULL)
AS
-- DECLARE
DECLARE @IDs TABLE(ID uniqueidentifier, insere bit, hotel uniqueidentifier);
DECLARE @message nvarchar(MAX)

-- #DATA
select 
		T.N.value('(id/text())[1]', 'uniqueidentifier') id
into #data	
from @data.nodes('utilisateurs/utilisateur') as T(N)

-- CAS 1
-- <employes><employe><id></id></employe></employes>

-- DELETE
BEGIN TRY
	delete from Employe 
	output deleted.Id, 0 into @IDs(ID, insere)
	where Id in (select Id from #data)
END TRY
BEGIN CATCH
	select @message = ERROR_MESSAGE() 
	RAISERROR (@message, 16, 1);  
	RETURN;
END CATCH

-- RETURN
select Id, insere from @IDs
go
-----------------------------------------------------------------------
CREATE PROC Chambre_Delete(@data xml=NULL)
AS
-- DECLARE
DECLARE @IDs TABLE(ID uniqueidentifier, insere bit, hotel uniqueidentifier);
DECLARE @message nvarchar(MAX)

-- #DATA
select 
		T.N.value('(id/text())[1]', 'uniqueidentifier') id
into #data	
from @data.nodes('utilisateurs/utilisateur') as T(N)

-- CAS 1
-- <chambres><chambre><id></id></chambre></chambres>

-- DELETE
BEGIN TRY
	delete from Chambre 
	output deleted.Id, 0 into @IDs(ID, insere)
	where Id in (select Id from #data)
END TRY
BEGIN CATCH
	select @message = ERROR_MESSAGE() 
	RAISERROR (@message, 16, 1);  
	RETURN;
END CATCH

-- RETURN
select Id, insere from @IDs
go
 -----------------------------------------------------------------------
create PROC GroupeChambre_Delete(@data xml=NULL)
 as
-- DECLARE
DECLARE @IDs TABLE(ID uniqueidentifier, insere bit, hotel uniqueidentifier);
DECLARE @message nvarchar(MAX)

-- #DATA
select 
		T.N.value('(id/text())[1]', 'uniqueidentifier') id
into #data	
from @data.nodes('utilisateurs/utilisateur') as T(N)

-- CAS 1
-- <groupechambres><groupechambre><id></id></groupechambre></groupechambres>

-- DELETE
BEGIN TRY
	delete from GroupeChambre 
	output deleted.Id, 0 into @IDs(ID, insere)
	where Id in (select Id from #data)
END TRY
BEGIN CATCH
	select @message = ERROR_MESSAGE() 
	RAISERROR (@message, 16, 1);  
	RETURN;
END CATCH

-- RETURN
select Id, insere from @IDs
go
-----------------------------------------------------------------------
create PROC Intervention_Delete(@data xml=NULL)
as
-- DECLARE
DECLARE @IDs TABLE(ID uniqueidentifier, insere bit, hotel uniqueidentifier);
DECLARE @message nvarchar(MAX)

-- #DATA
select 
		T.N.value('(id/text())[1]', 'uniqueidentifier') id
into #data	
from @data.nodes('utilisateurs/utilisateur') as T(N)

-- CAS 1
-- <interventions><intervention><id></id></intervention></interventions>

-- DELETE
BEGIN TRY
	delete from Intervention 
	output deleted.Id, 0 into @IDs(ID, insere)
	where Id in (select Id from #data)
END TRY
BEGIN CATCH
	select @message = ERROR_MESSAGE() 
	RAISERROR (@message, 16, 1);  
	RETURN;
END CATCH

-- RETURN
select Id, insere from @IDs
go
-----------------------------------------------------------------------
create PROC InterventionDetail_Delete(@data xml=NULL)
as
-- DECLARE
DECLARE @IDs TABLE(ID uniqueidentifier, insere bit, hotel uniqueidentifier);
DECLARE @message nvarchar(MAX)
DECLARE @countId int
DECLARE @countIntervention int

-- #DATA
select 
		T.N.value('(interventions/id/text())[1]', 'uniqueidentifier') intervention,
		T.N.value('(interventionDetail/id/text())[1]', 'uniqueidentifier') id
into #data	
from @data.nodes('interventionDetails') as T(N)

-- 2 CAS 
-- <interventionDetails><interventionDetail><id></id></interventionDetail></interventionDetails>
-- <interventionDetails><interventions><id></id></interventions></interventionDetails>

select @countId = COUNT(id) from #data 
select @countIntervention = COUNT(Intervention) from #data

-- DELETE
BEGIN TRY
	IF @countId > 0
		delete from InterventionDetail 
		output deleted.Id, 0 into @IDs(ID, insere)
		where Id in (select Id from #data)
	IF @countIntervention > 0
		delete from InterventionDetail 
		output deleted.Id, 0 into @IDs(ID, insere)
		where Intervention in (select Intervention from #data)
END TRY
BEGIN CATCH
	select @message = ERROR_MESSAGE() 
	RAISERROR (@message, 16, 1);  
	RETURN;
END CATCH

-- RETURN
select Id, insere from @IDs
go

-- *************************************************************************************************
-- CanDelete
-- *************************************************************************************************
create PROC Utilisateur_CanDelete(@data xml=NULL)
AS
select T.N.value('id[1]', 'uniqueidentifier') id
into #data
from @data.nodes('utilisateurs/utilisateur') as T(N)

select 'Hotel' tableName, COUNT(*) n from Hotel where Gouvernante in (select id from #data) or Reception in (select id from #data)  
UNION ALL
select 'Message' tableName, COUNT(*) n from [Message] where De in (select id from #data) or A in (select id from #data)   
GO
-----------------------------------------------------------------------------------------------------
Create PROC Hotel_CanDelete(@data xml=NULL)
AS
select T.N.value('id[1]', 'uniqueidentifier') id
into #data
from @data.nodes('utilisateurs/utilisateur') as T(N)

select 'GroupeChambre' tableName, COUNT(*) n from GroupeChambre where Hotel in (select id from #data)   
UNION ALL
select 'Chambre' tableName, COUNT(*) n from Chambre where Hotel in (select id from #data) 
UNION ALL
select 'HotelEmploye' tableName, COUNT(*) n from HotelEmploye where Hotel in (select id from #data)  
UNION ALL
select 'Intervention' tableName, COUNT(*) n from Intervention where Hotel in (select id from #data) 
GO
-----------------------------------------------------------------------------------------------------
Create PROC Employe_CanDelete(@data xml=NULL)
AS
select T.N.value('id[1]', 'uniqueidentifier') id
into #data
from @data.nodes('utilisateurs/utilisateur') as T(N)

select 'HotelEmploye' tableName, COUNT(*) n from HotelEmploye where Employe in (select id from #data)   
UNION ALL
select 'InterventionDetail' tableName, COUNT(*) n from InterventionDetail where EmployeAffecte in (select id from #data) 
GO
-----------------------------------------------------------------------------------------------------
CREATE PROC Chambre_CanDelete(@data xml=NULL)
AS
select T.N.value('id[1]', 'uniqueidentifier') id
into #data
from @data.nodes('utilisateurs/utilisateur') as T(N)

select 'InterventionDetail' tableName, COUNT(*) n from InterventionDetail where ChambreAffectee in (select id from #data)   

GO
-----------------------------------------------------------------------------------------------------
create PROC GroupeChambre_CanDelete(@data xml=NULL)
AS
select T.N.value('id[1]', 'uniqueidentifier') id
into #data
from @data.nodes('utilisateurs/utilisateur') as T(N)

select 'Chambre' tableName, COUNT(cgc.Chambre) n from ChambreGroupeChambre cgc where cgc.GroupeChambre in (select id from #data)   

GO
-----------------------------------------------------------------------------------------------------
create PROC [dbo].[Intervention_CanDelete](@data xml=NULL)
AS
select T.N.value('id[1]', 'uniqueidentifier') id
into #data
from @data.nodes('utilisateurs/utilisateur') as T(N)

select 'InterventionDetail' tableName, COUNT(*) n from InterventionDetail where Intervention in (select id from #data)   
GO