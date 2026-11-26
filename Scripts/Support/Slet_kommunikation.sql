declare
	@kommionikationid varchar(max) = '94DF84DC-980C-4902-BF83-A6C008C5EE90'
	, @beskedId varchar(max)
	;

begin transaction deletekommunikation

select @beskedId = beskedId from Kommunikation where id = @kommionikationid

delete from advis where BeskedId = @beskedId

delete from Kommunikation where id = @kommionikationid

delete from Besked where id = @beskedId


/*
commit transaction deletekommunikation
rollback transaction deletekommunikation
*/
