
create procedure [dbo].[SI_NopCommerceSpecificationOptions] @AttributeId nvarchar(100)
 as
 begin

 select T1.U_Description [name],T1.U_SortOrder [display_order] from
[@SI_ATTRIBUTE] T0
inner join [@SI_ATTRIBUTE_LINE] T1 on T0.U_AttributeID = T1.U_AttributeID
Where T0.U_AttributeID = @AttributeId and isnull(T1.U_Description, '') <> '' order by T1.U_SortOrder 

end