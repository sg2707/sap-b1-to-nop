
create procedure SI_NopCommerceProductSpecification @LastSpecSync datetime
 as
 begin
 select U_Description [name] from   [@SI_ATTRIBUTE] where U_CreatedDateTime
> @LastSpecSync or U_LastModifiedDateTime 
> @LastSpecSync

end