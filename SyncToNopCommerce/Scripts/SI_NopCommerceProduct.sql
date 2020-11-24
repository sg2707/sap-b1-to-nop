
 create procedure SI_NopCommerceProduct @LastProductSync datetime
 as

 begin
select  Product.ItemCode [sku], U_SI_PartNumber [manufacturer_part_number],CodeBars [gtin]
,concat (U_SI_Brand,' ',Grp.U_SI_ItemClassDesc,U_SI_PartNumber) [name],
concat (U_SI_Brand,Grp.ItemClass) [short_description],
concat (U_SI_Brand,Grp.ItemClass,
	(select distinct stuff((select ','+ T2.U_Value from [OITM] T0
  join [@SI_ITEMGROUP_ATTR] T1 on T0.ItmsGrpCod = T1.U_EntityClassID
  join [@SI_ITEM_ATTRIBUTE] T2 on T1.U_AttributeID = T2.U_AttributeID
  join [@SI_ATTRIBUTE] T3 on T2.U_AttributeID = T3.U_AttributeID where t0.ItemCode=Product.ItemCode and T1.U_Printable =1

 for xml path('')
    ),1,1,'') as userlist
from  [OITM] T0
  join [@SI_ITEMGROUP_ATTR] T1 on T0.ItmsGrpCod = T1.U_EntityClassID
  join [@SI_ITEM_ATTRIBUTE] T2 on T1.U_AttributeID = T2.U_AttributeID
  join [@SI_ATTRIBUTE] T3 on T2.U_AttributeID = T3.U_AttributeID where t0.ItemCode=Product.ItemCode and T1.U_Printable =1
group by T2.U_Value )
) [full_description] ,U_SI_Brand [manufacturer],
Grp.U_SI_MainGroup  [category],isnull(U_SI_Length,0) [length]
,isnull(U_SI_Width,0) [width],isnull(U_SI_Height,0) [height],isnull(U_SI_Weight,0) [weight]
, 10.00 [price], cast((Stock.OnHand - Stock.IsCommited)as int) as stock_quantity
from OITM [Product]
inner join [OITW] Stock on Product.ItemCode = Stock.ItemCode and Stock.WhsCode = '01'
inner join [OITB] Grp on Grp.ItmsGrpCod = Product.ItmsGrpCod
where frozenFor = 'N' and  Grp.U_SI_MainGroup is not null and ( (cast(concat(Convert(date,Product.CreateDate),
' ', convert(char(8), dateadd(second, Product.createts, '') , 114)) as datetime))
> @LastProductSync or (cast(concat(Convert(date,Product.UpdateDate),
' ', convert(char(8), dateadd(second, Product.UpdateTS, '') , 114)) as datetime))
> @LastProductSync) 


end

