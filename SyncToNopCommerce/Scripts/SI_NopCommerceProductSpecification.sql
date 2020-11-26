
create procedure SI_NopCommerceProductSpecification @LastSpecSync datetime
 as
 begin
select distinct T1.U_AttributeID[attribute_id], T1.U_Description[name], T1.U_ControlType [control_type] from [@SI_ITEMGROUP_ATTR] T0
join [@SI_ATTRIBUTE] T1 on T0.U_AttributeID = T1.U_AttributeID
where T0.U_Printable = 1 and 
 (T1.U_CreatedDateTime
> @LastSpecSync or T1.U_LastModifiedDateTime 
> @LastSpecSync)

end