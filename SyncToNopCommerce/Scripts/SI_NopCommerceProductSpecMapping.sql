create procedure SI_NopCommerceProductSpecMapping @ProductSku nvarchar(100)
 as
 begin
select T0.U_ControlType[control_type],T0.U_Description [name], T1.U_EntityID[attribute_id], 
	case when T0.U_ControlType = 2 then T4.U_Description else T1.U_Value  end as [option_name] from
[@SI_ATTRIBUTE] T0
inner join [@SI_ITEM_ATTRIBUTE] T1 on T0.U_AttributeID = T1.U_AttributeID
inner join OITM T2 on T2.ItemCode = T1.U_EntityID
inner join [@SI_ITEMGROUP_ATTR] T3 on T3.U_AttributeID = T1.U_AttributeID and T3.U_EntityClassID = T2.ItmsGrpCod
left join [@SI_ATTRIBUTE_LINE] T4 on T4.U_AttributeID = T1.U_AttributeID and T1.U_Value = U_ValueID
where T1.U_EntityID=@ProductSku and U_Printable = 1

end
